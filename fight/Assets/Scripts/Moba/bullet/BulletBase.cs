using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : UnitBase {
    public BulletCfg mCfg = null;
    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;
    public int mBulletType = (int)BulletType.Begin;

    public virtual void Init(BulletCfg cfg, FixVector3 pos) {
        base.Init(cfg, pos);
        mCfg = cfg;
        mBulletType = cfg.Type;
    }

    public override void Update() {
        base.Update();
        HitCheck();
    }

    public virtual void OnHit(UnitBase unitHit) {
        Fix damage = mAttr.GetAttr(UnitAttrType.AttackDamage);
        unitHit.mAttr.AddAttr(UnitAttrType.Hp, -damage);
        if (mCfg.HitDestroy == 1) {
            Kill = true;
        }
    }

    public void HitCheck() {
        if (mCfg.CheckHitType == FightConst.BulletHitCheckNone) {
            return;
        } else if (mCfg.CheckHitType == FightConst.BulletHitCheckTarget) {
            if (mUnitTar == null) {
                return;
            }
            if (!FightTool.IsHit(this, mUnitTar)) {
                return;
            }
            OnHit(mUnitTar);
        } else if (mCfg.CheckHitCamp == FightConst.BulletHitCheckAll) {
            int camp = 0;
            if (mCfg.CheckHitCamp == FightConst.CampGroupTypeSelf) {
                camp = Camp;
            } else if (mCfg.CheckHitCamp == FightConst.CampGroupTypeEnemy) {
                camp = FightTool.CampOther(Camp);
            } else if (mCfg.CheckHitCamp == FightConst.CampGroupTypeAll) {
                camp = 0;
            }
            if (mCfg.HitDestroy == 1) {
                List<UnitBase> unitList = null;
                if (mCfg.CheckHitUnitType == FightConst.UnitTypeTower) {
                    unitList = GameData.towerMgr.GetList();
                } else if (mCfg.CheckHitType == FightConst.UnitTypeSoldier) {
                    unitList = GameData.soldierMgr.GetList();
                } else if (mCfg.CheckHitType == FightConst.UnitTypeLive) {
                    unitList = GameData.liveMgr.GetList();
                }
                for (int i = 0; i < unitList.Count; i++) {
                    UnitBase u = unitList[i];
                    if (FightTool.IsHit(u, this)) {
                        OnHit(u);
                        return;
                    }
                }
            } else {
                List<UnitBase> unitList = null;
                if (mCfg.CheckHitUnitType == FightConst.UnitTypeTower) {
                    unitList = GameData.towerMgr.GetList();
                } else if (mCfg.CheckHitType == FightConst.UnitTypeSoldier) {
                    unitList = GameData.soldierMgr.GetList();
                } else if (mCfg.CheckHitType == FightConst.UnitTypeLive) {
                    unitList = GameData.liveMgr.GetList();
                }
                for (int i = 0; i < unitList.Count; i++) {
                    UnitBase u = unitList[i];
                    if (FightTool.IsHitEnter(u, this)) {
                        OnHit(u);
                    }
                }
            }
        }
    }
}
