using System.Collections;
using System.Collections.Generic;

public class GameLogic {
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
        GameApp.msgFrame.Update(GameApp.lockStepLogic.mLogicFrame);
        GameApp.towerMgr.Update();
        GameApp.soldierMgr.Update();
        GameApp.bulletMgr.Update();
        GameApp.transformMgr.Update();
    }

    public void UpdateRender(float interpolation) {
        if (interpolation < 0) {
            interpolation = 0;
        }
        if (interpolation > 1) {
            interpolation = 1;
        }
        GameApp.soldierMgr.UpdateRender(interpolation);
        GameApp.bulletMgr.UpdateRender(interpolation, false);
        //GameApp.liveMgr.UpdateAnimator();
    }

    public void StopBattle() {
        mIsGame = false;
        mLogicFrame = GameApp.lockStepLogic.mLogicFrame;
        if (mIsPause) {
            return;
        }
        mIsPause = true;
    }
}
