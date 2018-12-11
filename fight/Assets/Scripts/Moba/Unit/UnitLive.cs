using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitLive : UnitBase 
{
    public Fix mBlood = Fix.fix0;
    public SkillBase[] mSkills = new SkillBase[GameConst.SkillCellSize];

    public virtual void OnDeath()
    {

    }

    public virtual void UpdateLogic()
    {
        for (int i = 0; i < GameConst.SkillCellSize; i++)
        {
            SkillBase skill = mSkills[i];
            if (skill == null)
            {
                break;
            }
            skill.Update();
        }
    }
}
