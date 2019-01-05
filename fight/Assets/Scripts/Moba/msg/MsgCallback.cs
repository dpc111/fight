using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgCallback {
    public void OnMsg(object[] param) {
        if (param.Length < 2) {
            Debug.LogError(param.Length);
            return;
        }
        string name = param[1].GetType().ToString();
        MethodInfo method = this.GetType().GetMethod(name);
        if (method == null) {
            return;
        }
        method.Invoke(this, param);
    }

    public void OnMsg(object param) {
        string name = param.GetType().ToString();
        MethodInfo method = this.GetType().GetMethod(name);
        if (method == null) {
            return;
        }
        method.Invoke(this, new object[] { param });
    }

    public void MsgCreateTower(int uid, MsgCreateTower msg) {
        Debug.Log(msg.type + "," + msg.posX + "," + msg.posY + " frame " + GameApp.lockStepLogic.mLogicFrame);
    }

    public void MsgCreateUnit(int uid, MsgCreateUnit msg) {
        int index = msg.index;
        FixVector3 pos = new FixVector3(Fix.FromRaw(msg.pos.x), Fix.FromRaw(msg.pos.y), Fix.FromRaw(msg.pos.z));
        // test
        //if ((int)pos.x < 50) {
        //    uid = 111;
        //} else {
        //    uid = 222;
        //}
        Player player = GameApp.campMgr.GetPlayer(uid);
        if (player == null) {
            return;
        }
        int unitId = player.CardUnitId(index);
        if (unitId == 0) {
            return;
        }
        TowerCfg cfg = TowerFactory.GetCfg(unitId);
        if (cfg == null) {
            return;
        }
        Fix blockRange = cfg.BlockRange;
        if (!player.CardIsCoolDown(index)) {
            return;
        }
        if (!GameTool.CanCreateUnit(pos, blockRange)) {
            return;
        }
        player.CardUse(index);
        UnitProperty pro = new UnitProperty();
        pro.Camp = player.mCamp;
        pro.Pos = pos;
        if (pro.Camp == GameDefine.CampLeft) {
            pro.Dir = GameConst.CampLeftDir;
        } else if (pro.Camp == GameDefine.CampRight) {
            pro.Dir = GameConst.CampRightDir;
        }
        TowerBase tower = TowerFactory.Create(unitId, pro);
        UIApp.OnCreateTower(player, index);
    }
}
