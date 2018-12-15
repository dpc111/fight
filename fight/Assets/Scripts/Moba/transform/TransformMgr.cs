using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMgr {
    public List<TransformBase> mObjectList = new List<TransformBase>();
    public AStar mAstar = new AStar();

    public void Init() {
        mObjectList.Clear();
        mAstar.Init((Fix)200, (Fix)200, (Fix)1);
    }

    public void Add(TransformBase obj) {
        mObjectList.Add(obj);
    }

    public void Remove(TransformBase obj) {
        mObjectList.Remove(obj);
    }

    public void Update() {
        mAstar.ResetBlockDynamic();
        for (int i = 0; i < mObjectList.Count; i++) {
            mObjectList[i].Update();
        }
    }
}
