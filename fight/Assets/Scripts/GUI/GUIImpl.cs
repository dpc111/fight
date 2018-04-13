using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIImpl : MonoBehaviour {
    public GameObject buttonAddEntity1;
    public GameObject buttonAddEntity2;
    public GameObject buttonAddEntity3;

	void Start () {
        buttonAddEntity1 = GameObject.Find("UI Root/Camera/ButtonAddEntity1");
        if (buttonAddEntity1 == null)
        {
            Debug.LogError(".....");
        }
        UIEventListener.Get(buttonAddEntity1).onClick = OnClickButtonAddEntity1;
        buttonAddEntity2 = GameObject.Find("UI Root/Camera/ButtonAddEntity2");
        UIEventListener.Get(buttonAddEntity2).onClick = OnClickButtonAddEntity2;
        buttonAddEntity3 = GameObject.Find("UI Root/Camera/ButtonAddEntity3");
        UIEventListener.Get(buttonAddEntity3).onClick = OnClickButtonAddEntity3;
	}
	
	void Update () {
    }

    public void OnClickButtonAddEntity1(GameObject button)
    {
        CreateEntity(0);
    }

    public void OnClickButtonAddEntity2(GameObject button)
    {
        CreateEntity(1);
    }

    public void OnClickButtonAddEntity3(GameObject button)
    {
        CreateEntity(2);
    }

    void CreateEntity(int entType)
    {
        if (GameStatic.curEntity)
        {
            Destroy(GameStatic.curEntity);
            GameStatic.curEntity = null;
        }
        GameObject obj = GameStatic.entityMgr.CreateEntity(entType);
        GameStatic.curEntity = obj;
    }
}
