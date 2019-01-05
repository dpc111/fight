using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICooling : MonoBehaviour {
    private float mTimeCd = 1.0f;
    private float mTimeCur = 0.0f;
    private Image mImage = null;

    void Awake() {
        mTimeCur = mTimeCd;
        mImage = GetComponent<Image>();
    }

    void Update() {
        if (mTimeCur < mTimeCd) {
            mTimeCur += Time.deltaTime;
            mImage.fillAmount = mTimeCur / mTimeCd;
        }
    }

    public void Reset() {
        mTimeCur = 0.0f;
        mImage.fillAmount = 0.0f;
    }

    public void Reset(float cd) {
        Reset();
        mTimeCd = cd;
    }
}
