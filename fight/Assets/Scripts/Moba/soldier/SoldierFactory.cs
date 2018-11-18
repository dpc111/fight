using System.Collections;
using System.Collections.Generic;

public class SoldierFactory
{
    public BaseSoldier CreateSoldier()
    {
        BaseSoldier soldier = new Grizzly();
        GameData.listSoldier.Add(soldier);
        soldier.RecordLastPos();
        return soldier;
    }
}
