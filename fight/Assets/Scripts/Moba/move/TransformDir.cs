using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformDir : TransformBase
{
    public virtual void MoveDir(FixVector2 dir) 
    {
        mDir.Normalize();
        Move = true;
    }

   public virtual void UpdateLogic()
   {
       base.UpdateLogic();
       if (!Move)
           return;
       mPos += mDir * mSpeed * GameData.fixFrameLen;
   }
}
