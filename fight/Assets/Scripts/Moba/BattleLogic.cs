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

    public void UpdateFrame() {
        GameData.msgFrame.Update(GameData.lockStepLogic.mLogicFrame);
        GameData.towerMgr.Update();
        GameData.soldierMgr.Update();
        GameData.bulletMgr.Update();
        GameData.transformMgr.Update();
    }

    public void UpdateRender(float interpolation) {
        if (interpolation < 0) {
            interpolation = 0;
        }
        if (interpolation > 1) {
            interpolation = 1;
        }
        GameData.soldierMgr.UpdateRender(interpolation);
        GameData.bulletMgr.UpdateRender(interpolation);
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
