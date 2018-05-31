namespace Game
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using ProtoBuf;

    public class GameWord
    {
        public static GameWord gameWord = null;
        private static readonly object locker = new object();
        public static Dictionary<int, Entity> entitys = new Dictionary<int, Entity>();
        public static Dictionary<int, Bullet> bullets = new Dictionary<int, Bullet>();

        public static GameWord Instance()
        {
            if (gameWord == null)
            {
                lock (locker)
                {
                    gameWord = new GameWord();
                }
            }
            return gameWord;
        }

        public void Init() 
        {
            DeRegister();
            RegisterEvents();
            GridMgr.Init();
        }

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

        public void DeRegister()
        {
            Net.Event.DeregisterIn(this);
            Net.Event.DeregisterOut(this);
        }

        public void RegisterEvents()
        {
            Net.Event.RegisterIn("battle_msg.s_room_state", this, "s_room_state");
            Net.Event.RegisterIn("battle_msg.s_create_entity", this, "s_create_entity");
            Net.Event.RegisterIn("battle_msg.s_destroy_entity", this, "s_destroy_entity");
            Net.Event.RegisterIn("battle_msg.s_fire", this, "s_fire");
            Net.Event.RegisterIn("battle_msg.s_collision", this, "s_collision");

            Net.Event.RegisterOut("VOnCreateEntity", this, "VOnCreateEntity");

        }

        public Entity GetEntity(int id)
        {
            Entity entity = null;
            if (entitys.TryGetValue(id, out entity))
            {
                return entity;
            }
            return null;
        }

        public Bullet GetBullet(int id)
        {
            Bullet bullet = null;
            if (bullets.TryGetValue(id, out bullet))
            {
                return bullet;
            }
            return null;
        }

        public void s_room_state(battle_msg.s_room_state msg)
        {
            Debug.Log(msg.state);
        }

        public void s_create_entity(battle_msg.s_create_entity msg)
        {
            Entity entity = GetEntity(msg.einfo.id);
            if (entity != null)
            {
                Debug.Log("");
                return;
            }
            entity = new Entity();
            entity.id = msg.einfo.id;
            entity.typeId = msg.einfo.type_id;
            entity.camp = msg.einfo.camp;
            entity.row = msg.einfo.row;
            entity.col = msg.einfo.col;
            entity.blood = msg.einfo.blood;
            entity.cd = msg.einfo.cd;
            entitys[entity.id] = entity;
            entity.OnCreate();
        }

        public void s_destroy_entity(battle_msg.s_destroy_entity msg)
        {
            Entity entity = GetEntity(msg.eid);
            if (entity == null)
            {
                return;
            }
            entity.OnDestroy();
            entitys.Remove(msg.eid);
        }

        public void s_fire(battle_msg.s_fire msg)
        {
            Entity entity = GetEntity(msg.eid);
            if (entity == null)
            {
                Debug.Log("");
                return;
            }
            Bullet bullet = GetBullet(msg.binfo.id);
            if (bullet != null)
            {


                Debug.Log("");
                return;
            }
            bullet = new Bullet();
            bullet.id = msg.binfo.id;
            bullet.typeId = msg.binfo.type_id;
            bullet.camp = msg.binfo.camp;
            bullet.damage = msg.binfo.damage;
            bullet.pos.x = msg.binfo.pos.x;
            bullet.pos.y = msg.binfo.pos.y;
            bullet.pos.z = 0;
            bullet.speed.x = msg.binfo.pos.x;
            bullet.speed.y = msg.binfo.pos.y;
            bullet.speed.z = 0;
            bullets[bullet.id] = bullet;
            bullet.OnCreate();
        }

        public void s_collision(battle_msg.s_collision msg)
        {
            Entity entity = GetEntity(msg.einfo.id);
            if (entity == null)
            {
                Debug.Log("");
                return;
            }
            Bullet bullet = GetBullet(msg.binfo.id);
            if (bullet == null)
            {
                Debug.Log("");
                return;
            }
            entity.blood = msg.einfo.blood;
            if (msg.bullet_destroy)
            {
                bullet.OnDestroy();
                bullets.Remove(bullet.id);
            }
        }

        public void VOnCreateEntity(int typeId, int row, int col)
        {

        }
    }
}
