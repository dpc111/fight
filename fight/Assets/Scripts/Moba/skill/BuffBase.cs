using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase {
    public BuffCfg mCfg = null;
    public int mType;
    public Fix mTimeLife = Fix.fix0;
    public Fix mTimeAdd = Fix.fix0;
    public Fix mTimeLast = Fix.fix0;

    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;

    public virtual void Init(BuffCfg cfg) {
        mCfg = cfg;
        mTimeLife = mCfg.TimeLife;
        mTimeAdd = GameApp.timeCur;
        mTimeLast = GameApp.timeCur;
    }

    public virtual void Start() {

    }

    public virtual void End() {

    }

    public virtual void Update() {
        if (mTimeLife <= Fix.fix0) {
            return;
        }
        if (GameApp.timeCur - mTimeAdd <= mTimeLife) {
            return;
        }
        End();
    }

    public virtual void Refresh() {

    }
}
