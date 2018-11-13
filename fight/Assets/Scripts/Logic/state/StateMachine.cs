using System.Collections;
using System.Collections.Generic;

public class StateMachine 
{
    int mCurStateType = GameConst.ObjStateNull;
    StateBase mCurState = null;
    LiveObject mObj = null;

    public void UpdateLogic()
    {
        if (mCurState != null)
        {
            mCurState.UpdateLogic();
        }
    }

    public void ChangeState(int state, Fix arg)
    {
        ExitState();
        mCurState = null;
        if (state == GameConst.ObjStateTowerAttack)
            mCurState = new StateTowerAttack();
        else if (state == GameConst.ObjStateTowerStand)
            mCurState = new StateTowerStand();
        else if (state == GameConst.ObjStateNormal)
            mCurState = new StateNormal();
        else if (state == GameConst.ObjStateCooling)
            mCurState = new StateCooling();
        mCurState.OnInit(mObj);
        mCurState.mPrevState = mCurStateType;
        mCurStateType = state;
        mCurState.OnEnter(arg);
    }

    public void SetPrevState(int state)
    {
        mCurState.mPrevState = state;
    }

    public int GetPrevState()
    {
        return mCurState.mPrevState;
    }

    public int GetState()
    {
        return mCurStateType;
    }

    public void ExitState()
    {
        if (mCurState != null)
            mCurState.OnExit();
    }

    public void SetObj(LiveObject obj)
    {
        mObj = obj;
    }
}
