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
        Net.Event.RegisterOut("OnDestroyBloodBar", this, "OnDestroyBloodBar");
    }

    public object OnCreateBloodBar(Vector3 pos)
    {
        GameObject bar = Instantiate(GUIImpl.bloodBarPrefab) as GameObject;
        bar.transform.position = pos + new Vector3(-4, 5, 2);
        bar.transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
        bar.transform.rotation = Camera.main.transform.rotation;
        return bar;
    }

    public void OnDestroyBloodBar(object obj)
    {
        if (obj == null)
        {
            return;
        }
        GameObject bar = obj as GameObject;
        Destroy(bar);
    }

    public void OnSetBloodBar(object obj, int max, int cur)
    {
        if (obj == null)
        {
            return;
        }
        GameObject bar = obj as GameObject;
        GameObject slider = bar.transform.Find("UISlider").gameObject;
        slider.GetComponent<UISlider>().value = (float)cur / (float)max;
    }
}
