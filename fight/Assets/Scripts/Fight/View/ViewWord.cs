using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Net;

public class ViewWord : MonoBehaviour {

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	void Start () {
        DeregisterEvents();
        RegisterEvents();
	}
	
	void Update () {
		
	}

    public void DeregisterEvents()
    {
        Net.Event.DeregisterIn(this);
        Net.Event.DeregisterOut(this);
    }

    public void RegisterEvents()
    {
        Net.Event.RegisterOut("OnEntityCreate", this, "OnEntityCreate");
        Net.Event.RegisterOut("OnEntityDestroy", this, "OnEntityDestroy");
        Net.Event.RegisterOut("OnBulletCreate", this, "OnBulletCreate");
        Net.Event.RegisterOut("OnBulletDestory", this, "OnBulletDestroy");
    }

    public void OnEntityCreate(Game.Entity entity)
    {
        GameObject entObject = GameStatic.entityMgr.CreateEntity(entity.id, entity.typeId);
        entity.renderObj = entObject;
        entObject.transform.position = entity.pos;
        Debug.Log(entity.pos.x);
        Debug.Log(entity.pos.y);
        Debug.Log(entity.pos.z);
        entObject.GetComponent<Entity>().id = entity.id;
        entObject.GetComponent<Entity>().camp = entity.camp;
        GameStatic.gridMgr.GetGridInfo(entity.row, entity.col).SetGridUsed();
    }

    public void OnEntityDestroy(Game.Entity entity)
    {
        GameStatic.entityMgr.DestroyEnity(entity.id);
        GameStatic.gridMgr.GetGridInfo(entity.row, entity.col).SetGridUnUsed();
    }

    public void OnBulletCreate(Game.Bullet bullet)
    {
        GameObject bulletObject = GameStatic.bulletMgr.CreateBullet(bullet.id, bullet.typeId);
        bullet.renderObj = bulletObject;
        bulletObject.transform.position = bullet.pos;
        Rigidbody rig = bulletObject.GetComponent<Rigidbody>();
        rig.velocity = bullet.speed;
        bulletObject.GetComponent<Bullet>().camp = bullet.camp;
    }

    public void OnBulletDestroy(Game.Bullet bullet)
    {
        GameStatic.bulletMgr.DestoryBullet(bullet.id);
    }
}
