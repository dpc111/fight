using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCreateSoldier : SkillBase {
    public override void Trigger() {
        SoldierBase soldier = SoldierFactory.Create(mCfg.SoldierId, 
            mUnitTri.mTransform.Pos + (new FixVector3(mUnitTri.mTransform.mBlock.mBlockRange, Fix.fix0, Fix.fix0) * FightTool.CampDir(mUnitTri.Camp).x));
        Fix damage = mUnitTri.GetAttr(UnitAttrType.AttackDamage);
        soldier.mAttr.SetAttr(UnitAttrType.AttackDamage, damage);
        soldier.Camp = mUnitTri.Camp;
    }
}
