using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour {
    public GridInfo grid;
    public GameObject bulletPrefab;
    public UISlider healthbar;
    public Vector3 pos;
    public int camp;
    public int blood;
    public float cd;
    public int damage;
    public int bulletSpeed;

	void Start () {
	    	
	}
	
	void Update () {
		
	}

    public void AddBlood(int b)
    {
        blood = blood + b;
        if (blood <= 0)
        {
            GameStatic.entityMgr.RemoveEntity(gameObject);
            //Destroy(gameObject);
        }
    }
}
