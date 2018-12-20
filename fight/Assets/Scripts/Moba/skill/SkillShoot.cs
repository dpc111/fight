using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShoot : SkillBase {
    public override void Trigger() {
        BulletBase bullet = BulletFactory.Create(mCfg.BulletId, mUnitTri.mTransform.Pos + new FixVector3(2, 5, 0));
        bullet.mUnitTri = mUnitTri;
        bullet.mUnitTar = null;
        bullet.Camp = mUnitTri.Camp;
        Fix damage = mUnitTri.GetAttr(UnitAttrType.AttackDamage);
        bullet.mAttr.SetAttr(UnitAttrType.AttackDamage, damage);
        bullet.mTransform.MoveDir(FightTool.CampDir(bullet.Camp));
    }

    public override void Trigger(UnitBase tar) {
        BulletBase bullet = BulletFactory.Create(mCfg.BulletId, mUnitTri.mTransform.Pos);
        bullet.mUnitTri = mUnitTri;
        bullet.mUnitTar = tar;
        bullet.Camp = mUnitTri.Camp;
        Fix damage = mUnitTri.GetAttr(UnitAttrType.AttackDamage);
        bullet.mAttr.SetAttr(UnitAttrType.AttackDamage, damage);
        bullet.mTransform.MoveLock(tar.mTransform);
    }
}
