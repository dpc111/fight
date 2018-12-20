using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightTool {
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
        if (camp == GameConst.CampLeft) {
            return GameConst.CampRight;
        } else {
            return GameConst.CampLeft;
        }
    }

    public static FixVector3 CampDir(int camp) {
        if (camp == GameConst.CampLeft) {
            return GameConst.CampLeftDir;
        } else {
            return GameConst.CampRightDir;
        }
    }

    public static bool IsInAttackRange(UnitBase u1, UnitBase u2) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        Fix attackRange = u1.mAttr.GetAttr(UnitAttrType.AttackRange);
        if (Fix.Abs(pos1.x - pos2.x) > attackRange || Fix.Abs(pos1.z - pos2.z) > attackRange) {
            return false;
        }
        if ((pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.z - pos2.z) * (pos1.z - pos2.z) > attackRange * attackRange) {
            return false;
        }
        return true;
    }

    public static bool IsHit(UnitBase u1, UnitBase u2) {
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

    public static bool IsHit(UnitBase u, FixVector3 pos, Fix blockRange) {
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

    public static bool IsHitNextFrame(UnitBase u1, UnitBase u2) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        if (u1.mTransform.Move) {
            pos1 += u1.mTransform.Dir * u1.mTransform.Speed * GameData.timeFrame;
        }
        if (u2.mTransform.Move) {
            pos2 += u2.mTransform.Dir * u2.mTransform.Speed * GameData.timeFrame;
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

    public static bool IsHitEnter(UnitBase u1, UnitBase u2) {
        return !IsHit(u1, u2) && IsHitNextFrame(u1, u2);
    }

    public static bool IsHitExit(UnitBase u1, UnitBase u2) {
        return IsHit(u1, u2) && !IsHitNextFrame(u1, u2);
    }

    public static bool IsOutOfWorld(UnitBase u) {
        FixVector3 pos = u.mTransform.Pos;
        if (pos.x < Fix.fix0 || pos.x > GameConst.XMax || pos.z < Fix.fix0 || pos.z > GameConst.ZMax) {
            return true;
        }
        return false;
    }

    public static Fix SqrDistance(UnitBase u1, UnitBase u2) {
        FixVector3 pos1 = u1.mTransform.Pos;
        FixVector3 pos2 = u2.mTransform.Pos;
        Fix sqrDis = (pos1.x - pos2.x) * (pos1.x - pos2.x) + (pos1.z - pos2.z) * (pos1.z - pos2.z);
        return sqrDis;
    }

    public static bool CanCreateUnit(FixVector3 pos, Fix blockRange) {
        List<UnitBase> list = GameData.liveMgr.GetList();
        for (int i = 0; i < list.Count; i++) {
            UnitBase u = list[i];
            if (IsHit(u, pos, blockRange)) {
                return false;
            }
        }
        return true;
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
