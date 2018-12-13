using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TowerCfg : UnitCfg
{
    public int id = 0;
    public int type = 0;
    public int bullet_id = 0;
    public int soldier_id = 0;
    public int attack_num = 0;
}

public class TowerFactory 
{
    public static Dictionary<int, TowerCfg> towerCfgs = new Dictionary<int, TowerCfg>();

    public static void Init()
    {
        JObject cfgs = ConfigMgr.GetJObject("tower");
        if (cfgs == null)
            return;
        foreach (JToken cfgt in cfgs.Values())
        {
            JObject cfg = cfgt.ToObject<JObject>();
            TowerCfg tower = new TowerCfg();
            tower.hp = (Fix)(int)cfg["hp"];
            tower.armor = Fix.fix0;
            tower.moveType = (int)MoveType.Stand;
            tower.moveSpeed = Fix.fix0;
            tower.attCd = (Fix)(int)cfg["attack_cd"];
            tower.attRange = (Fix)(int)cfg["attack_range"];
            tower.attDamage = (Fix)(int)cfg["attack_damage"];
            tower.blockRange = (Fix)(int)cfg["block_range"];
            tower.prefab = (string)cfg["prefab"];
         
            tower.id = (int)cfg["id"];
            tower.type = (int)cfg["type"];
            tower.bullet_id = (int)cfg["bullet_id"];
            tower.soldier_id = (int)cfg["soldier_id"];
            tower.attack_num = (int)cfg["attack_num"];
            towerCfgs[tower.id] = tower;
        }
    }

    public static TowerCfg GetCfg(int id)
    {
        if (!towerCfgs.ContainsKey(id))
            return null;
        return towerCfgs[id];
    }

    public static TowerBase Create(int id, FixVector3 pos)
    {
        TowerCfg cfg = GetCfg(id);
        if (cfg == null)
            return null;
        TowerBase tower = null;
        if (cfg.type == (int)TowerType.Shoot)
            tower = new TowerShoot();
        else if (cfg.type == (int)TowerType.Soldier)
            tower = new TowerSoldier();
        if (tower == null)
            return null;
        tower.Init(cfg, pos);
        GameData.towerMgr.Add(tower);
        return tower;
    }
}
