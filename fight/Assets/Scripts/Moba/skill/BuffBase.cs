using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase 
{
    public int mId;
    public int mType;
    public BuffUpdateType mUpdateType = BuffUpdateType.Once;
    public BuffAddType mAddType = BuffAddType.Layer;
    public BuffRemoveType mRemoveType = BuffRemoveType.All;
    public int mMaxLayer = 0;
    public Fix mTime = Fix.fix0;
    public Fix mCd = Fix.fix0;
    public Fix mNum = Fix.fix0;

    public UnitBase mUnitTri = null;
    public UnitBase mUnitTar = null;
    public Fix mTimeAdd = Fix.fix0;
    public Fix mTimeLast = Fix.fix0;

    public virtual void Init(int type)
    {

    }

    public virtual void Start()
    {

    }

    public virtual void End()
    {

    }

    public virtual void Update()
    {
        if (mTime <= Fix.fix0)
            return;
        if (GameData.timeCur - mTimeAdd <= mTime)
            return;
        End();
    }

    public virtual void Refresh()
    {

    }
}
