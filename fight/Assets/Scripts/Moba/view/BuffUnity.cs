using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffUnity {
    public GameObject mGameObj = null;
    public string mPrefab = "";

     public virtual void Init(BuffCfg cfg) {
        mGameObj = null;
        mPrefab = cfg.Prefab;
    }

     public virtual void Start() {
         if (mGameObj != null || mPrefab == "") {
             return;
         }
         mGameObj = mGameObj = ResFactory.prefabs.Create(mPrefab);
         if (mGameObj == null) {
             Debug.LogError(mPrefab);
             return;
         }
         mGameObj.transform.localPosition = GetTar().mGameObj.transform.localPosition;
         mGameObj.transform.parent = GetTar().mGameObj.transform;
     }

     public virtual void End() {
         if (mGameObj == null) {
             return;
         }
         GameObject.Destroy(mGameObj);
     }

     public virtual UnitUnity GetTar() {
         return null;
     }
}
