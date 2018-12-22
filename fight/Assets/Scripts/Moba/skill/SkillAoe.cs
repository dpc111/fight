using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillAoe : SkillBase {
    public override void Trigger() {
        base.Trigger();
        List<UnitBase> units = GameApp.soldierMgr.GetList();
        for (int i = 0; i < units.Count; i++) {
            UnitBase u = units[i];
            if (!GameTool.IsSameCamp(mUnitTri, u)) {
                continue;
            }
            if (!GameTool.IsInAttackRange(mUnitTri, u)) {
                continue;
            }
        }
    }
}
