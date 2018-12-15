using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase {
    public SkillCfg mCfg = null;
    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;
    public UnitBase[] mUnitTarList = new UnitBase[GameConst.SkillUnitTarMax];
    public Fix mTimeTriNextTime = Fix.fix0;


    public virtual void Init(SkillCfg cfg, UnitBase unitTri) {
        mCfg = cfg;
        mUnitTri = unitTri;
        mTimeTriNextTime = GameData.timeCur + mUnitTri.GetAttr(UnitAttrType.AttackCd);
    }

    public virtual void Start() {

    }

    public virtual void Trigger() {

    }

    public virtual void End() {

    }

    public virtual void Update() {
        if (!Check()) {
            return;
        }
        Trigger();
        CheckOver();
    }

    public virtual bool Check() {
        if (mTimeTriNextTime > GameData.timeCur) {
            return false;
        }
        return true;
    }

    public virtual void CheckOver() {
        mTimeTriNextTime = GameData.timeCur + mUnitTri.GetAttr(UnitAttrType.AttackCd);
    }
}
