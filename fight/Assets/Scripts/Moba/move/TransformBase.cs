using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformBase 
{
    public int[] mBlockShape = new int[AStar.objectBlockLen];
    public int[] mBlockDynamic = new int[AStar.objectBlockLen];
    public int mBlockLen = 0;

    public FixVector2 mPos = new FixVector2();
    public FixVector2 mDir = new FixVector2();
    public Fix mSpeed = Fix.fixZero;
    public bool mIsMove = false;

    public bool Move { get {return mIsMove; } set { mIsMove = value; } }
    public virtual void MoveToTarget(FixVector2 target) { }
    public virtual void MoveDir(FixVector2 dir) { }
    public virtual void MoveLock(TransformBase objLock) { }

    public virtual void Init(FixVector2 pos, Fix blockR, Fix speed)
    {
        mPos = pos;
        mSpeed = speed;
        Move = false;
        if (blockR != Fix.fixZero)
        {
            SetBlockShap(blockR);
            SetBlockDynamic();
        }
        else
        {
            mBlockLen = 0;
        }
    }

    public virtual void UpdateLogic()
    {
        SetBlockDynamic();
        GameData.transformMgr.mAstar.SetBlockDynamic(ref mBlockDynamic, mBlockLen);
    }

    public void SetBlockShap(Fix r)
    {
        Fix w = GameData.transformMgr.mAstar.mWidth;
        Fix x = Fix.fixZero;
        Fix y = Fix.fixZero;
        int xNum = GameData.transformMgr.mAstar.mX;
        int n = (int)(r / w);
        int index = 0;
        mBlockLen = 0;
        for (int i = -n - 1; i <= n; ++i)
        {
            for (int j = -n - 1; j <= n; ++j)
            {
                x = (i + (Fix)1/(Fix)2) * w;
                y = (j + (Fix)1/(Fix)2) * w;
                if (x * x + y * y <= r * r)
                {
                    index = j * xNum + i;
                    mBlockShape[mBlockLen] = index;
                    mBlockLen++;
                }
            }
        }
    }

    public void SetBlockDynamic()
    {
        if (mBlockLen == 0)
            return;
        int indexCur = GameData.transformMgr.mAstar.ToGridIndex(mPos);
        for (int i = 0; i < mBlockLen; i++)
        {
            mBlockDynamic[i] = indexCur + mBlockShape[i];
        }
    }
}
