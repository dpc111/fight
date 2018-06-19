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
        public static float timeDiff = 0;

        public static float CurRoomTime()
        {
            return CurTime() - roomBeginTime;
        }

        public static float CurTime()
        {
            return (DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / 10000000;
        }

        public static void SetTimeDiff(float serverCurTime)
        {
            timeDiff = CurTime() - serverCurTime;
        }

        public static float ClientTime(float serverTime)
        {
            return serverTime + timeDiff;
        }

        public static void SetRoomBeginTime(float serverBeginTime)
        {
            roomBeginTime = ClientTime(serverBeginTime);
        }
    }

}