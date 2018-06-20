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
                //foreach (var entity in GameWord.entitys)
                //{
                //    entity.Value.Update(now);
                //}
                //foreach (var bullet in GameWord.bullets)
                //{
                //    bullet.Value.Update(now);
                //}
                var eiter = GameWord.entitys.GetEnumerator();
                bool iscon = eiter.MoveNext();
                while (iscon)
                {
                    Entity entity = eiter.Current.Value;
                    if (entity.del)
                    {
                        iscon = eiter.MoveNext();
                        GameWord.RemoveEntity(entity.id);
                        continue;
                    }
                    entity.Update(now);
                    iscon = eiter.MoveNext();
                }

                var biter = GameWord.bullets.GetEnumerator();
                iscon = biter.MoveNext();
                while (iscon)
                {
                    Bullet bullet = biter.Current.Value;
                    if (bullet.del)
                    {
                        iscon = biter.MoveNext();
                        GameWord.RemoveEntity(bullet.id);
                        continue;
                    }
                    bullet.Update(now);
                    iscon = biter.MoveNext();
                }
            }
        }
    }

}