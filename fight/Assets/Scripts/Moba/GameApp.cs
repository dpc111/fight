using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameApp {
    public static Net.UdpNet udpNet = new Net.UdpNet();
    public static MsgFrame msgFrame = new MsgFrame();
    public static GameLogic battleLogic = new GameLogic();
    public static LockStep lockStepLogic = new LockStep();
    public static CampMgr campMgr = new CampMgr();
    public static UnitMgr towerMgr = new UnitMgr();
    public static UnitMgr soldierMgr = new UnitMgr();
    public static UnitMgr bulletMgr = new UnitMgr();
    public static UnitMgr liveMgr = new UnitMgr();
    public static TransformMgr transformMgr = new TransformMgr();
    public static FixRandom fixRandom = new FixRandom(1000);
    public static Fix timeFrame = Fix.FromRaw(409); // 273
    public static Fix timeCur = Fix.fix0;

    public static void Init() {
        ConfigMgr.Init();
        ResFactory.Init();
        TowerFactory.Init();
        SoldierFactory.Init();
        BulletFactory.Init();
        SkillFactory.Init();
        BuffFactory.Init();
        udpNet.Start();
        msgFrame.Init();
        battleLogic.Init();
        lockStepLogic.Init();
        campMgr.Init();
        towerMgr.Init();
        soldierMgr.Init();
        bulletMgr.Init();
        transformMgr.Init();
        fixRandom = new FixRandom(1000);
        timeCur = Fix.fix0;
        Test();
    }

    public static void Test() {
        if (GameConst.Uid1 == GameConst.UidCur) {
            campMgr.SelfCamp = 1;
        } else {
            campMgr.SelfCamp = 2;
        }
        campMgr.CreatePlayer(GameConst.Uid1, 1);
        campMgr.CreatePlayer(GameConst.Uid2, 2);
    }

    public static void Stop() {
        udpNet.Stop();
        battleLogic.StopBattle();
        msgFrame.Release();
    }

    public static void UpdateLogic() {
        if (!battleLogic.IsRun()) {
            return;
        }
        udpNet.MainProcess();
        lockStepLogic.UpdateLogic();
    }
}
