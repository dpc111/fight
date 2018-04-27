namespace NetModel
{
    using System;
    using System.Net.Sockets;
    using System.Net;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Runtime.Remoting.Messaging;
    using System.IO;
    using System.Text;
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
                if (buffer.Length - wLen < msgLen)
                {
                    // error
                    return;
                }
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
                    Array.Copy(fullBytes, msgLen - wpos, buffer, 0, msgLen - buffer.Length + wpos);
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
                    // disconnect
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
