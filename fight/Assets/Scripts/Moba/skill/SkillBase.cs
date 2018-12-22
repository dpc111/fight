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
        mTimeTriNextTime = GameApp.timeCur + mUnitTri.GetAttr(GameDefine.AttrTypeAttackCd);
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
        if (mTimeTriNextTime > GameApp.timeCur) {
            return false;
        }
        return true;
    }

    public virtual void CheckOver() {
        mTimeTriNextTime = GameApp.timeCur + mUnitTri.GetAttr(GameDefine.AttrTypeAttackCd);
    }

    public virtual void TryTrigger() {
        int camp = 0;
        if (mCfg.CampGroupType == GameDefine.CampGroupTypeSelf) {
            camp = mUnitTri.Camp;
        } else if (mCfg.CampGroupType == GameDefine.CampGroupTypeEnemy) {
            camp = GameTool.CampOther(mUnitTri.Camp);
        } else if (mCfg.CampGroupType == GameDefine.CampGroupTypeAll) {
            camp = 0;
        }
        /////////////////////////////////////////////////////////////////////////////////
        if (mCfg.SkillTargetGroup == GameDefine.SkillTargetGroupNone) {
            Trigger();
        /////////////////////////////////////////////////////////////////////////////////
        } else if (mCfg.SkillTargetGroup == GameDefine.SkillTargetGroupNearestOne) {
            if (mCfg.SkillRangeType == GameDefine.SkillRangeTypeHitRange) {
                return;
            } else if (mCfg.SkillRangeType == GameDefine.SkillRangeTypeAttackRange) {
                if (mCfg.UnitType == GameDefine.UnitTypeTower) {
                    mUnitTar = GameApp.towerMgr.FindNearestInAttackRange(mUnitTri, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeSoldier) {
                    mUnitTar = GameApp.soldierMgr.FindNearestInAttackRange(mUnitTri, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeBullet) {
                    mUnitTar = GameApp.bulletMgr.FindNearestInAttackRange(mUnitTri, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeLive) {
                    mUnitTar = GameApp.liveMgr.FindNearestInAttackRange(mUnitTri, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeAll) {
                }
                if (mUnitTar == null) {
                    return;
                }
                Trigger(mUnitTar);
            } else if (mCfg.SkillRangeType == GameDefine.SkillRangeTypeAll) {
                if (mCfg.UnitType == GameDefine.UnitTypeTower) {
                    mUnitTar = GameApp.towerMgr.FindNearest(mUnitTri, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeSoldier) {
                    mUnitTar = GameApp.soldierMgr.FindNearest(mUnitTri, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeBullet) {
                    mUnitTar = GameApp.bulletMgr.FindNearest(mUnitTri, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeLive) {
                    mUnitTar = GameApp.liveMgr.FindNearest(mUnitTri, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeAll) {
                }
                if (mUnitTar == null) {
                    return;
                }
                Trigger(mUnitTar);
            }
        /////////////////////////////////////////////////////////////////////////////////
        } else if (mCfg.SkillTargetGroup == GameDefine.SkillTargetGroupLimitNum) {
            int tarNum = (int)mUnitTri.GetAttr(GameDefine.AttrTypeAttackNum);
            if (mCfg.SkillRangeType == GameDefine.SkillRangeTypeHitRange) {
            } else if (mCfg.SkillRangeType == GameDefine.SkillRangeTypeAttackRange) {
                if (mCfg.UnitType == GameDefine.UnitTypeTower) {
                    mUnitTarListNum = GameApp.towerMgr.FindListInAttackRange(mUnitTar, ref mUnitTarList, tarNum, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeSoldier) {
                    mUnitTarListNum = GameApp.soldierMgr.FindListInAttackRange(mUnitTar, ref mUnitTarList, tarNum, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeBullet) {
                    mUnitTarListNum = GameApp.bulletMgr.FindListInAttackRange(mUnitTar, ref mUnitTarList, tarNum, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeLive) {
                    mUnitTarListNum = GameApp.liveMgr.FindListInAttackRange(mUnitTar, ref mUnitTarList, tarNum, camp);
                } else if (mCfg.UnitType == GameDefine.UnitTypeAll) {
                }
                for (int i = 0; i < mUnitTarListNum; i++) {
                    Trigger(mUnitTarList[i]);
                }
            } else if (mCfg.SkillRangeType == GameDefine.SkillRangeTypeAll) {
                if (mCfg.UnitType == GameDefine.UnitTypeTower) {
                } else if (mCfg.UnitType == GameDefine.UnitTypeSoldier) {
                } else if (mCfg.UnitType == GameDefine.UnitTypeBullet) {
                } else if (mCfg.UnitType == GameDefine.UnitTypeLive) {
                } else if (mCfg.UnitType == GameDefine.UnitTypeAll) {
                }
            }
        /////////////////////////////////////////////////////////////////////////////////
        } else if (mCfg.SkillTargetGroup == GameDefine.SkillTargetGroupAll) {
            if (mCfg.SkillRangeType == GameDefine.SkillRangeTypeHitRange) {
            } else if (mCfg.SkillRangeType == GameDefine.SkillRangeTypeAttackRange) {
                List<UnitBase> unitList = null;
                if (mCfg.UnitType == GameDefine.UnitTypeTower) {
                    unitList = GameApp.towerMgr.GetList();
                } else if (mCfg.UnitType == GameDefine.UnitTypeSoldier) {
                    unitList = GameApp.soldierMgr.GetList();
                } else if (mCfg.UnitType == GameDefine.UnitTypeBullet) {
                    unitList = GameApp.bulletMgr.GetList();
                } else if (mCfg.UnitType == GameDefine.UnitTypeLive) {
                    unitList = GameApp.liveMgr.GetList();
                } else if (mCfg.UnitType == GameDefine.UnitTypeAll) {
                }
                if (unitList == null) {
                    return;
                }
                for (int i = 0; i < unitList.Count; i++) {
                    UnitBase u = unitList[i];
                    if (u.Kill || u == mUnitTri || (camp != 0 && camp != u.Camp)) {
                        continue;
                    }
                    if (GameTool.IsInAttackRange(mUnitTri, u)) {
                        Trigger(u);
                    }
                }
            } else if (mCfg.SkillRangeType == GameDefine.SkillRangeTypeAll) {
                List<UnitBase> unitList = null;
                if (mCfg.UnitType == GameDefine.UnitTypeTower) {
                    unitList = GameApp.towerMgr.GetList();
                } else if (mCfg.UnitType == GameDefine.UnitTypeSoldier) {
                    unitList = GameApp.soldierMgr.GetList();
                } else if (mCfg.UnitType == GameDefine.UnitTypeBullet) {
                    unitList = GameApp.bulletMgr.GetList();
                } else if (mCfg.UnitType == GameDefine.UnitTypeLive) {
                    unitList = GameApp.liveMgr.GetList();
                } else if (mCfg.UnitType == GameDefine.UnitTypeAll) {
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
