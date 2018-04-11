using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatic : MonoBehaviour {
    public static int gridMask;
    public static GridMgr gridMgr;
    public static EntityMgr entityMgr;

    void Awake()
    {
        SetMasks();
    }

	void Start () {
        gridMgr = GameObject.Find("GridMgr").GetComponent<GridMgr>();
        entityMgr = GameObject.Find("EntityMgr").GetComponent<EntityMgr>();
        if (gridMgr == null)
        {
            Debug.LogError("GridMgr");
        }
        if (entityMgr == null)
        {
            Debug.LogError("EntityMgr");
        }
	}

    public static void SetMasks()
    {
        //gridMask = LayerMask.NameToLayer("Grid");
        gridMask = 1 << LayerMask.NameToLayer("Grid");
    }
}
