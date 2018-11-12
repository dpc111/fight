using System.Collections;

// Used for action do over callbacks
public delegate void ActionCallback();
// Used for action do over callbacks
public delegate void ActionCallback<in T>(T value);

public class BaseAction {
    public bool mEnable = true;
    public int mType = GameConst.ActionChangeNull;
    public int mKind = GameConst.ActionKindNull;
    public ActionCallback mActionCb = null;
    public BaseObject mObj = null;
    public ActionMgr mMgr = null;

    public void SetMgr(ActionMgr mgr)
    {
        mMgr = mgr;
    }

    public ActionMgr GetMgr()
    {
        return mMgr;
    }

    public void RemoveSelf()
    {
        mMgr.RemoveAction(this);
    }

    public void SetType(int type)
    {
        mType = type;
    }

    public virtual void UpdateLogic()
    {

    }
}
