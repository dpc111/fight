namespace Net {
    using System;
    using System.Reflection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;
    using System.Runtime.InteropServices;
    using UnityEngine;

    public class UdpNet {
        public Rudp rudp;

        public Socket clientSocket;
        public static string clientIp = "0.0.0.0";
        public static int clientPort = 6001;

        public EndPoint serverPoint;
        public static string serverIp = "139.199.82.153";
        public static int serverPort = 7002;

        public Thread netThread;

        public object msgCallBackObj = null;
        public MethodInfo msgCallBack = null;

        public void Start() {
            UdpMsg.Init();
            rudp = new Rudp();
            rudp.Init(NetTool.GetMilSec());
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            clientSocket.Bind(new IPEndPoint(IPAddress.Parse(clientIp), clientPort));
            serverPoint = new IPEndPoint(IPAddress.Parse(serverIp), serverPort);
            netThread = new Thread(NetProcess);
            netThread.Start();
            ConnectToServer(111);
        }

        public void Stop() {
            DisconnectToServer();
            if (rudp != null) {
                rudp = null;
            }
            if (clientSocket != null) {
                clientSocket.Close();
                clientSocket = null;
            }
            if (serverPoint != null) {
                serverPoint = null;
            }
            if (netThread != null) {
                netThread.Abort();
                netThread = null;
            }
            if (msgCallBackObj != null) {
                msgCallBackObj = null;
            }
            if (msgCallBack != null) {
                msgCallBack = null;
            }
        }

        public void ConnectToServer(int uid) {
            if (rudp == null)
                return;
            UdpChunk c = new UdpChunk();
            c.size = (byte)0;
            c.type = UdpConst.udpTypeConnect;
            c.seq = 0;
            c.ack = uid;
            rudp.SendChunkForce(c);
        }

        public void DisconnectToServer() {
            if (rudp == null)
                return;
            UdpChunk c = new UdpChunk();
            c.size = (byte)0;
            c.type = UdpConst.udpTypeDisconnect;
            c.seq = 0;
            c.ack = 0;
            rudp.SendChunkForce(c);
        }

        public void RegisterMsgCallback(object obj, string name) {
            msgCallBackObj = obj;
            msgCallBack = obj.GetType().GetMethod(name);
        }

        public void Send(int msgid, object obj) {
            UdpChunk c = new UdpChunk();
            int offset = 0;
            NetTool.Int32ToBytes(ref c.buff, offset, msgid);
            offset += 4;
            int size = Marshal.SizeOf(obj);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, c.buff, offset, size);
            offset += size;
            Marshal.FreeHGlobal(ptr);
            c.size = (byte)offset;
            Monitor.Enter(rudp);
            rudp.SendBuffIn(c);
            Monitor.Exit(rudp);
        }

        public void Send(object obj) {
            UdpChunk c = new UdpChunk();
            int offset = 0;
            NetTool.Int32ToBytes(ref c.buff, offset, UdpMsg.MsgId(obj.GetType()));
            offset += 4;
            int size = Marshal.SizeOf(obj);
            IntPtr ptr = Marshal.AllocHGlobal(size);
            Marshal.StructureToPtr(obj, ptr, false);
            Marshal.Copy(ptr, c.buff, offset, size);
            offset += size;
            Marshal.FreeHGlobal(ptr);
            c.size = (byte)offset;
            Monitor.Enter(rudp);
            rudp.SendBuffIn(c);
            Monitor.Exit(rudp);
        }

        public void MainProcess() {
            while (true) {
                Monitor.Enter(rudp);
                UdpChunk c = rudp.RecvBuffOut();
                Monitor.Exit(rudp);
                if (c == null) {
                    break;
                }
                if (c.size < 4) {
                    break;
                }
                int offset = 0;
                int frame = NetTool.BytesToInt32(ref c.buff, 0);
                offset += 4;
                if (msgCallBackObj != null && msgCallBack != null) {
                    msgCallBack.Invoke(msgCallBackObj, new object[] { frame, 0, null });
                }
                while (c.size - offset > 8) {
                    int uid = NetTool.BytesToInt32(ref c.buff, offset);
                    offset += 4;
                    int msgid = NetTool.BytesToInt32(ref c.buff, offset);
                    offset += 4;
                    Type type = UdpMsg.MsgType(msgid);
                    if (type == null) {
                        Debug.Log("");
                        break;
                    }
                    int sizeObj = Marshal.SizeOf(type);
                    if (sizeObj > c.size - offset) {
                        Debug.Log("");
                        break;
                    }
                    IntPtr ptr = Marshal.AllocHGlobal(sizeObj);
                    Marshal.Copy(c.buff, offset, ptr, sizeObj);
                    offset += sizeObj;
                    object obj = Marshal.PtrToStructure(ptr, type);
                    Marshal.FreeHGlobal(ptr);
                    if (msgCallBackObj != null && msgCallBack != null) {
                        msgCallBack.Invoke(msgCallBackObj, new object[] { frame, uid, obj });
                    }
                }
            }
            Monitor.Enter(rudp);
            rudp.HandleProcess(NetTool.GetMilSec());
            Monitor.Exit(rudp);
        }

        private void NetProcess() {
            byte[] sendBuff = new byte[1024];
            byte[] recvBuff = new byte[1024];
            while (true) {
                do {
                    if (clientSocket == null) {
                        break;
                    }
                    if (!clientSocket.Poll(0, SelectMode.SelectRead)) {
                        break;
                    }
                    int len = clientSocket.ReceiveFrom(recvBuff, ref serverPoint);
                    if (len <= 0) {
                        break;
                    }
                    if (len < UdpConst.udpHeadByteAll) {
                        break;
                    }
                    int size = NetTool.BytesToInt8(ref recvBuff, 0);
                    if (len < size + UdpConst.udpHeadByteAll) {
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
                do {
                    if (clientSocket == null) {
                        break;
                    }
                    Monitor.Enter(rudp);
                    UdpChunk c = rudp.SendBuffOut();
                    Monitor.Exit(rudp);
                    if (c == null) {
                        break;
                    }
                    if (!clientSocket.Poll(0, SelectMode.SelectWrite)) {
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
                    if (len < dataLen) {
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