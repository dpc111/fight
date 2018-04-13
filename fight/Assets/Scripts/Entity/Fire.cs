using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour {
    public Entity self;
    private float lastFireTime;
    public bool open;

	void Start () {
        self = transform.GetComponent<Entity>();
        lastFireTime = Time.time;
        open = false;
	}
	
	void Update () {
		
	}

    void FixedUpdate () {
        if (!open)
        {
            return;
        }
        float now = Time.time;
        if (now - lastFireTime > self.cd)
        {
            fire();
            lastFireTime = now;
        }
    }

    public void OpenFire()
    {
        open = true;
        lastFireTime = Time.time;
    }

    public void CloseFire()
    {
        open = false;
    }

    void fire ()
    {
        if (self.bulletPrefab == null)
        {
            return;
        }
        GameObject bullet = Instantiate(self.bulletPrefab);
        bullet.transform.position = self.pos;
        Rigidbody rig = bullet.GetComponent<Rigidbody>();
        rig.velocity = new Vector3(0, 0, self.bulletSpeed);
        Destroy(bullet, 5);
    }
}
