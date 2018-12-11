using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillFire : SkillBase 
{
    public override void OnTrigger()
    {
        base.OnTrigger();
        Fire();
    }

    public override void Update()
    {
        base.Update();    
    }

    private void Fire()
    {

    }
}
