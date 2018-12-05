using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerShoot : TowerBase
{
    public int mBulletId = 0;

    public virtual void Init(TowerCfg cfg)
    {
        base.Init(cfg);
        mBulletId = cfg.bullet_id;
    }

    public virtual void Attack()
    {
        BulletBase bullet = BulletFactory.CreateBullet(mBulletId);
        bullet.mUnitTri = this;
        bullet.mUnitTar = null;
    }

    public virtual void Update()
    {
        base.Update();
    }
}
