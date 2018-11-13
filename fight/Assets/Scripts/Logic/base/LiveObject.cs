using System.Collections;
using System.Collections.Generic;

public class LiveObject : BaseObject {
    public Fix mFixHp = Fix.fixZero;
    public Fix mFixOrignalHp = Fix.fixZero;
    public Fix mFixDamage = Fix.fixZero;
    public bool mIsCooling = false;
    public Fix mFixAttackRange = Fix.fixZero;
    public Fix mFixAttackSpeed = Fix.fixZero;
    public LiveObject mLockAttackObj = null;
    public List<LiveObject> mListAttacked = new List<LiveObject>();
    public List<BaseObject> mListAttackedBullet = new List<BaseObject>();
    public List<LiveObject> mListAttacking = new List<LiveObject>();

    public override void KillSelf()
    {
        OnDead();
        base.KillSelf();
    }

    public virtual void LoadProperties()
    {

    }

    public void SetHp(Fix value)
    {
        mFixHp = value;
        mFixOrignalHp = value;
    }

    public void AddAttacked(LiveObject obj)
    {
        if (mListAttacked.Contains(obj))
            return;
        mListAttacked.Add(obj);
    }

    public void RemoveAttacked(LiveObject obj)
    {
        mListAttacked.Remove(obj);
    }

    public void AddAttackedBullet(BaseObject obj)
    {
        if (mListAttackedBullet.Contains(obj))
            return;
        mListAttackedBullet.Add(obj);
    }

    public void RemoveAttackedBullet(BaseObject obj)
    {
        mListAttackedBullet.Remove(obj);
    }

    public void AddAttackingObj(LiveObject obj)
    {
        if (mListAttacking.Contains(obj))
            return;
        mListAttacking.Add(obj);
    }

    public void RemoveAttacking(LiveObject obj)
    {
        mListAttacking.Remove(obj);
    }

    public int GetState()
    {
        return mStateMachine.GetState();
    }

    public void ChangeState(int state)
    {
        mStateMachine.ChangeState(state, (Fix)0);
    }

    public void ChangeState(int state, Fix arg)
    {
        mStateMachine.ChangeState(state, arg);
    }

    public int GetPrevState()
    {
        return mStateMachine.GetPrevState();
    }

    public void SetPrevState(int state)
    {
        mStateMachine.SetPrevState(state);
    }

    public void OnDamage(Fix damage)
    {
        if (mKilled)
            return;
        if (mObjType == GameConst.ObjTypeTower)
        {
            PlayAnimation("Hurt");
            DelayDo((Fix)0.5, delegate () { PlayAnimation("stand"); }, GameConst.ActionDelayToStand);
        }
        mFixHp -= damage;
        if (mFixHp <= Fix.fixZero)
            mKilled = true;
    }

    public void OnDead()
    {
        for (int i = mListAttackedBullet.Count - 1; i >= 0; i--)
        {
            BaseObject obj = mListAttackedBullet[i];
            obj.mUneffect = true;
            RemoveAttackedBullet(obj);
        }
        for (int i = mListAttacking.Count - 1; i >= 0; i--)
        {
            LiveObject obj = mListAttacking[i];
            obj.RemoveAttacked(this);
            RemoveAttacking(obj);
        }
        for (int i = mListAttacked.Count - 1; i >= 0; i--)
        {
            LiveObject obj = mListAttacked[i];
            obj.RemoveAttacking(this);
            RemoveAttacked(obj);
            if (obj.mObjType == GameConst.ObjTypeTower)
            {
                if (obj.GetState() != GameConst.ObjStateCooling)
                    obj.ChangeState(GameConst.ActionTowerStand);
                else
                    obj.SetPrevState(GameConst.ActionTowerStand);
            }
        }
    }
}
