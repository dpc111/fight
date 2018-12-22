using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCreateSoldier : SkillBase {
    public override void Trigger() {
        base.Trigger();
        SoldierBase soldier = SoldierFactory.Create(mCfg.SoldierId, 
            mUnitTri.mTransform.Pos + (new FixVector3(mUnitTri.mTransform.mBlock.mBlockRange, Fix.fix0, Fix.fix0) * GameTool.CampDir(mUnitTri.Camp).x));
        Fix damage = mUnitTri.GetAttr(GameDefine.AttrTypeAttackDamage);
        soldier.mAttr.SetAttr(GameDefine.AttrTypeAttackDamage, damage);
        soldier.Camp = mUnitTri.Camp;
    }
}
