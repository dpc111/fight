using System.Collections;
using System.Collections.Generic;

public class GameConst {
    public const int ObjStateNull = 0;
    public const int ObjStateCooling = 1;

    public const int ActionChangeNull = 0;
    public const int ActionChangePrevState = 1;
    public const int ActionTowerStand = 2;
    public const int ActionDelayToStand = 3;

    public const int ActionKindNull = 0;
    public const int ActionKindDelayDo = 1;
    public const int ActionKindMoveTo = 2;

    public const int ObjTypeNull = 0;
    public const int ObjTypeTower = 1;
    public const int ObjTypeSoldier = 2;
    public const int ObjTypeBullet = 3;
}
