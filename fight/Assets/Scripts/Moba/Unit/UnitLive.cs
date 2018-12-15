﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLive : UnitBase {
    SkillBase mSkill = null;

    public override void Init(UnitCfg cfg, FixVector3 pos) {
        base.Init(cfg, pos);
        mSkill = SkillFactory.Create(cfg.SkillId, this);
    }

    public override void Update() {
        base.Update();
        if (mSkill != null) {
            mSkill.Update();
        }
    }
}
