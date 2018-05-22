using System.Collections;
using System.Collections.Generic;
using Net;
using battle_msg;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoginMgr : MonoBehaviour {
    public string gameScene;
	void Start () {
        gameScene = "Scenes/Fight";
        Net.Event.RegisterOut("s_login", this, "s_login");
	}

    public void OnApplicationQuit()
    {
        Net.Event.DeregisterOut(this);
        Net.Event.DeregisterIn(this);
    }

    void s_login(battle_msg.s_login msg)
    {
        Debug.Log(msg.uid);
        Debug.Log(msg.name);
        Debug.Log(msg.icon);
        SceneManager.LoadScene(gameScene);
    }
}
