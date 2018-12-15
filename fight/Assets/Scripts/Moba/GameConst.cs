﻿using System.Collections;
using System.Collections.Generic;

public class GameConst {
    public const int UnitAttrBegin = 0;
    public const int UnitAttrBlockR = 1;
    public const int UnitAttrSpeed = 1;
    public const int UnitAttrBlood = 1;
    public const int UnitAttrCd = 1;
    public const int UnitAttrDamage = 1;
    public const int UnitAttrDamageRange = 1;
    public const int UnitAttrEnd = 1;

    public const int GroupNull = 0;
    public const int GroupTeamSingle = 1;
    public const int GroupTeamMul = 2;
    public const int GroupEnemySingle = 3;
    public const int GroupEnemyMul = 4;

    public const int ObjTypeNull = 0;
    public const int ObjTypeTower = 1;
    public const int ObjTypeSoldier = 2;
    public const int ObjTypeBullet = 3;
    public const int ObjTypeMagicStand = 4;
    public const int ObjTypeMagicGrizzly = 5;

    public const int SkillCellSize = 5;

    public static Fix XMax = (Fix)100;
    public static Fix ZMax = (Fix)100;

    public static int SkillUnitTarMax = 10;

    public static int CampLeft = 1;
    public static int CampRight = 2;
    public static FixVector3 CampLeftDir = new FixVector3(1, 0, 0);
    public static FixVector3 CampRightDir = new FixVector3(-1, 0, 0);
}

public enum UnitAttrType {
    None = 0,
    Hp,
    Armor,
    MoveSpeed,
    AttackCd,
    AttackRange,
    AttackDamage,
    AttackNum,
    Num
}

public enum BulletType {
    Begin = 0,
    Lock,
    Dir,
    End,
}

public enum SkillType {
    Begin = 0,
    Shoot,
    CreateSoldier,
    Aoe,
    End,
}

public enum SkillMoveType {
    Begin = 0,
    Lock,
    Dir,
    End,
}

public enum BuffType {
    Cac,
    AddHp,
    AddMaxHp,
    SubHp,
    SubMaxHp,
}

public enum BuffUpdateType {
    Once = 1,
    Loop
}

public enum BuffAddType {
    None = 1,
    Time,
    Layer,
    ResetTime,
}

public enum BuffRemoveType {
    All = 1,
    Layer,
}

public enum MoveType {
    Begin = 0,
    Walk,
    Lock,
    Dir,
    Stand,
    End,
}
