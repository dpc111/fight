using System.Collections;
using System.Collections.Generic;

public class MsgFrame 
{
    public static MsgCallback msgCallback = new MsgCallback();
    public Dictionary<int, List<object>> listFrame = new Dictionary<int, List<object>>();
    public int minFrame = 0;
    public int maxFrame = 0;

    public void Init()
    {
        listFrame.Clear();
        minFrame = 0;
        maxFrame = 0;
        GameData.udpNet.RegisterMsgCallback(this, "OnRecv");
    }

    public void OnRecv(int frame, int uid, object msg)
    {
        if (frame < minFrame)
            return;
        List<object> msgs = listFrame[frame];
        if (msgs == null)
        {
            msgs = new List<object>();
            listFrame[frame] = msgs;
        }
        msgs.Add(msg);
        if (frame > maxFrame)
            maxFrame = frame;
    }

    public bool CheckFrame(int frame)
    {
        if (frame < minFrame)
            return false;
        if (frame > maxFrame)
            return false;
        if (listFrame[frame] == null)
            return false;
        return true;
    }

    public void UpdateLogic(int frame)
    {
        if (frame < minFrame)
            return;
        if (frame > maxFrame)
            return;
        List<object> msgs = listFrame[frame];
        if (msgs == null)
            return;
        for (int i = 0; i < msgs.Count; i++)
            msgCallback.OnMsg(msgs[i]);
    }
}
