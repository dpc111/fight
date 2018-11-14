﻿using System.Collections;
using System.Collections.Generic;

public class StateNormal : StateBase 
{
    public StateNormal()
    {
        Init();
    }

    public override void OnInit(LiveObject obj)
    {
        mObj = obj;
    }

    public override void OnEnter(Fix arg)
    {

    }

    public override void OnExit()
    {

    }

    public override void UpdateLogic()
    {

    }

    public void Init()
    {
        mCurState = GameConst.ObjStateNormal;
    }
}
