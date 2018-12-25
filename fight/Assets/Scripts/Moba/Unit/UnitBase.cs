using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCfg {
    public Fix Hp = Fix.fix0;
    public Fix Armor = Fix.fix0;
    public Fix MoveSpeed = Fix.fix0;
    public Fix AttackCd = Fix.fix0;
    public Fix AttackRange = Fix.fix0;
    public Fix AttackDamage = Fix.fix0;
    public Fix AttackNum = Fix.fix0;
    public Fix BlockRange = Fix.fix0;
    public string Prefab = "";
    public int SkillId = 0;
}

public class UnitBase : UnitUnity {
    public UnitAttr mAttr = new UnitAttr();
    public TransformBase mTransform = new TransformBase();
    public BuffMgr mBuffMgr = new BuffMgr();
    public int Camp { get; set; }

    public virtual void Init(UnitCfg cfg, FixVector3 pos) {
        mAttr.Init(this);
        mTransform.Init(this, pos, cfg.BlockRange, cfg.MoveSpeed);
        GameApp.transformMgr.Add(mTransform);
        mBuffMgr.Init(this);
        mAttr.SetAttr(GameDefine.AttrTypeHp, cfg.Hp);
        mAttr.SetAttr(GameDefine.AttrTypeArmor, cfg.Armor);
        mAttr.SetAttr(GameDefine.AttrTypeMoveSpeed, cfg.MoveSpeed);
        mAttr.SetAttr(GameDefine.AttrTypeAttackCd, cfg.AttackCd);
        mAttr.SetAttr(GameDefine.AttrTypeAttackRange, cfg.AttackRange);
        mAttr.SetAttr(GameDefine.AttrTypeAttackDamage, cfg.AttackDamage);
        mAttr.SetAttr(GameDefine.AttrTypeAttackNum, cfg.AttackNum);
        Kill = false;
        base.Init(cfg);
    }

    public override void Destory() {
        base.Destory();
        mAttr = null;
        GameApp.transformMgr.Remove(mTransform);
        mTransform = null;
        mBuffMgr = null;
    }

    public override void Update() {
        if (Kill) {
            return;
        }
        if (mBuffMgr != null) {
            mBuffMgr.Update();
        }
        if (GameTool.IsOutOfWorld(this)) {
            Kill = true;
            return;
        }
        base.Update();
    }

    public override TransformBase GetTransform() {
        return mTransform;
    }

    public virtual void OnMoveStart() {

    }

    public virtual void OnMoveStop() {

    }

    public virtual void OnBeAttack(Fix damage) {
        mAttr.AddAttr(GameDefine.AttrTypeHp, -damage);
    }

    public bool CanBeAttack() {
        if (Kill) {
            return false;
        }
        return true;
    }

    public Fix GetAttr(int type) {
        return mAttr.GetAttr(type);
    }
}
