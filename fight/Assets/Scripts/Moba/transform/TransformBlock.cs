using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBlock {
    public TransformBase mTransform = null;
    public int[] mBlockShape = new int[AStar.objectBlockLen];
    public int[] mBlockDynamic = new int[AStar.objectBlockLen];
    public int mBlockLen = 0;
    public Fix mBlockRange = Fix.fix0;
    public int mBoxLen = 0;
    public Fix BlockRange { get { return mBlockRange; } set { mBlockRange = value; } }

    public void Init(TransformBase tran, Fix blockRange) {
        mTransform = tran;
        mBlockLen = 0;
        mBlockRange = blockRange;
        if (mBlockRange <= Fix.fix0) {
            return;
        }
        SetBlockShap();
        SetBlockDynamic();
    }

    public void SetBlockShap() {
        Fix w = GameApp.transformMgr.mAstar.mWidth;
        Fix x = Fix.fix0;
        Fix y = Fix.fix0;
        int xNum = GameApp.transformMgr.mAstar.mX;
        int n = (int)(mBlockRange / w);
        mBlockLen = 0;
        mBoxLen = n;
        for (int i = -n - 1; i <= n + 1; ++i) {
            for (int j = -n - 1; j <= n + 1; ++j) {
                x = i * w;
                y = j * w;
                if (x * x + y * y <= mBlockRange * mBlockRange) {
                    mBlockShape[mBlockLen] = j * xNum + i;
                    mBlockLen++;
                }
            }
        }
    }

    public void SetBlockDynamic() {
        if (mBlockLen == 0) {
            return;
        }
        int indexCur = GameApp.transformMgr.mAstar.ToGridIndex(mTransform.mPos);
        for (int i = 0; i < mBlockLen; i++) {
            mBlockDynamic[i] = indexCur + mBlockShape[i];
        }
    }

    public bool DynamicHasIndex(int index) {
        for (int i = 0; i < mBlockLen; i++) {
            if (mBlockDynamic[i] == index) {
                return true;
            }
        }
        return false;
    }
}
