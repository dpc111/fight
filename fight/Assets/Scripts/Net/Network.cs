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

    public class Network
    {
        public delegate void        AsyncConnectMethod(ConnectState state);
        public delegate void        ConnectCallback(string ip, int port, bool success, object userData);
        public const int            tcpPacketMax = 1024;
        public Socket               socket = null;
        public PacketReceiver       packetReceiver = null;
        public PacketSender         packetSender = null;
        public bool                 connected = false;

        public class ConnectState
        {
            public string ip = "";
            public int port = 0;
            public ConnectCallback connCb = null;
            public object userData = null;
            public Socket socket = null;
            public Network network = null;
            public string err = "";
        }

        public Network()
        {
            packetReceiver = new PacketReceiver(this);
            packetSender = new PacketSender(this);
            Message.Register();
        }

        ~Network()
        {

        }

        public void Start()
        {
            //ConnectTo(Config.ip, Config.port, null, null);
        }

        public void Reset()
        {
            Close();
            packetReceiver = new PacketReceiver(this);
            packetSender = new PacketSender(this);
            connected = false;
        }

        public void Close()
        {
            if (socket != null)
            {
                socket.Close();
            }
            socket = null;
            packetReceiver = null;
            packetSender = null;
            connected = false;
        }

        public virtual bool Valid()
        {
            if (socket == null)
            {
                return false;
            }
            if (!socket.Connected)
            {
                return false;
            }
            return true;
        }

        public void ConnectTo(string ip, int port, ConnectCallback callback, object useData) 
        {
            if (Valid())
            {
                return;
            }
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, Config.tcpPacketMax);
            socket.SetSocketOption(System.Net.Sockets.SocketOptionLevel.Socket, SocketOptionName.SendBuffer, Config.tcpPacketMax);
            socket.NoDelay = true;
            ConnectState state = new ConnectState();
            state.ip = ip;
            state.port = port;
            state.connCb = callback;
            state.userData = useData;
            state.socket = socket;
            state.network = this;
            connected = false;
            Event.RegisterIn("OnConnectState", this, "OnConnectState");
            var v = new AsyncConnectMethod(this.AsyncConnect);
            v.BeginInvoke(state, new AsyncCallback(this.AsyncConnectCb), state);
        }

        // 子线程
        private void AsyncConnect(ConnectState state) {
            try 
            {
                state.socket.Connect(state.ip, state.port);
            }
            catch (Exception e)
            {
                Debug.LogError("connect server failed " + e.ToString());
            }
        }

        // 子线程
        private void AsyncConnectCb(IAsyncResult ar)
        {
            Debug.Log("connect server success in AsyncConnectCb");
            ConnectState state = (ConnectState)ar.AsyncState;
            AsyncResult result = (AsyncResult)ar;
            AsyncConnectMethod caller = (AsyncConnectMethod)result.AsyncDelegate;
            caller.EndInvoke(ar);
            Event.FireIn("OnConnectState", new object[] { state });
        }

        // 主线程
        public void OnConnectState(ConnectState state) 
        {
            Debug.Log("connect server success in OnConnectState");
            Event.DeregisterIn(this);
            bool success = (state.err == "" && Valid());
            if (success)
            {
                packetReceiver = new PacketReceiver(this);
                packetReceiver.StartRecv();
                connected = true;
            }
            else
            {
                Close();
            }
            Event.FireAll("OnConnectState", new object[] { success });
            if (state.connCb != null)
            {
                state.connCb(state.ip, state.port, success, state.userData);
            }
        }

        public void OnCloseNetwork()
        {
            Close();
        }

        public void Send(object tmsg)
        {
            packetSender.Send(tmsg);
        }

        // 主线程
        public void Process()
        {
            if (!Valid())
            {
                return;
            }
            if (packetReceiver != null)
            {
                packetReceiver.Process();
            }
        }
    }
}