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
        public static string ip = "192.168.0.1";
        public static int port = 1001;
    }
}