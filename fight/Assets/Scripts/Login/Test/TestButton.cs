using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestButton : MonoBehaviour {
    public int flag;

	// Use this for initialization
	void Start () {
        flag = 1;
	}

    void OnClick()
    {
        //Debug.Log("on click button");
        if (flag == 1)
        {
            GetComponent<UISprite>().spriteName = "pause";
            //GetComponent<UIButton>().normalSprite = "pause";
            flag = 2;
        }
        else if (flag == 2)
        {
            GetComponent<UISprite>().spriteName = "start";
            //GetComponent<UIButton>().normalSprite = "start";
            flag = 1;
        }
    }
}
