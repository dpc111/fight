using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BulletCfg : UnitCfg
{
    public int id = 0;
    public int type = 0;
}

public class BulletFactory
{
    private static Dictionary<int, BulletCfg> bulletCfgs = new Dictionary<int, BulletCfg>();

    public static void Init()
    {
        JToken cfgs = ConfigMgr.GetJObject("bullet");
        if (cfgs == null)
            return;
        foreach (JToken cfgt in cfgs.Values())
        {
            JObject cfg = cfgt.ToObject<JObject>();
            BulletCfg bullet = new BulletCfg();
            bullet.hp = Fix.fix0;
            bullet.armor = Fix.fix0;
            bullet.moveType = (int)cfg["move_type"];
            bullet.moveSpeed = (Fix)(int)cfg["move_speed"];
            bullet.attCd = Fix.fix0;
            bullet.attRange = Fix.fix0;
            bullet.attDamage = Fix.fix0;
            bullet.blockRange = Fix.fix0;
            bullet.prefab = (string)cfg["prefab"];

            bullet.id = (int)cfg["id"];
            bullet.type = (int)cfg["type"];
            bulletCfgs[bullet.id] = bullet;
        }
    }

    public static BulletCfg GetCfg(int id)
    {
        if (!bulletCfgs.ContainsKey(id))
            return null;
        return bulletCfgs[id];
    }

    public static BulletBase Create(int id, FixVector3 pos)
    {
        BulletCfg cfg = GetCfg(id);
        if (cfg == null)
            return null;
        BulletBase bullet = null;
        if (cfg.type == (int)BulletType.Lock)
            bullet = new BulletLock();
        else if (cfg.type == (int)BulletType.Dir)
            bullet = new BulletDir();
        if (bullet == null)
            return null;
        bullet.Init(cfg, pos);
        GameData.bulletMgr.Add(bullet);
        return bullet;
    }
}
