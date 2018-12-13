using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SoldierCfg : UnitCfg
{
    public int id = 0;
    public int type = 0;
    public int bullet_id = 0;
}

public class SoldierFactory
{
    public static Dictionary<int, SoldierCfg> soldierCfgs = new Dictionary<int, SoldierCfg>();

    public static void Init()
    {
        JToken cfgs = ConfigMgr.GetJObject("soldier");
        if (cfgs == null)
            return;
        foreach (JToken cfgt in cfgs.Values())
        {
            JObject cfg = cfgt.ToObject<JObject>();
            SoldierCfg soldier = new SoldierCfg();
            soldier.hp = (Fix)(int)cfg["hp"];
            soldier.armor = Fix.fix0;
            soldier.moveType = (int)cfg["move_type"];
            soldier.moveSpeed = (Fix)(int)cfg["move_speed"];
            soldier.attCd = (Fix)(int)cfg["attack_cd"];
            soldier.attRange = (Fix)(int)cfg["attack_range"];
            soldier.attDamage = (Fix)(int)cfg["attack_damage"];
            soldier.blockRange = (Fix)(int)cfg["block_range"];
            soldier.prefab = (string)cfg["prefab"];

            soldier.id = (int)cfg["id"];
            soldier.type = (int)cfg["type"];
            soldier.bullet_id = (int)cfg["bullet_id"];
            soldierCfgs[soldier.id] = soldier;
        }
    }

    public static SoldierCfg GetCfg(int id)
    {
        if (!soldierCfgs.ContainsKey(id))
            return null;
        return soldierCfgs[id];
    }

    public static SoldierBase Create(int id, FixVector3 pos)
    {
        SoldierCfg cfg = GetCfg(id);
        if (cfg == null)
            return null;
        SoldierBase soldier = new SoldierBase();
        soldier.Init(cfg, pos);
        GameData.soldierMgr.Add(soldier);
        return soldier;
    }
}
