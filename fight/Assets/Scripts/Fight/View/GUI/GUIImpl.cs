using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIImpl : MonoBehaviour {
    public GameObject buttonAddEntity1;
    public GameObject buttonAddEntity2;
    public GameObject buttonAddEntity3;
    public static int curEntiyTypeId;
    public static GameObject bloodBarPrefab;


	void Start () {
        buttonAddEntity1 = GameObject.Find("UI Root/Camera/ButtonAddEntity1");
        UIEventListener.Get(buttonAddEntity1).onClick = OnClickButtonAddEntity1;
        buttonAddEntity2 = GameObject.Find("UI Root/Camera/ButtonAddEntity2");
        UIEventListener.Get(buttonAddEntity2).onClick = OnClickButtonAddEntity2;
        buttonAddEntity3 = GameObject.Find("UI Root/Camera/ButtonAddEntity3");
        UIEventListener.Get(buttonAddEntity3).onClick = OnClickButtonAddEntity3;
        bloodBarPrefab = Resources.Load("UI/BloodBar") as GameObject;
	}

    public static void SetCurEntityId(int id)
    {
        curEntiyTypeId = id;
    }
	
    public void OnClickButtonAddEntity1(GameObject button)
    {
        SetCurEntityId(1);
    }

    public void OnClickButtonAddEntity2(GameObject button)
    {
        SetCurEntityId(2);
    }

    public void OnClickButtonAddEntity3(GameObject button)
    {
        SetCurEntityId(3);
    }
}
