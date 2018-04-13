using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public int damage;
	void Start () {
		
	}
	
	void Update () {
		
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.layer == GameStatic.entityLayer)
        {
            Destroy(gameObject);
            col.gameObject.GetComponent<Entity>().AddBlood(-damage);
        }
    }
}
