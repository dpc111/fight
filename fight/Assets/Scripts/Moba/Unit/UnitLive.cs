using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLive : UnitBase 
{
    public Fix mTimeNextAtt = Fix.fix0;

    public override void Init(UnitCfg cfg, FixVector3 pos)
    {
        base.Init(cfg, pos);
        mTimeNextAtt = GameData.timeCur + mAttr.GetAttr(UnitAttrType.AttCd);
    }

    public override void Update()
    {
        base.Update();
        if (AttackCheck())
        {
            if (Attack())
            {
                AttackOver();
            }
        }
    }

    public virtual bool Attack()
    {
        return true;
    }

    public bool AttackCheck()
    {
        if (GameData.timeCur < mTimeNextAtt)
        {    
            return false;
        }
        return true;
    }

    public void AttackOver()
    {
        mTimeNextAtt = GameData.timeCur + mAttr.GetAttr(UnitAttrType.AttCd);
    }
}
