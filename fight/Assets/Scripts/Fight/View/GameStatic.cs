using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatic : MonoBehaviour {
    public static int entityLayer;
    public static int gridMask;
    public static int entityMask;
    public static GridMgr gridMgr;
    public static EntityMgr entityMgr;
    public static BulletMgr bulletMgr;
    public static GUIWord guiWord;

    void Awake()
    {
        SetMasks();
    }

	void Start () {
        gridMgr = GameObject.Find("GridMgr").GetComponent<GridMgr>();
        entityMgr = GameObject.Find("EntityMgr").GetComponent<EntityMgr>();
        bulletMgr = GameObject.Find("BulletMgr").GetComponent<BulletMgr>();
        guiWord = GameObject.Find("GUI").GetComponent<GUIWord>();
        if (gridMgr == null)
        {
            Debug.LogError("GridMgr");
        }
        if (entityMgr == null)
        {
            Debug.LogError("EntityMgr");
        }
        if (guiWord == null)
        {
            Debug.LogError("GuiWord");
        }
	}

    public static void SetMasks()
    {
        entityLayer = LayerMask.NameToLayer("Entity");
        gridMask = 1 << LayerMask.NameToLayer("Grid");
        entityMask = 1 << LayerMask.NameToLayer("Entity");
        //int uiLayer = LayerMask.NameToLayer("UI");
        //GameObject camera = GameObject.Find("Main Camera");
        //camera.cullingMask = 1 << uiLayer;
    }
}
