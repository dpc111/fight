using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase
{
    public Fix sCd;
    public Fix sRange;
    public UnitBase mUnit = null;
    public Fix mCd;
    public Fix mRange;

    public virtual void Init(UnitBase unit)
    {
        mUnit = unit;
    }

    public virtual void OnTrigger()
    {
        mCd = sCd;
    }

    public virtual void Update()
    {
        if (mCd > Fix.fix0)
        {
            mCd -= GameData.timeFrame;
            if (mCd <= Fix.fix0)
            {
                mCd = Fix.fix0;
                OnTrigger();
            }
        }
    }
}
