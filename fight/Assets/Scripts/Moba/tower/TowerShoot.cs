using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : TowerBase
{
    public int mBulletId = 0;
    public int mAttackNum = 0;
    private UnitBase[] mAttackList = new UnitBase[GameConst.TowerAttackNumMax];

    public override void Init(TowerCfg cfg, FixVector3 pos)
    {
        base.Init(cfg, pos);
        mBulletId = cfg.bullet_id;
        mAttackNum = cfg.attack_num;
    }

    public override void Update()
    {
        base.Update();
    }

    public override bool Attack()
    {
        BulletCfg bulletCfg = BulletFactory.GetCfg(mBulletId);
        if (bulletCfg == null)
        {
            return false;
        }
        if (bulletCfg.type == (int)BulletType.Dir)
        {
            return AttackDir();
        }
        else if (bulletCfg.type == (int)BulletType.Lock)
        {
            return AttackLock();
        }
        return true;
    }

    private bool AttackDir()
    {
        BulletBase bullet = BulletFactory.Create(mBulletId, mTransform.Pos + new FixVector3(2, 5, 0));
        bullet.mUnitTri = this;
        bullet.mUnitTar = null;
        Fix damage = mAttr.GetAttr(UnitAttrType.AttDamage);
        bullet.mAttr.SetAttr(UnitAttrType.AttDamage, damage);
        bullet.mTransform.MoveDir(new FixVector2(1, 0));
        return true;
    }

    private bool AttackLock()
    {
        int enemyNum = FightTool.FindEnemySoldierList(this, ref mAttackList, mAttackNum);
        if (enemyNum <= 0)
        {
            return false;
        }
        for (int i = 0; i < enemyNum; i++)
        {
            UnitBase enemy = mAttackList[i];
            BulletBase bullet = BulletFactory.Create(mBulletId, mTransform.Pos + new FixVector3(2, 5, 0));
            bullet.mUnitTri = this;
            bullet.mUnitTar = enemy;
            Fix damage = mAttr.GetAttr(UnitAttrType.AttDamage);
            bullet.mAttr.SetAttr(UnitAttrType.AttDamage, damage);
            bullet.mTransform.MoveLock(enemy.mTransform);
        }
        return true;
    }
}
