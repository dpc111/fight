using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletLock : BulletBase {
    public override void Update() {
        base.Update();
        if (mUnitTar == null || mUnitTar.Kill) {
            if (!Kill) {
                Kill = true;
            }
            return;
        }
    }
}
