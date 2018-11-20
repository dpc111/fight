using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MsgCallback 
{
    public void OnMsg(object msg)
    {
        //Debug.Log(msg.GetType().ToString() + "   " + GameData.lockStepLogic.mLogicFrame);
        string name = msg.GetType().ToString();
        MethodInfo method = this.GetType().GetMethod(name);
        if (method == null)
            return;
        method.Invoke(this, new object[] { msg });
    }

    public void MsgCreateTower(MsgCreateTower msg)
    {
        Debug.Log(msg.type + "," + msg.posx + "," + msg.posy + " frame " + GameData.lockStepLogic.mLogicFrame);
    }
}
