﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShoot : SkillBase {
    public override void Trigger(UnitBase tar) {
        base.Trigger(tar);
        if (tar == null) {
            UnitProperty pro = new UnitProperty();
            pro.Camp = mUnitTri.Camp;
            pro.Pos = mUnitTri.mTransform.Pos + mUnitTri.mTransform.RolMat * (new FixVector3(2, 4, 0));
            BulletBase bullet = BulletFactory.Create(mCfg.BulletId, pro);
            bullet.UnitTri = mUnitTri;
            bullet.UnitTar = null;
            Fix damage = mUnitTri.GetAttr(GameDefine.AttrTypeAttackDamage);
            bullet.mAttr.SetAttr(GameDefine.AttrTypeAttackDamage, damage);
            bullet.mTransform.MoveDir(GameTool.CampDir(bullet.Camp));
        } else {
            UnitProperty pro = new UnitProperty();
            pro.Camp = mUnitTri.Camp;
            pro.Pos = mUnitTri.mTransform.Pos + mUnitTri.mTransform.RolMat * (new FixVector3(1, 3, 0));
            BulletBase bullet = BulletFactory.Create(mCfg.BulletId, pro);
            bullet.UnitTri = mUnitTri;
            bullet.UnitTar = tar;
            Fix damage = mUnitTri.GetAttr(GameDefine.AttrTypeAttackDamage);
            bullet.mAttr.SetAttr(GameDefine.AttrTypeAttackDamage, damage);
            bullet.mTransform.MoveLock(tar.mTransform);
        }
    }

    public override void Trigger(FixVector3 vec) {
        base.Trigger(vec);
        UnitProperty pro = new UnitProperty();
        pro.Camp = mUnitTri.Camp;
        pro.Pos = mUnitTri.mTransform.Pos + mUnitTri.mTransform.RolMat * (new FixVector3(2, 4, 0));
        BulletBase bullet = BulletFactory.Create(mCfg.BulletId, pro);
        bullet.UnitTri = mUnitTri;
        bullet.UnitTar = null;
        Fix damage = mUnitTri.GetAttr(GameDefine.AttrTypeAttackDamage);
        bullet.mAttr.SetAttr(GameDefine.AttrTypeAttackDamage, damage);
        bullet.Move(vec);
    }
}
