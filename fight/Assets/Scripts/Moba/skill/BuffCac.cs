using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCac : BuffBase {

    public override void Init(BuffCfg cfg) {
        base.Init(cfg);
    }

    public override void Start() {
        base.Start();
        Cac();
    }

    public override void End() {
        base.End();
        DelCac();
    }

    //public override void Update() {
    //    base.Update();
    //}

    //public override void Refresh() {
    //    base.Refresh();
    //    Cac();
    //}

    private void Cac() {
        for (int i = 0; i < mCfg.MulEffectNum; i++) {
            mUnitTar.mAttr.MulAttr(mCfg.MulEffectType[i], mCfg.MulEffectValue[i]);
        }
        for (int i = 0; i < mCfg.AddEffectNum; i++) {
            mUnitTar.mAttr.AddAttr(mCfg.AddEffectType[i], mCfg.AddEffectValue[i]);
        } 
    }

    private void DelCac() {
        for (int i = 0; i < mCfg.MulEffectNum; i++) {
            mUnitTar.mAttr.MulAttr(mCfg.MulEffectType[i], Fix.fix1 / mCfg.MulEffectValue[i]);
        }
        for (int i = 0; i < mCfg.AddEffectNum; i++) {
            mUnitTar.mAttr.AddAttr(mCfg.AddEffectType[i], -mCfg.AddEffectValue[i]);
        }
    }
}
