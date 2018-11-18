using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public void Release()
    {
        listFrame.Clear();
        minFrame = 0;
        maxFrame = 0;
    }

    public void OnRecv(int frame, int uid, object msg)
    {
        Debug.Log(frame + " " + listFrame.Count);
        if (frame < minFrame)
            return;
        if (!listFrame.ContainsKey(frame))
        {
            List<object> newmsgs = new List<object>();
            listFrame[frame] = newmsgs;
        }
        if (frame > maxFrame)
            maxFrame = frame;
        if (msg == null)
            return;
        List<object> msgs = listFrame[frame];
        msgs.Add(msg);
    }

    public bool CheckFrame(int frame)
    {
        if (frame < minFrame)
            return false;
        if (frame > maxFrame)
            return false;
        if (!listFrame.ContainsKey(frame))
            return false;
        return true;
    }

    public void UpdateLogic(int frame)
    {
        if (frame < minFrame)
            return;
        if (frame > maxFrame)
            return;
        if (!listFrame.ContainsKey(frame))
            return;
        List<object> msgs = listFrame[frame];
        for (int i = 0; i < msgs.Count; i++)
            msgCallback.OnMsg(msgs[i]);
        listFrame.Remove(frame);
        minFrame = frame;
    }
}
