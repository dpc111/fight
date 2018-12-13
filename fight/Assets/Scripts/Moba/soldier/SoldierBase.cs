using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBase : UnitLive
{
    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;

    public override void Update()
    {
        base.Update();
        if (mUnitTar == null || mUnitTar.Kill)
        {
            mUnitTar = FightTool.FindEnemyUnitNearest(this);
            if (mUnitTar == null)
            {
                return;
            }
            mTransform.MoveLock(mUnitTar.mTransform);
        }
    }

    public override bool Attack()
    {
        base.Attack();
        if (mUnitTar == null)
        {
            return false;
        }
        return true;
    }
}
