using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMgr
{
    public List<BulletBase> mBullets = new List<BulletBase>();

    public void Init()
    {
        mBullets.Clear();
    }

    public void AddBullet(BulletBase bullet)
    {
        if (mBullets.Contains(bullet))
            return;
        mBullets.Add(bullet);
    }

    public void RemoveBullet(BulletBase bullet)
    {
        if (!mBullets.Contains(bullet))
            return;
        mBullets.Remove(bullet);
    }

    public void Update()
    {
        for (int i = 0; i < mBullets.Count; i++)
        {
            BulletBase bullet = mBullets[i];
            bullet.Update();
        }
    }
}
