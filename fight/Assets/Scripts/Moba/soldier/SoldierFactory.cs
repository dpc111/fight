using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SoldierCfg
{
    public int id;
    public int type;
    public string prefab;
    public int bullet_id;
}

public class SoldierFactory
{
    public static Dictionary<int, SoldierCfg> soldierCfgs = new Dictionary<int, SoldierCfg>();

    public static void Init()
    {
        JToken cfgs = ConfigMgr.GetJObject("soldier");
        if (cfgs == null)
            return;
        foreach (JToken cfg in cfgs.Children())
        {
            SoldierCfg soldier = new SoldierCfg();
            soldier.id = (int)cfg["id"];
            soldier.type = (int)cfg["type"];
            soldier.prefab = (string)cfg["prefab"];
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

    public static SoldierBase CreateSoldier(int id)
    {
        SoldierCfg cfg = GetCfg(id);
        if (cfg == null)
            return null;
        SoldierBase soldier = new SoldierBase();
        soldier.Init(cfg);
        GameData.soldierMgr.AddSoldier(soldier);
        return soldier;
    }

    public static void RemoveSoldier(SoldierBase soldier)
    {
        GameData.soldierMgr.RemoveSoldier(soldier);
    }
}
