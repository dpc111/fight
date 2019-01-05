using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResFactory {
    public static ResPrefab prefabs = new ResPrefab();
    public static ResSprite sprites = new ResSprite();

    public static void Init() {
        prefabs.Init();
        prefabs.Load("Entity/", "Elongata");
        prefabs.Load("Entity/", "plant");
        prefabs.Load("Entity/", "slime");
        prefabs.Load("Entity/", "minigolem");
        prefabs.Load("Entity/", "wasp");
        prefabs.Load("Entity/", "evilSlime");
        prefabs.Load("Bullet/", "Bullet");
        prefabs.Load("Bullet/", "LifeProjectile");
        prefabs.Load("Bullet/", "Bullet3003");
        prefabs.Load("Buff/", "LifeShield");
        prefabs.Load("Buff/", "AirShield");
        prefabs.Load("UI/", "range");
        sprites.Init();
    }
}
