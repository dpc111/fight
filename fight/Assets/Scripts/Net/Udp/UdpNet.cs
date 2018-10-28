namespace Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;


    class UdpNet {
        public Rudp rudp;
        public Socket clientSocket;
        public string clientIp = "0.0.0.0";
        public int clientPort = 6000;
        public EndPoint serverPoint;
        public string serverIp = "192.168.0.104";
        public int serverPort = 6000;
        public byte[] sendBuff = new byte[1024];
        public byte[] recvBuff = new byte[1024];

        public void Init() {
            rudp = new Rudp();
            rudp.Init(0);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            clientSocket.Bind(new IPEndPoint(IPAddress.Parse(clientIp), clientPort));
            serverPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
        }

        public void Send(byte[] msg, int size) {
            rudp.SendBuffIn(msg, size);
        }

        public void NetSend() {
            UdpChunk c = rudp.SendBuffOut();
            while (c != null) {
                NetTool.Int8ToBytes(ref sendBuff, 0, c.size);
                NetTool.Int8ToBytes(ref sendBuff, 1, c.type);
                NetTool.Int32ToBytes(ref sendBuff, 2, c.seq);
                NetTool.Int32ToBytes(ref sendBuff, 6, c.ack);
                Array.Copy(c.buff, 0, sendBuff, 0, c.size);
                clientSocket.SendTo(sendBuff, c.size + UdpConst.udpDataMaxLen, SocketFlags.None, serverPoint);
                c = rudp.SendBuffOut(); 
            }
        }

        public void Recive() {
            while (true) {
                int len = clientSocket.ReceiveFrom(recvBuff, ref serverPoint);
                if (len <= 0) {
                    continue;
                }
                if (len < UdpConst.udpHeadByteAll) {
                    continue;
                }
                int size = NetTool.BytesToInt8(ref recvBuff, 0);
                if (len < size + UdpConst.udpHeadByteAll) {
                    continue;
                }
                UdpChunk c = new UdpChunk();
                c.size = NetTool.BytesToInt8(ref recvBuff, 0);
                c.type = NetTool.BytesToInt8(ref recvBuff, 1);
                c.seq = NetTool.BytesToInt32(ref recvBuff, 2);
                c.ack = NetTool.BytesToInt32(ref recvBuff, 6);
                Array.Copy(recvBuff, 0, c.buff, 0, c.size);
                rudp.RecvBuffIn(c);
            }
        }
    }
}