using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLive : UnitBase {
    SkillBase mSkill = null;

    public override bool Kill {
        get {
            return base.Kill;
        }
        set {
            if (value) {
                State = GameDefine.UnitStateDeath;
                mSkill.OnKill();
            }
            base.Kill = value;
        }
    }

    public override void Init(UnitCfg cfg, UnitProperty pro) {
        base.Init(cfg, pro);
        mSkill = SkillFactory.Create(cfg.SkillId, this);
        State = GameDefine.UnitStateBorn;
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

    public override void OnMoveStart() {
        base.OnMoveStart();
        State = GameDefine.UnitStateWalk;
    }

    public override void OnMoveStop() {
        base.OnMoveStop();
        State = GameDefine.UnitStateIdle;
    }

    public override void OnBeAttack(Fix damage) {
        base.OnBeAttack(damage);
        State = GameDefine.UnitStateBeAttack;
    }
}
