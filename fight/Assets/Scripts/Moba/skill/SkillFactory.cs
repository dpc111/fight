using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class SkillCfg {
    public int Id = 0;
    public int Type = 0;
    public int IsAutoRelease = 0;
    public int SkillTar = 0;
    public int SkillAutoFindTarDir = 0;
    public int SkillAutoFindTarPos = 0;
    public int SkillAutoFindTarUnit = 0;
    public int CampGroup = 0;
    public int BulletId = 0;
    public int SoldierId = 0;
    public int BuffId = 0;
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
            skill.IsAutoRelease = (int)cfg["IsAutoRelease"];
            skill.SkillTar = (int)cfg["SkillTar"];
            skill.SkillAutoFindTarDir = (int)cfg["SkillAutoFindTarDir"];
            skill.SkillAutoFindTarPos = (int)cfg["SkillAutoFindTarPos"];
            skill.SkillAutoFindTarUnit = (int)cfg["SkillAutoFindTarUnit"];
            skill.CampGroup = (int)cfg["CampGroup"];
            skill.BulletId = (int)cfg["BulletId"];
            skill.SoldierId = (int)cfg["SoldierId"];
            skill.BuffId = (int)cfg["BuffId"];
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
