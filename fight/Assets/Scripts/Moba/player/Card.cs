using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card {
    public int mUnitId = 1;
    public Fix mCd = Fix.fix0;
    public Fix mTimeLast = Fix.fix0;
    public Fix mTimeNext = Fix.fix0;

    public void Init() {
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
}
