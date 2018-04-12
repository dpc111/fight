using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour {
    public List<GameObject> entityPerfabs = new List<GameObject>();
    public Dictionary<int, GameObject> entitys = new Dictionary<int, GameObject>(); 

	void Start () {
		
	}
	
	void Update () {
        
	}

    public void AddEntity ()
    {
        if (entityPerfabs.Count <= 0)
        {
            return;
        }
        if (GameStatic.curGridInfo == null)
        {
            return;
        }
        if (GameStatic.curEntity == null)
        {
            return;
        }
        //int rand = Random.Range(0, entityPerfabs.Count);
        //GameObject entPrefab = entityPerfabs[GameStatic.];
        //GameObject entObject = Instantiate(entPrefab);
        //entObject.transform.position = GameStatic.curGridInfo.center + new Vector3(0, 5, 0);
        GameStatic.curEntity.transform.position = GameStatic.curGridInfo.center + new Vector3(0, 5, 0);
        GameStatic.curEntity = null;
    }

    public GameObject CreateEntity(int entityType)
    {
        GameObject entPrefab = entityPerfabs[entityType];
        GameObject entObject = Instantiate(entPrefab);
        entObject.transform.position = Input.mousePosition;
        return entObject;
    }
}
