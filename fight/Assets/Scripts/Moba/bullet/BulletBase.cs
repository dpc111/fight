using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : UnitBase 
{
    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;

    public virtual void Init(BulletCfg cfg)
    {
        base.Init();
    }

    public virtual void Update()
    {
        base.Update();
    }
}
