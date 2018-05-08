using UnityEngine;
using System;
using System.Collections;
using System.Threading;
using Net;

public class App : MonoBehaviour 
{
    public Net.Network network = null;
    public Thread netThread = null;

    void Start()
    {
        InstallEvents();
        network = new Net.Network();
        network.Start();
        netThread = new Thread(new ThreadStart(this.ProcessNet));
        netThread.Start();
    }

    public void FixedUpdate()
    {
        Net.Event.ProcessOutEvent();
    }

    public void ProcessNet()
    {
        while (true)
        {
            if (network != null)
            {
                network.Process();
            }
            Net.Event.ProcessInEvent();
            System.Threading.Thread.Sleep(5);
        }
    }

    public void ProcessLogic()
    {

    }

    private void InstallEvents()
    {
        Net.Event.RegisterIn("battle.s2c_join", this, "s2c_join");
    }

    public void s2c_join(battle.s2c_join msg)
    {
        Debug.Log(msg.uid);
        Debug.Log(msg.name);
        Debug.Log(msg.icon);
    }
}

