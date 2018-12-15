using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCreateSoldier : SkillBase {
    public override void Trigger() {
        SoldierBase soldier = SoldierFactory.Create(mCfg.SoldierId, mUnitTri.mTransform.Pos);
        Fix damage = mUnitTri.GetAttr(UnitAttrType.AttackDamage);
        soldier.mAttr.SetAttr(UnitAttrType.AttackDamage, damage);
        soldier.Camp = mUnitTri.Camp;
    }
}
