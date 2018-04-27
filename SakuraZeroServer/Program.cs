using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroCommon.Protocol;
using SakuraZeroServer.Core;

namespace SakuraZeroServer
{
    class Program
    {
        private static string IP = "127.0.0.1";
        private static int PORT = 12345;

        static void Main(string[] args)
        {
            ServerNet serverNet = new ServerNet();
            serverNet.protocol = new ProtocolBytes();
            serverNet.Start(IP, PORT);


            Console.ReadKey();
        }
    }
}
