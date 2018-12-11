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
        mAstar.Init((Fix)200, (Fix)200, (Fix)1);
    }

    public void Add(TransformBase obj)
    {
        mObjectList.Add(obj);
    }

    public void Remove(TransformBase obj)
    {
        mObjectList.Remove(obj);
    }

    public void Update()
    {
        mAstar.ResetBlockDynamic();
        for (int i = 0; i < mObjectList.Count; i++)
        {
            mObjectList[i].Update();
        }
    }

    public TransformBase Create(int type)
    {
        TransformBase tran = null;
        if (type == (int)MoveType.Walk)
            tran = new TransformWalk();
        else if (type == (int)MoveType.Lock)
            tran = new TransformLock();
        else if (type == (int)MoveType.Dir)
            tran = new TransformDir();
        else if (type == (int)MoveType.Stand)
            tran = new TransformStand();
        if (tran == null)
            return null;
        Add(tran);
        return tran;
    }
}
