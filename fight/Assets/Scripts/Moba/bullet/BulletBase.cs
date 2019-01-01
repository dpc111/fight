using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBase : UnitBase {
    public BulletCfg Cfg { get; set; }
    public UnitBase UnitTri { get; set; }
    public UnitBase UnitTar { get; set; }
    public FixVector3 PosTar { get; set; }

    public virtual void Init(BulletCfg cfg, UnitProperty pro) {
        base.Init(cfg, pro);
        Cfg = cfg;
    }

    public override void Update() {
        base.Update();
        HitCheck();
    }

    public virtual void Move(FixVector3 vec) {

    }

    public virtual void OnHit(UnitBase unitHit) {
        Fix damage = mAttr.GetAttr(GameDefine.AttrTypeAttackDamage);
        unitHit.OnBeAttack(damage);
        if (Cfg.HitDestroy == 1) {
            Kill = true;
        }
    }

    public void HitCheck() {
        if (Cfg.CheckHitType == GameDefine.BulletHitCheckNone) {
            return;
        } else if (Cfg.CheckHitType == GameDefine.BulletHitCheckTarget) {
            if (UnitTar == null || UnitTar.Kill) {
                return;
            }
            if (!GameTool.IsHit2(this, UnitTar)) {
                return;
            }
            OnHit(UnitTar);
        } else if (Cfg.CheckHitType == GameDefine.BulletHitCheckAll) {
            int camp = 0;
            if (Cfg.CheckHitCamp == GameDefine.CampGroupTypeSelf) {
                camp = Camp;
            } else if (Cfg.CheckHitCamp == GameDefine.CampGroupTypeEnemy) {
                camp = GameTool.CampOther(Camp);
            } else if (Cfg.CheckHitCamp == GameDefine.CampGroupTypeAll) {
                camp = 0;
            }
            if (Cfg.HitDestroy == 1) {
                List<UnitBase> unitList = null;
                if (Cfg.CheckHitUnitType == GameDefine.UnitTypeTower) {
                    unitList = GameApp.towerMgr.GetList();
                } else if (Cfg.CheckHitUnitType == GameDefine.UnitTypeSoldier) {
                    unitList = GameApp.soldierMgr.GetList();
                } else if (Cfg.CheckHitUnitType == GameDefine.UnitTypeLive) {
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
                if (Cfg.CheckHitUnitType == GameDefine.UnitTypeTower) {
                    unitList = GameApp.towerMgr.GetList();
                } else if (Cfg.CheckHitUnitType == GameDefine.UnitTypeSoldier) {
                    unitList = GameApp.soldierMgr.GetList();
                } else if (Cfg.CheckHitUnitType == GameDefine.UnitTypeLive) {
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
