using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
    public Entity self;
    private float lastFireTime;

	void Start () {
        self = transform.GetComponent<Entity>();
        lastFireTime = Time.time;
	}
	
	void Update () {
		
	}

    void FixedUpdate () {
        float now = Time.time;
        if (now - lastFireTime > self.cd)
        {
            fire();
            lastFireTime = now;
        }
    }

    void fire ()
    {

    }
}
