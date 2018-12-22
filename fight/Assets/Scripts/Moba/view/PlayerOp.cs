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
                Player player = GameApp.campMgr.GetSelfCampPlayer();
                MsgCreateUnit msg = new MsgCreateUnit();
                msg.index = player.Data.CurIndex;
                msg.pos.x = (int)((Fix)hit.point.x).RawValue;
                msg.pos.y = (int)((Fix)hit.point.y).RawValue;
                msg.pos.z = (int)((Fix)hit.point.z).RawValue;
                GameApp.udpNet.Send(msg);
            }
        }

        if (Input.GetKeyDown(KeyCode.Q)) {
            Player player = GameApp.campMgr.GetSelfCampPlayer();
            player.Data.CurIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.W)) {
            Player player = GameApp.campMgr.GetSelfCampPlayer();
            player.Data.CurIndex = 2;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            Player player = GameApp.campMgr.GetSelfCampPlayer();
            player.Data.CurIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            Player player = GameApp.campMgr.GetSelfCampPlayer();
            player.Data.CurIndex = 1;
        }

        if (Input.GetKeyDown(KeyCode.Z)) {
            Player player = GameApp.campMgr.GetSelfCampPlayer();
            player.Data.CurIndex = 1;
        }
    }
}
