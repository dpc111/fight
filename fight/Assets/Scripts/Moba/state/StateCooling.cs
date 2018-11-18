using System.Collections;
using System.Collections.Generic;

public class StateCooling : StateBase
{
    public StateCooling()
    {
        Init();
    }

    public override void OnInit(LiveObject obj)
    {
        mObj = obj;
    }

    public override void OnEnter(Fix arg)
    {
        mObj.mIsCooling = true;
        Fix cd = arg;
        mObj.DelayDo(cd, delegate()
        {
            if (mPrevState != GameConst.ObjStateNull)
            {
                mObj.CheckState();
                mObj.ChangeState(mPrevState);
            }
        }, GameConst.ActionChangePrevState);
    }

    public override void OnExit()
    {
        mObj.mIsCooling = false;
        mObj.StopAction(GameConst.ActionChangePrevState);
    }

    public override void UpdateLogic()
    {
    
    }

    public void Init()
    {
        mCurState = GameConst.ObjStateCooling;
    }
}
