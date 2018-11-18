using System.Collections;
using System.Collections.Generic;

public class ActionMgrMgr { 
    public List<ActionMgr> mListActionMgr = new List<ActionMgr>();

    public void AddActionMgr(ActionMgr actionMgr)
    {
        mListActionMgr.Add(actionMgr);
    }

    public void RemoveActionMgr(ActionMgr actionMgr)
    {
        actionMgr.mEnable = false;
    }

    public void Release()
    {
        for (int i = mListActionMgr.Count - 1; i >= 0; i--)
        {
            mListActionMgr.Remove(mListActionMgr[i]);
        }
    }

    public void UpdateLogic()
    {
        for (int i = 0; i < mListActionMgr.Count; i++)
        {
            if (mListActionMgr[i].mEnable)
            {
                mListActionMgr[i].UpdateLogic();
            }
            if (!mListActionMgr[i].mEnable)
            {
                mListActionMgr.Remove(mListActionMgr[i]);
            }
        }
    }
}