using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour {
    public Dictionary<int, GameObject> entityPrefabs = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> entitys = new Dictionary<int, GameObject>();

	void Start () {
        Init();
	}
	
	void Update () {
        
	}

    public void Init()
    {
        int entityTypeCount = ConfigMgr.GetArrayCount("entity");
        for (int k = 0; k < entityTypeCount; k++)
        {
            string strEntityRes = (string)ConfigMgr.GetArrayValue("entity", k, "entity_prefab");
            GameObject prefab = Resources.Load(strEntityRes) as GameObject;
            if (prefab == null)
            {
                Debug.LogError(strEntityRes + " not exist");
                break;
            }
            int id = k + 1;
            entityPrefabs[id] = prefab;
        }
    }

    public GameObject CreateEntity(int id, int typeId, int camp)
    {
        GameObject entity = null;
        if (entitys.TryGetValue(id, out entity))
        {
            return null;
        }
        GameObject entPrefab = null;
        if (!entityPrefabs.TryGetValue(typeId, out entPrefab))
        {
            return null;
        }
        entity = Instantiate(entPrefab);
        if (camp == 2)
        {
            entity.transform.Rotate(0, 180, 0);
        }
        //entity.AddComponent<Entity>();
        entitys[id] = entity;
        return entity;
    }

    public void DestroyEnity(int id)
    {
        GameObject entity = null;
        if (!entitys.TryGetValue(id, out entity))
        {
            Debug.Log("");
            return;
        }
        entitys.Remove(id);
        Destroy(entity);
    }
}
