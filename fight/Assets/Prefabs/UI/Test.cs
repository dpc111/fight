using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private UISlider slider;
    // Use this for initialization  
    void Start()
    {
        slider = GetComponent<UISlider>();
        slider.value = 0;
    }

    // Update is called once per frame  
    void Update()
    {
        if (slider != null)
        {
            Debug.Log(slider.value);
            slider.value += 0.005f;
        }
    } 
}
