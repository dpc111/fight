using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffMgr
{
    private UnitBase mUnit = null;
    private List<BuffBase> mTarBuffs = new List<BuffBase>();
    private List<BuffBase> mTriBuffs = new List<BuffBase>();

    public void Init(UnitBase unit)
    {
        mUnit = unit;
        mTarBuffs.Clear();
        mTriBuffs.Clear();
    }

    public void Add(BuffBase buff)
    {
        if (mTarBuffs.Contains(buff))
            return;
        mTarBuffs.Add(buff);
    }

    public void AddTri(BuffBase buff)
    {
        if (mTriBuffs.Contains(buff))
            return;
        mTriBuffs.Add(buff);
    }

    public void Remove(BuffBase buff)
    {
        if (!mTarBuffs.Contains(buff))
            return;
        mTarBuffs.Remove(buff);
    }

    public void RemoveTri(BuffBase buff)
    {
        if (!mTriBuffs.Contains(buff))
            return;
        mTriBuffs.Remove(buff);
    }

    public void Update()
    {
        for (int i = 0; i < mTarBuffs.Count; i++)
        {
            BuffBase buff = mTarBuffs[i];
            buff.Update(); 
        }
    }

    public bool HasBuff(int type)
    {
        for (int i = 0; i < mTarBuffs.Count; i++)
        {
            BuffBase buff = mTarBuffs[i];
            if (buff.mType == type)
                return true;
        }
        return false;
    }

    public void Refresh(BuffBase buff)
    {
        if (buff.mType == (int)BuffType.Cac)
        {
            mUnit.mAttr.ResetAttr();
            for (int i = 0; i < mTarBuffs.Count; i++)
            {
                BuffBase b = mTarBuffs[i];
                if (b.mType != (int)BuffType.Cac)
                    continue;
                b.Refresh();
            }
        }
    }
}
