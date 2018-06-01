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

    void Awake()
    {
        SetMasks();
    }

	void Start () {
        gridMgr = GameObject.Find("GridMgr").GetComponent<GridMgr>();
        entityMgr = GameObject.Find("EntityMgr").GetComponent<EntityMgr>();
        bulletMgr = GameObject.Find("BulletMgr").GetComponent<BulletMgr>();
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
        entityLayer = LayerMask.NameToLayer("Entity");
        gridMask = 1 << LayerMask.NameToLayer("Grid");
        entityMask = 1 << LayerMask.NameToLayer("Entity");
    }
}
