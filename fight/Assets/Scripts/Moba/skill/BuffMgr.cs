using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMgr {
    private UnitBase mUnit = null;
    private List<BuffBase> mTarBuffs = new List<BuffBase>();
    private List<BuffBase> mTriBuffs = new List<BuffBase>();

    public void Init(UnitBase unit) {
        mUnit = unit;
        mTarBuffs.Clear();
        mTriBuffs.Clear();
    }

    public void Add(BuffBase buff) {
        if (mTarBuffs.Contains(buff)) {
            return;
        }
        if (buff.mCfg.BuffAdd == GameDefine.BuffAddLayer) {
            mTarBuffs.Add(buff);
        } else if (buff.mCfg.BuffAdd == GameDefine.BuffAddResetValue) {
            BuffBase old = FindBuff(buff.mCfg.Id);
            if (old == null) {
                mTarBuffs.Add(buff);
            } else {
                ResetData(old, buff);
            }
        } else if (buff.mCfg.BuffAdd == GameDefine.BuffAddTime) {
            BuffBase old = FindBuff(buff.mCfg.Id);
            if (old == null) {
                mTarBuffs.Add(buff);
            } else {
                old.mTimeLife += buff.mTimeLife;
            }
        } else if (buff.mCfg.BuffAdd == GameDefine.BuffAddResetTime) {
            BuffBase old = FindBuff(buff.mCfg.Id);
            if (old == null) {
                mTarBuffs.Add(buff);
            } else {
                old.mTimeAdd = GameApp.timeCur;
            }
        }
    }

    public void AddTri(BuffBase buff) {
        if (mTriBuffs.Contains(buff)) {
            return;
        }
        mTriBuffs.Add(buff);
    }

    public void Remove(BuffBase buff) {
        if (!mTarBuffs.Contains(buff)) {
            return;
        }
        mTarBuffs.Remove(buff);
    }

    public void RemoveTri(BuffBase buff) {
        if (!mTriBuffs.Contains(buff)) {
            return;
        }
        mTriBuffs.Remove(buff);
    }

    public void Update() {
        for (int i = 0; i < mTarBuffs.Count; i++) {
            BuffBase buff = mTarBuffs[i];
            buff.Update();
        }
    }

    public bool HasBuff(int id) {
        for (int i = 0; i < mTarBuffs.Count; i++) {
            BuffBase buff = mTarBuffs[i];
            if (buff.mCfg.Id == id) {
                return true;
            }
        }
        return false;
    }

    public bool HasBuff(int id, UnitBase tri) {
        BuffBase buff = FindBuff(id);
        if (buff == null) {
            return false;
        }
        if (buff.mUnitTri != tri) {
            return false;
        }
        return true;
    }

    public BuffBase FindBuff(int id) {
        for (int i = 0; i < mTarBuffs.Count; i++) {
            BuffBase buff = mTarBuffs[i];
            if (buff.mCfg.Id == id) {
                return buff;
            }
        }
        return null;
    }

    public BuffBase FindBuff(UnitBase tri) {
        for (int i = 0; i < mTarBuffs.Count; i++) {
            BuffBase buff = mTarBuffs[i];
            if (buff.mUnitTri == tri) {
                return buff;
            }
        }
        return null;
    }

    public void Refresh(BuffBase buff) {
        if (buff.mType == GameDefine.BuffTypeCac) {
            mUnit.mAttr.ResetAttr();
            for (int i = 0; i < mTarBuffs.Count; i++) {
                BuffBase b = mTarBuffs[i];
                if (b.mType != GameDefine.BuffTypeCac) {
                    continue;
                }
                b.Refresh();
            }
        }
    }

    private void ResetData(BuffBase old, BuffBase buff) {
        for (int i = 0; i < old.mCfg.MulEffectNum; i++) {
            if ((old.mCfg.MulEffectValue[i] < 0
                && old.mCfg.MulEffectValue[i] > buff.mCfg.MulEffectValue[i])
                || (old.mCfg.MulEffectValue[i] > 0
                && old.mCfg.MulEffectValue[i] < buff.mCfg.MulEffectValue[i])) {
                    old.mCfg.MulEffectValue[i] = buff.mCfg.MulEffectValue[i];                      
            }
        }
        for (int i = 0; i < old.mCfg.AddEffectNum; i++) {
            if ((old.mCfg.AddEffectValue[i] < 0
                && old.mCfg.AddEffectValue[i] > buff.mCfg.AddEffectValue[i])
                || (old.mCfg.AddEffectValue[i] > 0
                && old.mCfg.AddEffectValue[i] < buff.mCfg.AddEffectValue[i])) {
                    old.mCfg.AddEffectValue[i] = buff.mCfg.AddEffectValue[i];
            }
        }
    }
}
