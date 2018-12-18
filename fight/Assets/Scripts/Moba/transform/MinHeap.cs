using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinHeap {
    public GridPos[] mData;
    public int mLen;
    public int mCur;

    public MinHeap(int len) {
        mLen = len;
        mCur = 0;
        mData = new GridPos[mLen];
    }

    public void Clear() {
        mCur = 0;
    }

    public void Exchange(int pos1, int pos2) {
        GridPos tmp = mData[pos1];
        mData[pos1] = mData[pos2];
        mData[pos2] = tmp;
    }

    public void Push(GridPos data) {
        if (mCur >= mLen)
            return;
        mCur++;
        mData[mCur] = data;
        if (mCur <= 1)
            return;
        int pos1 = mCur;
        while (true) {
            int pos2 = pos1 / 2;
            if (pos2 <= 0) {
                break;
            }
            if (mData[pos2].GetValue() > mData[pos1].GetValue()) {
                Exchange(pos2, pos1);
                pos1 = pos2;
                if (pos1 <= 1) {
                    break;
                }
            } else {
                break;
            }
        }
    }

    public GridPos Pop() {
        if (mCur <= 0) {
            return null;
        }
        GridPos value = mData[1];
        if (mCur <= 1) {
            mData[1] = null;
            mCur--;
            return value;
        }
        mData[1] = mData[mCur];
        mData[mCur] = null;
        mCur--;
        int pos = 1;
        while (true) {
            int pos1 = pos * 2;
            int pos2 = pos * 2 + 1;
            int posChange = 0;
            if (pos1 <= mCur && mData[pos].GetValue() > mData[pos1].GetValue()) {
                posChange = pos1;
            }
            if (pos2 <= mCur && mData[pos].GetValue() > mData[pos2].GetValue()) {
                if (posChange != 0) {
                    if (mData[pos1].GetValue() > mData[pos2].GetValue()) {
                        posChange = pos2;
                    }
                } else {
                    posChange = pos2;
                }
            }
            if (posChange != 0) {
                Exchange(pos, posChange);
                pos = posChange;
            } else {
                break;
            }
        }
        return value;
    }
}
