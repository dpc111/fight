using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityMgr : MonoBehaviour {
    public struct EntityType
    {
        public int blood;
        public float cd;
        public int damage;
        public int bulletSpeed;
    }
    public List<GameObject> entityPrefabs = new List<GameObject>();
    public List<GameObject> bulletPrefabs = new List<GameObject>();
    public List<EntityType> entityTypes = new List<EntityType>();
    public Dictionary<int, GameObject> entitys = new Dictionary<int, GameObject>(); 

	void Start () {
        entityTypes.Add(new EntityType { blood = 100, cd = 1f, damage = 3, bulletSpeed = 10 });
        entityTypes.Add(new EntityType { blood = 200, cd = 0.5f, damage = 5, bulletSpeed = 20 });
        entityTypes.Add(new EntityType { blood = 300, cd = 0.3f, damage = 10, bulletSpeed = 30 });
	}
	
	void Update () {
        
	}

    public void AddEntity ()
    {
        if (entityPrefabs.Count <= 0)
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
        GameStatic.curEntity.transform.position = GameStatic.curGridInfo.center + new Vector3(0, 5, 0);
        GameStatic.curEntity.GetComponent<Fire>().open = true;
        GameStatic.curEntity.GetComponent<Entity>().pos = GameStatic.curEntity.transform.position;
        GameStatic.curEntity.GetComponent<Entity>().grid = GameStatic.curGridInfo;
        GameStatic.curEntity.GetComponent<Entity>().SetBloodBar();
        if (GameStatic.curGridInfo.row <= 5)
        {
            GameStatic.curEntity.GetComponent<Entity>().camp = 1;
        }
        else
        {
            GameStatic.curEntity.GetComponent<Entity>().camp = 2;
        }
        GameStatic.curEntity = null;
    }

    public void RemoveEntity(GameObject entity)
    {
        GridInfo gridInfo = entity.GetComponent<Entity>().grid;
        if (gridInfo != null)
        {
            gridInfo.SetGridUnUsed();
        }
        Destroy(entity);
    }

    public GameObject CreateEntity(int entityType)
    {
        GameObject entPrefab = entityPrefabs[entityType];
        GameObject entObject = Instantiate(entPrefab);
        entObject.AddComponent<Entity>();   
        entObject.AddComponent<Fire>();
        entObject.transform.position = Input.mousePosition;
        EntityType entType = entityTypes[entityType];
        Entity ent = entObject.GetComponent<Entity>();
        ent.bloodMax = entType.blood;
        ent.blood = entType.blood;
        ent.cd = entType.cd;
        ent.damage = entType.damage;
        ent.bulletSpeed = entType.bulletSpeed;
        ent.bulletPrefab = bulletPrefabs[entityType];
        return entObject;
    }
}
