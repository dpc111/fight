using System.Collections;

public delegate void ActionCallback();
public delegate void ActionCallback<in T>(T value);

public class BaseAction 
{
    public ActionCallback mActionCb = null;
    public BaseObject mObj = null;
    public ActionMgr mMgr = null;
    public int mType = GameConst.ActionChangeNull;
    public int mKind = GameConst.ActionKindNull;
    public bool mEnable = true;

    public virtual void UpdateLogic()
    {

    }

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
}
