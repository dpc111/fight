using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUnity {
    public GameObject mGameObj = null;
    //public UnitAnimator mAnimator = null;
    public FixVector3 mLastPos = new FixVector3(Fix.fix0, Fix.fix0, Fix.fix0);
    public FixVector3 mNextPos = new FixVector3(Fix.fix0, Fix.fix0, Fix.fix0);
    FixVector3 mRot;
    FixVector3 mScale;
    public bool mIsKill = false;
    public virtual bool Kill { get { return mIsKill; } set { mIsKill = value; } }
    public int mState = GameDefine.UnitStateNone;
    public virtual int State {
        get {
            return mState;
        }
        set {
            mState = value;
            UnitAnimator ani = mGameObj.GetComponent<UnitAnimator>();
            if (ani != null) {
                ani.SetState(mState);
            }
        }
    }

    public virtual void Init(UnitCfg cfg) {
        mGameObj = ResFactory.prefabs.Create(cfg.Prefab);
        if (mGameObj == null) {
            Debug.LogError(cfg.Prefab);
        }
        mGameObj.transform.localPosition = GetTransform().Pos.ToVector3();
        if (GetTransform().Rol.ToVector3() != Vector3.zero) {
            mGameObj.transform.Rotate(GetTransform().Rol.ToVector3());
        }
        mLastPos = GetTransform().Pos;
        mNextPos = GetTransform().Pos;
        //mAnimator = new UnitAnimator();
        //mAnimator.Init(mGameObj);
        mGameObj.AddComponent<UnitAnimator>();
        State = GameDefine.UnitStateBorn;
    }

    public virtual void Destory() {
        ResFactory.prefabs.Destory(mGameObj);
    }

    public virtual void Update() {
        TransformBase transform = GetTransform();
        if (transform.Move) {
            mLastPos = transform.Pos;
            mNextPos = mLastPos + transform.Dir * transform.Speed * GameApp.timeFrame;
            mGameObj.transform.localPosition = transform.Pos.ToVector3();
        }
    }

    public virtual TransformBase GetTransform() {
        return null;
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
        //mAnimator.Update();
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
