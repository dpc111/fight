﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillChangeCamp : SkillBase {
    public override void Trigger(UnitBase tar) {
        base.Trigger(tar);
        //SoldierBase enemy = FightTool.FindEnemySoldierInRangeNearest(mUnitTri);
        //enemy.mCamp = FightTool.CampOther(enemy.mCamp);
        //enemy.Reset();
    }
}
