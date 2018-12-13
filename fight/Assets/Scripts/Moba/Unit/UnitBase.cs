using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCfg
{
    public Fix hp = Fix.fix0;
    public Fix armor = Fix.fix0;
    public int moveType = 0;
    public Fix moveSpeed = Fix.fix0;
    public Fix attCd = Fix.fix0;
    public Fix attRange = Fix.fix0;
    public Fix attDamage = Fix.fix0;
    public Fix blockRange = Fix.fix0;
    public string prefab = "";
}

public class UnitBase 
{
    public UnitUnity mUnitUnity = new UnitUnity();
    public UnitAttr mAttr = new UnitAttr();
    public TransformBase mTransform = null;
    public BuffMgr mBuffMgr = new BuffMgr();
    public bool mIsKill = false;
    public int mCamp = 1;
    public bool Kill { get { return mIsKill; } set { mIsKill = value; } }
    public int Camp { get { return mCamp; } set { mCamp = value; } }

    public virtual void Init(UnitCfg cfg, FixVector3 pos)
    {
        mUnitUnity.Init(cfg, pos);
        mAttr.Init();
        mTransform = GameData.transformMgr.Create(cfg.moveType);
        mTransform.Init(pos, cfg.blockRange, cfg.moveSpeed);
        mBuffMgr.Init(this);
        mAttr.SetAttr(UnitAttrType.Hp, cfg.hp);
        mAttr.SetAttr(UnitAttrType.Armor, cfg.armor);
        mAttr.SetAttr(UnitAttrType.MoveSpeed, cfg.moveSpeed);
        mAttr.SetAttr(UnitAttrType.AttCd, cfg.attCd);
        mAttr.SetAttr(UnitAttrType.AttRange, cfg.attRange);
        mAttr.SetAttr(UnitAttrType.AttDamage, cfg.attDamage);
        Kill = false;
    }

    public virtual void Destory()
    {
        mUnitUnity.Destory();
        mUnitUnity = null;
        mAttr = null;
        GameData.transformMgr.Remove(mTransform);
        mTransform = null;
        mBuffMgr = null;
    }

    public virtual void Update()
    {
        if (Kill)
        {  
            return;
        }
        if (mBuffMgr != null)
        {
            mBuffMgr.Update();
        }
        mUnitUnity.SetGameObjectPosition(mTransform.Pos);
        if (mTransform.CheckOutWorld())
        {
            Kill = true;
            return;
        }
    }

    public bool CanBeAttack()
    {
        if (Kill)
        {
            return false;
        }
        return true;
    }
}
