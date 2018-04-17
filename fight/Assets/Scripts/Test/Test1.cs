using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test1 : MonoBehaviour {

	// Use this for initialization
    private UISlider slider;
    // Use this for initialization  
    void Start()
    {

        Debug.Log("..........");
        slider = GetComponent<UISlider>();
        slider.value = 0;
    }

    // Update is called once per frame  
    void Update()
    {
        if (slider != null)
        {
            Debug.Log(slider.value);
            slider.value += 0.0005f;
        }
    } 
}


//public class Test1 : MonoBehaviour
//{

//    public GameObject pb;
//    private UISlider slider;

//    void Start()
//    {
//        pb = GameObject.Find("Progress Bar");
//        if (pb == null)
//        {
//            Debug.LogError("null pb");
//            return;
//        }
//        slider = pb.GetComponent<UISlider>();
//        slider.value = 0;
//        Debug.LogError("aaaaaa");
//    }

//    // Update is called once per frame  
//    void Update()
//    {
//        if (slider != null)
//        {
//            Debug.Log(slider.value);
//            slider.value += 0.0005f;
//        }
//        else
//        {
//            //Debug.LogError(".....");
//        }
//    }


//}