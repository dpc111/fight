using System.Collections;
using System.Collections.Generic;

public class TowerFactory 
{
    public BaseTower CreateTower()
    {
        BaseTower tower = new MagicStand();
        tower.ChangeState(GameConst.ObjStateTowerStand);
        GameData.listTower.Add(tower);
        return tower;
    }
}
