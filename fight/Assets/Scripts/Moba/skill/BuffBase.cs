using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase : BuffUnity {
    public BuffCfg mCfg = null;
    public int mType;
    public Fix mTimeLife = Fix.fix0;
    public Fix mTimeAdd = Fix.fix0;
    public Fix mTimeLast = Fix.fix0;

    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;

    public virtual void Init(BuffCfg cfg) {
        base.Init(cfg);
        mCfg = cfg;
        mTimeLife = mCfg.TimeLife;
        mTimeAdd = GameApp.timeCur;
        mTimeLast = GameApp.timeCur;
    }

    public override void Start() {
        base.Start();
    }

    public override void End() {
        base.End();
    }

    public override UnitUnity GetTar() {
        return mUnitTar;
    }

    public virtual void Update() {
        if (mTimeLife <= Fix.fix0 || 
            GameApp.timeCur - mTimeAdd <= mTimeLife) {
            return;
        }
        BuffFactory.Remove(this);
    }

    public virtual void Refresh() {
       
    }
}
