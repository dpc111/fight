using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;

public class MsgCallback 
{
    public void OnMsg(object msg)
    {
        string name = msg.GetType().ToString();
        MethodInfo method = msg.GetType().GetMethod(name);
        if (method == null)
            return;
        method.Invoke(this, new object[] { msg });
    }
}
