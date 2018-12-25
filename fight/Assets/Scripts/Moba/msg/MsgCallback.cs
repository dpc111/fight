﻿using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgCallback {
    public void OnMsg(object[] param) {
        if (param.Length < 2) {
            Debug.LogError("sssss " + param.Length);
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
        if ((int)pos.x < 50) {
            Debug.Log("11111111111");
            uid = 111;
        } else {
            Debug.Log("22222222222");
            uid = 222;
        }
        Player player = GameApp.campMgr.GetPlayer(uid);
        Debug.Log(uid);
        if (player == null) {
            Debug.LogError(uid);
            return;
        }
        int unitId = player.CardUnitId(index);
        if (unitId == 0) {
            Debug.LogError(index);
            return;
        }
        TowerCfg cfg = TowerFactory.GetCfg(unitId);
        if (cfg == null) {
            Debug.LogError("");
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
            pro.Rol = new FixVector3(0, 0, 0);
        } else if (pro.Camp == GameDefine.CampRight) {
            pro.Rol = new FixVector3(0, 180, 0);
        }
        TowerBase tower = TowerFactory.Create(unitId, pro);
    }
}
