using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAoe : SkillBase {
    public override void Trigger() {
        base.Trigger();
        List<SoldierBase> units = GameData.soldierMgr.GetList();
        for (int i = 0; i < units.Count; i++) {
            UnitBase u = units[i];
            if (!FightTool.IsSameCamp(mUnitTri, u)) {
                continue;
            }
            if (!FightTool.IsInAttackRange(mUnitTri, u)) {
                continue;
            }
        }
    }
}
