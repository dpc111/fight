using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TowerCfg
{
    public int id;
    public int type;
    public string prefab;
    public int bullet_id;
    public int soldier_id;
}

public class TowerFactory 
{
    public static Dictionary<int, TowerCfg> towerCfgs = new Dictionary<int, TowerCfg>();

    public static void Init()
    {
        JToken cfgs = ConfigMgr.GetJObject("tower");
        if (cfgs == null)
            return;
        foreach (JToken cfg in cfgs.Children())
        {
            TowerCfg tower = new TowerCfg();
            tower.id = (int)cfg["id"];
            tower.type = (int)cfg["type"];
            tower.prefab = (string)cfg["prefab"];
            tower.bullet_id = (int)cfg["bullet_id"];
            tower.soldier_id = (int)cfg["soldier_id"];
            towerCfgs[tower.id] = tower;
        }
    }

    public static TowerCfg GetCfg(int id)
    {
        if (!towerCfgs.ContainsKey(id))
            return null;
        return towerCfgs[id];
    }

    public static TowerBase CreateTower(int id)
    {
        TowerCfg cfg = GetCfg(id);
        if (cfg == null)
            return null;
        TowerBase tower = new TowerBase();
        tower.Init(cfg);
        GameData.towerMgr.AddTower(tower);
        return tower;
    }

    public static void RemoveTower(TowerBase tower) 
    {
        GameData.towerMgr.RemoveTower(tower);
    }
}
