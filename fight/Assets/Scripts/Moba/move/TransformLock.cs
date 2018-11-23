using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformLock : TransformBase 
{
    public TransformBase mObjLock = null;

    public virtual void MoveLock(TransformBase objLock) 
    {
        if (objLock == null)
            return;
        mObjLock = objLock;
        Move = true;
    }

    public virtual void UpdateLogic()
    {
        base.UpdateLogic();
        if (!Move)
            return;
        mDir = mObjLock.mPos - mPos;
        mDir.Normalize();
        mPos += mDir * mSpeed * GameData.fixFrameLen;
    }

}
