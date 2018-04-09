using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMgr : MonoBehaviour {
    public Dictionary<int, GridInfo> Grids = new Dictionary<int, GridInfo>();
    public Material UnUsedMat;
    public Material UsedMat;
    public bool IsVisible = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ChangeVisible(bool visible)
    {
        if (IsVisible == visible)
        {
            return;
        }
        IsVisible = visible;
        foreach (Transform tran in transform)
        {
            tran.GetComponent<Renderer>().enabled = IsVisible;
        }
    }

    public void SetGridUsed(Transform grid)
    {
        grid.GetComponent<Renderer>().material = UsedMat;
    }


    public void SetGridUnUsed(Transform grid)
    {
        grid.GetComponent<Renderer>().material = UnUsedMat;
    }

    public int GridId(int row, int col)
    {
        return row * 100 + col;
    }
}
