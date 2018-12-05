using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ConfigMgr : MonoBehaviour {

    public static Dictionary<string, JObject> jsonObjects = new Dictionary<string, JObject>();
    public static Dictionary<string, JArray> jsonArrays = new Dictionary<string, JArray>();

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

	void Start() {
        Init();
	}

    public void Init()
    {
        Load("Assets/Scripts/Common/Json/entity.json", "entity");
        Load("Assets/Scripts/Common/Json/bullet.json", "bullet");
    }

    public void Load(string fileName, string jsonName)
    {
        //JObject obj = null;
        //if (jsonObjects.TryGetValue(jsonName, out obj))
        //{
        //    return;
        //}
        //string fileData = File.ReadAllText(fileName, Encoding.UTF8);
        //string jsonData = "{" + jsonName + ":" + fileData + "}";
        //JObject jsonObj = JObject.Parse(jsonData);
        //jsonObjects[jsonName] = jsonObj;

        JArray array = null;
        if (jsonArrays.TryGetValue(fileName, out array))
        {
            return;
        }
        string fileData = File.ReadAllText(fileName, Encoding.UTF8);
        JArray jsonArray = JArray.Parse(fileData);
        jsonArrays[jsonName] = jsonArray;
    }

    public static JObject GetJObject(string jsonName)
    {
        JObject obj = null;
        if (!jsonObjects.TryGetValue(jsonName, out obj))
        {
            return null;
        }
        return obj;
    }

    public static int GetArrayCount(string jsonName)
    {
        JArray array = null;
        if (!jsonArrays.TryGetValue(jsonName, out array))
        {
            return 0;
        }
        return array.Count;
    }

    public static JToken GetArrayValue(string jsonName, int k1, string k2)
    {
        JArray array = null;
        if (!jsonArrays.TryGetValue(jsonName, out array))
        {
            return null;
        }
        return array[k1][k2];
    }

    public static JToken GetValue(string jsonName, string k1, string k2)
    {
        JObject obj = null;
        if (!jsonObjects.TryGetValue(jsonName, out obj))
        {
            return 0;
        }
        return obj[jsonName][k1][k2];
    }

    //public static JToken GetValue(string jsonName, object key1, object key2)
    //{
    //    JObject obj = null;
    //    if (!jsonObjects.TryGetValue(jsonName, out obj))
    //    {
    //        return 0;
    //    }
    //    return obj[jsonName][key1][key2];
    //}
}
