using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ConfigMgr : MonoBehaviour {

    public static Dictionary<string, JObject> jsonObjects = new Dictionary<string, JObject>();

	void Start () {
		
	}

    void Load(string fileName, string jsonName)
    {
        JObject obj = null;
        if (!jsonObjects.TryGetValue(jsonName, out obj))
        {
            return;
        }
        string fileData = File.ReadAllText(fileName, Encoding.UTF8);
        string jsonData = "{" + jsonName + ":" + fileData + "}";
        JObject jsonObj = JObject.Parse(jsonData);
        jsonObjects[jsonName] = jsonObj;
    }

    static JToken GetValue(string jsonName, object key1, object key2)
    {
        JObject obj = null;
        if (!jsonObjects.TryGetValue(jsonName, out obj))
        {
            return 0;
        }
        return obj[jsonName][key1][key2];
    }
}
