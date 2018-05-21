using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        string account = inputAccountLabel.GetComponent<UILabel>().text;
        string password = inputPasswordLabel.GetComponent<UILabel>().text;
        Debug.Log(account);
        Debug.Log(password);
    }
	
}
