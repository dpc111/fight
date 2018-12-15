using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMoveDir : TransformMoveBase {
    public override void Update() {
        mTransform.mPos += mTransform.mDir * mTransform.mSpeed * GameData.timeFrame;
    }

    public void Move(FixVector2 dir) {
        mTransform.mDir = dir.GetNormalize();
    }
}
