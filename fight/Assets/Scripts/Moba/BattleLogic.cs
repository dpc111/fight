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
        GameData.msgFrame.Update(GameData.lockStepLogic.mLogicFrame);
        GameData.towerMgr.Update();
        GameData.bulletMgr.Update();
        GameData.transformMgr.Update();
        //if (mIsGame && GameData.listSoldier.Count == 0)
        //{
        //    //StopBattle();
        //    //GameData.Stop();
        //}
    }

    public void UpdateRenderPosition(float interval)
    {
        //for (int i = 0; i < GameData.listBullet.Count; i++)
        //{
        //    GameData.listBullet[i].UpdateRenderPosition(interval);
        //}
        //for (int i = 0; i < GameData.listSoldier.Count; i++)
        //{
        //    GameData.listSoldier[i].UpdateRenderPosition(interval);
        //}
    }

    public void StopBattle()
    {
        mIsGame = false;
        mLogicFrame = GameData.lockStepLogic.mLogicFrame;
        if (mIsPause)
            return;
        //for (int i = GameData.listTower.Count - 1; i >= 0; i--)
        //{
        //    GameData.listTower[i].KillSelf();
        //}
        //for (int i = GameData.listBullet.Count - 1; i >= 0; i--)
        //{
        //    GameData.listBullet[i].KillSelf();
        //}
        //for (int i = GameData.listSoldier.Count - 1; i >= 0; i--)
        //{
        //    GameData.listSoldier[i].KillSelf();
        //}
        mIsPause = true;
    }
}
