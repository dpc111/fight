using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetObject : MonoBehaviour {
    void Awake()
    {
        // 防止载入新场景时被销毁
        DontDestroyOnLoad(gameObject);
    }

	void Start () {
        Debug.Log("net start .........");
        Net.App.Instance().Start();
	}

    void FixedUpdate()
    {
        Net.App.Instance().ProcessMain();
    }

    void OnApplicationQuit()
    {
        Debug.Log("net quit.........");
        Net.App.Instance().Quit();
    }
}
