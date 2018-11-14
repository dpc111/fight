using System.Collections;
using System.Collections.Generic;

public class BattleLogic 
{
    LockStepLogic mLockStepLogic = null;
    public static int mLogicFrame = 0;
    public bool mIsPause = true;
    public bool mIsGame = false;

    public void UpdateLogic()
    {
        if (mIsPause)
            return;
        mLockStepLogic.UpdateLogic();
        GameData.udpNet.MainProcess();
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
        GameData.frameMsg.UpdateLogic(mLockStepLogic.mLogicFrame);
        if (mIsGame && GameData.listSoldier.Count == 0)
        {
            StopBattle();
        }
    }

    public void StopBattle()
    {
        mIsGame = false;
        mLogicFrame = mLockStepLogic.mLogicFrame;
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
        GameData.Release();
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

    public void Init()
    {
        mLockStepLogic = new LockStepLogic();
        mLockStepLogic.mBattleLogic = this;
        mIsPause = true;
    }

    public void StartBattle()
    {
        GameData.Init();
        mLockStepLogic.Init();
        mIsPause = false;
        
    }
}
