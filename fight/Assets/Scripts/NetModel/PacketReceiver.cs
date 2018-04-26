namespace NetModel
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

    public class PacketReceiver
    {
        public delegate void AsyncReceiveMethod();
        private Network network = null;
        private MsgReader msgReader = null;
        public byte[] buffer;
        public int wpos = 0;
        public int rpos = 0;
        public int rlen = 0;

        public void MonitorEnter()
        {
            Monitor.Enter(this);
        }

        public void MonitorExit()
        {
            Monitor.Exit(this);
        }

        public PacketReceiver(Network net) 
        {
            network = net;
            msgReader = new MsgReader(this);
            buffer = new byte[Config.tcpPacketMax];
        }

        public int Free()
        {
            MonitorEnter();
            int len = 0;
            if (wpos == buffer.Length)
            {
                if (rpos == 0)
                {
                    MonitorExit();
                    return len;
                }
                wpos = 0;
            }
            if (rpos <= wpos)
            {
                len = buffer.Length - wpos;
                MonitorExit();
                return len;
            }
            MonitorExit();
            return rpos - wpos - 1;
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
                // 必须有空间可写，否则我们阻塞在线程中直到有空间为止
                int first = 0;
                int space = Free();
                while (space == 0)
                {
                    if (first > 0)
                    {
                        if (first > 1000)
                        {
                            // error
                            Event.FireIn("EventCloseNetwork", new object[] { network });
                            return;
                        }
                        System.Threading.Thread.Sleep(5);
                    }
                    ++first;
                    space = Free();
                }
                int len = 0;
                try
                {
                    len = socket.Receive(buffer, wpos, space, 0);
                }
                catch (SocketException e)
                {
                    Event.FireIn("EventCloseNetwork", new object[] { network });
                    return;
                }
                if (len > 0)
                {
                    //Interlocked.Add(ref wpos, len);
                    MonitorEnter();
                    wpos = wpos + len;
                    rlen = rlen + len;
                    MonitorExit();
                }
                else
                {
                    Event.FireIn("EventCloseNetwork", new object[] { network });
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

        // 网络主线程
        public void Process()
        {
            MonitorEnter();
            msgReader.Process();
            MonitorExit();
        }
        
    }
}