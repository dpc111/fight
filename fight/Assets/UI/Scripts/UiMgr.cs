using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UiMgr : MonoBehaviour {
    public Button bnt1;

	// Use this for initialization
	void Start () {
        //bnt1 = transform.Find("Button").GetComponent<Button>();
        bnt1 = GameObject.Find("Canvas/Button").GetComponent<Button>();
        if (bnt1 == null) {
            Debug.LogError("111111");
            return;
        }
        bnt1.onClick.AddListener(OnClick);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnClick() {
        Debug.LogError("22222");
    }
}
