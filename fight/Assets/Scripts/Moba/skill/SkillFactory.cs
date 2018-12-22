using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SkillCfg {
    public int Id = 0;
    public int Type = 0;
    public int CampGroupType = 0;
    public int UnitType = 0;
    public int SkillRangeType = 0;
    public int SkillTargetGroup = 0;
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
            skill.CampGroupType = (int)cfg["CampGroupType"];
            skill.UnitType = (int)cfg["UnitType"];
            skill.SkillRangeType = (int)cfg["SkillRangeType"];
            skill.SkillTargetGroup = (int)cfg["SkillTargetGroup"];
            skill.MoveType = (int)cfg["MoveType"];
            skill.BulletId = (int)cfg["BulletId"];
            skill.SoldierId = (int)cfg["SoldierId"];
            skillCfgs[skill.Id] = skill;
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
            Debug.LogError(id);
            return null;
        }
        SkillBase skill = null;
        if (cfg.Type == GameDefine.SkillTypeShoot) {
            skill = new SkillShoot();
        } else if (cfg.Type == GameDefine.SkillTypeCreateSoldier) {
            skill = new SkillCreateSoldier();
        } else if (cfg.Type == GameDefine.SkillTypeAoe) {
            skill = new SkillAoe();
        }
        if (skill == null) {
            return null;
        }
        skill.Init(cfg, unit);
        return skill;
    }
}
