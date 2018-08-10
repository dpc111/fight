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
            Reset();
        }

        public virtual void OnDestroy()
        {
            foreach (var v in entitys)
            {
                v.Value.OnDestroy();
            }
            entitys.Clear();
            foreach (var v in bullets)
            {
                v.Value.OnDestroy();
            }
            bullets.Clear();
        }

        public void Reset()
        {
            foreach (var v in entitys)
            {
                v.Value.OnDestroy();
            }
            entitys.Clear();
            foreach (var v in bullets)
            {
                v.Value.OnDestroy();
            }
            bullets.Clear();
            GridMgr.Reset();
        }

        public void DeRegister()
        {
            Net.Event.DeregisterIn(this);
            Net.Event.DeregisterOut(this);
        }

        public void RegisterEvents()
        {
            Net.Event.RegisterIn("battle_msg.s_get_room_info", this, "s_get_room_info");
            Net.Event.RegisterIn("battle_msg.s_room_state", this, "s_room_state");
            Net.Event.RegisterIn("battle_msg.s_create_entity", this, "s_create_entity");
            //Net.Event.RegisterIn("battle_msg.s_destroy_entity", this, "s_destroy_entity");
            Net.Event.RegisterIn("battle_msg.s_fire", this, "s_fire");
            Net.Event.RegisterIn("battle_msg.s_collision", this, "s_collision");
            Net.Event.RegisterIn("battle_msg.s_update_state", this, "s_update_state");

            Net.Event.RegisterIn("VOnCreateEntity", this, "VOnCreateEntity");
            Net.Event.RegisterIn("VFightSenceLoadOver", this, "VFightSenceLoadOver");
        }

        public static Entity GetEntity(int id)
        {
            Entity entity = null;
            if (entitys.TryGetValue(id, out entity))
            {
                return entity;
            }
            return null;
        }

        public static Bullet GetBullet(int id)
        {
            Bullet bullet = null;
            if (bullets.TryGetValue(id, out bullet))
            {
                return bullet;
            }
            return null;
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // tool function
        public void GetRoomInfo()
        {
            battle_msg.c_get_room_info msg = new battle_msg.c_get_room_info();
            Net.App.Instance().Send(msg);
        }

        public bool ExistEntity(int id)
        {
            return entitys.ContainsKey(id);
        }

        public static void CreateEntity(battle_msg.entity_info info)
        {
            Entity entity = GetEntity(info.id);
            if (entity != null)
            {
                Debug.Log("");
                return;
            }
            entity = new Entity();
            entity.id = info.id;
            entity.typeId = info.type_id;
            entity.camp = info.camp;
            entity.row = info.row;
            entity.col = info.col;
            entity.blood = info.blood;
            entity.bloodMax = info.blood;
            entity.cd = info.cd;
            entity.pos.x = info.pos.x;
            entity.pos.y = info.pos.y;
            entity.pos.z = info.pos.z;
            entity.OnCreate();
            GridMgr.CreateEntity(entity.row, entity.col, entity.id);
            entitys[entity.id] = entity;
            // test
            entity.SetBlood(info.blood);
        }

        public static void RemoveEntity(int id)
        {
            Entity entity = GetEntity(id);
            if (entity == null)
            {
                return;
            }
            GridMgr.DestroyEntity(id);
            entity.OnDestroy();
            entitys.Remove(id);
        }

        public bool ExistBullet(int id)
        {
            return bullets.ContainsKey(id);
        }

        public static void CreateBullet(battle_msg.bullet_info info)
        {
            Bullet bullet = GetBullet(info.id);
            if (bullet != null)
            {
                Debug.Log("");
                return;
            }
            bullet = new Bullet();
            bullet.id = info.id;
            bullet.typeId = info.type_id;
            bullet.camp = info.camp;
            bullet.damage = info.damage;
            bullet.path = info.path;
            bullet.beginPos.x = info.pos.x;
            bullet.beginPos.y = info.pos.y;
            bullet.beginPos.z = info.pos.z;
            bullet.pos.x = info.pos.x;
            bullet.pos.y = info.pos.y;
            bullet.pos.z = info.pos.z;
            bullet.beginSpeed.x = info.begin_speed.x;
            bullet.beginSpeed.y = info.begin_speed.y;
            bullet.beginSpeed.z = info.begin_speed.z;
            bullet.speed.x = info.speed.x;
            bullet.speed.y = info.speed.y;
            bullet.speed.z = info.speed.z;
            bullet.createTime = info.begin_time;
            bullet.OnCreate();
            bullets[bullet.id] = bullet;
        }

        public static void RemoveBullet(int id)
        {
            Bullet bullet = GetBullet(id);
            if (bullet == null)
            {
                return;
            }
            bullet.OnDestroy();
            bullets.Remove(id);
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // event msg
        public void s_get_room_info(battle_msg.s_get_room_info msg)
        {
            Reset();
            State.SetTimeDiff(msg.info.now_time);
            State.SetRoomState(msg.info.state);
            foreach (var v in msg.entitys)
            {
                CreateEntity(v);
            }
            foreach (var v in msg.bullets)
            {
                CreateBullet(v);
            }
        }

        public void s_room_state(battle_msg.s_room_state msg)
        {
            State.SetTimeDiff(msg.info.now_time);
            State.SetRoomState(msg.info.state);
        }

        public void s_create_entity(battle_msg.s_create_entity msg)
        {
            CreateEntity(msg.einfo);  
        }

        //public void s_destroy_entity(battle_msg.s_destroy_entity msg)
        //{
        //    if (!ExistEntity(msg.eid))
        //    {
        //        return;
        //    }
        //    RemoveEntity(msg.eid);
        //}

        public void s_fire(battle_msg.s_fire msg)
        {
            if (!ExistEntity(msg.eid))
            {
                return;
            }
            Entity entity = GetEntity(msg.eid);
            CreateBullet(msg.binfo);
        }

        public void s_collision(battle_msg.s_collision msg)
        {
            if (!ExistEntity(msg.einfo.id))
            {
                return;
            }
            if (!ExistBullet(msg.binfo.id))
            {
                return;
            }
            Entity entity = GetEntity(msg.einfo.id);
            entity.SetBlood(msg.einfo.blood);
            //if (msg.bullet_destroy)
            //{
            //    RemoveBullet(msg.binfo.id);
            //}
        }

        public void s_update_state(battle_msg.s_update_state msg)
        {
            foreach (var st in msg.states)
            {
                int state = st.state;
                int id = st.id;
                Entity entity = null;
                Bullet bullet = null;
                if (state == Const.ENTITY_STATE_BORN ||
                    state == Const.ENTITY_STATE_IDLE ||
                    state == Const.ENTITY_STATE_FIRE ||
                    state == Const.ENTITY_STATE_DEATH ||
                    state == Const.ENTITY_STATE_DEL)
                {
                    entity = GetEntity(id);
                    if (entity == null)
                    {
                        continue;
                    }
                }
                else if (state == Const.BULLET_STATE_BORN ||
                    state == Const.BULLET_STATE_FLY ||
                    state == Const.BULLET_STATE_HIT ||
                    state == Const.BULLET_STATE_DEL)
                {
                    bullet = GetBullet(id);
                    if (bullet == null)
                    {
                        continue;
                    }
                }

                switch (state)
                {
                    case Const.ENTITY_STATE_BORN:
                        Net.Event.FireOut("OnEntityBore", new object[] { entity.renderObj }); 
                        break;
                    case Const.ENTITY_STATE_IDLE:
                        break;
                    case Const.ENTITY_STATE_FIRE:
                        Net.Event.FireOut("OnEntityFire", new object[] { entity.renderObj }); 
                        break;
                    case Const.ENTITY_STATE_DEATH:
                        Net.Event.FireOut("OnEntityDeath", new object[] { entity.renderObj });                        
                        break;
                    case Const.ENTITY_STATE_DEL:
                        RemoveEntity(id);
                        break; 

                    case Const.BULLET_STATE_BORN:
                        break;
                    case Const.BULLET_STATE_FLY:
                        break;
                    case Const.BULLET_STATE_HIT:
                        break;
                    case Const.BULLET_STATE_DEL:
                        RemoveBullet(id);
                        break;
                    default:
                        break;
                }
            }
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // event view
        public void VOnCreateEntity(int typeId, int row, int col)
        {
            if (GridMgr.ExistEntity(row, col))
            {
                return;
            }
            battle_msg.c_create_entity msg = new battle_msg.c_create_entity();
            msg.type_id = typeId;
            msg.row = row;
            msg.col = col;
            Net.App.Instance().Send(msg);
        }

        public void VFightSenceLoadOver()
        {
            GetRoomInfo();
        }
    }
}
