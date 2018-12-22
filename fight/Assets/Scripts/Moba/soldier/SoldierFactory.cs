using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SoldierCfg : UnitCfg {
    public int Id = 0;
    public int Type = 0;
}

public class SoldierFactory {
    public static Dictionary<int, SoldierCfg> soldierCfgs = new Dictionary<int, SoldierCfg>();

    public static void Init() {
        JToken cfgs = ConfigMgr.GetJObject("soldier");
        if (cfgs == null) {
            return;
        }
        foreach (JToken cfgt in cfgs.Values()) {
            JObject cfg = cfgt.ToObject<JObject>();
            SoldierCfg soldier = new SoldierCfg();
            soldier.Hp = (Fix)(int)cfg["Hp"];
            soldier.Armor = Fix.fix0;
            soldier.MoveSpeed = (Fix)(int)cfg["MoveSpeed"];
            soldier.AttackCd = (Fix)(int)cfg["AttackCd"];
            soldier.AttackRange = (Fix)(int)cfg["AttackRange"];
            soldier.AttackDamage = (Fix)(int)cfg["AttackDamage"];
            soldier.AttackNum = (Fix)(int)cfg["AttackNum"];
            soldier.BlockRange = (Fix)(int)cfg["BlockRange"];
            soldier.Prefab = (string)cfg["Prefab"];
            soldier.SkillId = (int)cfg["SkillId"];
            soldier.Id = (int)cfg["Id"];
            soldier.Type = (int)cfg["Type"];
            soldierCfgs[soldier.Id] = soldier;
        }
    }

    public static SoldierCfg GetCfg(int id) {
        if (!soldierCfgs.ContainsKey(id)) {
            return null;
        }
        return soldierCfgs[id];
    }

    public static SoldierBase Create(int id, FixVector3 pos) {
        SoldierCfg cfg = GetCfg(id);
        if (cfg == null) {
            return null;
        }
        SoldierBase soldier = new SoldierBase();
        soldier.Init(cfg, pos);
        GameApp.soldierMgr.Add(soldier);
        GameApp.liveMgr.Add(soldier);
        return soldier;
    }
}
