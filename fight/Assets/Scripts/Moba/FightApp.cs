using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightApp : MonoBehaviour {

	void Start () {
        GameData.Init();
	}
	
	void Update () {
        GameData.UpdateLogic();
	}
}
