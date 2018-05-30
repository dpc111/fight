namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Mgr
    {
        public static Dictionary<int, Entity> entitys = new Dictionary<int, Entity>();
        public static Dictionary<int, Bullet> bullets = new Dictionary<int, Bullet>();

        public virtual void OnDestroy()
        {
            foreach (var v in entitys)
            {
                v.Value.OnDestroy();
            }
            foreach (var v in bullets)
            {
                v.Value.OnDestroy();
            }
        }

        public void RegisterEvents()
        {
           
        }
    }
}
