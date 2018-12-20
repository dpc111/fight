using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBase {
    public SkillCfg mCfg = null;
    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;
    public UnitBase[] mUnitTarList = new UnitBase[GameConst.SkillUnitTarMax];
    public int mUnitTarListNum = 0;
    public Fix mTimeTriNextTime = Fix.fix0;


    public virtual void Init(SkillCfg cfg, UnitBase unitTri) {
        mCfg = cfg;
        mUnitTri = unitTri;
        mUnitTarListNum = 0;
        mTimeTriNextTime = GameData.timeCur + mUnitTri.GetAttr(UnitAttrType.AttackCd);
    }

    public virtual void Start() {

    }

    public virtual void Trigger() {

    }

    public virtual void Trigger(UnitBase tar) {

    }

    public virtual void End() {

    }

    public virtual void Update() {
        if (!Check()) {
            return;
        }
        TryTrigger();
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

    public virtual void TryTrigger() {
        int camp = 0;
        if (mCfg.CampGroupType == FightConst.CampGroupTypeSelf) {
            camp = mUnitTri.Camp;
        } else if (mCfg.CampGroupType == FightConst.CampGroupTypeEnemy) {
            camp = FightTool.CampOther(mUnitTri.Camp);
        } else if (mCfg.CampGroupType == FightConst.CampGroupTypeAll) {
            camp = 0;
        }
        /////////////////////////////////////////////////////////////////////////////////
        if (mCfg.SkillTargetGroup == FightConst.SkillTargetGroupNone) {
            Trigger();
        /////////////////////////////////////////////////////////////////////////////////
        } else if (mCfg.SkillTargetGroup == FightConst.SkillTargetGroupNearestOne) {
            if (mCfg.SkillRangeType == FightConst.SkillRangeTypeHitRange) {
                return;
            } else if (mCfg.SkillRangeType == FightConst.SkillRangeTypeAttackRange) {
                if (mCfg.UnitType == FightConst.UnitTypeTower) {
                    mUnitTar = GameData.towerMgr.FindNearestInAttackRange(mUnitTri, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeSoldier) {
                    mUnitTar = GameData.soldierMgr.FindNearestInAttackRange(mUnitTri, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeBullet) {
                    mUnitTar = GameData.bulletMgr.FindNearestInAttackRange(mUnitTri, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeLive) {
                    mUnitTar = GameData.liveMgr.FindNearestInAttackRange(mUnitTri, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeAll) {
                }
                if (mUnitTar == null) {
                    return;
                }
                Trigger(mUnitTar);
            } else if (mCfg.SkillRangeType == FightConst.SkillRangeTypeAll) {
                if (mCfg.UnitType == FightConst.UnitTypeTower) {
                    mUnitTar = GameData.towerMgr.FindNearest(mUnitTri, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeSoldier) {
                    mUnitTar = GameData.soldierMgr.FindNearest(mUnitTri, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeBullet) {
                    mUnitTar = GameData.bulletMgr.FindNearest(mUnitTri, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeLive) {
                    mUnitTar = GameData.liveMgr.FindNearest(mUnitTri, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeAll) {
                }
                if (mUnitTar == null) {
                    return;
                }
                Trigger(mUnitTar);
            }
        /////////////////////////////////////////////////////////////////////////////////
        } else if (mCfg.SkillTargetGroup == FightConst.SkillTargetGroupLimitNum) {
            int tarNum = (int)mUnitTri.GetAttr(UnitAttrType.AttackNum);
            if (mCfg.SkillRangeType == FightConst.SkillRangeTypeHitRange) {
            } else if (mCfg.SkillRangeType == FightConst.SkillRangeTypeAttackRange) {
                if (mCfg.UnitType == FightConst.UnitTypeTower) {
                    mUnitTarListNum = GameData.towerMgr.FindListInAttackRange(mUnitTar, ref mUnitTarList, tarNum, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeSoldier) {
                    mUnitTarListNum = GameData.soldierMgr.FindListInAttackRange(mUnitTar, ref mUnitTarList, tarNum, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeBullet) {
                    mUnitTarListNum = GameData.bulletMgr.FindListInAttackRange(mUnitTar, ref mUnitTarList, tarNum, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeLive) {
                    mUnitTarListNum = GameData.liveMgr.FindListInAttackRange(mUnitTar, ref mUnitTarList, tarNum, camp);
                } else if (mCfg.UnitType == FightConst.UnitTypeAll) {
                }
                for (int i = 0; i < mUnitTarListNum; i++) {
                    Trigger(mUnitTarList[i]);
                }
            } else if (mCfg.SkillRangeType == FightConst.SkillRangeTypeAll) {
                if (mCfg.UnitType == FightConst.UnitTypeTower) {
                } else if (mCfg.UnitType == FightConst.UnitTypeSoldier) {
                } else if (mCfg.UnitType == FightConst.UnitTypeBullet) {
                } else if (mCfg.UnitType == FightConst.UnitTypeLive) {
                } else if (mCfg.UnitType == FightConst.UnitTypeAll) {
                }
            }
        /////////////////////////////////////////////////////////////////////////////////
        } else if (mCfg.SkillTargetGroup == FightConst.SkillTargetGroupAll) {
            if (mCfg.SkillRangeType == FightConst.SkillRangeTypeHitRange) {
            } else if (mCfg.SkillRangeType == FightConst.SkillRangeTypeAttackRange) {
                List<UnitBase> unitList = null;
                if (mCfg.UnitType == FightConst.UnitTypeTower) {
                    unitList = GameData.towerMgr.GetList();
                } else if (mCfg.UnitType == FightConst.UnitTypeSoldier) {
                    unitList = GameData.soldierMgr.GetList();
                } else if (mCfg.UnitType == FightConst.UnitTypeBullet) {
                    unitList = GameData.bulletMgr.GetList();
                } else if (mCfg.UnitType == FightConst.UnitTypeLive) {
                    unitList = GameData.liveMgr.GetList();
                } else if (mCfg.UnitType == FightConst.UnitTypeAll) {
                }
                if (unitList == null) {
                    return;
                }
                for (int i = 0; i < unitList.Count; i++) {
                    UnitBase u = unitList[i];
                    if (u.Kill || u == mUnitTri || (camp != 0 && camp != u.Camp)) {
                        continue;
                    }
                    if (FightTool.IsInAttackRange(mUnitTri, u)) {
                        Trigger(u);
                    }
                }
            } else if (mCfg.SkillRangeType == FightConst.SkillRangeTypeAll) {
                List<UnitBase> unitList = null;
                if (mCfg.UnitType == FightConst.UnitTypeTower) {
                    unitList = GameData.towerMgr.GetList();
                } else if (mCfg.UnitType == FightConst.UnitTypeSoldier) {
                    unitList = GameData.soldierMgr.GetList();
                } else if (mCfg.UnitType == FightConst.UnitTypeBullet) {
                    unitList = GameData.bulletMgr.GetList();
                } else if (mCfg.UnitType == FightConst.UnitTypeLive) {
                    unitList = GameData.liveMgr.GetList();
                } else if (mCfg.UnitType == FightConst.UnitTypeAll) {
                }
                if (unitList == null) {
                    return;
                }
                for (int i = 0; i < unitList.Count; i++) {
                    UnitBase u = unitList[i];
                    if (u.Kill || u == mUnitTri || (camp != 0 && camp != u.Camp)) {
                        continue;
                    }
                    Trigger(u);
                }
            }
        }
    }
}
