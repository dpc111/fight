using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMgr {
    public Card[] mCards = new Card[5];
    public int mCardNum = 0;

    public void Init() {

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
        if (index < 0 || index > mCardNum) {
            return null;
        }
        return mCards[index];
    }
}
