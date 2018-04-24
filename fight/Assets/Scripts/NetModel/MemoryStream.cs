namespace NetModel
{
    using UnityEngine;
    using System;
    using System.Net;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using System.Runtime.InteropServices;

    public class MemoryStream : ObjectPool<MemoryStream>
    {
        public const int bufMax = 1024 * 4;
        public int rpos = 0;
        public int wpos = 0;
        private byte[] data = new byte[bufMax];
        private static System.Text.ASCIIEncoding converter = new System.Text.ASCIIEncoding();

        public byte[] Data()
        {
            return data;
        }

        public void ReclaimObject()
        {
            Clear();
            ReclaimObject(this);
        }

        public void Clear()
        {
            rpos = 0;
            wpos = 0;
            if (data.Length > bufMax)
            {
                data = new byte[bufMax];
            }
        }

		//---------------------------------------------------------------------------------
        public Int32 ReadInt32()
        {
            rpos += 4;
            return BitConverter.ToInt32(data, rpos - 2);
        }

        public string ReadString()
        {
            int offset = rpos;
            while (data[rpos] != 0)
            {
                ++rpos;
            }
            return converter.GetString(data, offset, rpos - offset - 1);
        }

        public void WriteInt8(SByte v)
        {
            data[wpos] = (Byte)v;
            ++wpos;
        }

        public void WriteInt32(Int32 v)
        {
            for (int i = 0; i < 4; i++)
            {
                WriteInt8((SByte)(v >> i * 8 & 0xff));
            }
        }
    }
}