using System.Collections;
using System.Collections.Generic;
using System.IO;


public class GameData {
    public static Fix fixFrameLen = Fix.FromRaw(273);
    public static List<BaseTower> listTower = new List<BaseTower>();
    public static List<BaseSoldier> listSoldier = new List<BaseSoldier>();
    public static List<BaseBullet> listBullet = new List<BaseBullet>();
    public static TowerFactory factoryTower = new TowerFactory();
    public static SoldierFactory factorySoldier = new SoldierFactory();
    public static BulletFactory factoryBullet = new BulletFactory();
    public static ActionMgrMgr actionMgrMgr = new ActionMgrMgr();
    public static FixRandom fixRandom = new FixRandom(1000);

    public static void Init()
    {
        fixRandom = new FixRandom(1000);
    }

    public static void Release()
    {
        actionMgrMgr.Release();
    }
}
