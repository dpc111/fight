using System.Collections;
using System.Collections.Generic;

public class FrameMsg 
{
    public Dictionary<int, List<object>> listFrame = new Dictionary<int, List<object>>();
    public static MsgCallback msgCallback = new MsgCallback();
    public int minFrame = 0;
    public int maxFrame = 0;

    public void Init()
    {
        listFrame.Clear();
        minFrame = 0;
        maxFrame = 0;
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
