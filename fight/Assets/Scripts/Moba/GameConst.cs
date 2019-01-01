using System.Collections;
using System.Collections.Generic;

public class GameConst {
    public static int Uid1 = 111;
    public static int Uid2 = 222;
    public static int UidCur = 222;

    public static Fix XMax = (Fix)100;
    public static Fix ZMax = (Fix)100;

    public static int BuffCacSize = 5;
    public static Fix SkillAoeUpdateTime = Fix.FromRaw(1024);

    public const int SkillCellSize = 5;
    public static int SkillUnitTarMax = 10;

    public static FixVector3 AixY = new FixVector3(0, 1, 0);
    public static FixVector3 InitForward = new FixVector3(1, 0, 0);
    public static FixVector3 CampLeftDir = new FixVector3(1, 0, 0);
    public static FixVector3 CampRightDir = new FixVector3(-1, 0, 0);
}
