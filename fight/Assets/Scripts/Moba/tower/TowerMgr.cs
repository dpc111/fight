using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMgr
{
    public List<TowerBase> mTowers = new List<TowerBase>();

    public void Init() 
    {
        mTowers.Clear();
    }

    public void AddTower(TowerBase tower)
    {
        if (mTowers.Contains(tower))
            return;
        mTowers.Add(tower);
    }

    public void RemoveTower(TowerBase tower)
    {
        if (!mTowers.Contains(tower))
            return;
        mTowers.Remove(tower);
    }

    public void Update()
    {
        for (int i = 1; i < mTowers.Count; i++)
        {
            TowerBase tower = mTowers[i];
            tower.Update();
        }
    }
}
