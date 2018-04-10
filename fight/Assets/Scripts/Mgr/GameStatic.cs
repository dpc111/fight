using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStatic : MonoBehaviour {
    public static int gridMask;
	
	void Awake () {
        SetMasks();
	}

    public static void SetMasks()
    {
        //gridMask = LayerMask.NameToLayer("Grid");
        gridMask = 1 << LayerMask.NameToLayer("Grid");
    }
}
