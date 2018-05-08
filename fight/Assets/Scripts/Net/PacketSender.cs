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
    using System.Runtime.Remoting.Messaging;
    using System.IO;
    using ProtoBuf;

    public class PacketSender
    {
        public delegate void AsyncSendMethod();
        AsyncSendMethod asyncSendMethod;
        public Network network;
        private byte[] buffer;
        public int wpos = 0;
        public int spos = 0;
        public int wLen = 0;
 
        public PacketSender(Network network_) 
        {
            asyncSendMethod = new AsyncSendMethod(this.AsyncSend);
            network = network_;
            buffer = new byte[Config.tcpPacketMax];
            Clear();
        }

        public void Clear()
        {
            wpos = 0;
            spos = 0;
            wLen = 0;
        }

        public void Send<T>(T tmsg)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.Serialize<T>(ms, tmsg);
                int msgLen = (int)ms.Position;
                if (buffer.Length - wLen < msgLen + 10)
                {
                    return;
                }

                string msgName = tmsg.GetType().Name;
                msgName = "battle." + msgName;
                Debug.Log("send msg " + msgName);
                Array.Copy(BitConverter.GetBytes(msgName.Length), 0, buffer, wpos, 4);
                wpos = wpos + 4;
                wLen = wLen + 4;

                Array.Copy(BitConverter.GetBytes(msgLen), 0, buffer, wpos, 4);
                wpos = wpos + 4;
                wLen = wLen + 4;

                Array.Copy(BitConverter.GetBytes(MsgType.pb), 0, buffer, wpos, 4);
                wpos = wpos + 4;
                wLen = wLen + 4;

                int sid = 1;
                Array.Copy(BitConverter.GetBytes(sid), 0, buffer, wpos, 4);
                wpos = wpos + 4;
                wLen = wLen + 4;

                int tid = 1;
                Array.Copy(BitConverter.GetBytes(tid), 0, buffer, wpos, 4);
                wpos = wpos + 4;
                wLen = wLen + 4;

                Array.Copy(System.Text.Encoding.ASCII.GetBytes(msgName), 0, buffer, wpos, msgName.Length);
                wpos = wpos + msgName.Length;
                wLen = wLen + msgName.Length;

                var fullBytes = ms.GetBuffer();
                if (buffer.Length - wpos > msgLen)
                {
                    Array.Copy(fullBytes, 0, buffer, wpos, msgLen);
                    wpos = wpos + msgLen;
                    wLen = wLen + msgLen;
                }
                else if (buffer.Length - wpos == msgLen)
                {
                    Array.Copy(fullBytes, 0, buffer, wpos, msgLen);
                    wpos = 0;
                    wLen = wLen + msgLen;
                }
                else
                {
                    Array.Copy(fullBytes, 0, buffer, wpos, buffer.Length - wpos);
                    Array.Copy(fullBytes, buffer.Length - wpos, buffer, 0, msgLen - buffer.Length + wpos);
                    wpos = msgLen - buffer.Length + wpos;
                    wLen = wLen + msgLen;
                }
                
            }
            asyncSendMethod.BeginInvoke(OnSent, null);
        }

        // 子线程
        void AsyncSend()
        {
            if (network == null || !network.Valid())
            {
                return;
            }
            Socket socket = network.socket;
            while (true)
            {
                if (wLen <= 0)
                {
                    return;
                }
                int len;
                try
                {
                    if (buffer.Length - spos >= wLen)
                    {
                        len = socket.Send(buffer, spos, wLen, 0);
                    }
                    else
                    {
                        len = socket.Send(buffer, spos, buffer.Length - spos, 0);
                    }
                }
                catch (Exception e)
                {
                    Event.FireIn("OnCloseNetwork", new object[] { network });
                    Debug.Log("AsyncSend:" + e.ToString());
                    return;
                }
                spos = spos + len;
                wLen = wLen - len;
                if (spos == buffer.Length)
                {
                    spos = 0;
                }
            }
        }

        // 子线程
        private void OnSent(IAsyncResult ar)
        {
            AsyncResult result = (AsyncResult)ar;
            AsyncSendMethod caller = (AsyncSendMethod)result.AsyncDelegate;
            caller.EndInvoke(ar);
        }
    }
}
