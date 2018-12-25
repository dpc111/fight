using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class TowerCfg : UnitCfg {
    public int Id = 0;
    public Fix CreateCd = Fix.fix0;
}

public class TowerFactory {
    public static Dictionary<int, TowerCfg> towerCfgs = new Dictionary<int, TowerCfg>();

    public static void Init() {
        JObject cfgs = ConfigMgr.GetJObject("tower");
        if (cfgs == null) {
            return;
        }
        foreach (JToken cfgt in cfgs.Values()) {
            JObject cfg = cfgt.ToObject<JObject>();
            TowerCfg tower = new TowerCfg();
            tower.Hp = (Fix)(int)cfg["Hp"];
            tower.Armor = Fix.fix0;
            tower.MoveSpeed = Fix.fix0;
            tower.AttackCd = (Fix)(int)cfg["AttackCd"];
            tower.AttackRange = (Fix)(int)cfg["AttackRange"];
            tower.AttackDamage = (Fix)(int)cfg["AttackDamage"];
            tower.AttackNum = (Fix)(int)cfg["AttackNum"];
            tower.BlockRange = (Fix)(int)cfg["BlockRange"];
            tower.Prefab = (string)cfg["Prefab"];
            tower.SkillId = (int)cfg["SkillId"];
            tower.Id = (int)cfg["Id"];
            tower.CreateCd = (Fix)(int)cfg["CreateCd"];
            towerCfgs[tower.Id] = tower;
        }
    }

    public static TowerCfg GetCfg(int id) {
        if (!towerCfgs.ContainsKey(id)) {
            return null;
        }
        return towerCfgs[id];
    }

    public static TowerBase Create(int id, UnitProperty pro) {
        TowerCfg cfg = GetCfg(id);
        if (cfg == null) {
            return null;
        }
        TowerBase tower = new TowerBase();
        if (tower == null) {
            return null;
        }
        tower.Init(cfg, pro);
        GameApp.towerMgr.Add(tower);
        return tower;
    }
}
