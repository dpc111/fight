using System.Collections;
using System.Collections.Generic;

public struct MsgCreateTower {
    public int type;
    public int posX;
    public int posY;
}

public struct MsgPlayerInfo {
    public int uid;
    public string name;
    public string headUrl;
    public int camp;
}

public struct FightInfo {
    public MsgPlayerInfo[] players;
    public int playerNum;
}

public struct MsgPos {
    public int x;
    public int y;
    public int z;
}

public struct MsgCreateUnit {
    public int index;
    public MsgPos pos;
}