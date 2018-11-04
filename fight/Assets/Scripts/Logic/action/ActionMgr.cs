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

    public void StopActionAll()
    {
        for (int i = mListAction.Count - 1; i >= 0; i--)
        {
            mListAction.Remove(mListAction[i]);
        }
    }

    public void StopActionByLable(string lable)
    {
        for (int i = mListAction.Count - 1; i >= 0; i--)
        {
            if (mListAction[i].mScLabel == lable)
            {
                mListAction.Remove(mListAction[i]);
            }
        }
    }

    public void StopActionByName(string name)
    {
        for (int i = mListAction.Count - 1; i >= 0; i--)
        {
            if (mListAction[i].mScName == name)
            {
                mListAction.Remove(mListAction[i]);
            }
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
