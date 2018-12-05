using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFire : SkillBase 
{
    public virtual void OnTrigger()
    {
        base.OnTrigger();
        Fire();
    }

    public virtual void Update()
    {
        base.UpdateLogic();    
    }

    private void Fire()
    {

    }
}
