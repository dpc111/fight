using System.Collections;
using System.Collections.Generic;

public class StateTowerAttack : StateBase 
{
    public StateTowerAttack()
    {

    }

    public override void OnInit(LiveObject obj)
    {
        mObj = obj;
    }

    public override void OnEnter(Fix arg)
    {
        BaseSoldier soldier = (BaseSoldier)mObj.mLockAttackObj;
        GameData.factoryBullet.CreateBullet(mObj, soldier, mObj.mFv3LogicPos, soldier.mFv3LogicPos);
        mObj.ChangeState(GameConst.ObjStateCooling, mObj.mFixAttackSpeed);
    }

    public override void OnExit()
    {

    }

    public override void UpdateLogic()
    {

    }

    public void Init()
    {
        mCurState = GameConst.ObjStateTowerAttack;
    }
}
