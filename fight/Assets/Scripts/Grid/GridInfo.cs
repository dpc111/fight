using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridInfo : MonoBehaviour {
    public bool used = false;
    public Vector3 center = new Vector3(0, 0, 0);

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetGridUsed()
    {
        transform.GetComponent<Renderer>().material = GameStatic.gridMgr.usedMat;
    }

    public void SetGridUnUsed()
    {
        transform.GetComponent<Renderer>().material = GameStatic.gridMgr.unUsedMat;
    }

    void OnMouseEnter()
    {
        //transform.GetComponent<Renderer>().enabled = true;
    }

    void OnMouseExit()
    {
        //transform.GetComponent<Renderer>().enabled = false;
    }

}
