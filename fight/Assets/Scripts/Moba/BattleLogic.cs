using System.Collections;
using System.Collections.Generic;

public class BattleLogic {
    public static int mLogicFrame = 0;
    public bool mIsPause = true;
    public bool mIsGame = false;

    public void Init() {
        mIsPause = false;
        mIsGame = true;
    }

    public bool IsRun() {
        return !mIsPause && mIsGame;
    }

    public void FrameLockLogic() {
        GameData.msgFrame.Update(GameData.lockStepLogic.mLogicFrame);
        GameData.towerMgr.Update();
        GameData.soldierMgr.Update();
        GameData.bulletMgr.Update();
        GameData.transformMgr.Update();
        //if (mIsGame && GameData.listSoldier.Count == 0)
        //{
        //    //StopBattle();
        //    //GameData.Stop();
        //}
    }

    public void UpdateRenderPosition(float interpolation) {
        if (interpolation < 0) {
            interpolation = 0;
        }
        if (interpolation > 1) {
            interpolation = 1;
        }
        GameData.soldierMgr.UpdateRenderPosition(interpolation);
        GameData.bulletMgr.UpdateRenderPosition(interpolation);
    }

    public void StopBattle() {
        mIsGame = false;
        mLogicFrame = GameData.lockStepLogic.mLogicFrame;
        if (mIsPause) {
            return;
        }
        mIsPause = true;
    }
}
