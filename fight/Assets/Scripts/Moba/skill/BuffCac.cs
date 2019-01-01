using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCac : BuffBase {
    public int mAddAttr;
    public Fix mAddValue;
    public int mMulAttr;
    public Fix mMulValue;
    public int mAddCdAttr;
    public Fix mAddCdValue;

    public override void Init(BuffCfg cfg) {
        base.Init(cfg);
    }

    public override void Start() {
        if (mAddAttr != GameDefine.AttrTypeNone) {
            mUnitTar.mAttr.AddAttr(mAddAttr, mAddValue);
        }
        if (mMulAttr != GameDefine.AttrTypeNone) {
            mUnitTar.mAttr.MulAttr(mMulAttr, mMulValue + 1);
        }
    }

    public override void End() {

    }

    public override void Update() {
        base.Update();
        if (mTimeLast - GameApp.timeCur < mCfg.TimeCd) {
            return;
        }
        mTimeLast = GameApp.timeCur;
        if (mAddCdAttr != GameDefine.AttrTypeNone) {
            mUnitTar.mAttr.AddAttr(mAddCdAttr, mAddCdValue);
        }
    }

    public override void Refresh() {
        base.Refresh();
        if (mAddAttr != GameDefine.AttrTypeNone) {
            mUnitTar.mAttr.AddAttr(mAddAttr, mAddValue);
        }
        if (mMulAttr != GameDefine.AttrTypeNone) {
            mUnitTar.mAttr.MulAttr(mMulAttr, mMulValue + 1);
        }
    }
}
