using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDir : BulletBase {
    public override void Move(FixVector3 vec) {
        mTransform.MoveDir(vec);
    }
}

