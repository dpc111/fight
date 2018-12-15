using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBase : UnitLive {
    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;

    public virtual void Init(SoldierCfg cfg, FixVector3 pos) {
        base.Init(cfg, pos);
    }

    public override void Update() {
        base.Update();
        if (mUnitTar == null || mUnitTar.Kill) {
            mUnitTar = FightTool.FindEnemyUnitNearest(this);
            if (mUnitTar == null) {
                return;
            }
            mTransform.MoveLock(mUnitTar.mTransform);
        }
    }

    public virtual void Reset() {
        mUnitTar = null;
    }

}
