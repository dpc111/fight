using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData {
    public int Uid { get; set; }
    public string Name { get; set; }
    public string HeadUrl { get; set; }
    public int CurIndex { get; set; }
    public int Gold { get; set; }
    public int GoldTotal { get; set; }

    public void Init() {
        Uid = 0;
        Name = "";
        HeadUrl = "";
        CurIndex = 1;
        Gold = 0;
        GoldTotal = 0;
    }
}
