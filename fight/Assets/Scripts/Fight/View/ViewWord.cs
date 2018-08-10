using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Net;

public class ViewWord : MonoBehaviour {
    void Awake()
    {
        //DontDestroyOnLoad(transform.gameObject);
    }

	void Start () {
        DeregisterEvents();
        RegisterEvents();
        Net.Event.FireIn("VFightSenceLoadOver");
	}

    void Update()
    {

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
        Net.Event.RegisterOut("OnBulletDestroy", this, "OnBulletDestroy");
        Net.Event.RegisterOut("OnBulletUpdateState", this, "OnBulletUpdateState");
        Net.Event.RegisterOut("OnEntityBore", this, "OnEntityBore");
        Net.Event.RegisterOut("OnEntityFire", this, "OnEntityFire");
        Net.Event.RegisterOut("OnEntityDeath", this, "OnEntityDeath");
    }

    public void OnEntityCreate(Game.Entity entity)
    {
        GameObject entObject = GameStatic.entityMgr.CreateEntity(entity.id, entity.typeId, entity.camp);
        entity.renderObj = entObject;
        entObject.transform.position = entity.pos;
        Debug.Log(entity.pos.x);
        Debug.Log(entity.pos.y);
        Debug.Log(entity.pos.z);
        GameStatic.gridMgr.GetGridInfo(entity.row, entity.col).SetGridUsed();
        entity.bloodBar = GameStatic.guiWord.OnCreateBloodBar(entObject.transform.position);
    }

    public void OnEntityDestroy(Game.Entity entity)
    {
        GameObject ent = entity.renderObj as GameObject;
        ent.GetComponent<EntityAnimator>().SetState(EntityAnimator.stateIdle);
        GameStatic.entityMgr.DestroyEntity(entity.id);
        GameStatic.gridMgr.GetGridInfo(entity.row, entity.col).SetGridUnUsed();
    }

    public void OnBulletCreate(Game.Bullet bullet)
    {
        GameObject bulletObject = GameStatic.bulletMgr.CreateBullet(bullet.id, bullet.typeId);
        bullet.renderObj = bulletObject;
        bulletObject.transform.position = bullet.pos;
        Rigidbody rig = bulletObject.GetComponent<Rigidbody>();
        rig.velocity = bullet.speed;
    }

    public void OnBulletDestroy(Game.Bullet bullet)
    {
        GameStatic.bulletMgr.DestoryBullet(bullet.id);
    }

    public void OnBulletUpdateState(object obj, Vector3 pos, Vector3 speed)
    {
        if (obj == null)
        {
            return;
        }
        GameObject bullet = obj as GameObject;
        bullet.transform.position = pos;
        Rigidbody rig = bullet.GetComponent<Rigidbody>();
        rig.velocity = speed;
    }

    public void OnEntityBore(object obj)
    {
        //if (obj == null)
        //{
        //    return;
        //}
        //GameObject entity = obj as GameObject;
        //entity.GetComponent<EntityAnimator>().SetState(EntityAnimator.stateApper);               
    }

    public void OnEntityFire(object obj)
    {
        if (obj == null)
        {
            return;
        }
        GameObject entity = obj as GameObject;
        entity.GetComponent<EntityAnimator>().SetState(EntityAnimator.stateAttack); 
    }

    public void OnEntityDeath(object obj)
    {
        if (obj == null)
        {
            return;
        }
        GameObject entity = obj as GameObject;
        entity.GetComponent<EntityAnimator>().SetState(EntityAnimator.stateDeath);
    }
}
