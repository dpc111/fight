using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonEv : MonoBehaviour {
    void Start() {
        Button btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick); 
    }
    private void OnClick() {
        Debug.Log("Button Clicked. TestClick.");
    }
}
