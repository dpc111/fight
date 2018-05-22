namespace Net
{
    using System;
    using System.Net.Sockets;
    using System.Net;
    using System.Collections;
    using System.Collections.Generic;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Runtime.Remoting.Messaging;

    public class Config
    {
        public static int tcpPacketMax = 10240;
        public static string ip = "139.199.82.153";
        public static int port = 7769;
        public static string loginIp = "139.199.82.153";
        public static int loginPort = 7769;
        public static string fightIp = "139.199.82.153";
        public static int fightPort = 7769;
    }
}