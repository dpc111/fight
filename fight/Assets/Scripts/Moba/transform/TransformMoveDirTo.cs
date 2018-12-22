using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMoveDirTo : TransformMoveBase {
    public FixVector2 mPosTarget = new FixVector2();
    public FixVector2 mPosStart = new FixVector2();
    public Fix mTimeMove = Fix.fix0;
    public Fix mTimePass = Fix.fix0;

    public override void Update() {
        mTimePass += GameApp.timeFrame;
        Fix timeScale = mTimePass / mTimeMove;
        if (timeScale >= (Fix)1) {
            mTransform.mPos = mPosTarget;
            mTransform.Move = false;
            return;
        }
        mTransform.mPos = mPosStart + (mPosTarget - mPosStart) * timeScale;
    }

    public void Move(FixVector2 pos) {
        mPosTarget = pos;
        mPosStart = mTransform.mPos;
        mTimeMove = FixVector2.Distance(mPosStart, mPosTarget) / mTransform.mSpeed;
        mTimePass = Fix.fix0;
        if (mTimeMove <= Fix.fix0) {
            mTransform.Move = false;
        } else {
            mTransform.Move = true;
        }
    }
}
