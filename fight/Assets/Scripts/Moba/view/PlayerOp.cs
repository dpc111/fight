using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerOp : MonoBehaviour {
    public float mMouseCheckLastTime = 0;

    void Start() {

    }

    void Update() {
        if (Input.GetMouseButtonUp(0)) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
#if UNITY_ANDROID || UNITY_IPHONE
                if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId)) {
#else
                if (!EventSystem.current.IsPointerOverGameObject()) {
#endif
                    Player player = GameApp.campMgr.GetSelfCampPlayer();
                    MsgCreateUnit msg = new MsgCreateUnit();
                    msg.index = player.Data.CurIndex;
                    msg.pos.x = (int)((Fix)hit.point.x).RawValue;
                    msg.pos.y = (int)((Fix)hit.point.y).RawValue;
                    msg.pos.z = (int)((Fix)hit.point.z).RawValue;
                    GameApp.udpNet.Send(msg);
                }
            }
        }
    }
}
