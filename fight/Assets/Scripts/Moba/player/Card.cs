using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {
    public int mUnitId = 0;
    public Fix mCd = Fix.fix0;
    public Fix mTimeLast = Fix.fix0;
    public Fix mTimeNext = Fix.fix0;

    public void Init(int id, Fix cd) {
        mUnitId = id;
        mCd = cd;
        Reset();
    }

    public void Update() {

    }

    public bool IsCoolDown() {
        return mTimeNext <= GameData.timeCur;
    }

    public void Reset() {
        mTimeLast = GameData.timeCur;
        mTimeNext = GameData.timeCur + mCd;
    }

    public void Use() {
        Reset();
    }
}
