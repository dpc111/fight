using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class BulletCfg : UnitCfg {
    public int Id = 0;
    public int Type = 0;
}

public class BulletFactory {
    private static Dictionary<int, BulletCfg> bulletCfgs = new Dictionary<int, BulletCfg>();

    public static void Init() {
        JToken cfgs = ConfigMgr.GetJObject("bullet");
        if (cfgs == null) {
            return;
        }
        foreach (JToken cfgt in cfgs.Values()) {
            JObject cfg = cfgt.ToObject<JObject>();
            BulletCfg bullet = new BulletCfg();
            bullet.Hp = Fix.fix0;
            bullet.Armor = Fix.fix0;
            bullet.MoveSpeed = (Fix)(int)cfg["MoveSpeed"];
            bullet.AttackCd = Fix.fix0;
            bullet.AttackRange = Fix.fix0;
            bullet.AttackDamage = Fix.fix0;
            bullet.BlockRange = Fix.fix0;
            bullet.Prefab = (string)cfg["Prefab"];
            bullet.Id = (int)cfg["Id"];
            bullet.Type = (int)cfg["Type"];
            bulletCfgs[bullet.Id] = bullet;
        }
    }

    public static BulletCfg GetCfg(int id) {
        if (!bulletCfgs.ContainsKey(id)) {
            return null;
        }
        return bulletCfgs[id];
    }

    public static BulletBase Create(int id, FixVector3 pos) {
        BulletCfg cfg = GetCfg(id);
        if (cfg == null) {
            return null;
        }
        BulletBase bullet = null;
        if (cfg.Type == (int)BulletType.Lock) {
            bullet = new BulletLock();
        } else if (cfg.Type == (int)BulletType.Dir) {
            bullet = new BulletDir();
        }
        if (bullet == null) {
            return null;
        }
        bullet.Init(cfg, pos);
        GameData.bulletMgr.Add(bullet);
        return bullet;
    }
}
