using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroServer.Controller;
using SakuraZeroServer.Tool;
using SakuraZeroCommon.Protocol;
using SakuraZeroServer.Model;

namespace SakuraZeroServer.Core
{
   public  class Conn
    {
        public const int BUFFER_SIZE = 1024;
        public Socket socket;
        public bool isUse = false;
        public byte[] readBuff = new byte[BUFFER_SIZE];
        public int buffCount = 0;       // 当前读缓冲区的长度
        public byte[] lenBytes = new byte[sizeof(UInt32)];
        public Int32 msgLength = 0;
        public long lastTickTime = long.MinValue;   // 心跳时间
        public Player player;
        public User user;

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
            // 心跳处理
            lastTickTime = TimeStamp.GetTimeStamp();
        }

        public string GetAddress()
        {
            return isUse ? socket.RemoteEndPoint.ToString() : "无法获取地址";
        }

        public void Send(ProtocolBase protocol)
        {
            ServerNet.Instance.Send(this, protocol);
        }

        /// <summary>
        /// 下线
        /// </summary>
        public void Logout()
        {
            DataManager.Instance.SavaPlayer(player);
            player = null;
            Close(); 
        }

        public void Close()
        {
            if (isUse)
            {
                if (player != null)
                {
                    // 玩家退出处理
                    Logout();
                }
                Console.WriteLine("[断开连接]" + GetAddress());
                socket.Close();
                isUse = false;
                user = null;
            }
        }
    }
}
