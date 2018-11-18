using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockStepLogic 
{
    float mTotalTime = 0;
    float mNextGameTime = 0;
    float mFrameLen = 0;
    float mFrameInterval = 0;
    public int mLogicFrame = 1;

    public void Init()
    {
        mFrameLen = (float)GameData.fixFrameLen;
        mTotalTime = 0;
        mNextGameTime = 0;
        mFrameInterval = 0;
        mLogicFrame = 1;
    }

    public void UpdateLogic()
    {
        float deltaTime = UnityEngine.Time.deltaTime;
        mTotalTime += deltaTime;
        //test
        while (mTotalTime > mNextGameTime && GameData.msgFrame.CheckFrame(mLogicFrame))
        //while (mTotalTime > mNextGameTime)
        {
            GameData.battleLogic.FrameLockLogic();
            mNextGameTime += mFrameLen;
            mLogicFrame += 1;
            //Debug.Log(mLogicFrame);
            //test
            //if (mLogicFrame == 50)
            //{
                //GameData.udpNet.DisconnectToServer(0);
                //GameData.Stop();
                //return;
            //}
            MsgCreateTower msg = new MsgCreateTower();
            msg.type = 1;
            msg.posx = 11;
            msg.posy = 111;
            GameData.udpNet.Send(1, msg);
        }
        mFrameInterval = (mFrameLen - (mNextGameTime - mTotalTime)) / mFrameLen;
        GameData.battleLogic.UpdateRenderPosition(mFrameInterval);
    }
}
