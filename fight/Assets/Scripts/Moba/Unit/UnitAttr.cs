using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttr {
    public UnitBase mUnit = null;
    public Fix[] mAttrCfg = new Fix[GameDefine.AttrTypeNum];
    public Fix[] mAttrOrg = new Fix[GameDefine.AttrTypeNum];
    public Fix[] mAttrCur = new Fix[GameDefine.AttrTypeNum];

    public void Init(UnitBase unit) {
        mUnit = unit;
        for (int i = 0; i < GameDefine.AttrTypeNum; i++) {
            mAttrCfg[i] = Fix.fix0;
            mAttrOrg[i] = Fix.fix0;
            mAttrCur[i] = Fix.fix0;
        }
    }

    public void ResetAttr() {
        for (int i = 0; i < GameDefine.AttrTypeNum; i++) {
            mAttrCur[i] = mAttrOrg[i];
        }
    }

    public Fix GetAttr(int attr) {
        if (attr < GameDefine.AttrTypeNone || attr >= GameDefine.AttrTypeNum) {
            return Fix.fix0;
        }
        return mAttrCur[attr];
    }

    public void SetAttr(int attr, Fix value) {
        if (attr < GameDefine.AttrTypeNone || attr >= GameDefine.AttrTypeNum) {
            return;
        }
        mAttrCur[attr] = value;
    }

    public void AddAttr(int attr, Fix value) {
        if (attr < GameDefine.AttrTypeNone || attr >= GameDefine.AttrTypeNum) {
            return;
        }
        mAttrCur[attr] += value;
        if (mAttrCur[attr] < Fix.fix0) {
            mAttrCur[attr] = Fix.fix0;
        }
        OnAddAttr(attr, mAttrCur[attr]);
    }

    public void MulAttr(int attr, Fix value) {
        if (attr < GameDefine.AttrTypeNone || attr >= GameDefine.AttrTypeNum) {
            return;
        }
        mAttrCur[attr] *= value;
    }

    public void OnAddAttr(int attr, Fix value) {
        if (attr == GameDefine.AttrTypeHp) {
            if (value <= Fix.fix0) {
                mUnit.Kill = true;
            }
        }
    }
}
