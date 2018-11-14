using System.Collections;
using System.Collections.Generic;

public class LockStepLogic 
{
    float mTotalTime = 0;
    float mNextGameTime = 0;
    float mFrameLen = 0;
    float mFrameInterval = 0;
    public int mLogicFrame = 0;
    public BattleLogic mBattleLogic = null;

    public LockStepLogic()
    {
        Init();
    }

    public void Init()
    {
        mFrameLen = (float)GameData.fixFrameLen;
        mTotalTime = 0;
        mNextGameTime = 0;
        mFrameInterval = 0;
        mLogicFrame = 0;
    }

    public void UpdateLogic()
    {
        float deltaTime = UnityEngine.Time.deltaTime;
        mTotalTime += deltaTime;
        while (mTotalTime > mNextGameTime && GameData.msgFrame.CheckFrame(mLogicFrame))
        {
            mBattleLogic.FrameLockLogic();
            mNextGameTime += mFrameLen;
            mLogicFrame += 1;
        }
        mFrameInterval = (mFrameLen - (mNextGameTime - mTotalTime)) / mFrameLen;
        mBattleLogic.UpdateRenderPosition(mFrameInterval);
    }
}
