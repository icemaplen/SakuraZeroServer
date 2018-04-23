using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer
{
    class Conn
    {
        public const int BUFFER_SIZE = 1024;
        public Socket socket;
        public bool isUse = false;
        public byte[] readBuff = new byte[BUFFER_SIZE];
        public int buffCount = 0;       // 当前读缓冲区的长度

        public int BuffRemain
        {
            get { return BUFFER_SIZE - buffCount; }
        }       // 缓冲区剩余的字节数

        public Conn()
        {
            readBuff = new byte[BUFFER_SIZE];
        }

        public void Init(Socket socket)
        {
            this.socket = socket;
            isUse = true;
            buffCount = 0;
        }

        public string GetAddress()
        {
            return isUse ? socket.RemoteEndPoint.ToString() : "无法获取地址";
        }

        public void Close()
        {
            if (isUse)
            {
                Console.WriteLine("[断开连接]" + GetAddress());
                socket.Close();
                isUse = false;
            }
        }
    }
}
