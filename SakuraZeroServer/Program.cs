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
        static void Main(string[] args)
        {
            ServerNet serverNet = new ServerNet();
            serverNet.protocol = new ProtocolBytes();
            serverNet.Start("127.0.0.1", 12345);


            Console.ReadKey();
        }
    }
}
