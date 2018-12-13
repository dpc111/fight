using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAttr
{
    public Fix[] mAttrCfg = new Fix[(int)UnitAttrType.Num];
    public Fix[] mAttrOrg = new Fix[(int)UnitAttrType.Num];
    public Fix[] mAttrCur = new Fix[(int)UnitAttrType.Num];

    public void Init() 
    {
        for (int i = 0; i < (int)UnitAttrType.Num; i++)
        {
            mAttrCfg[i] = Fix.fix0;
            mAttrOrg[i] = Fix.fix0;
            mAttrCur[i] = Fix.fix0;
        }
    }

    public void ResetAttr()
    {
        for (int i = 0; i < (int)UnitAttrType.Num; i++)
        {
            mAttrCur[i] = mAttrOrg[i];
        }
    }

    public Fix GetAttr(UnitAttrType attr)
    {
        if (attr < UnitAttrType.None || attr >= UnitAttrType.Num)
            return Fix.fix0;
        return mAttrCur[(int)attr];
    }

    public void SetAttr(UnitAttrType attr, Fix value)
    {
        if (attr < UnitAttrType.None || attr >= UnitAttrType.Num)
            return;
        mAttrCur[(int)attr] = value;
    }

    public void AddAttr(UnitAttrType attr, Fix value)
    {
        if (attr < UnitAttrType.None || attr >= UnitAttrType.Num)
            return;
        mAttrCur[(int)attr] += value; 
    }

    public void MulAttr(UnitAttrType attr, Fix value)
    {
        if (attr < UnitAttrType.None || attr >= UnitAttrType.Num)
            return;
        mAttrCur[(int)attr] *= value;
    }
}
