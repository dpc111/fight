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

        public static void Register(string name, Type type)
        {
            protoMap[name] = type;
        }

        public static void Register()
        {
            Register("battle_msg.s_login_hall", typeof(battle_msg.s_login_hall));
            Register("battle_msg.s_login", typeof(battle_msg.s_login));
            Register("battle_msg.s_get_room_info", typeof(battle_msg.s_get_room_info));
            Register("battle_msg.s_room_state", typeof(battle_msg.s_room_state));
            Register("battle_msg.s_create_entity", typeof(battle_msg.s_create_entity));
            Register("battle_msg.s_fire", typeof(battle_msg.s_fire));
            Register("battle_msg.s_collision", typeof(battle_msg.s_collision));
            Register("battle_msg.s_destroy_entity", typeof(battle_msg.s_destroy_entity));
            Register("battle_msg.s_update_state", typeof(battle_msg.s_update_state));
        }

        public static Type GetProtoType(string name)
        {
            Type type = null;
            if (!protoMap.TryGetValue(name, out type))
            {
                Debug.Log(name);
                return null;
            }
            return type;
        }
    }
}