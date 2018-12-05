using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BulletCfg
{
    public int id;
    public int type;
    public string prefab;
}

public class BulletFactory
{
    private static Dictionary<int, BulletCfg> bulletCfgs = new Dictionary<int, BulletCfg>();

    public static void Init()
    {
        JToken cfgs = ConfigMgr.GetJObject("bullet");
        if (cfgs == null)
            return;
        foreach (JToken cfg in cfgs.Children()) 
        {
            BulletCfg bullet = new BulletCfg();
            bullet.id = (int)cfg["id"];
            bullet.type = (int)cfg["type"];
            bullet.prefab = (string)cfg["prefab"];
            bulletCfgs[bullet.id] = bullet;
        }
    }

    public static BulletCfg GetCfg(int id)
    {
        if (!bulletCfgs.ContainsKey(id))
            return null;
        return bulletCfgs[id];
    }

    public static BulletBase CreateBullet(int id)
    {
        BulletCfg cfg = GetCfg(id);
        if (cfg == null)
            return null;
        BulletBase bullet = new BulletBase();
        bullet.Init(cfg);
        GameData.bulletMgr.AddBullet(bullet);
        return bullet;
    }

    public static void RemoveBullet(BulletBase bullet)
    {
        GameData.bulletMgr.RemoveBullet(bullet);
    }
}
