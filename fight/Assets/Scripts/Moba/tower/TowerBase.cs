using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBase : UnitBase
{
    public Fix mTimeNextAtt = Fix.fix0;

    public virtual void Init(TowerCfg cfg)
    {
        base.Init();
        mTimeNextAtt = GameData.timeCur + mAttr.GetAttr(UnitAttrType.AttCd);
    }

    public virtual void Attack()
    {

    }

    public virtual void Update()
    {
        base.Update();
        if (CheckAttackCd())
        {
            Attack();
        }
    }

    public bool CheckAttackCd()
    {
        if (GameData.timeCur < mTimeNextAtt)
            return false;
        mTimeNextAtt = GameData.timeCur + mAttr.GetAttr(UnitAttrType.AttCd);
        return true;
    }
}
