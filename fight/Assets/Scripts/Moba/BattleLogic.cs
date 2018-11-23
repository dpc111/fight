using System.Collections;
using System.Collections.Generic;

public class BattleLogic 
{
    public static int mLogicFrame = 0;
    public bool mIsPause = true;
    public bool mIsGame = false;

    public void Init()
    {
        mIsPause = false;
        mIsGame = true;
    }

    public bool IsRun()
    {
        return !mIsPause && mIsGame;
    }

    public void FrameLockLogic()
    {
        for (int i = 0; i < GameData.listBullet.Count; i++)
        {
            GameData.listBullet[i].RecordLastPos();
        }
        for (int i = 0; i < GameData.listSoldier.Count; i++)
        {
            GameData.listSoldier[i].RecordLastPos();
        }
        GameData.actionMgrMgr.UpdateLogic();
        for (int i = 0; i < GameData.listTower.Count; i++)
        {
            GameData.listTower[i].UpdateLogic();
        }
        for (int i = 0; i < GameData.listBullet.Count; i++)
        {
            GameData.listBullet[i].UpdateLogic();
        }
        for (int i = 0; i < GameData.listSoldier.Count; i++)
        {
            GameData.listSoldier[i].UpdateLogic();
        }
        GameData.msgFrame.UpdateLogic(GameData.lockStepLogic.mLogicFrame);
        GameData.transformMgr.UpdateLogic();
        if (mIsGame && GameData.listSoldier.Count == 0)
        {
            //StopBattle();
            //GameData.Stop();
        }
    }

    public void UpdateRenderPosition(float interval)
    {
        for (int i = 0; i < GameData.listBullet.Count; i++)
        {
            GameData.listBullet[i].UpdateRenderPosition(interval);
        }
        for (int i = 0; i < GameData.listSoldier.Count; i++)
        {
            GameData.listSoldier[i].UpdateRenderPosition(interval);
        }
    }

    public void StopBattle()
    {
        mIsGame = false;
        mLogicFrame = GameData.lockStepLogic.mLogicFrame;
        if (mIsPause)
            return;
        for (int i = GameData.listTower.Count - 1; i >= 0; i--)
        {
            GameData.listTower[i].KillSelf();
        }
        for (int i = GameData.listBullet.Count - 1; i >= 0; i--)
        {
            GameData.listBullet[i].KillSelf();
        }
        for (int i = GameData.listSoldier.Count - 1; i >= 0; i--)
        {
            GameData.listSoldier[i].KillSelf();
        }
        mIsPause = true;
    }
}
