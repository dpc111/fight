namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Entity
    {
        public Vector3 lastLocalPos = new Vector3(0f, 0f, 0f);
        public Vector3 lastLocalDir = new Vector3(0f, 0f, 0f);
        public Vector3 pos = new Vector3(0f, 0f, 0f);
        public Vector3 dir = new Vector3(0f, 0f, 0f);
        public float velocity = 0f;
        public object renderObj = null;
        public int id = 0;
        public int typeId = 0;
        public int camp = 0;
        public int row = 0;
        public int col = 0;
        public float cd = 1f;

        public object bloodBar = null;
        public int blood = 0;
        public int bloodMax = 0;

        public Entity()
        {

        }

        public virtual void OnCreate()
        {
            Net.Event.FireOut("OnEntityCreate", new object[] {this});
        }

        public virtual void OnDestroy()
        {
            Net.Event.FireOut("OnEntityDestroy", new object[] {this});
        }
    }
}
