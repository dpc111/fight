using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMgr : MonoBehaviour {
    public GameObject gridPrefab;
    public Dictionary<int, GridInfo> grids = new Dictionary<int, GridInfo>();
    public Material unUsedMat;
    public Material usedMat;
    public Material focusMat;

    public Camera raycastCam;
    private Ray ray;
    private RaycastHit gridHit;

    public int high = 20;
    public int width = 20;
    public int rowNum = 10;             // x
    public int colNum = 5;              // z
    public int gridSize = 10;
    public bool isVisible = true;

	void Start () {
        InitGrids();
        ChangeVisible(true);
	}

    void Update()
    //void LateUpdate()
    {
        if (isVisible && GUIImpl.curEntiyTypeId == 0)
        {
            ChangeVisible(false);
        }
        if (!isVisible && GUIImpl.curEntiyTypeId != 0)
        {
            ChangeVisible(true);
        }
        ray = raycastCam.ScreenPointToRay(Input.mousePosition);
        if (GUIImpl.curEntiyTypeId != 0 && Physics.Raycast(ray, out gridHit, 1000, GameStatic.gridMask))
        {
            if (Input.GetMouseButtonUp(0))
            {
                GridInfo gridInfo = gridHit.transform.GetComponent<GridInfo>();
                Net.Event.FireIn("VOnCreateEntity", GUIImpl.curEntiyTypeId, gridInfo.row, gridInfo.col);
                GUIImpl.curEntiyTypeId = 0;
            }
        }
    }

    void OnMouseEnter()
    {
        ChangeVisible(true);
    }

    void OnMouseExit()
    {
        ChangeVisible(false);
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

    public GridInfo GetGridInfo(int row, int col)
    {
        Debug.Log(row);
        Debug.Log(col);
        GridInfo info = null;
        if (grids.TryGetValue(GridId(row, col), out info))
        {
            return info;
        }
        return null;
    }

    public void InitGrids()
    {
        for (int i = 0; i < rowNum; i++)
        {
            for (int j = 0; j < colNum; j++)
            {

                //GameObject gridGO = (GameObject)Instantiate(gridPrefab, transform, false);
                GameObject gridGO = (GameObject)Instantiate(gridPrefab);
                gridGO.transform.position = new Vector3(i * gridSize + gridSize / 2f, 0, j * gridSize + gridSize / 2f);
                gridGO.transform.localScale = new Vector3(gridSize, 0.01f, gridSize);
                gridGO.transform.parent = transform;
                GridInfo info = gridGO.GetComponent<GridInfo>();
                info.center = gridGO.transform.position;
                info.row = i + 1;
                info.col = j + 1;
                grids[GridId(info.row, info.col)] = info;
            }
        }
    }

   
}
