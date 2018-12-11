using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformDir : TransformBase
{
    public override void MoveDir(FixVector2 dir) 
    {
        mDir.Normalize();
        Move = true;
    }

   public override void Update()
   {
       base.Update();
       if (!Move)
           return;
       mPos += mDir * mSpeed * GameData.timeFrame;
   }
}
