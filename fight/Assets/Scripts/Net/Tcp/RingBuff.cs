using System;
using UnityEngine;
using System.Threading;

namespace Net {
    public class RingBuffer1 {
        private byte[] mBuffer;
        private int mPosWrite;
        private int mPosRead;
        private int mDataLen;

        public RingBuffer1(int size) {
            mPosWrite = 0;
            mPosRead = 0;
            mDataLen = 0;
            mBuffer = new byte[size];
        }

        public void Reset() {
            mPosWrite = 0;
            mPosRead = 0;
            mDataLen = 0;
        }

        public int GetDataLen() {
            int len = mDataLen;
            return len;
        }

        public int GetResLen() {
            int len = mBuffer.Length - mDataLen;
            return len;
        }

        public void Write(byte[] srcBuffer, int offset, int len) {
            int resLen = mBuffer.Length - mDataLen;
            if (resLen < len) {
                return;
            }
            if (mPosWrite + len < mBuffer.Length) {
                Array.Copy(srcBuffer, offset, mBuffer, mPosWrite, len);
                mPosWrite += len;
                mDataLen += len;
            } else {
                int writeLen = mBuffer.Length - mPosWrite;
                Array.Copy(srcBuffer, offset, mBuffer, mPosWrite, writeLen);
                Array.Copy(srcBuffer, offset + writeLen, mBuffer, 0, len - writeLen);
                mPosWrite = len - writeLen;
                mDataLen += len;
            }
        }

        public void Read(byte[] desBuffer, int offset, int len) {
            if (len > mDataLen) {
                return;
            }
            if (mPosRead + len < mBuffer.Length) {
                Array.Copy(mBuffer, mPosRead, desBuffer, offset, len);
                mPosRead += len;
                mDataLen -= len;
            } else {
                int readLen = mBuffer.Length - mPosRead;
                Array.Copy(mBuffer, mPosRead, desBuffer, offset, readLen);
                Array.Copy(mBuffer, 0, desBuffer, offset + readLen, len - readLen);
                mPosRead = len - readLen;
                mDataLen -= len;
            }
        }

        public int ReadAll(byte[] desBuffer, int offset) {
            int len = mDataLen;
            if (len > mDataLen) {
                return -1;
            }
            if (mPosRead + len < mBuffer.Length) {
                Array.Copy(mBuffer, mPosRead, desBuffer, offset, len);
                mPosRead += len;
                mDataLen -= len;
            } else {
                int readLen = mBuffer.Length - mPosRead;
                Array.Copy(mBuffer, mPosRead, desBuffer, offset, readLen);
                Array.Copy(mBuffer, 0, desBuffer, offset + readLen, len - readLen);
                mPosRead = len - readLen;
                mDataLen -= len;
            }
            return len;
        }
    }
}