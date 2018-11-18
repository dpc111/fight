using System.Collections;
using System.Collections.Generic;

public class GameConst {
    public const int ObjTypeNull = 0;
    public const int ObjTypeTower = 1;
    public const int ObjTypeSoldier = 2;
    public const int ObjTypeBullet = 3;
    public const int ObjTypeMagicStand = 4;
    public const int ObjTypeMagicGrizzly = 5;

    public const int ObjStateNull = 0;
    public const int ObjStateCooling = 1;
    public const int ObjStateNormal = 2;
    public const int ObjStateTowerAttack = 3;
    public const int ObjStateTowerStand = 4;
    public const int ObjStateSoldierMove = 5;

    public const int ActionChangeNull = 0;
    public const int ActionChangePrevState = 1;
    public const int ActionTowerStand = 2;
    public const int ActionDelayToStand = 3;

    public const int ActionKindNull = 0;
    public const int ActionKindDelayDo = 1;
    public const int ActionKindMoveTo = 2;
}
