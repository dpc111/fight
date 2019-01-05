using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMgr {
    public static int mCardNumMax = 5;
    public Card[] mCards = new Card[mCardNumMax];
    public int mCardNum = 0;
    public Player mPlayer = null;

    public void Init(Player player) {
        for (int i = 0; i < mCardNumMax; i++) {
            mCards[i] = null;
        }
        mCardNum = 0;
        mPlayer = player;
    }

    public void Update() {
        for (int i = 0; i < mCardNum; i++) {
            Card card = mCards[i];
            if (card == null) {
                continue;
            }
            card.Update();
        }
    }

    public Card GetCard(int index) {
        index = index - 1;
        if (index < 0 || index > mCardNum) {
            return null;
        }
        return mCards[index];
    }

    public void Create(int[] ids, int num) {
        for (int i = 0; i < num; i++) {
            int id = ids[i];
            TowerCfg cfg = TowerFactory.GetCfg(id);
            if (cfg == null) {
                continue;
            }
            Card card = new Card();
            card.Init(cfg.Id, cfg.CreateCd);
            mCards[mCardNum] = card;
            mCardNum++;
            UIApp.SetCreateTowerCd(mPlayer, i + 1, cfg.CreateCd);
        }
    }
}
