using System.Collections;
using System.Collections.Generic;

public class MoveTo : BaseAction {
    Fix mFixMoveTime = Fix.fixZero;
    Fix mFixMoveElpaseTime = Fix.fixZero;
    FixVector3 mFv3Distance = new FixVector3(Fix.fixZero, Fix.fixZero, Fix.fixZero);
    FixVector3 mFv3StartPos = new FixVector3(Fix.fixZero, Fix.fixZero, Fix.fixZero);
    FixVector3 mFv3EndPos = new FixVector3(Fix.fixZero, Fix.fixZero, Fix.fixZero);

    public override void UpdateLogic()
    {
        bool actionOver = false;
        mFixMoveElpaseTime += GameData.fixFrameLen;
        Fix timeScale = mFixMoveElpaseTime / mFixMoveTime;
        if (timeScale >= (Fix)1)
        {
            timeScale = (Fix)1;
            actionOver = true;
        }
        FixVector3 newPos = mFv3StartPos + mFv3Distance * timeScale;
        mObj.mFv3LogicPos = newPos;
        if (actionOver)
        {
            RemoveSelf();
            if (mActionCb != null)
            {
                mActionCb();
            }
        }
    }

    public void Init(BaseObject unitBody, FixVector3 startPos, FixVector3 endPos, Fix time, ActionCallback cb)
    {
        mKind = GameConst.ActionKindMoveTo;
        mObj = unitBody;
        mObj.mFv3LogicPos = startPos;
        mFixMoveTime = time;
        mFv3Distance = endPos - startPos;
        mFv3StartPos = startPos;
        mFv3EndPos = endPos;
        if (mFixMoveTime == Fix.fixZero)
        {
            mFixMoveTime = (Fix)0.1f;
        }
        mActionCb = cb;
    }
}
