using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMoveLock : TransformMoveBase {
    public TransformBase mTransformLock = null;

    public override void Update() {
        mTransform.mDir = mTransformLock.mPos - mTransform.mPos;
        mTransform.mDir.Normalize();
        mTransform.mPos += mTransform.mDir * mTransform.mSpeed * GameConst.TimeFrame;
    }

    public void Move(TransformBase tranLock) {
        if (tranLock == null) {
            mTransform.Move = false;
            return;
        }
        mTransformLock = tranLock;
    }
}
