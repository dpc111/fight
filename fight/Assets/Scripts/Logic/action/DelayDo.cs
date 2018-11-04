using System.Collections;

public class DelayDo : BaseAction
{
    Fix mFixPlanTime = Fix.fixZero;
    Fix mFixElapseTime = Fix.fixZero;

    public override void UpdateLogic()
    {
        mFixElapseTime = mFixPlanTime + GameData.fixFrameLen;
        if (mFixElapseTime >= mFixPlanTime)
        {
            RemoveSelf();
            if (mActionCb != null)
            {
                mActionCb();
            }
        }
    }

    public void Init(Fix time, ActionCallback cb)
    {
        mScName = "delaydo";
        mFixPlanTime = time;
        mActionCb = cb;
    }
}
