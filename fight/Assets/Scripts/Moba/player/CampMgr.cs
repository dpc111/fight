using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampMgr {
    public Dictionary<int, Player> mPlayers = new Dictionary<int, Player>();
    public Player[] mCamps = new Player[3];
    public int SelfCamp { get; set; }

    public void Init() {
        mPlayers.Clear();
        for (int i = 0; i < 3; i++) {
            mCamps[i] = null;
        }
    }

    public Player GetPlayer(int uid) {
        if (!mPlayers.ContainsKey(uid)) {
            return null;
        }
        return mPlayers[uid];
    }

    public Player GetCampPlayer(int camp) {
        if (camp < 0 || camp > 2) {
            return null;
        } 
        return mCamps[camp];
    }

    public Player GetSelfCampPlayer() {
        return GetCampPlayer(SelfCamp);
    }

    public void Add(Player player) {
        mPlayers[player.mUid] = player;
        mCamps[player.mCamp] = player;
    }

    public int PlayerCamp(int uid) {
        Player player = GetCampPlayer(uid);
        if (player == null) {
            return GameConst.CampNone;
        }
        return player.mCamp;
    }

    public Player CreatePlayer(int id, int camp) {
        if (camp <= 0 || camp > 2) {
            return null;
        }
        Player player = new Player();
        player.Init(id, camp);
        Add(player);
        return player;
    }
}
