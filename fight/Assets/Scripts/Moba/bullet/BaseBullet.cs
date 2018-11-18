using System.Collections;
using System.Collections.Generic;

public class BaseBullet : LiveObject 
{
    protected Fix mFixDamage = Fix.fixZero;
    protected LiveObject mSrc = null;
    protected LiveObject mDest = null;
    protected FixVector3 mFv3SrcPos = new FixVector3();
    protected FixVector3 mFv3DestPos = new FixVector3();

    virtual public void InitData(LiveObject src, LiveObject dest, FixVector3 srcPos, FixVector3 destPos)
    {
        mObjType = GameConst.ObjTypeBullet;
        LoadProperties();
        mSrc = src;
        mDest = dest;
        mFv3SrcPos = srcPos;
        mFv3DestPos = destPos;
        mFixDamage = (Fix)20;
        mDest.AddAttackedBullet(this);
    }

    virtual public void LoadProperties()
    {

    }

    virtual public void CreateBody(string name)
    {

    }

    virtual public void Shoot()
    {

    }

    virtual public void DoShootDest()
    {
        if (!mUneffect)
        {
            mDest.mListAttackedBullet.Remove(this);
            mDest.OnDamage(mFixDamage);
        }
        mKilled = true;
    }

    virtual public void UpdateLogic()
    {
        CheckEvent();
    }
}
