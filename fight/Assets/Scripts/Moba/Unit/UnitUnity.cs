using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitUnity {
    public UnitBase mUnit = null;
    public GameObject mGameObj;
    public FixVector3 mLastPos = new FixVector3(Fix.fix0, Fix.fix0, Fix.fix0);
    public FixVector3 mNextPos = new FixVector3(Fix.fix0, Fix.fix0, Fix.fix0);
    FixVector3 mRot;
    FixVector3 mScale;


    public void Init(UnitBase unit, UnitCfg cfg) {
        mUnit = unit;
        mGameObj = ResFactory.prefabs.Create(cfg.Prefab, mUnit.mTransform.Pos.ToVector3(), Quaternion.identity);
        if (mGameObj == null) {
            Debug.LogError(cfg.Prefab);
        }
        //mGameObj.transform.localPosition = mUnit.mTransform.Pos.ToVector3();
        //mGameObj.SetActive(true);
    }

    public void Destory() {
        ResFactory.prefabs.Destory(mGameObj);
    }


    public void Update() {
        if (mUnit.mTransform.Move) {
            mLastPos = mUnit.mTransform.Pos;
            mNextPos = mLastPos + mUnit.mTransform.Dir * mUnit.mTransform.Speed * GameApp.timeFrame;
            mGameObj.transform.localPosition = mUnit.mTransform.Pos.ToVector3();
        }

    }

    public void UpdateRender(float interpolation, bool IsUpdateForward) {
        if (mUnit.Kill || !mUnit.mTransform.Move) {
            return;
        }
        mGameObj.transform.localPosition = Vector3.Lerp(mLastPos.ToVector3(), mNextPos.ToVector3(), interpolation);
        if (IsUpdateForward) {
            mGameObj.transform.forward = mUnit.mTransform.Dir.ToVector3();
        }
    }

    public void PlayAnimation(string name) {

    }

    public void PlayAnimationQueue(string name) {

    }

    public void StopAnimation() {
        Animation animation = mGameObj.transform.GetComponent<Animation>();
        if (animation != null)
            animation.Stop();
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

    public void DestroyGameObject() {
        GameObject.Destroy(mGameObj);
        mGameObj.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void SetGameObjectName(string name) {
        mGameObj.name = name;
    }

    public string GetGameOjectName() {
        return mGameObj.name;
    }

    public void SetGameObjectPosition(FixVector3 position) {
        mGameObj.transform.localPosition = position.ToVector3();
    }

    public void SetColor(float r, float g, float b) {
        mGameObj.GetComponent<SpriteRenderer>().color = new Color(r, g, b, 1);
    }
}
