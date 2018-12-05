using System.Collections;
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


}

public enum UnitAttrType
{
    None = 0,
    Hp,
    Armor,
    MoveSpeed,
    AttCd,
    AttRange,
    AttDamage,
    Num
}

public enum BuffType
{
    Cac,
    AddHp,
    AddMaxHp,
    SubHp,
    SubMaxHp,
}

public enum BuffUpdateType
{
    Once = 1,
    Loop
}

public enum BuffAddType
{
    None = 1,
    Time,
    Layer,
    ResetTime,
}

public enum BuffRemoveType
{
    All = 1,
    Layer,
}
