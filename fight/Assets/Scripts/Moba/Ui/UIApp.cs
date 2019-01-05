using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIApp {
    public static void SetCreateTowerCd(Player player, int index, Fix cd) {
        if (!GameApp.campMgr.IsSelfPlayer(player)) {
            return;
        }
        GameObject obj = UIImpl.bntTowerObjs[index - 1];
        if (obj == null) {
            return;
        }
        Debug.Log("index .. " + index + " " + (float)cd);
        obj.GetComponent<UICooling>().Reset((float)cd);
    }

    public static void OnCreateTower(Player player, int index) {
        if (!GameApp.campMgr.IsSelfPlayer(player)) {
            return;
        }
        GameObject obj = UIImpl.bntTowerObjs[index - 1];
        if (obj == null) {
            return;
        }
        obj.GetComponent<UICooling>().Reset();
    }
}
