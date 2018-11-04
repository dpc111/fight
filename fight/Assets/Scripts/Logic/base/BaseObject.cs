using System.Collections;

public class BaseObject : UnityObject 
{
    public MoveTo mMoveToAction = null;
    public string mScName = "";
    public ActionMgr mActionMgr = null;
    public bool mUneffect = false;

    public BaseObject()
    {
        Init();
    }

    public void Init()
    {
        mActionMgr = new ActionMgr();
        GameData.actionMgrMgr.AddActionMgr(mActionMgr);
    }

    public void MoveTo(FixVector3 startPos, FixVector3 endPos, Fix time, ActionCallback cb = null)
    {
        if (mMoveToAction != null)
        {
            return;
        }
        mMoveToAction = new MoveTo();
        mMoveToAction.Init(this, startPos, endPos, time, cb);
        mActionMgr.AddAction(mMoveToAction);
    }

    public void DelayDo(Fix time, ActionCallback cb, string label = null)
    {
        DelayDo delayDoAction = new DelayDo();
        delayDoAction.Init(time, cb);
        if (label != null)
        {
            delayDoAction.SetLable(label);
        }
        mActionMgr.AddAction(delayDoAction);
    }

    public void StopMove()
    {
        if (mMoveToAction == null)
        {
            return;
        }
        mActionMgr.RemoveAction(mMoveToAction);
        mMoveToAction = null;
    }

    public void StopActionByLable(string label)
    {
        mActionMgr.StopActionByLable(label);
    }

    public void StopActionByName(string label)
    {
        mActionMgr.StopActionByName(label);
    }

    public void StopAllAction()
    {
        mActionMgr.StopActionAll();
    }

    public void KillActionMgr()
    {
        mActionMgr.mEnable = false;
    }

    virtual public void SetPosition(FixVector3 pos)
    {
        mFv3LogicPos = pos;
    }

    public FixVector3 GetPosition()
    {
        return mFv3LogicPos;
    }

    public void CheckIsDead()
    {
        if (mKilled)
        {
            KillSelf();
        }
    }

    public void KillSelf()
    {
        StopAllAction();
        KillActionMgr();
        mKilled = true;
        CheckEvent();
    }

    public void RecordLastPos(){
        mFv3LastPos = mFv3LogicPos;
    }

    public void CheckEvent()
    {

    }
}
