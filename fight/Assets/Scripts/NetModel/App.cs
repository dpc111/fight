namespace NetModel
{
    using UnityEngine;
    using System;
    using System.Net.Sockets;
    using System.Net;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading;
    using System.Runtime.Remoting.Messaging;

    public class App 
    {
        public static App app = null;
        public Network network = null;

        public App()
        {
            app = this;
            network = new Network();
            network.Start();
        }

        public void process()
        {
            if (network != null)
            {
                network.Process();
            }
            Event.ProcessInEvent();
        }
    }

}