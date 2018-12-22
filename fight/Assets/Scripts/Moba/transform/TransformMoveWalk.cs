using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMoveTarget : TransformMoveBase {
    public int[] mPath = new int[AStar.findPathLen];
    public int mPathLen = 0;
    public int mPathIndex = 0;

    public FixVector2 mPosTarget = new FixVector2();
    public FixVector2 mPosStart = new FixVector2();
    public FixVector2 mPosEnd = new FixVector2();
    public Fix mOverLen = Fix.fix0;
    public Fix mTimeMove = Fix.fix0;
    public Fix mTimePass = Fix.fix0;

    public override void Update() {
        if (mTimeMove == Fix.fix0) {
            return;
        }
        mTimePass += GameApp.timeFrame;
        Fix timeScale = mTimePass / mTimeMove;
        if (timeScale >= (Fix)1) {
            mTransform.mPos = mPosEnd;
            SetNextStep();
            return;
        }
        mTransform.mPos = mPosStart + (mPosEnd - mPosStart) * timeScale;
        FixVector2 pos = mPosStart + (mPosEnd - mPosStart) * timeScale;
        if (GameApp.transformMgr.CheckHit(mTransform, pos)) {
            Move(mPosTarget, mOverLen);
            Debug.LogError("try again");
            return;
        }
        mTransform.mPos = pos;
    }

    public void Move(FixVector2 target, Fix overLen) {
        mPathLen = 0;
        mPathIndex = 0;
        mPosTarget = target;
        mOverLen = overLen;
        int begin = GameApp.transformMgr.mAstar.ToGridIndex(mTransform.mPos);
        int end = GameApp.transformMgr.mAstar.ToGridIndex(mPosTarget);
        GameApp.transformMgr.FindPathReset(mTransform);
        bool ok = GameApp.transformMgr.mAstar.FindPath(begin, end, ref mPath, ref mPathLen, mOverLen);
        if (!ok) {
            mTransform.Move = false;
            GameApp.transformMgr.mAstar.TestShowBlock();
            return;
        }
        SetNextStep();
    }

    private void SetNextStep() {
        if (!mTransform.Move) {
            return;
        }
        if (mPathIndex >= mPathLen) {
            mTransform.Move = false;
            return;
        }
        mPosStart = GameApp.transformMgr.mAstar.ToCenterPos(mPath[mPathIndex]);
        ++mPathIndex;
        if (mPathIndex >= mPathLen) {
            mTransform.Move = false;
            return;
        }
        mPosEnd = GameApp.transformMgr.mAstar.ToCenterPos(mPath[mPathIndex]);
        mTimeMove = FixVector2.Distance(mPosStart, mPosEnd) / mTransform.mSpeed;
        mTimePass = Fix.fix0;
        mTransform.mPos = mPosStart;
        mTransform.mDir = mPosEnd - mPosStart;
        mTransform.mDir.Normalize();
    }
}
