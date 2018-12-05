using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformWalk : TransformBase
{
    public int[] mPath = new int[AStar.findPathLen];
    public int mPathLen = 0;
    public int mPathIndex = 0;

    public FixVector2 mPosTarget = new FixVector2();
    public FixVector2 mPosStart = new FixVector2();
    public FixVector2 mPosEnd = new FixVector2();
    public Fix mTimeMove = Fix.fix0;
    public Fix mTimePass = Fix.fix0;

    public virtual void Init(FixVector2 pos, Fix blockR, Fix speed)
    {
        base.Init(pos, blockR, speed);
        mPathLen = 0;
        mPathIndex = 0;
        SetNextStep();
    }

    public virtual void Update()
    {
        base.Update();
        if (!Move)
            return;
        mTimePass += GameData.timeFrame;
        Fix timeScale = mTimePass / mTimeMove;
        if (timeScale >= (Fix)1)
        {
            timeScale = (Fix)1;
            mPos = mPosEnd;
            if (!SetNextStep())
                Move = false;
            return;
        }
        mPos = mPosStart + (mPosEnd - mPosStart) * timeScale;
    }

    public bool SetNextStep()
    {
        if (mPathIndex >= mPathLen)
            return false;
        mPosStart = GameData.transformMgr.mAstar.ToCenterPos(mPath[mPathIndex]);
        ++mPathIndex;
        if (mPathIndex >= mPathLen)
            return false;
        mPosEnd = GameData.transformMgr.mAstar.ToCenterPos(mPath[mPathIndex]);
        mTimeMove = FixVector2.Distance(mPosStart, mPosEnd) / mSpeed;
        mTimePass = Fix.fix0;
        mPos = mPosStart;
        mDir = mPosEnd - mPosStart;
        mDir.Normalize();
        return true;
    }

    public virtual void MoveToTarget(FixVector2 target)
    {
        mPathLen = 0;
        mPathIndex = 0;
        mPosTarget = target;
        int begin = GameData.transformMgr.mAstar.ToGridIndex(mPos);
        int end = GameData.transformMgr.mAstar.ToGridIndex(mPosTarget);
        GameData.transformMgr.mAstar.FindPath(begin, end, ref mPath, ref mPathLen);
        if (SetNextStep())
            Move = true;
    }
}
