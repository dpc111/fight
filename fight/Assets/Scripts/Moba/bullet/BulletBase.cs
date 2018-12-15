using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : UnitBase {
    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;

    public override void Update() {
        base.Update();
    }

    public virtual void OnHit(UnitBase unitHit) {
        Kill = true;
        Fix damage = mAttr.GetAttr(UnitAttrType.AttackDamage);
        unitHit.mAttr.AddAttr(UnitAttrType.Hp, damage);
    }
}
