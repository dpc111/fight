namespace Net {
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Threading;

    public class NetTool {
        public static byte BytesToInt8(ref byte[] buff, int start) {
            byte value = buff[start];
            return value;
        }

        public static short BytesToInt16(ref byte[] buff, int start) {
            short value = (short)((buff[start] & 0xFF)
                | (buff[start + 1] & 0xFF) << 8);
            return value;
        }

        public static int BytesToInt32(ref byte[] buff, int start) {
            int value = (int)((buff[start + 0] & 0xFF)
                | (buff[start + 1] & 0xFF) << 8
                | (buff[start + 2] & 0xFF) << 16
                | (buff[start + 3] & 0xFF) << 24);
            return value;
        }

        public static void Int8ToBytes(ref byte[] buff, int start, int value) {
            buff[start + 0] = (byte)(value & 0xFF);
        }

        public static void Int32ToBytes(ref byte[] buff, int start, int value) {
            buff[start + 0] = (byte)(value & 0xFF);
            buff[start + 1] = (byte)((value >> 8) & 0xFF);
            buff[start + 2] = (byte)((value >> 16) & 0xFF);
            buff[start + 3] = (byte)((value >> 24) & 0xFF);
        }

        public static long GetMilSec() {
            long curTicks = DateTime.Now.Ticks;
            DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
            long curMil = (curTicks - dt.Ticks) / 10000;
            return curMil;
        }
    }
}