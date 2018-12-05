using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase 
{
    public UnitUnity mUnitUnity = new UnitUnity();
    public UnitAttr mAttr = new UnitAttr();
    public TransformBase mTransform = new TransformBase();
    public BuffMgr mBuffMgr = new BuffMgr();

    public virtual void Init()
    {
        mAttr.Init();
        mBuffMgr.Init(this);
    }

    public virtual void Update()
    {
        if (mTransform != null)
            mTransform.Update();
        if (mBuffMgr != null)
            mBuffMgr.Update();
    }
}
