using System.Collections;
using System.Collections.Generic;

public class StateTowerStand : StateBase 
{
    public static Fix mFixTestCount = Fix.fixZero;
    public static string mTestContent = "";

    public StateTowerStand()
    {
        Init();
    }

    public override void OnInit(LiveObject obj)
    {
        mObj = obj;
    }

    public override void OnEnter(Fix arg)
    {
        mObj.PlayAnimation("stand");
    }

    public override void OnExit()
    {

    }

    public override void UpdateLogic()
    {
        for (int i = 0; i < GameData.listSoldier.Count; i++)
        {
            var soldier = GameData.listSoldier[i];
            Fix distance = FixVector3.Distance(mObj.mFv3LogicPos, soldier.mFv3LogicPos);
            if (distance <= mObj.mFixAttackRange)
            {
                mObj.mLockAttackObj = soldier;
                mObj.AddAttackingObj(soldier);
                soldier.AddAttacked(mObj);
                mObj.ChangeState(GameConst.ObjStateTowerAttack);
            }
        }
    }

    public void Init()
    {
        mCurState = GameConst.ObjStateTowerStand;
    }
}
