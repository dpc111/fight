using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPathBoomerang : BulletPathBase {
    public override void OnMoveStop() {
        base.OnMoveStop();
        if (UnitTri == null || UnitTri.Kill) {
            Kill = true;
            return;
        }
        if (GameTool.WhichPosNearer(mTransform.Pos, PosTar, UnitTri.mTransform.Pos) == 1) {
            mTransform.MoveDirTo(UnitTri.mTransform.Pos);
        } else {
            Kill = true;
            return;
        }
    }

    public override void Move(FixVector3 vec) {
        PosTar = vec;
        mTransform.MoveDirTo(PosTar);
    }
}
