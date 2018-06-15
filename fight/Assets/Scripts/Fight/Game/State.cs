namespace Game
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using ProtoBuf;

    public class State
    {
        public static int roomState;
        public static float roomBeginTime;

        public float GetRoomTime()
        {
            return GetCurTime() - roomBeginTime;
        }

        public float GetCurTime()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }
    }

}