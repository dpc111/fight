using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUIWord : MonoBehaviour {
	void Start () {
        DeregisterEvents();
        RegisterEvents();
	}
	
	void Update () {

    }

    public void DeregisterEvents()
    {
        Net.Event.DeregisterIn(this);
        Net.Event.DeregisterOut(this);
    }

    public void RegisterEvents()
    {
        Net.Event.RegisterOut("OnSetBloodBar", this, "OnSetBloodBar");
    }

    public void OnSetBloodBar(object obj, int max, int cur)
    {
        if (obj == null)
        {
            return;
        }
        GameObject bar = obj as GameObject;
        bar.GetComponent<UISlider>().value = cur / max;
    }
}
