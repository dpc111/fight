using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetObject : MonoBehaviour {
    void Awake()
    {
        // 防止载入新场景时被销毁
        //DontDestroyOnLoad(gameObject);
    }

	void Start () {
        Net.App.Instance().Start();
	}
	
	void Update () {
		
	}

    void FixedUpdate()
    {
        Net.App.Instance().ProcessMain();
    }

    void OnApplicationQuit()
    {
        Net.App.Instance().Quit();
    }
}
