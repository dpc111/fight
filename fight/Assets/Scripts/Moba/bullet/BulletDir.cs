using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletDir : BulletBase {
    public override void Update() {
        base.Update();
        UnitBase unit = FightTool.FindBulletHit(this);
        if (unit != null) {
            OnHit(unit);
        }
    }
}

