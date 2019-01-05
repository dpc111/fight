using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTool {
    public static Fix CfgFix(int v) {
        return (Fix)v / Fix.fix1000;
    }

    public static bool IsSameCamp(UnitBase u1, UnitBase u2) {
        if (u1 == null || u2 == null) {
            return false;
        }
        if (u1.Camp != u2.Camp) {
            return false;
        }
        return true;
    }

    public static int CampOther(int camp) {
        if (camp == GameDefine.CampLeft) {
            return GameDefine.CampRight;
        } else {
            return GameDefine.CampLeft;
        }
    }

    public static FixVector3 CampDir(int camp) {
        if (camp == GameDefine.CampLeft) {
            return GameConst.CampLeftDir;
        } else {
            return GameConst.CampRightDir;
        }
    }

    public static bool IsInAttackRange(UnitBase u1, UnitBase u2) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        Fix len = u1.mAttr.GetAttr(GameDefine.AttrTypeAttackRange) + u2.mTransform.mBlock.BlockRange;
        if (Fix.Abs(pos1.x - pos2.x) > len || Fix.Abs(pos1.z - pos2.z) > len) {
            return false;
        }
        if ((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.z - pos2.z) * (pos1.z - pos2.z) > len * len) {
            return false;
        }
        return true;
    }

    public static bool IsHit2(UnitBase u1, UnitBase u2) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        Fix blockRange = u2.mTransform.mBlock.BlockRange;
        if (Fix.Abs(pos1.x - pos2.x) > blockRange || Fix.Abs(pos1.z - pos2.z) > blockRange) {
            return false;
        }
        if ((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.z - pos2.z) * (pos1.z - pos2.z) > blockRange * blockRange) {
            return false;
        }
        return true;
    }

    public static bool IsHit12(UnitBase u1, Fix range, UnitBase u2) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        Fix len = range + u2.mTransform.mBlock.BlockRange;
        if (Fix.Abs(pos1.x - pos2.x) > len || Fix.Abs(pos1.z - pos2.z) > len) {
            return false;
        }
        if ((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.z - pos2.z) * (pos1.z - pos2.z) > len * len) {
            return false;
        }
        return true;
    }

    public static bool IsHit12(UnitBase u, FixVector3 pos, Fix blockRange) {
        FixVector3 pos1 = u.mTransform.Pos;
        Fix len = blockRange + u.mTransform.mBlock.BlockRange;
        if (Fix.Abs(pos1.x - pos.x) > len || Fix.Abs(pos1.z - pos.z) > len) {
            return false;
        }
        if ((pos1.x - pos.x) * (pos1.x - pos.x) + (pos1.z - pos.z) * (pos1.z - pos.z) > len * len) {
            return false;
        }
        return true;
    }

    public static bool IsHit2(UnitBase u1, UnitBase u2, Fix blockRange) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        if (Fix.Abs(pos1.x - pos2.x) > blockRange || Fix.Abs(pos1.z - pos2.z) > blockRange) {
            return false;
        }
        if ((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.z - pos2.z) * (pos1.z - pos2.z) > blockRange * blockRange) {
            return false;
        }
        return true;
    }

    public static bool IsHit2NextFrame(UnitBase u1, UnitBase u2) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        if (u1.mTransform.Move) {
            pos1 += u1.mTransform.Dir * u1.mTransform.Speed * GameConst.TimeFrame;
        }
        if (u2.mTransform.Move) {
            pos2 += u2.mTransform.Dir * u2.mTransform.Speed * GameConst.TimeFrame;
        }
        Fix blockRange = u2.mTransform.mBlock.BlockRange;
        if (Fix.Abs(pos1.x - pos2.x) > blockRange || Fix.Abs(pos1.z - pos2.z) > blockRange) {
            return false;
        }
        if ((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.z - pos2.z) * (pos1.z - pos2.z) > blockRange * blockRange) {
            return false;
        }
        return true;
    }

    public static bool IsHit2NextFrame(UnitBase u1, UnitBase u2, Fix blockRange) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        if (u1.mTransform.Move) {
            pos1 += u1.mTransform.Dir * u1.mTransform.Speed * GameConst.TimeFrame;
        }
        if (u2.mTransform.Move) {
            pos2 += u2.mTransform.Dir * u2.mTransform.Speed * GameConst.TimeFrame;
        }
        if (Fix.Abs(pos1.x - pos2.x) > blockRange || Fix.Abs(pos1.z - pos2.z) > blockRange) {
            return false;
        }
        if ((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.z - pos2.z) * (pos1.z - pos2.z) > blockRange * blockRange) {
            return false;
        }
        return true;
    }

    public static bool IsHit2Enter(UnitBase u1, UnitBase u2) {
        return !IsHit2(u1, u2) && IsHit2NextFrame(u1, u2);
    }

    public static bool IsHit2Exit(UnitBase u1, UnitBase u2) {
        return IsHit2(u1, u2) && !IsHit2NextFrame(u1, u2);
    }

    public static bool IsHit2Enter(UnitBase u1, UnitBase u2, Fix blockRange) {
        return !IsHit2(u1, u2, blockRange) && IsHit2NextFrame(u1, u2, blockRange);
    }

    public static bool IsHit2Exit(UnitBase u1, UnitBase u2, Fix blockRange) {
        return IsHit2(u1, u2, blockRange) && !IsHit2NextFrame(u1, u2, blockRange);
    }

    public static bool IsOutOfWorld(UnitBase u) {
        FixVector3 pos = u.mTransform.Pos;
        if (pos.x < Fix.fix0 || pos.x > GameConst.XMax || pos.z < Fix.fix0 || pos.z > GameConst.ZMax) {
            return true;
        }
        return false;
    }

    public static void PosLimitIntoWorld(ref FixVector3 pos) {
        if (pos.x < Fix.fix0) {
            pos.x = Fix.fix0;
        }
        if (pos.z < Fix.fix0) {
            pos.z = Fix.fix0;
        }
        if (pos.x > GameConst.XMax) {
            pos.x = GameConst.XMax;
        }
        if (pos.z > GameConst.ZMax) {
            pos.z = GameConst.ZMax;
        }
    }

    public static Fix SqrDistance(UnitBase u1, UnitBase u2) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        Fix sqrDis = (pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.z - pos2.z) * (pos1.z - pos2.z);
        return sqrDis;
    }

    public static bool CanCreateUnit(FixVector3 pos, Fix blockRange) {
        List<UnitBase> list = GameApp.liveMgr.GetList();
        for (int i = 0; i < list.Count; i++) {
            UnitBase u = list[i];
            if (IsHit12(u, pos, blockRange)) {
                return false;
            }
        }
        return true;
    }

    public static int WhichPosNearer(FixVector3 pos, FixVector3 pos1, FixVector3 pos2) {
        if (FixVector3.SqrDistance(pos, pos1) < FixVector3.SqrDistance(pos, pos2)) {
            return 1;
        }
        return 2;
    }

    //public static UnitBase FindEnemySoldier(UnitBase unit) {
    //    List<SoldierBase> units = GameData.soldierMgr.GetList();
    //    for (int i = 0; i < units.Count; i++) {
    //        UnitBase u = units[i];
    //        if (FightTool.IsSameCamp(unit, u)) {
    //            continue;
    //        }
    //        if (!FightTool.IsInAttackRange(unit, u)) {
    //            continue;
    //        }
    //        return u;
    //    }
    //    return null;
    //}

    //public static UnitBase FindEnemyTower(UnitBase unit) {
    //    List<TowerBase> units = GameData.towerMgr.GetList();
    //    for (int i = 0; i < units.Count; i++) {
    //        UnitBase u = units[i];
    //        if (FightTool.IsSameCamp(unit, u)) {
    //            continue;
    //        }
    //        if (!FightTool.IsInAttackRange(unit, u)) {
    //            continue;
    //        }
    //        return u;
    //    }
    //    return null;
    //}

    //public static int FindEnemySoldierList(UnitBase unit, ref UnitBase[] list, int num) {
    //    List<SoldierBase> units = GameData.soldierMgr.GetList();
    //    int index = 0;
    //    for (int i = 0; i < units.Count; i++) {
    //        UnitBase u = units[i];
    //        if (FightTool.IsSameCamp(unit, u)) {
    //            continue;
    //        }
    //        if (!FightTool.IsInAttackRange(unit, u)) {
    //            continue;
    //        }
    //        list[index] = u;
    //        index++;
    //        if (index >= num) {
    //            return index;
    //        }
    //    }
    //    return index;
    //}

    //public static int FindEnemyTowerList(UnitBase unit, ref UnitBase[] list, int num) {
    //    List<TowerBase> units = GameData.towerMgr.GetList();
    //    int index = 0;
    //    for (int i = 0; i < units.Count; i++) {
    //        UnitBase u = units[i];
    //        if (FightTool.IsSameCamp(unit, u)) {
    //            continue;
    //        }
    //        if (!FightTool.IsInAttackRange(unit, u)) {
    //            continue;
    //        }
    //        list[index] = u;
    //        index++;
    //        if (index >= num) {
    //            return index;
    //        }
    //    }
    //    return index;
    //}

    //public static UnitBase FindEnemyTowerNearest(UnitBase unit) {
    //    List<TowerBase> units = GameData.towerMgr.GetList();
    //    UnitBase nearestUnit = null;
    //    Fix nearestSqrDis = Fix.fix0;
    //    for (int i = 0; i < units.Count; i++) {
    //        UnitBase u = units[i];
    //        if (FightTool.IsSameCamp(unit, u)) {
    //            continue;
    //        }
    //        Fix sqrDis = FightTool.SqrDistance(unit, u);
    //        if (nearestUnit == null) {
    //            nearestUnit = u;
    //            nearestSqrDis = sqrDis;
    //        }
    //        if (sqrDis < nearestSqrDis) {
    //            nearestUnit = u;
    //            nearestSqrDis = sqrDis;
    //        }
    //    }
    //    return nearestUnit;
    //}

    //public static UnitBase FindEnemyTowerInRangeNearest(UnitBase unit) {
    //    List<TowerBase> units = GameData.towerMgr.GetList();
    //    UnitBase nearestUnit = null;
    //    Fix nearestSqrDis = Fix.fix0;
    //    for (int i = 0; i < units.Count; i++) {
    //        UnitBase u = units[i];
    //        if (FightTool.IsSameCamp(unit, u)) {
    //            continue;
    //        }
    //        if (!FightTool.IsInAttackRange(unit, u)) {
    //            continue;
    //        }
    //        Fix sqrDis = FightTool.SqrDistance(unit, u);
    //        if (nearestUnit == null) {
    //            nearestUnit = u;
    //            nearestSqrDis = sqrDis;
    //        }
    //        if (sqrDis < nearestSqrDis) {
    //            nearestUnit = u;
    //            nearestSqrDis = sqrDis;
    //        }
    //    }
    //    return nearestUnit;
    //}

    //public static SoldierBase FindEnemySoldierNearest(UnitBase unit) {
    //    List<SoldierBase> units = GameData.soldierMgr.GetList();
    //    SoldierBase nearestUnit = null;
    //    Fix nearestSqrDis = Fix.fix0;
    //    for (int i = 0; i < units.Count; i++) {
    //        SoldierBase u = units[i];
    //        if (FightTool.IsSameCamp(unit, u)) {
    //            continue;
    //        }
    //        Fix sqrDis = FightTool.SqrDistance(unit, u);
    //        if (nearestUnit == null) {
    //            nearestUnit = u;
    //            nearestSqrDis = sqrDis;
    //        }
    //        if (sqrDis < nearestSqrDis) {
    //            nearestUnit = u;
    //            nearestSqrDis = sqrDis;
    //        }
    //    }
    //    return nearestUnit;
    //}

    //public static SoldierBase FindEnemySoldierInRangeNearest(UnitBase unit) {
    //    List<SoldierBase> units = GameData.soldierMgr.GetList();
    //    SoldierBase nearestUnit = null;
    //    Fix nearestSqrDis = Fix.fix0;
    //    for (int i = 0; i < units.Count; i++) {
    //        SoldierBase u = units[i];
    //        if (FightTool.IsSameCamp(unit, u)) {
    //            continue;
    //        }
    //        if (!FightTool.IsInAttackRange(unit, u)) {
    //            continue;
    //        }
    //        Fix sqrDis = FightTool.SqrDistance(unit, u);
    //        if (nearestUnit == null) {
    //            nearestUnit = u;
    //            nearestSqrDis = sqrDis;
    //        }
    //        if (sqrDis < nearestSqrDis) {
    //            nearestUnit = u;
    //            nearestSqrDis = sqrDis;
    //        }
    //    }
    //    return nearestUnit;
    //}

    //public static UnitBase FindEnemyUnitNearest(UnitBase unit) {
    //    UnitBase tower = FightTool.FindEnemyTowerNearest(unit);
    //    UnitBase soldier = FightTool.FindEnemySoldierNearest(unit);
    //    if (tower == null) {
    //        return soldier;
    //    }
    //    if (soldier == null) {
    //        return tower;
    //    }
    //    if (FightTool.SqrDistance(unit, tower) < FightTool.SqrDistance(unit, soldier)) {
    //        return tower;
    //    } else {
    //        return soldier;
    //    }
    //}

    //public static UnitBase FindEnemyUnitInRangeNearest(UnitBase unit) {
    //    UnitBase tower = FightTool.FindEnemyTowerInRangeNearest(unit);
    //    UnitBase soldier = FightTool.FindEnemySoldierInRangeNearest(unit);
    //    if (tower == null) {
    //        return soldier;
    //    }
    //    if (soldier == null) {
    //        return tower;
    //    }
    //    if (FightTool.SqrDistance(unit, tower) < FightTool.SqrDistance(unit, soldier)) {
    //        return tower;
    //    } else {
    //        return soldier;
    //    }
    //}
}
