using System.Collections;

public class BaseObject : UnityObject 
{
    public int mObjType = GameConst.ObjTypeNull;
    public bool mUneffect = false;
    public MoveTo mActionMoveTo = null;
    public ActionMgr mActionMgr = null;
    public StateMachine mStateMachine = null;

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
        if (mActionMoveTo != null)
            return;
        mActionMoveTo = new MoveTo();
        mActionMoveTo.Init(this, startPos, endPos, time, cb);
        mActionMgr.AddAction(mActionMoveTo);
    }

    public void DelayDo(Fix time, ActionCallback cb, int type = 0)
    {
        DelayDo delayDoAction = new DelayDo();
        delayDoAction.Init(time, cb);
        if (type != GameConst.ActionChangeNull)
        {
            delayDoAction.SetType(type);
        }
        mActionMgr.AddAction(delayDoAction);
    }

    public void StopMove()
    {
        if (mActionMoveTo == null)
            return;
        mActionMgr.RemoveAction(mActionMoveTo);
        mActionMoveTo = null;
    }

    public void StopAction(int type)
    {
        mActionMgr.StopAction(type);
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

    public void RecordLastPos()
    {
        mFv3LastPos = mFv3LogicPos;
    }

    virtual public void KillSelf()
    {
        StopAllAction();
        KillActionMgr();
        if (mStateMachine != null)
            mStateMachine.ExitState();
        mKilled = true;
        CheckEvent();
    }

    public void CheckIsDead()
    {
        if (mKilled)
            KillSelf();
    }

    virtual public void CheckState()
    {

    }

    public void CheckEvent()
    {
        if (!mKilled)
            return;
        mActionMgr.StopActionByKind(GameConst.ActionKindDelayDo);
    }
}
