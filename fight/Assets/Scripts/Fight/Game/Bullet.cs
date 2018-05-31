namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Net;

    public class Bullet
    {
        public Vector3 lastLocalPos = new Vector3(0f, 0f, 0f);
        public Vector3 lastLocalDir = new Vector3(0f, 0f, 0f);
        public Vector3 pos = new Vector3(0f, 0f, 0f);
        public Vector3 dir = new Vector3(0f, 0f, 0f);
        public Vector3 speed = new Vector3(0f, 0f, 0f);
        public float velocity = 0f;
        public object renderObj = null;
        public int id = 0;
        public int typeId = 0;
        public int camp = 0;
        public int damage = 0;
        public bool del = false;

        public Bullet()
        {

        }

        public virtual void OnCreate()
        {
            Net.Event.FireOut("OnBulletCreate", new object[] {this});
            del = false;
        }

        public virtual void OnDestroy()
        {
            Net.Event.FireOut("OnBulletDestroy", new object[] {this});
            del = true;
        }

        public virtual void OnPosChange(Vector3 newPos)
        {
            pos = newPos;
            Net.Event.FireOut("OnBulletPosChange", new object[] {this});
        }

        public virtual void OnDirChange(Vector3 newDir)
        {
            dir = newDir;
            Net.Event.FireOut("OnBulletDirChange", new object[] {this});
        }
    }
}
