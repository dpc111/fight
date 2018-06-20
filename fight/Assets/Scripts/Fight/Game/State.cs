namespace Game
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using ProtoBuf;

    public class State
    {
        public static bool run = false;
        public static int roomState;
        public static double timeDiff = 0;

        public static void SetRoomState(int state)
        {
            roomState = state;
            if (state == 1 || state == 2)
            {
                run = true; 
            } 
            else
            {
                run = false;
            }
        }

        public static void SetTimeDiff(double serverCurTime)
        {
            timeDiff = CurTime() - serverCurTime;
        }

        public static double CurServerTime()
        {
            return CurTime() - timeDiff;
        }

        private static double CurTime()
        {
            return (double)(DateTime.Now.ToUniversalTime().Ticks - 621355968000000000) / (double)10000000;
        }
    }

}