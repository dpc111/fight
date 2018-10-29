namespace Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Runtime.InteropServices;

    class UdpNet
    {
        public Rudp rudp;

        public Socket clientSocket;
        public string clientIp = "0.0.0.0";
        public int clientPort = 6000;

        public EndPoint serverPoint;
        public string serverIp = "192.168.0.104";
        public int serverPort = 6000;

        public Thread netThread;

        public void Start()
        {
            rudp = new Rudp();
            rudp.Init(0);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            clientSocket.Bind(new IPEndPoint(IPAddress.Parse(clientIp), clientPort));
            serverPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            netThread = new Thread(NetProcess);
            netThread.Start();
        }

        public void Stop()
        {
            if (rudp != null)
            {
                rudp = null;
            }
            if (clientSocket != null)
            {
                clientSocket.Close();
                clientSocket = null;
            }
            if (serverPoint != null)
            {
                serverPoint = null;
            }
            if (netThread != null)
            {
                netThread.Abort();
                netThread = null;
            }
        }

        public void Send(int uid, int msgid, Object obj)
        {
            UdpChunk c = new UdpChunk();
            NetTool.Int32ToBytes(ref c.buff, 0, uid);
            NetTool.Int32ToBytes(ref c.buff, 4, msgid);
            int size = Marshal.SizeOf(obj);  
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, c.buff, 8, size);
            Marshal.FreeHGlobal(ptr);
            c.size = (byte)(size + 8);
            Monitor.Enter(rudp);
            rudp.SendBuffIn(c);
            Monitor.Exit(rudp);
        }

        public void MainProcess()
        {
            while (true)
            {
                Monitor.Enter(rudp);
                UdpChunk c = rudp.RecvBuffOut();
                Monitor.Exit(rudp);
                if (c == null)
                {
                    break;
                }
                if (c.size < 12)
                {
                    break;
                }
                int uid = NetTool.BytesToInt32(ref c.buff, 0);
                int msgid = NetTool.BytesToInt32(ref c.buff, 4);
                Type type = UdpMsg.Proto(msgid);
                if (type == null)
                {
                    break;
                }
                int sizeObj = Marshal.SizeOf(type);
                if (sizeObj < c.size - 8)  
                {  
                    break;  
                }
                IntPtr ptr = Marshal.AllocHGlobal(sizeObj);
                Marshal.Copy(c.buff, 8, ptr, sizeObj);
                object obj = Marshal.PtrToStructure(ptr, type);
                Marshal.FreeHGlobal(ptr);
                // msg call back
            }
            Monitor.Enter(rudp);
            rudp.HandleProcess(0);
            Monitor.Exit(rudp);
        }

        private void NetProcess()
        {
            byte[] sendBuff = new byte[1024];
            byte[] recvBuff = new byte[1024];
            while (true)
            {
                do
                {
                    if (clientSocket == null)
                    {
                        break;
                    }
                    if (!clientSocket.Poll(0, SelectMode.SelectRead))
                    {
                        break;
                    }
                    int len = clientSocket.ReceiveFrom(recvBuff, ref serverPoint);
                    if (len <= 0)
                    {
                        break;
                    }
                    if (len < UdpConst.udpHeadByteAll)
                    {
                        break;
                    }
                    int size = NetTool.BytesToInt8(ref recvBuff, 0);
                    if (len < size + UdpConst.udpHeadByteAll)
                    {
                        break;
                    }
                    UdpChunk c = new UdpChunk();
                    c.size = NetTool.BytesToInt8(ref recvBuff, 0);
                    c.type = NetTool.BytesToInt8(ref recvBuff, 1);
                    c.seq = NetTool.BytesToInt32(ref recvBuff, 2);
                    c.ack = NetTool.BytesToInt32(ref recvBuff, 6);
                    Array.Copy(recvBuff, 10, c.buff, 0, c.size);
                    Monitor.Enter(rudp);
                    rudp.RecvBuffIn(c);
                    Monitor.Exit(rudp);
                } while (false);
                do
                {
                    if (clientSocket == null)
                    {
                        break;
                    }
                    Monitor.Enter(rudp);
                    UdpChunk c = rudp.SendBuffOut();
                    Monitor.Exit(rudp);
                    if (c == null)
                    {
                        break;
                    }
                    if (!clientSocket.Poll(0, SelectMode.SelectWrite))
                    {
                        Monitor.Enter(rudp);
                        rudp.SendChunkForce(c);
                        Monitor.Exit(rudp);
                        break;
                    }
                    NetTool.Int8ToBytes(ref sendBuff, 0, c.size);
                    NetTool.Int8ToBytes(ref sendBuff, 1, c.type);
                    NetTool.Int32ToBytes(ref sendBuff, 2, c.seq);
                    NetTool.Int32ToBytes(ref sendBuff, 6, c.ack);
                    Array.Copy(c.buff, 0, sendBuff, 10, c.size);
                    int dataLen = c.size + UdpConst.udpHeadByteAll;
                    int len = clientSocket.SendTo(sendBuff, dataLen, SocketFlags.None, serverPoint);
                    if (len < dataLen)
                    {
                        Monitor.Enter(rudp);
                        rudp.SendChunkForce(c);
                        Monitor.Exit(rudp);
                    }
                } while (false);
                Thread.Sleep(10);
            }
        }
    }
}