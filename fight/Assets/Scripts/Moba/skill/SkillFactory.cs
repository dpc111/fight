using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SkillCfg {
    public int Id = 0;
    public int Type = 0;
    public int MoveType = 0;
    public int BulletId = 0;
    public int SoldierId = 0;
}

public class SkillFactory {
    private static Dictionary<int, SkillCfg> skillCfgs = new Dictionary<int, SkillCfg>();

    public static void Init() {
        JToken cfgs = ConfigMgr.GetJObject("skill");
        if (cfgs == null) {
            return;
        }
        foreach (JToken cfgt in cfgs.Values()) {
            JObject cfg = cfgt.ToObject<JObject>();
            SkillCfg skill = new SkillCfg();
            skill.Id = (int)cfg["Id"];
            skill.Type = (int)cfg["Type"];
            skill.MoveType = (int)cfg["MoveType"];
            skill.BulletId = (int)cfg["BulletId"];
            skill.SoldierId = (int)cfg["SoldierId"];
            skillCfgs[skill.Id] = skill;
            Debug.Log(skill.Id);
        }
    }

    public static SkillCfg GetCfg(int id) {
        if (!skillCfgs.ContainsKey(id)) {
            return null;
        }
        return skillCfgs[id];
    }

    public static SkillBase Create(int id, UnitBase unit) {
        SkillCfg cfg = GetCfg(id);
        if (cfg == null) {
            Debug.LogError("11" + id);
            return null;
        }
        SkillBase skill = null;
        if (cfg.Type == (int)SkillType.Shoot) {
            skill = new SkillShoot();
        } else if (cfg.Type == (int)SkillType.CreateSoldier) {
            skill = new SkillCreateSoldier();
        } else if (cfg.Type == (int)SkillType.Aoe) {
            skill = new SkillAoe();
        }
        if (skill == null) {
            Debug.LogError("11");
            return null;
        }
        skill.Init(cfg, unit);
        return skill;
    }
}
