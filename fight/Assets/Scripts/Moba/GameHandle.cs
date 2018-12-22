using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandle : MonoBehaviour {
	void Start () {
        GameApp.Init();
	}
	
	void Update () {
        GameApp.UpdateLogic();
	}
}
