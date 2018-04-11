using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMgr : MonoBehaviour {
    public GameObject gridPrefab;
    public Dictionary<int, GridInfo> grids = new Dictionary<int, GridInfo>();
    public Material unUsedMat;
    public Material usedMat;

    public Camera raycastCam;
    private Ray ray;
    private RaycastHit gridHit;

    public int high = 20;
    public int width = 20;
    public int rowNum = 10;
    public int colNum = 5;
    public int gridSize = 20;
    public bool isVisible = true;

	// Use this for initialization
	void Start () {
        initGrids();
        ChangeVisible(true);
	}
	
	// Update is called once per frame
	void Update () {
        ray = raycastCam.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonUp(0))
        {
            if (Physics.Raycast(ray, out gridHit, 1000, GameStatic.gridMask))
            {
                gridHit.transform.GetComponent<GridInfo>().SetGridUsed();
            }
        }
	}

    public void ChangeVisible(bool visible)
    {
        if (isVisible == visible)
        {
            return;
        }
        isVisible = visible;
        foreach (Transform tran in transform)
        {
            tran.GetComponent<Renderer>().enabled = isVisible;
        }
    }

    public int GridId(int row, int col)
    {
        return row * 100 + col;
    }

    public void initGrids()
    {
        for (int i = 1; i <= rowNum; i++)
        {
            for (int j = 1; j <= colNum; j++)
            {
                GameObject gridGO = (GameObject)Instantiate(gridPrefab, transform, false);
                gridGO.transform.position = new Vector3((j - 1) * gridSize + gridSize / 2, 0, (i - 1) * gridSize + gridSize / 2);
                gridGO.transform.localScale = new Vector3(gridSize, 0.01f, gridSize);
                gridGO.transform.parent = transform;
                gridGO.GetComponent<GridInfo>().center = gridGO.transform.position;
            }
        }
    }

   
}
