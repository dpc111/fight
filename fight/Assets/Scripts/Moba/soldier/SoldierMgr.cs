using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMgr
{
    public List<SoldierBase> mSoldiers = new List<SoldierBase>();

    public void Init()
    {
        mSoldiers.Clear();
    }

    public void AddSoldier(SoldierBase soldier)
    {
        if (mSoldiers.Contains(soldier))
            return;
        mSoldiers.Add(soldier);
    }

    public void RemoveSoldier(SoldierBase soldier)
    {
        if (!mSoldiers.Contains(soldier))
            return;
        mSoldiers.Remove(soldier);
    }

    public void Update()
    {
        for (int i = 0; i < mSoldiers.Count; i++)
        {
            SoldierBase soldier = mSoldiers[i];
            soldier.Update();
        }
    }
}
