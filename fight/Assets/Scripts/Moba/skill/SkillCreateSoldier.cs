﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCreateSoldier : SkillBase {
    public override void Trigger(UnitBase tar) {
        base.Trigger(tar);
        UnitProperty pro = new UnitProperty();
        pro.Camp = mUnitTri.Camp;
        pro.Pos = mUnitTri.mTransform.Pos + (new FixVector3(mUnitTri.mTransform.mBlock.mBlockRange, Fix.fix0, Fix.fix0) * GameTool.CampDir(mUnitTri.Camp).x);
        if (pro.Camp == GameDefine.CampLeft) {
            pro.Rol = new FixVector3(0, 0, 0);
        } else if (pro.Camp == GameDefine.CampRight) {
            pro.Rol = new FixVector3(0, 180, 0);
        }
        SoldierBase soldier = SoldierFactory.Create(mCfg.SoldierId, pro);
        Fix damage = mUnitTri.GetAttr(GameDefine.AttrTypeAttackDamage);
        soldier.mAttr.SetAttr(GameDefine.AttrTypeAttackDamage, damage);
    }
}
