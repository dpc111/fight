using System.Collections;
using System.Collections.Generic;

public class BaseTower : LiveObject 
{
    public BaseTower()
    {
        Init();
    }

    virtual public void UpdateLogic()
    {
        mStateMachine.UpdateLogic();
        CheckIsDead();
        CheckEvent();
    }

    public override void SetPosition(FixVector3 pos)
    {
        mFv3LogicPos = pos;
    }

    public override void CheckState()
    {
        CheckSoldierOutRange();
    }

    void Init()
    {
        mObjType = GameConst.ObjTypeTower;
        mStateMachine = new StateMachine();
        mStateMachine.SetObj(this);
    }

    void CheckSoldierOutRange()
    {
        if (mLockAttackObj == null)
            return;
        Fix distance = FixVector3.Distance(mFv3LogicPos, mLockAttackObj.mFv3LogicPos);
        if (distance > mFixAttackRange)
            SetPrevState(GameConst.ObjStateTowerStand);
    }
}
