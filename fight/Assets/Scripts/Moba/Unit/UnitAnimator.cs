using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAnimator : MonoBehaviour {
    public int mState = 0;
    public Animator mAnimator = null;

    void Start() {
        mAnimator = GetComponent<Animator>();
        if (mAnimator == null) {
            return;
        }
    }

    //public void Init(GameObject obj) {
    //    mAnimator = obj.GetComponent<Animator>();
    //    if (mAnimator == null) {
    //        return;
    //    }
    //    Debug.LogError("2");
    //}

	void Update () {
        if (mAnimator == null) {
            return;
        }
        if (!mAnimator.IsInTransition(0)) {
            AnimatorStateInfo info = mAnimator.GetCurrentAnimatorStateInfo(0);
            if (info.fullPathHash == Animator.StringToHash("Base Layer.Walk") ||
                info.fullPathHash == Animator.StringToHash("Base Layer.Attack") ||
                info.fullPathHash == Animator.StringToHash("Base Layer.BeAttack")) {
                SetState(GameDefine.UnitStateIdle);
            }
        }
        
	}

    public void SetState(int state) {
        if (mAnimator == null) {
            return;
        }
        mState = state;
        mAnimator.SetInteger("State", state);
    }

}
