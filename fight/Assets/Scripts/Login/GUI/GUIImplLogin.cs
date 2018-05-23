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
	}

    public void OnClickButtonLogin(GameObject button)
    {
        Net.Event.FireIn("OnLoginConnect");
    }

    void OnDestroy()
    {
        Net.Event.DeregisterOut(this);
        Net.Event.DeregisterIn(this);
    }
}
