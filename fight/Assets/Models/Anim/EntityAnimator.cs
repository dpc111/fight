using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityAnimator : MonoBehaviour {
    public static int stateApper = 1;
    public static int stateIdle = 2;
    public static int stateAttack = 3;
    public static int stateDeath = 4;
    Animator animator;

	void Start () {
        animator = GetComponent<Animator>();
	}

    public void SetState(int state)
    {
        animator.SetInteger("state", state);
    }
	
	void Update () {
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    animator.SetInteger("state", 1);
        //    Debug.Log("1");
        //}
        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    animator.SetInteger("state", 2);
        //    Debug.Log("2");
        //}
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    animator.SetInteger("state", 3);
        //    Debug.Log("3");
        //}
        if (!animator.IsInTransition(0))
        {
            //Debug.LogError("");
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            int state = animator.GetInteger("state");
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Appear"))
            {
                animator.SetInteger("state", 2);
            }
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Idle"))
            {
                //animator.SetInteger("state", 1);                
            }
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Attack"))
            {
               animator.SetInteger("state", 2);
            }
            if (stateInfo.fullPathHash == Animator.StringToHash("Base Layer.Death"))
            {
                //animator.SetInteger("state", 1);
            }
        }
	}
}
