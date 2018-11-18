using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Prefabs
{
    void Start()
    {

    }

    void Update()
    {

    }

    public static GameObject Create(string path)
    {
        GameObject obj = (GameObject)GameObject.Instantiate(Resources.Load(path));
        return obj;
    }

    public static GameObject Create(string path, UnityObject obj, GameObject parentObj = null)
    {
        GameObject gameObj = Create(path);
        obj.mGameObj = gameObj;
        if (parentObj != null)
            obj.mGameObj.transform.SetParent(parentObj.gameObject.transform);
        return gameObj;
    }
}