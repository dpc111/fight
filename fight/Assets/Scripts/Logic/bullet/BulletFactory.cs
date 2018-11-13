using System.Collections;
using System.Collections.Generic;

public class BulletFactory 
{
    public BaseBullet CreateBullet(LiveObject src, LiveObject dest, FixVector3 srcPos, FixVector3 destPos)
    {
        BaseBullet bullet = new DirectionShootBullet();
        bullet.CreateBody("");
        bullet.InitData(src, dest, srcPos, destPos);
        bullet.Shoot();
        if (bullet != null)
        {
            bullet.UpdateRenderPosition(0);
            bullet.RecordLastPos();
            GameData.listBullet.Add(bullet);
        }
        return bullet;
    }

    void RemoveBullet(BaseBullet bullet)
    {
        GameData.listBullet.Remove(bullet);
    }
}
