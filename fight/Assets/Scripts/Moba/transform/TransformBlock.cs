using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBlock {
    public TransformBase mTransform = null;
    public int[] mBlockShape = new int[AStar.objectBlockLen];
    public int[] mBlockDynamic = new int[AStar.objectBlockLen];
    public int mBlockLen = 0;
    public Fix mBlockRange = Fix.fix0;
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

    public void Update() {
        SetBlockDynamic();
        GameData.transformMgr.mAstar.SetBlockDynamic(ref mBlockDynamic, mBlockLen);
    }

    public void SetBlockShap() {
        Fix w = GameData.transformMgr.mAstar.mWidth;
        Fix x = Fix.fix0;
        Fix y = Fix.fix0;
        int xNum = GameData.transformMgr.mAstar.mX;
        int n = (int)(mBlockRange / w);
        int index = 0;
        mBlockLen = 0;
        for (int i = -n - 1; i <= n; ++i) {
            for (int j = -n - 1; j <= n; ++j) {
                x = (i + (Fix)1 / (Fix)2) * w;
                y = (j + (Fix)1 / (Fix)2) * w;
                if (x * x + y * y <= mBlockRange * mBlockRange) {
                    index = j * xNum + i;
                    mBlockShape[mBlockLen] = index;
                    mBlockLen++;
                }
            }
        }
    }

    public void SetBlockDynamic() {
        if (mBlockLen == 0) {
            return;
        }
        int indexCur = GameData.transformMgr.mAstar.ToGridIndex(mTransform.mPos);
        for (int i = 0; i < mBlockLen; i++) {
            mBlockDynamic[i] = indexCur + mBlockShape[i];
        }
    }
}
