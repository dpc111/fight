﻿namespace Net
{
    using System;
    using UnityEngine;
    using System.IO;
    using ProtoBuf;

    struct MsgType
    {
        public const int pb = 1;
        public const int script = 2;
    }

    enum MsgIndex
    {
        NameLen = 1,
        Len = 2,
        MsgType = 3,
        Sid = 4,
        Tid = 5,
        MsgName = 6,
        Msg = 7,
    }

    public class MsgReader
    {
        private int             nameLen = 0;
        private int             len = 0;
        private int             msgType = MsgType.pb;
        private int             sid = 0;
        private int             tid = 0;
        private MsgIndex        curIndex = MsgIndex.NameLen;

        private PacketReceiver  receiver = null;
        private byte[]          intData = new byte[4];
        private byte[]          msgName = new byte[1024];
        private byte[]          msg = new byte[10240];
        private const int       errInt = Int32.MaxValue;

        public MsgReader(PacketReceiver receiver_)
        {
            receiver = receiver_;
        }

        public void Reset()
        {
            nameLen = 0;
            len = 0;
            msgType = 0;
            sid = 0;
            tid = 0;
            curIndex = MsgIndex.NameLen;
        }

        public object MsgParse()
        {
            string name = System.Text.Encoding.UTF8.GetString(msgName, 0, nameLen - 1);
            Type type = Message.GetProtoType(name);
            if (type == null)
            {
                Debug.Log(name);
                return null;
            }
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write(msg, 0, len);
            ms.Position = 0;
            var m = ProtoBuf.Serializer.NonGeneric.Deserialize(type, ms);
            return m;
        }

        public int ReadInt()
        {
            if (receiver.ringBuffer.GetDataLen() < 4)
            {
                return errInt;
            }
            receiver.ringBuffer.Read(intData, 0, 4);
            return BitConverter.ToInt32(intData, 0);
        }

        public bool ReadString(byte[] data, int strLen)
        {
            if (receiver.ringBuffer.GetDataLen() < strLen)
            {
                return false;
            }
            receiver.ringBuffer.Read(data, 0, strLen);
            data[strLen] = 0;
            return true;
        }

        public void Process()
        {
            while (true)
            {
                if (curIndex == MsgIndex.NameLen)
                {
                    nameLen = ReadInt();
                    if (nameLen == errInt)
                    {
                        break;
                    }
                    curIndex = MsgIndex.Len;
                }
                else if (curIndex == MsgIndex.Len)
                {
                    len = ReadInt();
                    if (len == errInt)
                    {
                        break;
                    }
                    curIndex = MsgIndex.MsgType;
                }
                else if (curIndex == MsgIndex.MsgType)
                {
                    msgType = ReadInt();
                    if (msgType == errInt)
                    {
                        break;
                    }
                    curIndex = MsgIndex.Sid;
                }
                else if (curIndex == MsgIndex.Sid)
                {
                    sid = ReadInt();
                    if (nameLen == errInt)
                    {
                        break;
                    }
                    curIndex = MsgIndex.Tid;
                }
                else if (curIndex == MsgIndex.Tid)
                {
                    tid = ReadInt();
                    if (nameLen == errInt)
                    {
                        break;
                    }
                    curIndex = MsgIndex.MsgName;
                }
                else if (curIndex == MsgIndex.MsgName)
                {
                    if (!ReadString(msgName, nameLen)) 
                    {
                        break;
                    }
                    curIndex = MsgIndex.Msg;
                }
                else if (curIndex == MsgIndex.Msg) 
                {
                    if (!ReadString(msg, len)) 
                    {
                        break;
                    }
                    try
                    {
                        curIndex = MsgIndex.NameLen;
                        object m = MsgParse();
                        string name = System.Text.Encoding.Default.GetString(msgName, 0, nameLen - 1);
                        Debug.Log("recv msg: " + name);
                        Event.FireAll(name, new object[] { m });
                        Reset();
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e.ToString());
                        Reset();
                    }
                }
            }
        }
    }
}