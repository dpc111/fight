using UnityEngine;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class JsonTest : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

        //string json = " { battle : " + File.ReadAllText("D:/self/game1/server/etc/config/json/battle.json", Encoding.UTF8) + "}";
        //Debug.Log(json);
        //JObject obj = JObject.Parse(json);
        //Debug.Log(obj["battle"][3]["roomName"].ToString());
        ////Debug.Log(obj[4]["id"].ToString());

        //JArray ints = (JArray)obj["battle"];

        //foreach (var inter in ints)
        //{
        //    Debug.Log(inter["id"]);
        //}
    }

}