using System.Collections;
using System.Collections.Generic;

public class StateMachine 
{
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
        return mCurState.mCurState;
    }

    public void ExitState()
    {
        if (mCurState != null)
        {
            mCurState.OnExit();
        }
    }

    public void SetObj(LiveObject obj)
    {
        mObj = obj;
    }
}
