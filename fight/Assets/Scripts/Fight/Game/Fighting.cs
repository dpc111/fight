namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using ProtoBuf;

    public class Fighting
    {
        public const double spf = 0.02f;
        public static double lastUpdateTime = 0;
        public static List<int> removeEntityList = new List<int>();
        public static List<int> removeBulletList = new List<int>();
        public static void Update() {
            if (!State.run)
            {
                return;
            }
            double now = State.CurServerTime();
            //Debug.Log(now + "  " + lastUpdateTime + "  " + spf);
            if (now - lastUpdateTime > spf)
            {
                lastUpdateTime = now;
                foreach (var v in GameWord.entitys)
                {
                    Entity entity = v.Value;
                    entity.Update(now);
                    if (entity.del)
                    {
                        removeEntityList.Add(entity.id);
                    }
                }
                foreach (int id in removeEntityList)
                {
                    GameWord.RemoveEntity(id);
                }
                removeEntityList.Clear();

                foreach (var v in GameWord.bullets)
                {
                    Bullet bullet = v.Value;
                    bullet.Update(now);
                    if (bullet.del)
                    {
                        removeBulletList.Add(bullet.id);
                    }
                }
                foreach (int id in removeBulletList)
                {
                    GameWord.RemoveBullet(id);
                }
                removeBulletList.Clear();
                //var eiter = GameWord.entitys.GetEnumerator();
                //bool iscon = eiter.MoveNext();
                //while (iscon)
                //{
                //    Entity entity = eiter.Current.Value;
                //    if (entity.del)
                //    {
                //        iscon = eiter.MoveNext();
                //        GameWord.RemoveEntity(entity.id);
                //        continue;
                //    }
                //    entity.Update(now);
                //    iscon = eiter.MoveNext();
                //}

                //var biter = GameWord.bullets.GetEnumerator();
                //iscon = biter.MoveNext();
                //while (iscon)
                //{
                //    Bullet bullet = biter.Current.Value;
                //    if (bullet.del)
                //    {
                //        iscon = biter.MoveNext();
                //        GameWord.RemoveBullet(bullet.id);
                //        continue;
                //    }
                //    bullet.Update(now);
                //    iscon = biter.MoveNext();
                //}
            }
        }
    }

}