using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMgrBase<T> where T : UnitBase {
    public List<T> mUnits = new List<T>();

    public virtual void Init() {
        mUnits.Clear();
    }

    public virtual void Add(T unit) {
        if (mUnits.Contains(unit)) {
            return;
        }
        mUnits.Add(unit);
    }

    public virtual void Remove(T unit) {
        if (!mUnits.Contains(unit)) {
            return;
        }
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

    public virtual void UpdateRender(float interpolation, bool IsUpdateForward = true) {
        for (int i = 0; i < mUnits.Count; i++) {
            T unit = mUnits[i];
            if (unit.Kill) {
                continue;
            }
            unit.UpdateRender(interpolation, IsUpdateForward);
        }
    }

    public List<T> GetList() {
        return mUnits;
    }

    public T FindRandomInAttackRange(UnitBase unit, int camp = 0) {
        for (int i = 1; i < mUnits.Count; i++) {
            T u = mUnits[i];
            if (u.Kill) {
                continue;
            }
            if (camp != 0 && camp != u.Camp) {
                continue;
            }
            if (GameTool.IsInAttackRange(unit, u)) {
                return u;
            }
        }
        return null;
    }

    public T FindNearest(UnitBase unit, int camp = 0) {
        T nearestUnit = null;
        Fix nearestSqrDis = Fix.fix0;
        for (int i = 0; i < mUnits.Count; i++) {
            T u = mUnits[i];
            if (u.Kill) {
                continue;
            }
            if (camp != 0 && camp != u.Camp) {
                continue;
            }
            Fix sqrDis = GameTool.SqrDistance(unit, u);
            if (nearestUnit == null) {
                nearestUnit = u;
                nearestSqrDis = sqrDis;
            }
            if (sqrDis < nearestSqrDis) {
                nearestUnit = u;
                nearestSqrDis = sqrDis;
            }
        }
        return nearestUnit;
    }

    public T FindNearestInAttackRange(UnitBase unit, int camp = 0) {
        T nearestUnit = null;
        Fix nearestSqrDis = Fix.fix0;
        for (int i = 0; i < mUnits.Count; i++) {
            T u = mUnits[i];
            if (u.Kill) {
                continue;
            }
            if (camp != 0 && camp != u.Camp) {
                continue;
            }
            if (!GameTool.IsInAttackRange(unit, u)) {
                continue;
            }
            Fix sqrDis = GameTool.SqrDistance(unit, u);
            if (nearestUnit == null) {
                nearestUnit = u;
                nearestSqrDis = sqrDis;
            }
            if (sqrDis < nearestSqrDis) {
                nearestUnit = u;
                nearestSqrDis = sqrDis;
            }
        }
        return nearestUnit;
    }

    public int FindListInAttackRange(UnitBase unit, ref UnitBase[] list, int num, int camp = 0) {
        int index = 0;
        for (int i = 0; i < mUnits.Count; i++) {
            T u = mUnits[i];
            if (u.Kill) {
                continue;
            }
            if (camp != 0 && camp != u.Camp) {
                continue;
            }
            if (!GameTool.IsInAttackRange(unit, u)) {
                continue;
            }
            list[index] = u;
            index++;
            if (index >= num) {
                return index;
            }
        }
        return index;
    }
}
