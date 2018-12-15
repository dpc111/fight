using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOp : MonoBehaviour {
    public float mMouseCheckLastTime = 0;

    void Start() {

    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                TowerFactory.Create(1001, new FixVector3(hit.point.x, hit.point.y, hit.point.z));
            }
        }
    }
}
