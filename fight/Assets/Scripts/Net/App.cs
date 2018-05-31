namespace Net
{
    using UnityEngine;
    using System;
    using System.Collections;
    using System.Threading;
    using Game;

    public class App
    {
        public static App app = null;
        private static readonly object locker = new object();
        public Net.Network network = null;
        public Thread netThread = null;

        public static App Instance()
        {
            if (app == null)
            {
                lock (locker)
                {
                    app = new App();
                }
            }
            return app;
        }

        public void Start()
        {
            App.Instance().InstallEvents();
            App.Instance().network = new Net.Network();
            App.Instance().network.Start();
            App.Instance().netThread = new Thread(new ThreadStart(App.app.ProcessNet));
            App.Instance().netThread.Start();
            Game.GameWord.Instance().Init();
        }

        public void Quit() 
        {
            App.Instance().network.Close();
            App.Instance().netThread.Abort();
        }

        public void ProcessMain()
        {
            Net.Event.ProcessOutEvent();
        }


        public void ProcessNet()
        {
            while (true)
            {
                if (App.Instance().network != null)
                {
                    App.Instance().network.Process();
                }
                Net.Event.ProcessInEvent();
                System.Threading.Thread.Sleep(5);
            }
        }

        private void InstallEvents()
        {
            Net.Event.RegisterIn("Send", this, "OnSend");
            Net.Event.RegisterIn("OnLoginConnect", this, "OnLoginConnect");
            Net.Event.RegisterIn("OnFightConnect", this, "OnFightConnect");
        }

        public void Send(object msg)
        {
            Net.Event.FireIn("Send", msg);
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        // event
        public void OnSend(object tmsg)
        {
            App.Instance().network.Send(tmsg);
        }

        public void OnLoginConnect()
        {
            App.Instance().network.Reset();
            App.Instance().network.ConnectTo(Config.loginIp, Config.loginPort, null, null);
        }

        public void OnFightConnect()
        {
            App.Instance().network.Reset();
            App.Instance().network.ConnectTo(Config.fightIp, Config.fightPort, null, null);
        }
    }
}