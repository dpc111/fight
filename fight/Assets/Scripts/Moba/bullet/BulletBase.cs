using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : UnitBase {
    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;
    public int mBulletType = (int)BulletType.Begin;

    public virtual void Init(BulletCfg cfg, FixVector3 pos) {
        base.Init(cfg, pos);
        mBulletType = cfg.Type;
    }

    public override void Update() {
        base.Update();
    }

    public virtual void OnHit(UnitBase unitHit) {
        Kill = true;
        Fix damage = mAttr.GetAttr(UnitAttrType.AttackDamage);
        unitHit.mAttr.AddAttr(UnitAttrType.Hp, -damage);
    }
}
