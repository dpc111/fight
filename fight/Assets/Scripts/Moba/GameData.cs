using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData {
    public static List<BaseTower> listTower = new List<BaseTower>();
    public static List<BaseSoldier> listSoldier = new List<BaseSoldier>();
    public static List<BaseBullet> listBullet = new List<BaseBullet>();
    public static TowerFactory factoryTower = new TowerFactory();
    public static SoldierFactory factorySoldier = new SoldierFactory();
    public static BulletFactory factoryBullet = new BulletFactory();
    public static ActionMgrMgr actionMgrMgr = new ActionMgrMgr();
    public static BattleLogic battleLogic = new BattleLogic();
    public static LockStepLogic lockStepLogic = new LockStepLogic();
    public static MsgFrame msgFrame = new MsgFrame();
    public static Net.UdpNet udpNet = new Net.UdpNet();
    public static FixRandom fixRandom = new FixRandom(1000);
    //public static Fix fixFrameLen = Fix.FromRaw(273);
    //public static Fix fixFrameLen = (Fix)0.1;
    public static Fix fixFrameLen = Fix.FromRaw(409);

    public static void Init()
    {
        fixRandom = new FixRandom(1000);
        udpNet.Start();
        msgFrame.Init();
        battleLogic.Init();
        lockStepLogic.Init();
    }

    public static void Stop()
    {
        udpNet.Stop();
        battleLogic.StopBattle();
        actionMgrMgr.Release();
        msgFrame.Release();
    }

    public static void UpdateLogic()
    {
        if (!battleLogic.IsRun())
            return;
        udpNet.MainProcess();
        lockStepLogic.UpdateLogic();
    }
}
