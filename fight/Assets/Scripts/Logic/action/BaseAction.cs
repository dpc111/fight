using System.Collections;

// Used for action do over callbacks
public delegate void ActionCallback();
// Used for action do over callbacks
public delegate void ActionCallback<in T>(T value);

public class BaseAction {
    public ActionCallback mActionCb = null;
    public bool mEnable = true;
    public string mScLabel = "";
    public string mScName = "";
    public BaseObject mUnit = null;
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

    public void SetLable(string value)
    {
        mScLabel = value;
    }

    public virtual void UpdateLogic()
    {

    }
}
