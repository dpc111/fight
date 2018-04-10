using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUILogic : MonoBehaviour {
    public GridMgr gridScript;

    public Camera raycastCam;
    private Ray ray;
    private RaycastHit gridHit;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        ray = raycastCam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonUp(0))
        {
            ProcessGrid();
        }
    }

     void ProcessGrid()
    {
        if (Physics.Raycast(ray, out gridHit, 1000, GameStatic.gridMask))
        {
            GameObject grid  = gridHit.transform.gameObject;
            gridScript.SetGridUnUsed(grid.transform);
            Debug.Log("......11");
        }
        else
        {
            Debug.Log("......22");
        }
    }
}
