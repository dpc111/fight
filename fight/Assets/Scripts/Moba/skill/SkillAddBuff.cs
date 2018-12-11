using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAddBuff : SkillBase
{
    public Fix sMulDamage = Fix.fix0;
    public Fix sAddDamage = Fix.fix0;
    public Fix sMulCd = Fix.fix0;
    public Fix sAddCd = Fix.fix0;
    public Fix sMulRange = Fix.fix0;
    public Fix sAddRange = Fix.fix0;
    public Fix sMulSpeed = Fix.fix0;
    public Fix sAddSpeed = Fix.fix0;

    public Fix mMulDamage = Fix.fix0;
    public Fix mAddDamage = Fix.fix0;
    public Fix mMulCd = Fix.fix0;
    public Fix mAddCd = Fix.fix0;
    public Fix mMulRange = Fix.fix0;
    public Fix mAddRange = Fix.fix0;
    public Fix mMulSpeed = Fix.fix0;
    public Fix mAddSpeed = Fix.fix0;

    public override void OnTrigger()
    {
        base.OnTrigger();
    }

    public override void Update()
    {
        base.Update();
    }

    public void AddBuff()
    {

    }
}
