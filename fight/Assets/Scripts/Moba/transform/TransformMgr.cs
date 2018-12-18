using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformMgr {
    public List<TransformBase> mObjectList = new List<TransformBase>();
    public List<TransformBase> mObjectStatic = new List<TransformBase>();
    public List<TransformBase> mObjectDynamic = new List<TransformBase>();
    public AStar mAstar = new AStar();

    public void Init() {
        mObjectList.Clear();
        mAstar.Init((Fix)200, (Fix)200, (Fix)1);
    }

    public void Add(TransformBase obj) {
        mObjectList.Add(obj);
    }

    public void AddStatic(TransformBase obj) {
        if (mObjectStatic.Contains(obj)) {
            return;
        }
        mObjectStatic.Add(obj);
    }

    public void AddDynamic(TransformBase obj) {
        if (mObjectDynamic.Contains(obj)) {
            return;
        }
        mObjectDynamic.Add(obj);
    }

    public void Remove(TransformBase obj) {
        if (mObjectList.Contains(obj)) {
            mObjectList.Remove(obj);
        }
        if (mObjectStatic.Contains(obj)) {
            mObjectStatic.Remove(obj);
        }
        if (mObjectDynamic.Contains(obj)) {
            mObjectDynamic.Remove(obj);
        }
    }

    public void Update() {
        for (int i = 0; i < mObjectList.Count; i++) {
            mObjectList[i].Update();
        }
    }

    public void FindPathReset(TransformBase tran) {
        mAstar.ResetCach();
        for (int i = 0; i < mObjectList.Count; i++) {
            TransformBase obj = mObjectList[i];
            if (obj == tran) {
                continue;
            }
            obj.mBlock.SetBlockDynamic();
            mAstar.SetBlockDynamic(ref obj.mBlock.mBlockDynamic, obj.mBlock.mBlockLen);
        }
    }

    public bool CheckHit(TransformBase tran, FixVector2 pos) {
        GridPos pos1 = mAstar.GetGridPos(pos);
        if (pos1 == null) {
            return false;
        }
        for (int i = 0; i < mObjectList.Count; i++) {
            TransformBase obj = mObjectList[i];
            if (obj == tran) {
                continue;
            }
            GridPos pos2 = mAstar.GetGridPos(obj.mPos);
            if (pos2 == null) {
                continue;
            }
            if (Mathf.Abs(pos1.x - pos2.x) >= tran.mBlock.mBoxLen || Mathf.Abs(pos1.z - pos2.z) >= tran.mBlock.mBoxLen) {
                continue;
            }
            obj.mBlock.SetBlockDynamic();
            if (obj.mBlock.DynamicHasIndex(pos1.index)) {
                return true;
            }
        }
        return false;
    }
}
