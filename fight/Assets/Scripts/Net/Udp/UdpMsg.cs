namespace Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    public class UdpMsg
    {
        public static Dictionary<int, Type> msgMap = new Dictionary<int, Type>();
        public static Dictionary<Type, int> msgIdMap = new Dictionary<Type, int>();

        public static void Register(int msgid, Type msgType)
        {
            msgMap[msgid] = msgType;
            msgIdMap[msgType] = msgid;
        }

        public static void Init()
        {
            Register(1, Type.GetType("MsgCreateTower"));
        }

        public static Type MsgType(int msgid)
        {
            Type msgType = null;
            if (!msgMap.TryGetValue(msgid, out msgType))
            {
                return null;
            }
            return msgType;
        }

        public static int MsgId(Type msgType)
        {
            int msgId = 0;
            if (!msgIdMap.TryGetValue(msgType, out msgId))
            {
                return 0;
            }
            return msgId;
        }
    }
}