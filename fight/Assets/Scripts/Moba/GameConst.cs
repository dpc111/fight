using System.Collections;
using System.Collections.Generic;

public class GameConst {
    public static Fix XMax = (Fix)100;
    public static Fix ZMax = (Fix)100;

    public const int SkillCellSize = 5;
    public static int SkillUnitTarMax = 10;

    public static FixVector3 AixY = new FixVector3(0, 1, 0);
    public static FixVector3 InitForward = new FixVector3(1, 0, 0);
    public static FixVector3 CampLeftDir = new FixVector3(1, 0, 0);
    public static FixVector3 CampRightDir = new FixVector3(-1, 0, 0);
}
