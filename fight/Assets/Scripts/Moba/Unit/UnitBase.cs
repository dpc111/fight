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

public class UnitBase {
    public UnitUnity mUnitUnity = new UnitUnity();
    public UnitAttr mAttr = new UnitAttr();
    public TransformBase mTransform = new TransformBase();
    public BuffMgr mBuffMgr = new BuffMgr();
    public bool mIsKill = false;
    public int mCamp = 1;
    public bool Kill { get { return mIsKill; } set { mIsKill = value; } }
    public int Camp { get { return mCamp; } set { mCamp = value; } }

    public virtual void Init(UnitCfg cfg, FixVector3 pos) {
        mAttr.Init(this);
        mTransform.Init(pos, cfg.BlockRange, cfg.MoveSpeed);
        GameData.transformMgr.Add(mTransform);
        mUnitUnity.Init(this, cfg);
        mBuffMgr.Init(this);
        mAttr.SetAttr(UnitAttrType.Hp, cfg.Hp);
        mAttr.SetAttr(UnitAttrType.Armor, cfg.Armor);
        mAttr.SetAttr(UnitAttrType.MoveSpeed, cfg.MoveSpeed);
        mAttr.SetAttr(UnitAttrType.AttackCd, cfg.AttackCd);
        mAttr.SetAttr(UnitAttrType.AttackRange, cfg.AttackRange);
        mAttr.SetAttr(UnitAttrType.AttackDamage, cfg.AttackDamage);
        mAttr.SetAttr(UnitAttrType.AttackNum, cfg.AttackNum);
        Kill = false;
    }

    public virtual void Destory() {
        mUnitUnity.Destory();
        mUnitUnity = null;
        mAttr = null;
        GameData.transformMgr.Remove(mTransform);
        mTransform = null;
        mBuffMgr = null;
    }

    public virtual void Update() {
        if (Kill) {
            return;
        }
        if (mBuffMgr != null) {
            mBuffMgr.Update();
        }
        mUnitUnity.Update();
        if (FightTool.IsOutOfWorld(this)) {
            Kill = true;
            return;
        }
    }

    public bool CanBeAttack() {
        if (Kill) {
            return false;
        }
        return true;
    }

    public Fix GetAttr(UnitAttrType type) {
        return mAttr.GetAttr(type);
    }
}
