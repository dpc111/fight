using System;
using UnityEngine;
using System.Threading;

namespace Net
{
    public class RingBuffer
    {
        private byte[]              buffer;
        private int                 writePos;
        private int                 readPos;
        private int                 dataLen;

        public RingBuffer(int size) 
        {
            Monitor.Enter(this);
            writePos = 0;
            readPos = 0;
            dataLen = 0;
            buffer = new byte[size];
            Monitor.Exit(this);
        }

        public void Reset()
        {
            Monitor.Enter(this);
            writePos = 0;
            readPos = 0;
            dataLen = 0;
            Monitor.Exit(this);
        }

        public int GetDataLen()
        {
            Monitor.Enter(this);
            int len = dataLen;
            Monitor.Exit(this);
            return len;
        }

        public int GetResLen()
        {
            Monitor.Enter(this);
            int len = buffer.Length - dataLen;
            Monitor.Exit(this);
            return len;
        }

        public void Write(byte[] srcBuffer, int offset, int len)
        {
            Monitor.Enter(this);
            int resLen = buffer.Length - dataLen;
            if (resLen < len)
            {
                Monitor.Exit(this);
                throw new Exception("buffer is not enough");
            }
            if (writePos + len < buffer.Length)
            {
                Array.Copy(srcBuffer, offset, buffer, writePos, len);
                writePos += len;
                dataLen += len;
            }
            else
            {
                int writeLen = buffer.Length - writePos;
                Array.Copy(srcBuffer, offset, buffer, writePos, writeLen);
                Array.Copy(srcBuffer, offset + writeLen, buffer, 0, len - writeLen);
                writePos = len - writeLen;
                dataLen += len;
            }
            Monitor.Exit(this);
        }

        public void Read(byte[] desBuffer, int offset, int len)
        {
            Monitor.Enter(this);
            if (len > dataLen)
            {
                Monitor.Exit(this);
                Debug.LogError("");
                throw new Exception("data in ringbuffer is not enough");
            }
            if (readPos + len < buffer.Length)
            {
                Array.Copy(buffer, readPos, desBuffer, offset, len);
                readPos += len;
                dataLen -= len;
            }
            else
            {
                int readLen = buffer.Length - readPos;
                Array.Copy(buffer, readPos, desBuffer, offset, readLen);
                Array.Copy(buffer, 0, desBuffer, offset + readLen, len - readLen);
                readPos = len - readLen;
                dataLen -= len;
            }
            Monitor.Exit(this);
        }

        public int ReadAll(byte[] desBuffer, int offset)
        {
            Monitor.Enter(this);
            int len = dataLen;
            if (len > dataLen)
            {
                Monitor.Exit(this);
                Debug.LogError("");
                throw new Exception("data in ringbuffer is not enough");
            }
            if (readPos + len < buffer.Length)
            {
                Array.Copy(buffer, readPos, desBuffer, offset, len);
                readPos += len;
                dataLen -= len;
            }
            else
            {
                int readLen = buffer.Length - readPos;
                Array.Copy(buffer, readPos, desBuffer, offset, readLen);
                Array.Copy(buffer, 0, desBuffer, offset + readLen, len - readLen);
                readPos = len - readLen;
                dataLen -= len;
            }
            Monitor.Exit(this);
            return len;
        }
    }
}