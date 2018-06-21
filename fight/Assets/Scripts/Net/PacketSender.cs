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
        public delegate void                AsyncSendMethod();
        AsyncSendMethod                     asyncSendMethod;
        public Network                      network;
        public RingBuffer                   ringBuffer;
        private byte[]                      sendBuffer;

        public PacketSender(Network net) 
        {
            asyncSendMethod = new AsyncSendMethod(this.AsyncSend);
            network = net;
            ringBuffer = new RingBuffer(Config.recvRingBuffMax);
            sendBuffer = new byte[Config.recvRingBuffMax];
            Reset();
        }

        public void Reset()
        {
            ringBuffer.Reset();
            Array.Clear(sendBuffer, 0, sendBuffer.Length);
        }

        public void Send(object tmsg)
        {
            using (var ms = new System.IO.MemoryStream())
            {
                ProtoBuf.Serializer.NonGeneric.Serialize(ms, tmsg);
                int msgLen = (int)ms.Position;
                while (ringBuffer.GetResLen() < msgLen + 10)
                {
                    System.Threading.Thread.Sleep(10);
                }
                string msgName = tmsg.GetType().Name;
                msgName = "battle_msg." + msgName;
                Debug.Log("send msg " + msgName);
                int sid = 1;
                int tid = 1;
                ringBuffer.Write(BitConverter.GetBytes(msgName.Length), 0, 4);
                ringBuffer.Write(BitConverter.GetBytes(msgLen), 0, 4);
                ringBuffer.Write(BitConverter.GetBytes(MsgType.pb), 0, 4);
                ringBuffer.Write(BitConverter.GetBytes(sid), 0, 4);
                ringBuffer.Write(BitConverter.GetBytes(tid), 0, 4);
                ringBuffer.Write(System.Text.Encoding.ASCII.GetBytes(msgName), 0, msgName.Length);
                ringBuffer.Write(ms.GetBuffer(), 0, msgLen);
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
                if (ringBuffer.GetDataLen() <= 0)
                {
                    return;
                }
                try
                {
                    int len = ringBuffer.ReadAll(sendBuffer, 0);
                    len = socket.Send(sendBuffer, 0, len, 0);
                }
                catch (Exception e)
                {
                    Event.FireIn("OnCloseNetwork", new object[] { network });
                    Debug.Log("AsyncSend:" + e.ToString());
                    return;
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
