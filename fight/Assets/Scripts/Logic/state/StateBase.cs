using System.Collections;
using System.Collections.Generic;

public class StateBase {
    public LiveObject mObj = null;
    public int mPrevState = GameConst.ObjStateNull;
    public int mCurState = GameConst.ObjStateNull;
    virtual public void OnInit(LiveObject obj)
    {

    }

    virtual public void OnEnter(Fix arg)
    {

    }

    virtual public void OnExit()
    {

    }

    virtual public void UpdateLogic()
    {

    }
}
