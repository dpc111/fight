using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitRange {
    private GameObject mGameObj = null;

    public void Init(UnitUnity u) {
        if (u.UnitType != GameDefine.UnitTypeTower) {
            return;
        }
        Fix range = u.GetAttr(GameDefine.AttrTypeAttackRange);
        if (range <= Fix.fix0 || range >= GameConst.XMax) {
            return;
        }
        mGameObj = ResFactory.prefabs.Create("range");
        mGameObj.transform.localPosition = u.GetTransform().Pos.ToVector3();
        mGameObj.transform.localScale = new Vector3((float)range * 2, 1, (float)range * 2);
        mGameObj.SetActive(UIImpl.IsAttackRangeShow);
    }

    public void Destory() {
        if (mGameObj == null) {
            return;
        }
        ResFactory.prefabs.Destory(mGameObj);
        mGameObj = null;
    }

    public void SetRange(Fix range) {
        if (mGameObj == null) {
            return;
        }
        mGameObj.transform.localScale = new Vector3((float)range * 2, 1, (float)range * 2);
    }

    public void SetActive(bool active) {
        if (mGameObj == null) {
            return;
        }
        mGameObj.SetActive(active);
    }
}
