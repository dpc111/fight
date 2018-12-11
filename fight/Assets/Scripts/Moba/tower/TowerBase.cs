using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : UnitBase
{
    public Fix mTimeNextAtt = Fix.fix0;

    public virtual void Init(TowerCfg cfg, FixVector3 pos)
    {
        base.Init(cfg, pos);
        mTimeNextAtt = GameData.timeCur + mAttr.GetAttr(UnitAttrType.AttCd);
    }

    public override void Update()
    {
        base.Update();
        if (AttackCheckCd())
        {
            Attack();
        }
    }

    public virtual void Attack()
    {

    }

    public bool AttackCheckCd()
    {
        if (GameData.timeCur < mTimeNextAtt)
            return false;
        mTimeNextAtt = GameData.timeCur + mAttr.GetAttr(UnitAttrType.AttCd);
        return true;
    }

    //public FixVector3 AttackPos()
    //{

    //}
}
