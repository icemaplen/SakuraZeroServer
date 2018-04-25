using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroCommon.Prorocal;
using SakuraZeroServer.Core;

namespace SakuraZeroServer
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine(DataManager.Instance.Register("123","123"));
            //Console.WriteLine(DataManager.Instance.VerifyUser("123","123")?.Username);
            ServerNet serverNet = new ServerNet();
            serverNet.protocal = new ProtocalBytes();
            serverNet.Start("127.0.0.1", 12345);


            Console.ReadKey();
        }
    }
}
