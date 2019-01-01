using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAoe : SkillBase {
    private FixVector3 mPosLast = new FixVector3();
    private Fix mRangeLast = Fix.fix0;
    private Fix mTimeLast = Fix.fix0;

    public override void Init(SkillCfg cfg, UnitBase unitTri) {
        base.Init(cfg, unitTri);
        mPosLast = unitTri.mTransform.Pos;
        mRangeLast = unitTri.GetAttr(GameDefine.AttrTypeAttackRange);
        mTimeLast = GameApp.timeCur;
    }

    public override void Update() {
        base.Update();
        if (GameApp.timeCur - mTimeLast <= GameConst.SkillAoeUpdateTime) {
            return;
        }
        mTimeLast = GameApp.timeCur;
        if (mPosLast == mUnitTri.mTransform.Pos &&
            mRangeLast == mUnitTri.GetAttr(GameDefine.AttrTypeAttackRange)) {
            return;
        }
        if (mCfg.BuffId == 0) {
            return;
        }
        mPosLast = mUnitTri.mTransform.Pos;
        mRangeLast = mUnitTri.GetAttr(GameDefine.AttrTypeAttackRange);
        List<UnitBase> list = null;
        if (mCfg.SkillTar == GameDefine.SkillTarUnitTower) {
            list = GameApp.towerMgr.GetList();
        } else if (mCfg.SkillTar == GameDefine.SkillTarUnitSoldier) {
            list = GameApp.soldierMgr.GetList();
        } else if (mCfg.SkillTar == GameDefine.SkillTarUnitLive) {
            list = GameApp.liveMgr.GetList();
        }
        for (int i = 0; i < list.Count; i++) {
            UnitBase unit = list[i];
            BuffBase buff = unit.mBuffMgr.FindBuff(mUnitTri);
            if (buff == null) {
                continue;
            }
            if (!GameTool.IsInAttackRange(mUnitTri, unit)) {
                unit.mBuffMgr.Remove(buff);
            }
        }
    }

    public override void Trigger(UnitBase tar) {
        base.Trigger(tar);
        if (mCfg.BuffId != 0) {
            if (tar.mBuffMgr.HasBuff(mCfg.BuffId, mUnitTri)) {
                return;
            }
            BuffFactory.Create(mUnitTri, tar, mCfg.BuffId);
        }
    }

    public override void OnKill() {
        base.OnKill();
        List<UnitBase> list = null;
        if (mCfg.SkillTar == GameDefine.SkillTarUnitTower) {
            list = GameApp.towerMgr.GetList();
        } else if (mCfg.SkillTar == GameDefine.SkillTarUnitSoldier) {
            list = GameApp.soldierMgr.GetList();
        } else if (mCfg.SkillTar == GameDefine.SkillTarUnitLive) {
            list = GameApp.liveMgr.GetList();
        }
        for (int i = 0; i < list.Count; i++) {
            UnitBase unit = list[i];
            BuffBase buff = unit.mBuffMgr.FindBuff(mUnitTri);
            if (buff == null) {
                continue;
            }
            unit.mBuffMgr.Remove(buff);
        }
    }
}
