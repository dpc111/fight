﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameData {
    public static Net.UdpNet udpNet = new Net.UdpNet();
    public static MsgFrame msgFrame = new MsgFrame();
    public static BattleLogic battleLogic = new BattleLogic();
    public static LockStepLogic lockStepLogic = new LockStepLogic();
    public static TowerMgr towerMgr = new TowerMgr();
    public static SoldierMgr soldierMgr = new SoldierMgr();
    public static BulletMgr bulletMgr = new BulletMgr();
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
        udpNet.Start();
        msgFrame.Init();
        battleLogic.Init();
        lockStepLogic.Init();
        towerMgr.Init();
        soldierMgr.Init();
        bulletMgr.Init();
        transformMgr.Init();
        fixRandom = new FixRandom(1000);
        timeCur = Fix.fix0;
        Test();
    }

    public static void Test() {
        TowerFactory.Create(1001, new FixVector3(30, 0, 30));
        UnitBase soldier = SoldierFactory.Create(2001, new FixVector3(10, 0, 40));
        soldier.mTransform.MoveTarget(new FixVector3(100, 0, 50));
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
