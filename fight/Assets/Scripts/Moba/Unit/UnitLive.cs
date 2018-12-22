using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLive : UnitBase {
    SkillBase mSkill = null;

    public override void Init(UnitCfg cfg, FixVector3 pos) {
        base.Init(cfg, pos);
        mSkill = SkillFactory.Create(cfg.SkillId, this);
        GameApp.liveMgr.Add(this);
    }

    public override void Destory() {
        base.Destory();
        GameApp.liveMgr.Remove(this);
    }

    public override void Update() {
        base.Update();
        if (mSkill != null) {
            mSkill.Update();
        }
    }
}
