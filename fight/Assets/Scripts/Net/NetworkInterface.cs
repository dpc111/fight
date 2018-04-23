namespace Net
{
	using UnityEngine;
	using System;
	using System.Net.Sockets;
	using System.Net;
	using System.Collections;
	using System.Collections.Generic;
	using System.Text;
	using System.Text.RegularExpressions;
	using System.Threading;
	using System.Runtime.Remoting.Messaging;

	using MsgID = System.UInt16;
	using MsgLength = System.UInt16;

	//网络模块
	//处理连接、收发数据
	public class NetworkInterface
	{
		public delegate void AsyncConnectMethod(ConnectState state);
		public delegate void ConnectCallback(string ip, int port, bool success, object userData);
        public const int tcpPacketMax = 1460;
        protected Socket socket = null;
		PacketReceiver packetReceiver = null;
		PacketSender packetSender = null;
		public bool connected = false;
		
		public class ConnectState
		{
			public string connectIP = "";
			public int connectPort = 0;
			public ConnectCallback connectCB = null;
			public object userData = null;
			public Socket socket = null;
			public NetworkInterface networkInterface = null;
			public string error = "";
		}
		
		public NetworkInterface()
		{
			Reset();
		}

		~NetworkInterface()
		{
			Dbg.DebugMsg("NetworkInterface destructed");
			Reset();
		}

		public virtual Socket Sock()
		{
			return socket;
		}
		
		public void Reset()
		{
			if(Valid())
			{
			    Dbg.DebugMsg(string.Format("NetworkInterface::reset(), close socket from {0}", socket.RemoteEndPoint.ToString()));
         	    socket.Close(0);
			}
			socket = null;
			packetReceiver = null;
			packetSender = null;
			connected = false;
		}
		
        public void Close()
        {
           if(socket != null)
			{
				socket.Close(0);
				socket = null;
				Event.FireAll("onDisconnected", new object[]{});
            }
            socket = null;
            connected = false;
        }

		public virtual PacketReceiver PacketReceiver()
		{
			return packetReceiver;
		}
		
		public virtual bool Valid()
		{
			return ((socket != null) && (socket.Connected == true));
		}
		
        //当前线程触发
		public void OnConnectionState(ConnectState state)
		{
			Net.Event.DeregisterIn(this);
			bool success = (state.error == "" && Valid());
			if (success)
			{
                Dbg.DebugMsg(string.Format("connect to {0} is success!", state.socket.RemoteEndPoint.ToString()));
				packetReceiver = new PacketReceiver(this);
				packetReceiver.StartRecv();
				connected = true;
			}
			else
			{
				Reset();
                Dbg.ErrorMsg(string.Format("connect error! ip: {0}:{1}, err: {2}", state.connectIP, state.connectPort, state.error));
			}
			Event.FireAll("OnConnectionState", new object[] { success });
			if (state.connectCB != null)
				state.connectCB(state.connectIP, state.connectPort, success, state.userData);
		}

		private static void ConnectCB(IAsyncResult ar)
		{
			ConnectState state = null;
			try 
			{
				//Retrieve the socket from the state object.
				state = (ConnectState) ar.AsyncState;
				//Complete the connection.
				state.socket.EndConnect(ar);
                Event.FireIn("OnConnectionState", new object[] { state });
			} 
			catch (Exception e) 
			{
				state.error = e.ToString();
                Event.FireIn("OnConnectionState", new object[] { state });
			}
		}

		//在非主线程执行：连接服务器
		private void AsyncConnect(ConnectState state)
		{
			Dbg.DebugMsg(string.Format("will connect to '{0}:{1}' ...", state.connectIP, state.connectPort));
			try
			{
				state.socket.Connect(state.connectIP, state.connectPort);
			}
			catch (Exception e)
			{
				Dbg.ErrorMsg(string.Format("connect to '{0}:{1}' fault! error = '{2}'", state.connectIP, state.connectPort, e));
				state.error = e.ToString();
			}
		}

		//在非主线程执行：连接服务器结果回调
		private void AsyncConnectCB(IAsyncResult ar)
		{
			ConnectState state = (ConnectState)ar.AsyncState;
			AsyncResult result = (AsyncResult)ar;
			AsyncConnectMethod caller = (AsyncConnectMethod)result.AsyncDelegate;
			Dbg.DebugMsg(string.Format("connect to '{0}:{1}' finish. error = '{2}'", state.connectIP, state.connectPort, state.error));
			//Call EndInvoke to retrieve the results.
			caller.EndInvoke(ar);
            Event.FireIn("OnConnectionState", new object[] { state });
		}

		public void ConnectTo(string ip, int port, ConnectCallback callback, object userData)
		{
			if (Valid())
				throw new InvalidOperationException("Have already connected!");
			socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
			socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, NetApp.app.GetInitArgs().getRecvBufferSize() * 2);
			socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.SendBuffer, NetApp.app.GetInitArgs().getSendBufferSize() * 2);
			socket.NoDelay = true;
			ConnectState state = new ConnectState();
			state.connectIP = ip;
			state.connectPort = port;
			state.connectCB = callback;
			state.userData = userData;
			state.socket = socket;
			state.networkInterface = this;
			Dbg.DebugMsg("connect to " + ip + ":" + port + " ...");
			connected = false;
			//先注册一个事件回调，该事件在当前线程触发
            Event.RegisterIn("OnConnectionState", this, "OnConnectionState");
			var v = new AsyncConnectMethod(this.AsyncConnect);
			v.BeginInvoke(state, new AsyncCallback(this.AsyncConnectCB), state);
		}

		public bool Send(MemoryStream stream)
		{
			if (!Valid())
				throw new ArgumentException("invalid socket!");
			if (packetSender == null)
				packetSender = new PacketSender(this);
			return packetSender.Send(stream);
		}

		public void Process()
		{
			if (!Valid())
				return;

			if (packetReceiver != null)
				packetReceiver.Process();
		}
	}
}
