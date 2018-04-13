using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour {
    public bool used = false;
    public Vector3 center = new Vector3(0, 0, 0);

	void Start () {
		
	}
	
	void Update () {
		
	}

    public void SetGridUsed()
    {
        used = true;
        transform.GetComponent<Renderer>().material = GameStatic.gridMgr.usedMat;
    }

    public void SetGridUnUsed()
    {
        used = false;
        transform.GetComponent<Renderer>().material = GameStatic.gridMgr.unUsedMat;
    }

    void OnMouseEnter()
    {
        if (GameStatic.curEntity == null)
        {
            return;
        }
        transform.GetComponent<Renderer>().material = GameStatic.gridMgr.focusMat;
    }

    void OnMouseExit()
    {
        if (used)
        {
            SetGridUsed();
        }
        else
        {
            SetGridUnUsed();
        }
    }

}
