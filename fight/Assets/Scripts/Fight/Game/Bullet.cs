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
        public Vector3 beginPos = new Vector3(0f, 0f, 0f);
        public Vector3 pos = new Vector3(0f, 0f, 0f);
        public Vector3 dir = new Vector3(0f, 0f, 0f);
        public Vector3 beginSpeed = new Vector3(0f, 0f, 0f);
        public Vector3 speed = new Vector3(0f, 0f, 0f);
        public float velocity = 0f;
        public object renderObj = null;
        public int id = 0;
        public int typeId = 0;
        public int camp = 0;
        public int damage = 0;
        public int path = 1;
        public bool del = false;
        public double createTime = 0f;

        public Bullet()
        {

        }

        public void Update(double now)
        {
            if (del)
            {
                return;
            }
            float interval = (float)(now - createTime);
            if (path == 1)
            {
                speed = beginSpeed;
                pos = beginPos + speed * interval;
            } else if(path == 2) {
                speed.y = beginSpeed.y - Config.gravity * interval;
                pos.x = beginPos.x + beginSpeed.x * interval;
                pos.y = beginPos.y + beginSpeed.y * interval - 0.5f * Config.gravity * interval * interval;
            }
            if (pos.x < Config.bulletXMin ||
                pos.x > Config.bulletXMax ||
                pos.y < Config.bulletYMin ||
                pos.y > Config.bulletYMax ||
                pos.z < Config.bulletZMin ||
                pos.z > Config.bulletZMax ||
                interval > Config.bulletLifeTime)
            {
                del = true;
                return;
            }
            if (Vector3.Distance(pos, lastLocalPos) > 2.0f)
            {
                Net.Event.FireOut("OnBulletUpdateState", new object[] { this.renderObj, pos, speed });
                lastLocalPos = pos;
            }
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
