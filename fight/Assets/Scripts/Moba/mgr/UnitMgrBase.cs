using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMgrBase<T> where T : UnitBase {
    public List<T> mUnits = new List<T>();

    public virtual void Init() {
        mUnits.Clear();
    }

    public virtual void Add(T unit) {
        if (mUnits.Contains(unit))
            return;
        mUnits.Add(unit);
    }

    public virtual void Remove(T unit) {
        if (!mUnits.Contains(unit))
            return;
        mUnits.Remove(unit);
    }

    public virtual void Update() {
        for (int i = 0; i < mUnits.Count; i++) {
            T unit = mUnits[i];
            unit.Update();
        }

        for (int i = mUnits.Count - 1; i >= 0; i--) {
            T unit = mUnits[i];
            if (unit.Kill) {
                unit.Destory();
                Remove(unit);
            }
        }
    }

    public virtual void UpdateRenderPosition(float interpolation) {
        for (int i = 0; i < mUnits.Count; i++) {
            T unit = mUnits[i];
            unit.mUnitUnity.UpdateRenderPosition(interpolation);
        }
    }

    public List<T> GetList() {
        return mUnits;
    }
}
