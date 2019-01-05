using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player {
    public PlayerData Data { get; set; }
    public CardMgr mCardMgr = new CardMgr();
    public int mUid = 0;
    public int mCamp = 0;

    public void Init(int uid, int camp) {
        mUid = uid;
        mCamp = camp;
        mCardMgr.Init(this);
        Data = new PlayerData();
        Data.Init();
    }

    public bool CardIsCoolDown(int index) {
        Card card = mCardMgr.GetCard(index);
        if (card == null) {
            return false;
        }
        return card.IsCoolDown();
    }

    public void CardUse(int index) {
        Card card = mCardMgr.GetCard(index);
        if (card == null) {
            return;
        }
        card.Use();
    }

    public int CardUnitId(int index) {
        Card card = mCardMgr.GetCard(index);
        if (card == null) {
            return 0;
        }
        return card.mUnitId;
    }
}
