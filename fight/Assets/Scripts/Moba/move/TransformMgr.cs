using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMgr 
{
    public List<TransformBase> mObjectList = new List<TransformBase>();
    public AStar mAstar = new AStar();

    public void Init()
    {
        mObjectList.Clear();
        mAstar.Init((Fix)500, (Fix)300, (Fix)5);
    }

    public void AddObject(TransformBase obj)
    {
        mObjectList.Add(obj);
    }

    public void RemoveObject(TransformBase obj)
    {
        mObjectList.Remove(obj);
    }

    public void UpdateLogic()
    {
        mAstar.ResetBlockDynamic();
        for (int i = 0; i < mObjectList.Count; i++)
        {
            mObjectList[i].UpdateLogic();
        }
    }
}
