using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPathBoomerang : BulletPathBase {
    public FixVector3 mBackPos;

    public override void OnMoveStop() {
        base.OnMoveStop();
        if (mUnitTri == null) {
            return;
        }
        if (GameTool.WhichPosNearer(mTransform.Pos, mBackPos, mUnitTri.mTransform.Pos) == 1) {
            mTransform.MoveDirTo(mTransform.Pos);
        } else {
            Kill = true;
        }
    }

    public void MoveTo(FixVector3 pos) {
        mBackPos = pos;
        mTransform.MoveDirTo(mBackPos);
    }
}
