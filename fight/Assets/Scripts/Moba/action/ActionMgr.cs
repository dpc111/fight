using System.Collections;
using System.Collections.Generic;

public class ActionMgr {
    public List<BaseAction> mListAction = new List<BaseAction>();
    public bool mEnable = true;

    public void AddAction(BaseAction action)
    {
        mListAction.Add(action);
        action.SetMgr(this);
    }

    public void RemoveAction(BaseAction action)
    {
        action.mEnable = false;
    }

    public void StopAction(int type)
    {
        for (int i = mListAction.Count - 1; i >= 0; i--)
        {
            if (mListAction[i].mType == type)
            {
                mListAction.Remove(mListAction[i]);
            }
        }
    }

    public void StopActionByKind(int kind)
    {
        for (int i = mListAction.Count - 1; i >= 0; i--)
        {
            if (mListAction[i].mKind == kind)
            {
                mListAction.Remove(mListAction[i]);
            }
        }
    }

    public void StopActionAll()
    {
        for (int i = mListAction.Count - 1; i >= 0; i--)
        {
            mListAction.Remove(mListAction[i]);
        }
    }

    public void UpdateLogic()
    {
        for (int i = 0; i < mListAction.Count; i++)
        {
            if (mListAction[i].mEnable)
            {
                mListAction[i].UpdateLogic();
            }
        }
        for (int i = mListAction.Count - 1; i >= 0; i--)
        {
            if (!mListAction[i].mEnable)
            {
                mListAction.Remove(mListAction[i]);
            }
        }
    }
}
