using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : UnitBase {
    public BulletCfg mCfg = null;
    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;

    public virtual void Init(BulletCfg cfg, FixVector3 pos) {
        base.Init(cfg, pos);
        mCfg = cfg;
    }

    public override void Update() {
        base.Update();
        HitCheck();
    }

    public virtual void OnHit(UnitBase unitHit) {
        Fix damage = mAttr.GetAttr(GameDefine.AttrTypeAttackDamage);
        unitHit.OnBeAttack(damage);
        if (mCfg.HitDestroy == 1) {
            Kill = true;
        }
    }

    public void HitCheck() {
        if (mCfg.CheckHitType == GameDefine.BulletHitCheckNone) {
            return;
        } else if (mCfg.CheckHitType == GameDefine.BulletHitCheckTarget) {
            if (mUnitTar == null || mUnitTar.Kill) {
                return;
            }
            if (!GameTool.IsHit2(this, mUnitTar)) {
                return;
            }
            OnHit(mUnitTar);
        } else if (mCfg.CheckHitType == GameDefine.BulletHitCheckAll) {
            int camp = 0;
            if (mCfg.CheckHitCamp == GameDefine.CampGroupTypeSelf) {
                camp = Camp;
            } else if (mCfg.CheckHitCamp == GameDefine.CampGroupTypeEnemy) {
                camp = GameTool.CampOther(Camp);
            } else if (mCfg.CheckHitCamp == GameDefine.CampGroupTypeAll) {
                camp = 0;
            }
            if (mCfg.HitDestroy == 1) {
                List<UnitBase> unitList = null;
                if (mCfg.CheckHitUnitType == GameDefine.UnitTypeTower) {
                    unitList = GameApp.towerMgr.GetList();
                } else if (mCfg.CheckHitUnitType == GameDefine.UnitTypeSoldier) {
                    unitList = GameApp.soldierMgr.GetList();
                } else if (mCfg.CheckHitUnitType == GameDefine.UnitTypeLive) {
                    unitList = GameApp.liveMgr.GetList();
                }
                for (int i = 0; i < unitList.Count; i++) {
                    UnitBase u = unitList[i];
                    if (u.Kill) {
                        continue;
                    }
                    if (camp != 0 && camp != u.Camp) {
                        continue;
                    }
                    if (GameTool.IsHit2(this, u)) {
                        OnHit(u);
                        break;
                    }
                }
            } else {
                List<UnitBase> unitList = null;
                if (mCfg.CheckHitUnitType == GameDefine.UnitTypeTower) {
                    unitList = GameApp.towerMgr.GetList();
                } else if (mCfg.CheckHitUnitType == GameDefine.UnitTypeSoldier) {
                    unitList = GameApp.soldierMgr.GetList();
                } else if (mCfg.CheckHitUnitType == GameDefine.UnitTypeLive) {
                    unitList = GameApp.liveMgr.GetList();
                }
                Fix attackRange = GetAttr(GameDefine.AttrTypeAttackRange);
                for (int i = 0; i < unitList.Count; i++) {
                    UnitBase u = unitList[i];
                    if (u.Kill) {
                        continue;
                    }
                    if (camp != 0 && camp != u.Camp) {
                        continue;
                    }
                    if (GameTool.IsHit2Enter(u, this, attackRange)) {
                        OnHit(u);
                    }
                }
            }
        }
    }
}
