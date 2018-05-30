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

        public Entity()
        {

        }

        public virtual void OnDestroy()
        {

        }
    }
}
