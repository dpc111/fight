﻿using UnityEngine;
using System.Collections;

public class UnityObject
{
    public string mScBundle = "";
    public string mScAsset = "";
    public int mObjType = GameConst.ObjTypeNull;
    public bool mKilled = false;
    public GameObject mGameObj;
    public FixVector3 mFv3LastPos = new FixVector3(Fix.fixZero, Fix.fixZero, Fix.fixZero);
    public FixVector3 mFv3LogicPos = new FixVector3(Fix.fixZero, Fix.fixZero, Fix.fixZero);
    FixVector3 mFv3LogicRot;
    FixVector3 mFv3LogicScale;

    public void CreateFromPrefab(string path, UnityObject script)
    {
        Prefabs.Create(path, script);
    }

    public void UpdateRenderPosition(float interpolation)
    {
        if (mKilled)
            return;
        if ((mObjType == GameConst.ObjTypeSoldier || mObjType == GameConst.ObjTypeBullet) && interpolation != 0)
            mGameObj.transform.localPosition = Vector3.Lerp(mFv3LastPos.ToVector3(), mFv3LogicPos.ToVector3(), interpolation);
        else
            mGameObj.transform.localPosition = mFv3LogicPos.ToVector3();
    }

    public void PlayAnimation(string name)
    {

    }

    public void PlayAnimationQueue(string name)
    {

    }

    public void StopAnimation()
    {
        Animation animation = mGameObj.transform.GetComponent<Animation>();
        if (animation != null)
            animation.Stop();
    }

    public void SetScale(FixVector3 value)
    {
        mFv3LogicScale = value;
        mGameObj.transform.localScale = value.ToVector3();
    }

    public FixVector3 GetScale()
    {
        return mFv3LogicScale;
    }

    public void SetRotation(FixVector3 value)
    {
        mFv3LogicRot = value;
        mGameObj.transform.localEulerAngles = value.ToVector3();
        SetVisible(true);
    }

    public FixVector3 GetRotation()
    {
        return mFv3LogicRot;
    }

    public void SetVisible(bool value)
    {
        mGameObj.SetActive(value);
    }

    public void DestroyGameObject()
    {
        GameObject.Destroy(mGameObj);
        mGameObj.transform.localPosition = new Vector3(10000, 10000, 0);
    }

    public void SetGameObjectName(string name)
    {
        mGameObj.name = name;
    }

    public string GetGameOjectName()
    {
        return mGameObj.name;
    }

    public void SetGameObjectPosition(FixVector3 position)
    {
        mGameObj.transform.localPosition = position.ToVector3();
    }

    public void SetColor(float r, float g, float b)
    {
        mGameObj.GetComponent<SpriteRenderer>().color = new Color(r, g, b, 1);
    }
}