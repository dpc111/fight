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
    public Fix mSpeed = Fix.fix0;
    public bool mIsMove = false;

    public FixVector3 mGPos = new FixVector3();
    public FixVector3 mGDir = new FixVector3();

    public Fix mBlockRange = Fix.fix0;

    public bool Move { get { return mIsMove; } set { mIsMove = value; } }
    public FixVector3 Pos { get { mGPos.x = mPos.x; mGPos.z = mPos.y; return mGPos; } }
    public FixVector3 Dir { get { mGDir.x = mDir.x; mGDir.z = mDir.y; return mGDir; } }
    public Fix BlockRange { get { return mBlockRange; } set { mBlockRange = value; } }
    public virtual void MoveToTarget(FixVector2 target) { }
    public virtual void MoveDir(FixVector2 dir) { }
    public virtual void MoveLock(TransformBase objLock) { }

    public virtual void Init(FixVector3 pos, Fix blockRange, Fix speed)
    {
        mPos = new FixVector2(pos.x, pos.z);
        mGPos = pos;
        mSpeed = speed;
        Move = false;
        mBlockRange = blockRange;
        if (mBlockRange != Fix.fix0)
        {
            SetBlockShap(mBlockRange);
            SetBlockDynamic();
        }
        else
        {
            mBlockLen = 0;
        }
    }

    public virtual void Update()
    {
        SetBlockDynamic();
        GameData.transformMgr.mAstar.SetBlockDynamic(ref mBlockDynamic, mBlockLen);
    }

    public void SetBlockShap(Fix r)
    {
        Fix w = GameData.transformMgr.mAstar.mWidth;
        Fix x = Fix.fix0;
        Fix y = Fix.fix0;
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

    public bool CheckOutWorld()
    {
        if (mPos.x >= Fix.fix0 && mPos.x <= GameConst.XMax && mPos.y >= Fix.fix0 && mPos.y <= GameConst.ZMax)
            return false;
        return true;
    }

    public static bool CheckHit1(TransformBase t1, TransformBase t2)
    {
        if (t1 == null || t2 == null)
            return false;
        if (FixVector3.SqrMod(t1.Pos - t2.Pos) > Fix.Sqr(t1.BlockRange))
            return false;
        return true;
    }

    public static bool CheckHit2(TransformBase t1, TransformBase t2)
    {
        if (t1 == null || t2 == null)
            return false;
        if (FixVector3.SqrMod(t1.Pos - t2.Pos) > Fix.Sqr(t2.BlockRange))
            return false;
        return true;
    }

    public static bool CheckHit12(TransformBase t1, TransformBase t2)
    {
        if (t1 == null || t2 == null)
            return false;
        if (FixVector3.SqrMod(t1.Pos - t2.Pos) > Fix.Sqr(t1.BlockRange + t2.BlockRange))
            return false;
        return true;
    }
}
