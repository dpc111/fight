using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;
    public int camp;

	void Start () {
		
	}
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
           
        if (col.gameObject.layer == GameStatic.entityLayer
            && col.gameObject.GetComponent<Entity>().grid != null
            && col.gameObject.GetComponent<Entity>().camp != camp)
        {
            Destroy(gameObject);
            col.gameObject.GetComponent<Entity>().AddBlood(-damage);
        }
    }
}
