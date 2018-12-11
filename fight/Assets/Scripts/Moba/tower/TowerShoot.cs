using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : TowerBase
{
    public int mBulletId = 0;

    public override void Init(TowerCfg cfg, FixVector3 pos)
    {
        base.Init(cfg, pos);
        mBulletId = cfg.bullet_id;
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Attack()
    {
        BulletBase bullet = BulletFactory.CreateBullet(mBulletId, mTransform.Pos);
        bullet.mUnitTri = this;
        bullet.mUnitTar = null;
        Fix damage = mAttr.GetAttr(UnitAttrType.AttDamage);
        bullet.mAttr.SetAttr(UnitAttrType.AttDamage, damage);
    }
}
