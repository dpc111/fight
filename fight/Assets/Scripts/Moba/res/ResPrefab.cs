﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResPrefab : ResBase<GameObject> {
    public GameObject Create(string name) {
        GameObject prefab = Get(name);
        if (prefab == null) {
            return null;
        }
        GameObject obj = GameObject.Instantiate(prefab) as GameObject;
        return obj;
    }

    public GameObject Create(string name, Vector3 pos, Quaternion rol) {
         GameObject prefab = Get(name);
        if (prefab == null) {
            return null;
        }
        GameObject obj = GameObject.Instantiate(prefab, pos, rol) as GameObject;
        return obj;
    }

    public void Destory(GameObject obj) {
        GameObject.Destroy(obj);
    }
}
