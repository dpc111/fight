using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLock : TransformBase 
{
    public TransformBase mObjLock = null;

    public override void MoveLock(TransformBase objLock) 
    {
        if (objLock == null)
            return;
        mObjLock = objLock;
        Move = true;
    }

    public override void Update()
    {
        base.Update();
        if (!Move)
            return;
        mDir = mObjLock.mPos - mPos;
        mDir.Normalize();
        mPos += mDir * mSpeed * GameData.timeFrame;
    }

}
