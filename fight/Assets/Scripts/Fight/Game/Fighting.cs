namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using ProtoBuf;

    public class Fighting
    {
        public float lastUpdateTime = 0;
        public void Update() {
            float now = State.CurRoomTime();
            if (lastUpdateTime > now)
            {
                foreach (var entity in GameWord.entitys)
                {
                    entity.Value.Update(now);
                }
                foreach (var bullet in GameWord.bullets)
                {
                    bullet.Value.Update(now);
                }
            }
        }
    }

}