namespace NetModel
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;

    public class Message
    {
        public int id = 0;
        public string name;
        public int len = -1;
        public System.Reflection.MethodInfo handler = null;
        public static Dictionary<string, Message> messages = new Dictionary<string, Message>();

        public Message(int msgid, string msgname, int msglen, System.Reflection.MethodInfo msghandler)
        {
            id = msgid;
            name = msgname;
            len = msglen;
            handler = msghandler;
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
    }
}