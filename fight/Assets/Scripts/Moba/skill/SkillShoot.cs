using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShoot : SkillBase {
    public override void Trigger() {
        if (mCfg.MoveType == (int)SkillMoveType.Dir) {
            ShootDir();
        } else if (mCfg.MoveType == (int)SkillMoveType.Lock) {
            ShootLock();
        }
    }

    private void ShootDir() {
        BulletBase bullet = BulletFactory.Create(mCfg.BulletId, mUnitTri.mTransform.Pos + new FixVector3(2, 5, 0));
        bullet.mUnitTri = mUnitTri;
        bullet.mUnitTar = null;
        bullet.Camp = mUnitTri.Camp;
        Fix damage = mUnitTri.GetAttr(UnitAttrType.AttackDamage);
        bullet.mAttr.SetAttr(UnitAttrType.AttackDamage, damage);
        bullet.mTransform.MoveDir(FightTool.CampDir(bullet.Camp));
    }

    private void ShootLock() {
        int tarNum = (int)mUnitTri.GetAttr(UnitAttrType.AttackNum);
        if (tarNum < 0 || tarNum > GameConst.SkillUnitTarMax) {
            return;
        }
        int enemyNum = FightTool.FindEnemySoldierList(mUnitTri, ref mUnitTarList, tarNum);
        if (enemyNum <= 0) {
            return;
        }
        for (int i = 0; i < enemyNum; i++) {
            UnitBase enemy = mUnitTarList[i];
            BulletBase bullet = BulletFactory.Create(mCfg.BulletId, mUnitTri.mTransform.Pos + new FixVector3(2, 5, 0));
            bullet.mUnitTri = mUnitTri;
            bullet.mUnitTar = enemy;
            bullet.Camp = mUnitTri.Camp;
            Fix damage = mUnitTri.GetAttr(UnitAttrType.AttackDamage);
            bullet.mAttr.SetAttr(UnitAttrType.AttackDamage, damage);
            bullet.mTransform.MoveLock(enemy.mTransform);
        }
    }

}
