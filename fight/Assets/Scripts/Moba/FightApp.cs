using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightApp : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameData.Init();
	}
	
	// Update is called once per frame
	void Update () {
        GameData.UpdateLogic();
	}
}
