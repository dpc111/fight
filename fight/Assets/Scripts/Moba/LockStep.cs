using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStep {
    float mTotalTime = 0;
    float mNextGameTime = 0;
    float mFrameLen = 0;
    float mFrameInterval = 0;
    public int mLogicFrame = 1;

    public void Init() {
        mFrameLen = (float)GameConst.TimeFrame;
        mTotalTime = 0;
        mNextGameTime = 0;
        mFrameInterval = 0;
        mLogicFrame = 1;
    }

    public void UpdateLogic() {
        float deltaTime = UnityEngine.Time.deltaTime;
        mTotalTime += deltaTime;
        while (mTotalTime > mNextGameTime && GameApp.msgFrame.CheckFrame(mLogicFrame)) {
            GameApp.battleLogic.UpdateFrame();
            mNextGameTime += mFrameLen;
            mLogicFrame += 1;
            GameApp.timeCur += GameConst.TimeFrame;
            //test
            if (mLogicFrame == 1000) {
                GameApp.udpNet.DisconnectToServer();
                return;
            }
            //MsgCreateTower msg = new MsgCreateTower();
            //msg.type = 1;
            //msg.posx = 11;
            //msg.posy = 111;
            //GameData.udpNet.Send(1, msg);
            //GameData.udpNet.Send(1, msg);
        }
        mFrameInterval = (mFrameLen - (mNextGameTime - mTotalTime)) / mFrameLen;
        GameApp.battleLogic.UpdateRender(mFrameInterval);
    }
}
