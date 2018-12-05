using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMgrF : MonoBehaviour {
    public Dictionary<int, GameObject> bulletPrefabs = new Dictionary<int, GameObject>();
    public Dictionary<int, GameObject> bullets = new Dictionary<int, GameObject>(); 

	void Start () {
        Init();
	}
	
	void Update () {
		
	}

    public void Init()
    {
        int entityTypeCount = ConfigMgr.GetArrayCount("bullet");
        for (int k = 0; k < entityTypeCount; k++)
        {
            string strBulletRes = (string)ConfigMgr.GetArrayValue("bullet", k, "bullet_prefab");
            GameObject prefab = Resources.Load(strBulletRes) as GameObject;
            if (prefab == null)
            {
                Debug.LogError(strBulletRes + " not exist");
                break;
            }
            int id = k + 1;
            bulletPrefabs[id] = prefab;
        }
    }

    public GameObject CreateBullet(int id, int typeId)
    {
        GameObject bullet = null;
        if (bullets.TryGetValue(id, out bullet))
        {
            return null;
        }
        GameObject prefab = null;
        if (!bulletPrefabs.TryGetValue(typeId, out prefab)) 
        {
            return null;
        }
        bullet = Instantiate(prefab);
        bullets[id] = bullet;
        return bullet;
    }

    public void DestoryBullet(int id)
    {
        GameObject bullet = null;
        if (!bullets.TryGetValue(id, out bullet))
        {
            return;
        }
        bullets.Remove(id);
        Destroy(bullet);
    }
}
