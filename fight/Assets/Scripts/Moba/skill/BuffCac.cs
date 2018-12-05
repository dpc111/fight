﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffCac : BuffBase
{
    public UnitAttrType mAddAttr;
    public Fix mAddValue;
    public UnitAttrType mMulAttr;
    public Fix mMulValue;
    public UnitAttrType mAddCdAttr;
    public Fix mAddCdValue;

    public virtual void Init(int type)
    {
        base.Init(type);
    }

    public virtual void Start()
    {
        if (mAddAttr != UnitAttrType.None)
            mUnitTar.mAttr.AddAttr(mAddAttr, mAddValue);
        if (mMulAttr != UnitAttrType.None)
            mUnitTar.mAttr.MulAttr(mMulAttr, mMulValue + 1);
    }

    public virtual void End()
    {
        
    }

    public virtual void Update()
    {
        base.Update();
        if (mTimeLast - GameData.timeCur < mCd)
            return;
        mTimeLast = GameData.timeCur;
        if (mAddCdAttr != UnitAttrType.None)
            mUnitTar.mAttr.AddAttr(mAddCdAttr, mAddCdValue);
    }

    public virtual void Refresh()
    {
        base.Refresh();
        if (mAddAttr != UnitAttrType.None)
            mUnitTar.mAttr.AddAttr(mAddAttr, mAddValue);
        if (mMulAttr != UnitAttrType.None)
            mUnitTar.mAttr.MulAttr(mMulAttr, mMulValue + 1);
    }
}
