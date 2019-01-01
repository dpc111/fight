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

    public virtual void Trigger(UnitBase tar) {
        if (mUnitTri != null) {
            mUnitTri.State = GameDefine.UnitStateAttack;
        }
    }

    public virtual void Trigger(FixVector3 vec) {
        if (mUnitTri != null) {
            mUnitTri.State = GameDefine.UnitStateAttack;
        }
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
        if (mCfg.IsAutoRelease == 0) {
            return;
        }
        if (mCfg.SkillTar == GameDefine.SkillTarNone) {
            Trigger(null);
        } else if (mCfg.SkillTar == GameDefine.SkillTarDir) {
            AutoFindDirTrigger();
        } else if (mCfg.SkillTar == GameDefine.SkillTarPos) {
            AutoFindPosTrigger();
        } else if (mCfg.SkillTar == GameDefine.SkillTarUnitTower) {
            AutoFindUnitTrigger(GameApp.towerMgr);
        } else if (mCfg.SkillTar == GameDefine.SkillTarUnitSoldier) {
            AutoFindUnitTrigger(GameApp.soldierMgr);
        } else if (mCfg.SkillTar == GameDefine.SkillTarUnitLive) {
            AutoFindUnitTrigger(GameApp.liveMgr);
        }
    }

    public virtual void AutoFindDirTrigger() {
        if (mCfg.SkillAutoFindTarDir == GameDefine.SkillAutoFindTarDirSelfDirX) {
            FixVector3 dir = GameTool.CampDir(mUnitTri.Camp);
            Trigger(dir);
        } else if (mCfg.SkillAutoFindTarDir == GameDefine.SkillAutoFindTarDirEnemyDirX) {
            FixVector3 dir = GameTool.CampDir(GameTool.CampOther(mUnitTri.Camp));
            Trigger(dir);
        } else if (mCfg.SkillAutoFindTarDir == GameDefine.SkillAutoFindTarDirNearestEnemyUnit) {
            UnitBase unit = GameApp.liveMgr.FindNearestInAttackRange(mUnitTri, GameTool.CampOther(mUnitTri.Camp));
            if (unit == null) {
                return;
            }
            FixVector3 dir = FixVector3.Dir(mUnitTri.mTransform.Pos, unit.mTransform.Pos);
            Trigger(dir);            
        }
    }
    public virtual void AutoFindPosTrigger() {
        if (mCfg.SkillAutoFindTarPos == GameDefine.SkillAutoFindTarPosEnmeyX) {
            Fix len = mUnitTri.GetAttr(GameDefine.AttrTypeAttackRange);
            FixVector3 pos = mUnitTri.mTransform.Pos + GameTool.CampDir(mUnitTri.Camp) * len;
            GameTool.PosLimitIntoWorld(ref pos);
            Trigger(pos);
        } else if (mCfg.SkillAutoFindTarPos == GameDefine.SkillAutoFindTarPosNereastUnitTower) {
            UnitBase unit = GameApp.towerMgr.FindNearestInAttackRange(mUnitTri, GameTool.CampOther(mUnitTri.Camp));
            if (unit == null) {
                return;
            }
            Trigger(unit.mTransform.Pos);
        } else if (mCfg.SkillAutoFindTarPos == GameDefine.SkillAutoFindTarPosNereastUnitSoldier) {
            UnitBase unit = GameApp.soldierMgr.FindNearestInAttackRange(mUnitTri, GameTool.CampOther(mUnitTri.Camp));
            if (unit == null) {
                return;
            }
            Trigger(unit.mTransform.Pos);
        } else if (mCfg.SkillAutoFindTarPos == GameDefine.SkillAutoFindTarPosNereastUnitLive) {
            UnitBase unit = GameApp.liveMgr.FindNearestInAttackRange(mUnitTri, GameTool.CampOther(mUnitTri.Camp));
            if (unit == null) {
                return;
            }
            Trigger(unit.mTransform.Pos);
        }
    }

    public virtual void AutoFindUnitTrigger(UnitMgr mgr) {
        int camp = 0;
        if (mCfg.CampGroup == GameDefine.CampGroupSelf) {
            camp = mUnitTri.Camp;
        } else if (mCfg.CampGroup == GameDefine.CampGroupEnemy) {
            camp = GameTool.CampOther(mUnitTri.Camp);
        } else if (mCfg.CampGroup == GameDefine.CampGroupAll) {
            camp = 0;
        }
        if (mCfg.SkillAutoFindTarUnit == GameDefine.SkillAutoFindTarUnitNearestOne) {
            mUnitTar = mgr.FindNearestInAttackRange(mUnitTri, camp);
            if (mUnitTar == null) {
                return;
            }
            Trigger(mUnitTar);
        } else if (mCfg.SkillAutoFindTarUnit == GameDefine.SkillAutoFindTarUnitRandomLimitNum) {
            int tarNum = (int)mUnitTri.GetAttr(GameDefine.AttrTypeAttackNum);
            mUnitTarListNum = mgr.FindListInAttackRange(mUnitTar, ref mUnitTarList, tarNum, camp);
            for (int i = 0; i < mUnitTarListNum; i++) {
                Trigger(mUnitTarList[i]);
            }
        } else if (mCfg.SkillAutoFindTarUnit == GameDefine.SkillAutoFindTarUnitAll) {
            List<UnitBase> list = mgr.GetList();
            for (int i = 0; i < list.Count; i++) {
                UnitBase u = list[i];
                if (u.Kill || u == mUnitTri || (camp != 0 && camp != u.Camp)) {
                    continue;
                }
                Trigger(u);
            }
        }
    }

    public virtual void OnKill() {

    }
}
