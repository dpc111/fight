using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using Net;
using battle_msg;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LoginMgr : MonoBehaviour {
    public string gameScene;
    public static string state = "";
    public static GUIImplLogin loginScript = null;
    public static LoginState connState = LoginState.Login;

	void Start () {
        gameScene = "Scenes/Fight";
        loginScript = GameObject.Find("GUI").GetComponent<GUIImplLogin>();
        Net.Event.RegisterOut("OnConnectState", this, "OnConnectState");
        Net.Event.RegisterOut("battle_msg.s_login", this, "s_login");
	}

    void OnDestroy()
    {
        Net.Event.DeregisterOut(this);
        Net.Event.DeregisterIn(this);
    }

    public void OnConnectState(bool success)
    {
        string account = loginScript.inputAccountLabel.GetComponent<UILabel>().text;
        string password = loginScript.inputPasswordLabel.GetComponent<UILabel>().text;
        if (success)
        {
            if (connState == LoginState.Login)
            {
                battle_msg.c_login msg = new battle_msg.c_login();
                msg.uid = Convert.ToInt32(account);
                msg.password = password;
                Net.App.Instance().Send(msg);
            }
            else if (connState == LoginState.Fight)
            {

            }
           
        }
        else
        {
            Debug.Log("connect server failed");
        }
    }

    public void s_login(battle_msg.s_login msg)
    {
        Debug.Log(msg.uid);
        Debug.Log(msg.name);
        Debug.Log(msg.icon);
        connState = LoginState.Fight;
        SceneManager.LoadScene(gameScene);
    }

    enum LoginState
    {
        Login = 1,
        Fight = 2,
    };
}
