using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerSoldier : TowerBase
{
    public int mSoldierId = 0;

    public override void Init(TowerCfg cfg, FixVector3 pos)
    {
        base.Init(cfg, pos);
        mSoldierId = cfg.soldier_id;
    }

    public override void Update()
    {
        base.Update();
    }

    public override bool Attack()
    {
        SoldierBase soldier = SoldierFactory.Create(mSoldierId, mTransform.Pos);
        Fix damage = mAttr.GetAttr(UnitAttrType.AttDamage);
        soldier.mAttr.SetAttr(UnitAttrType.AttDamage, damage);
        return true;
    }
}
