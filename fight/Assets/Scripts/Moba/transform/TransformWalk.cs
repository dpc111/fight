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

    public override void Init(FixVector3 pos, Fix blockRange, Fix speed)
    {
        base.Init(pos, blockRange, speed);
        mPathLen = 0;
        mPathIndex = 0;
        SetNextStep();
    }

    public override void Update()
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

    public override void MoveToTarget(FixVector2 target)
    {
        mPathLen = 0;
        mPathIndex = 0;
        mPosTarget = target;
        int begin = GameData.transformMgr.mAstar.ToGridIndex(mPos);
        int end = GameData.transformMgr.mAstar.ToGridIndex(mPosTarget);
        bool ok = GameData.transformMgr.mAstar.FindPath(begin, end, ref mPath, ref mPathLen);
        if (!ok)
        {
            Debug.LogError("");
            return;
        }
        if (SetNextStep())
            Move = true;
    }

    private bool SetNextStep()
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
}
