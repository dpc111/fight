using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData {
    public static TowerMgr towerMgr = new TowerMgr();
    public static SoldierMgr soldierMgr = new SoldierMgr();
    public static BulletMgr bulletMgr = new BulletMgr();
    public static TransformMgr transformMgr = new TransformMgr();
    public static BattleLogic battleLogic = new BattleLogic();
    public static LockStepLogic lockStepLogic = new LockStepLogic();
    public static MsgFrame msgFrame = new MsgFrame();
    public static Net.UdpNet udpNet = new Net.UdpNet();
    public static FixRandom fixRandom = new FixRandom(1000);
    //public static Fix fixFrameLen = Fix.FromRaw(273);
    public static Fix timeFrame = Fix.FromRaw(409);
    public static Fix timeCur = Fix.fix0;

    public static void Init()
    {
        towerMgr.Init();
        bulletMgr.Init();
        fixRandom = new FixRandom(1000);
        udpNet.Start();
        msgFrame.Init();
        battleLogic.Init();
        lockStepLogic.Init();
        transformMgr.Init();
        timeCur = Fix.fix0;
    }

    public static void Stop()
    {
        udpNet.Stop();
        battleLogic.StopBattle();
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
