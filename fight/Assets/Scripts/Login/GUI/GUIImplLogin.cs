using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUIImplLogin : MonoBehaviour
{
    public GameObject inputAccountLabel;
    public GameObject inputPasswordLabel;
    public GameObject buttonLogin;

	void Start () {
        inputAccountLabel = GameObject.Find("UI Root/Container/GridLogin/01_account/Label");
        inputPasswordLabel = GameObject.Find("UI Root/Container/GridLogin/02_password/Label");
        buttonLogin = GameObject.Find("UI Root/Container/GridLogin/03_login");
        UIEventListener.Get(buttonLogin).onClick = OnClickButtonLogin;
        Net.Event.RegisterOut("OnConnectState", this, "OnConnectState");
	}

    public void OnClickButtonLogin(GameObject button)
    {
        
        Net.Event.FireIn("OnLoginConnect");
    }

    public void OnApplicationQuit()
    {
        Net.Event.DeregisterOut(this);
        Net.Event.DeregisterIn(this);
    }

    public void OnConnectState(bool success)
    {
        string account = inputAccountLabel.GetComponent<UILabel>().text;
        string password = inputPasswordLabel.GetComponent<UILabel>().text;
        if (success)
        {
            battle_msg.c_login msg = new battle_msg.c_login();
            msg.uid = Convert.ToInt32(account);
            msg.password = password;
        }
        else
        {
            Debug.Log("connect server failed");
        }
    }
	
}
