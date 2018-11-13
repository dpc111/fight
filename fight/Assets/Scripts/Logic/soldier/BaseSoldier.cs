using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseSoldier : LiveObject
{
    public BaseSoldier()
    {
        Init();
    }

    public override void CheckState()
    {

    }

    virtual public void UpdateLogic()
    {
        mStateMachine.UpdateLogic();
        CheckIsDead();
        CheckEvent();
    }

    void Init()
    {
        mObjType = GameConst.ObjTypeSoldier;
        mStateMachine = new StateMachine();
        mStateMachine.SetObj(this);
    }

    public void Move()
    {
        ChangeState(GameConst.ObjStateSoldierMove);
    }
}
