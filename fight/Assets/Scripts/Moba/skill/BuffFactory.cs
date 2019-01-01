using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class BuffCfg {
    public int Id = 0;
    public int Type = 0;
    public int BuffAdd = 0;
    public int BuffUpdate = 0;
    public Fix TimeLife = Fix.fix0;
    public Fix TimeCd = Fix.fix0;
    public int[] MulEffectType = new int[GameConst.BuffCacSize];
    public Fix[] MulEffectValue = new Fix[GameConst.BuffCacSize];
    public int MulEffectNum = 0;
    public int[] AddEffectType = new int[GameConst.BuffCacSize];
    public Fix[] AddEffectValue = new Fix[GameConst.BuffCacSize];
    public int AddEffectNum = 0;
}

public class BuffFactory {
    private static Dictionary<int, BuffCfg> buffCfgs = new Dictionary<int, BuffCfg>();

    public static void Init() {
        JToken cfgs = ConfigMgr.GetJObject("buff");
        if (cfgs == null) {
            return;
        }
        foreach (JToken cfgt in cfgs.Values()) {
            JObject cfg = cfgt.ToObject<JObject>();
            BuffCfg buff = new BuffCfg();
            buff.Id = (int)cfg["Id"];
            buff.Type = (int)cfg["Type"];
            buff.BuffAdd = (int)cfg["BuffAdd"];
            buff.BuffUpdate = (int)cfg["BuffUpdate"];
            buff.TimeLife = GameTool.CfgFix((int)cfg["TimeLife"]);
            buff.TimeCd = GameTool.CfgFix((int)cfg["TimeCd"]);
            int index = 0;
            foreach (JToken v in cfg["MulEffectType"]) {
                buff.MulEffectType[index] = (int)v;
                index++;
                buff.MulEffectNum = index;
                if (index >= GameConst.BuffCacSize) {
                    break;
                }
            }
            index = 0;
            foreach (JToken v in cfg["MulEffectValue"]) {
                buff.MulEffectValue[index] = GameTool.CfgFix((int)v);
                index++;
                if (index >= GameConst.BuffCacSize) {
                    break;
                }
            }
            index = 0;
            foreach (JToken v in cfg["AddEffectType"]) {
                buff.AddEffectType[index] = (int)v;
                index++;
                buff.AddEffectNum = index;
                if (index >= GameConst.BuffCacSize) {
                    break;
                }
            }
            index = 0;
            foreach (JToken v in cfg["AddEffectValue"]) {
                buff.AddEffectValue[index] = GameTool.CfgFix((int)v);
                index++;
                if (index >= GameConst.BuffCacSize) {
                    break;
                }
            }
            buffCfgs[buff.Id] = buff;
        }
    }

    public static BuffCfg GetCfg(int id) {
        if (!buffCfgs.ContainsKey(id)) {
            return null;
        }
        return buffCfgs[id];
    }

    public static BuffBase Create(UnitBase tri, UnitBase tar, int id) {
        BuffCfg cfg = GetCfg(id);
        if (cfg == null) {
            return null;
        }
        BuffBase buff = null;
        if (cfg.Type == GameDefine.BuffTypeCac) {
            buff = new BuffCac();
        }
        if (buff == null) {
            return null;
        }
        buff.mUnitTri = tri;
        buff.mUnitTar = tar;
        tar.mBuffMgr.Add(buff);
        tri.mBuffMgr.AddTri(buff);
        buff.Init(cfg);
        buff.Start();
        return buff;
    }

    public static void RemoveBuff(BuffBase buff) {
        buff.End();
        buff.mUnitTar.mBuffMgr.Remove(buff);
        buff.mUnitTri.mBuffMgr.RemoveTri(buff);
    }
}
