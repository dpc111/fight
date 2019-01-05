using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class UIImpl {
    public static GameObject[] bntTowerObjs = new GameObject[GameConst.TowerTakeNum];
    public static Button[] bntTowers = new Button[GameConst.TowerTakeNum];
    public static Toggle tgShowAttackRange = null;
    public static bool IsAttackRangeShow = true;

    public static void Init() {
        for (int i = 0; i < GameConst.TowerTakeNum; i++) {
            bntTowerObjs[i] = null;
            bntTowers[i] = null;
            bntTowerObjs[i] = GameObject.Find("Canvas/Plane/Button" + (i + 1));
            if (bntTowerObjs[i] == null) {
                continue;
            }
            bntTowers[i] = bntTowerObjs[i].GetComponent<Button>();
            UIEventListener1 btnListener = bntTowers[i].gameObject.AddComponent<UIEventListener1>();
            btnListener.OnClick += OnClick;
            bntTowerObjs[i].AddComponent<UICooling>();
        }
        tgShowAttackRange = GameObject.Find("Canvas/ToggleShowAttackRange").GetComponent<Toggle>();
        tgShowAttackRange.onValueChanged.AddListener(OnTgShowAttackRangeValueChange);
        IsAttackRangeShow = true;
    }

    private static void OnClick(GameObject obj) {
        int index = ButtonCreateTowerNameToIndex(obj.name);
        Player player = GameApp.campMgr.GetSelfCampPlayer();
        if (!player.CardIsCoolDown(index)) {
            return;
        }
        player.Data.CurIndex = index;
    }

    private static void OnTgShowAttackRangeValueChange(bool b) {
        List<UnitBase> list = GameApp.towerMgr.GetList();
        for (int i = 0; i < list.Count; i++) {
            UnitBase unit = list[i];
            unit.mUnitRange.SetActive(b);
        }
        IsAttackRangeShow = b;
    }

    public static int ButtonCreateTowerNameToIndex(string name) {
        int index = 1;
        if (name == "Button1") {
            index = 1;
        } else if (name == "Button2") {
            index = 2;
        } else if (name == "Button3") {
            index = 3;
        } else if (name == "Button4") {
            index = 4;
        } else if (name == "Button5") {
            index = 5;
        }
        return index;
    }

    public static string ButtonCreateTowerIndexToName(int index) {
        return "button" + index;
    }

}
