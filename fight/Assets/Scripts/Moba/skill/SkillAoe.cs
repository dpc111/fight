using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAoe : SkillBase {
    private Fix mTimeLast = Fix.fix0;

    public override void Init(SkillCfg cfg, UnitBase unitTri) {
        base.Init(cfg, unitTri);
        mTimeLast = GameApp.timeCur;
    }

    public override void Update() {
        base.Update();
        if (mCfg.BuffId == 0) {
            return;
        }
        if (GameApp.timeCur - mTimeLast <= GameConst.SkillAoeUpdateTime) {
            return;
        }
        mTimeLast = GameApp.timeCur;
        List<UnitBase> list = null;
        if (mCfg.SkillTar == GameDefine.SkillTarUnitTower) {
            list = GameApp.towerMgr.GetList();
        } else if (mCfg.SkillTar == GameDefine.SkillTarUnitSoldier) {
            list = GameApp.soldierMgr.GetList();
        } else if (mCfg.SkillTar == GameDefine.SkillTarUnitLive) {
            list = GameApp.liveMgr.GetList();
        }
        if (list == null) {
            return;
        }
        for (int i = 0; i < list.Count; i++) {
            UnitBase unit = list[i];
            BuffBase buff = unit.mBuffMgr.FindBuff(mUnitTri);
            if (buff == null) {
                continue;
            }
            if (!GameTool.IsInAttackRange(mUnitTri, unit)) {
                BuffFactory.Remove(buff);
            }
        }
    }

    public override void Trigger(UnitBase tar) {
        base.Trigger(tar);
        if (mCfg.BuffId == 0 ||
            mUnitTri.Id == tar.Id ||
            tar.mBuffMgr.HasBuff(mCfg.BuffId, mUnitTri)) {
            return;
        }
        BuffFactory.Create(mUnitTri, tar, mCfg.BuffId);
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
        if (list == null) {
            return;
        }
        for (int i = 0; i < list.Count; i++) {
            UnitBase unit = list[i];
            BuffBase buff = unit.mBuffMgr.FindBuff(mUnitTri);
            if (buff == null) {
                continue;
            }
            BuffFactory.Remove(buff);
        }
    }
}
