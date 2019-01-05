using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUnity {
    public GameObject mGameObj = null;
    public UnitRange mUnitRange = null;
    public UnitAnimator mAnimator = null;
    public FixVector3 mLastPos = new FixVector3(Fix.fix0, Fix.fix0, Fix.fix0);
    public FixVector3 mNextPos = new FixVector3(Fix.fix0, Fix.fix0, Fix.fix0);
    FixVector3 mRot;
    FixVector3 mScale;
    public int mState = GameDefine.UnitStateNone;
    public int UnitType { get; set; }
    public virtual bool Kill { get; set; }
    public virtual int State {
        get {
            return mState;
        }
        set {
            mState = value;
            mAnimator.SetState(mState);
        }
    }

    public virtual void Init(UnitCfg cfg) {
        mGameObj = ResFactory.prefabs.Create(cfg.Prefab);
        mGameObj.transform.localPosition = GetTransform().Pos.ToVector3();
        if (mGameObj.transform.forward != Vector3.zero && GetTransform().Dir.ToVector3() != Vector3.zero) {
            mGameObj.transform.forward = GetTransform().Dir.ToVector3();
        }
        mLastPos = GetTransform().Pos;
        mNextPos = GetTransform().Pos;
        UnitType = cfg.UnitType;
        mUnitRange = new UnitRange();
        mUnitRange.Init(this);
        mAnimator = new UnitAnimator();
        mAnimator.Init(mGameObj);
        State = GameDefine.UnitStateBorn;
    }

    public virtual void Destory() {
        ResFactory.prefabs.Destory(mGameObj);
        mUnitRange.Destory();
    }

    public virtual void Update() {
        TransformBase transform = GetTransform();
        if (transform.Move) {
            mLastPos = transform.Pos;
            mNextPos = mLastPos + transform.Dir * transform.Speed * GameConst.TimeFrame;
            mGameObj.transform.localPosition = transform.Pos.ToVector3();
        }
    }

    public virtual void OnAttackRangeChange(Fix value) {
        mUnitRange.SetRange(value);
    } 

    public virtual TransformBase GetTransform() {
        return null;
    }

    public virtual Fix GetAttr(int type) {
        return Fix.fix0;
    }

    public void UpdateRender(float interpolation, bool IsUpdateForward) {
        TransformBase transform = GetTransform();
        if (Kill || !transform.Move) {
            return;
        }
        mGameObj.transform.localPosition = Vector3.Lerp(mLastPos.ToVector3(), mNextPos.ToVector3(), interpolation);
        if (IsUpdateForward && transform.Dir.ToVector3() != Vector3.zero) {
            mGameObj.transform.forward = transform.Dir.ToVector3();
        }
    }

    public void UpdateAnimator() {
        mAnimator.Update();
    }

    public void SetScale(FixVector3 value) {
        mScale = value;
        mGameObj.transform.localScale = value.ToVector3();
    }

    public FixVector3 GetScale() {
        return mScale;
    }

    public void SetRotation(FixVector3 value) {
        mRot = value;
        mGameObj.transform.localEulerAngles = value.ToVector3();
        SetVisible(true);
    }

    public FixVector3 GetRotation() {
        return mRot;
    }

    public void SetVisible(bool value) {
        mGameObj.SetActive(value);
    }

    public void SetGameObjectPosition(FixVector3 position) {
        mGameObj.transform.localPosition = position.ToVector3();
    }

    public void SetColor(float r, float g, float b) {
        mGameObj.GetComponent<SpriteRenderer>().color = new Color(r, g, b, 1);
    }
}
