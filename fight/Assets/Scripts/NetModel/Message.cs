namespace Net
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using ProtoBuf;

    public class Message
    {
        public int id = 0;
        public string name;
        public int len = -1;
        public System.Reflection.MethodInfo handler = null;
        public static Dictionary<string, Message> messages = new Dictionary<string, Message>();
        public static Dictionary<string, Type> protoMap = new Dictionary<string, Type>();

        public Message(int msgid, string msgname, int msglen, System.Reflection.MethodInfo msghandler)
        {
            id = msgid;
            name = msgname;
            len = msglen;
            handler = msghandler;
            //Register(typeof(ProtoBuf).Assembly);
        }

        public static void Clear()
        {
            messages = new Dictionary<string, Message>();
        }

        public static void BindFixedMessage()
        {

        }

        public void HandleMessage()
        {

        }

        public static void Register(System.Reflection.Assembly assembly)
        {
            if (protoMap.Count > 0)
            {
                // error
                return;
            }
            foreach (Type type in assembly.GetTypes())
            {
                if (!type.IsAbstract && !type.IsInterface && type.GetCustomAttributes(typeof(ProtoBuf.ProtoContractAttribute), false).Length > 0)
                    protoMap[type.Name] = type;
            }
        }

        public static Type GetProtoType(string name)
        {
            Type type = protoMap[name]; 
            if (type != null)
            {
                return type;
            }
            return null;
        }
    }
}