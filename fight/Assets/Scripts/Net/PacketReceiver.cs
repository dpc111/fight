namespace Net_
{
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
	
    //包接收模块(与服务端网络部分的名称对应)
    //处理网络数据的接收
	public class PacketReceiver
	{
		public delegate void AsyncReceiveMethod(); 
		private MessageReader messageReader = null;
		private NetworkInterface networkInterface = null;
		private byte[] buffer;
		//socket向缓冲区写的起始位置
		int wpos = 0;
		//主线程读取数据的起始位置
		int rpos = 0;

		public PacketReceiver(NetworkInterface networkInterface)
		{
			Init(networkInterface);
		}

		~PacketReceiver()
		{
			Dbg.DebugMsg("PacketReceiver::~PacketReceiver(), destroyed!");
		}

		void Init(NetworkInterface network)
		{
            networkInterface = network;
			buffer = new byte[NetApp.app.GetInitArgs().recvBufferMax];
			messageReader = new MessageReader();
		}

		public NetworkInterface NetworkInterface()
		{
			return networkInterface;
		}

		public void Process()
		{
			int twpos = Interlocked.Add(ref wpos, 0);
			if (rpos < twpos)
			{
				messageReader.Process(buffer, (UInt32)rpos, (UInt32)(twpos - rpos));
				Interlocked.Exchange(ref rpos, twpos);
			}
			else if (twpos < rpos)
			{
				messageReader.Process(buffer, (UInt32)rpos, (UInt32)(buffer.Length - rpos));
				messageReader.Process(buffer, (UInt32)0, (UInt32)twpos);
				Interlocked.Exchange(ref rpos, twpos);
			}
			else
			{
				// 没有可读数据
			}
		}

		int Free()
		{
			int trpos = Interlocked.Add(ref rpos, 0);
			if (wpos == buffer.Length)
			{
				if (trpos == 0)
				{
					return 0;
				}
				Interlocked.Exchange(ref wpos, 0);
			}
			if (trpos <= wpos)
			{
				return buffer.Length - wpos;
			}
			return trpos - wpos - 1;
		}

		public void StartRecv()
		{
			var v = new AsyncReceiveMethod(this.AsyncReceive);
			v.BeginInvoke(new AsyncCallback(OnRecv), null);
		}

		private void AsyncReceive()
		{
			if (networkInterface == null || !networkInterface.Valid())
			{
				Dbg.WarningMsg("network interface invalid!");
				return;
			}

			var socket = networkInterface.Sock();

			while (true)
			{
				//必须有空间可写，否则我们阻塞在线程中直到有空间为止
				int first = 0;
				int space = Free();
				while (space == 0)
				{
					if (first > 0)
					{
						if (first > 1000)
						{
							Dbg.ErrorMsg("no space!");
							Event.FireIn("_closeNetwork", new object[] { networkInterface });
							return;
						}
						Dbg.WarningMsg("waiting for space");
						System.Threading.Thread.Sleep(5);
					}
					first += 1;
					space = Free();
				}
				int bytesRead = 0;
				try
				{
					bytesRead = socket.Receive(buffer, wpos, space, 0);
				}
				catch (SocketException se)
				{
					Dbg.ErrorMsg(string.Format("receive error, disconnect from '{0}'! error = '{1}'", socket.RemoteEndPoint, se));
					Event.FireIn("_closeNetwork", new object[] { networkInterface });
					return;
				}
				if (bytesRead > 0)
				{
					// 更新写位置
					Interlocked.Add(ref wpos, bytesRead);
				}
				else
				{
					Dbg.WarningMsg(string.Format("PacketReceiver::_asyncReceive(): receive 0 bytes, disconnect from '{0}'!", socket.RemoteEndPoint));
					Event.FireIn("_closeNetwork", new object[] { networkInterface });
					return;
				}
			}
		}

		private void OnRecv(IAsyncResult ar)
		{
			AsyncResult result = (AsyncResult)ar;
			AsyncReceiveMethod caller = (AsyncReceiveMethod)result.AsyncDelegate;
			caller.EndInvoke(ar);
		}
	}
} 
