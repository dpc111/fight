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
            mUnitTar = GameApp.liveMgr.FindNearest(this, GameTool.CampOther(Camp));
            if (mUnitTar == null) {
                Kill = true;
                return;
            }
            mTransform.MoveTarget(mUnitTar.mTransform.Pos, GetAttr(GameDefine.AttrTypeAttackRange));
        }
    }

    public virtual void Reset() {
        mUnitTar = null;
    }

}
