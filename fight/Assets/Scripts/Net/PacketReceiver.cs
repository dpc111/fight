namespace Net
{
    using System;
    using UnityEngine;
    using System.Net.Sockets;
    using System.Net;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Runtime.Remoting.Messaging;

    public class PacketReceiver
    {
        public delegate void            AsyncReceiveMethod();
        private Network                 network = null;
        private MsgReader               msgReader = null;
        public RingBuffer               ringBuffer = null;
        public byte[]                   readBuffer;

        public PacketReceiver(Network net) 
        {
            network = net;
            msgReader = new MsgReader(this);
            ringBuffer = new RingBuffer(Config.recvRingBuffMax);
            readBuffer = new byte[Config.tcpPacketMax];
            Reset();
        }

        public void Reset() 
        {
            msgReader.Reset();
            ringBuffer.Reset();
            Array.Clear(readBuffer, 0, readBuffer.Length);
        }

        public void StartRecv() 
        {
            var v = new AsyncReceiveMethod(this.AsyncReceive);
            v.BeginInvoke(new AsyncCallback(OnReceive), null);
        }

        // 非主线程
        private void AsyncReceive()
        {
            if (network == null || !network.Valid())
            {
                return;
            }
            Socket socket = network.socket;
            while (true)
            {
                try
                {
                    int len = socket.Receive(readBuffer, 0, Config.tcpPacketMax, 0);
                    if (len < 0)
                    {
                        Event.FireIn("OnCloseNetwork", new object[] { network });
                        return;
                    }
                    while (ringBuffer.GetResLen() < len)
                    {
                        System.Threading.Thread.Sleep(10);
                    }
                    ringBuffer.Write(readBuffer, 0, len);
                }
                catch (SocketException e)
                {
                    Debug.LogError(e.ToString());
                    Event.FireIn("OnCloseNetwork", new object[] { network });
                    return;
                }
            }
        }

        // 非主线程
        private void OnReceive(IAsyncResult ar)
        {
            AsyncResult result = (AsyncResult)ar;
            AsyncReceiveMethod caller = (AsyncReceiveMethod)result.AsyncDelegate;
            caller.EndInvoke(ar);
        }

        // 主线程
        public void Process()
        {
            msgReader.Process();
        }
        
    }
}