using System;
using System.Net.Sockets;
using SakuraZeroServer.Tool;
using SakuraZeroCommon.Protocol;
using SakuraZeroServer.Model;

namespace SakuraZeroServer.Core
{
    public class Conn
    {
        public const int BUFFER_SIZE = 1024;
        public Socket socket;
        public bool isUse = false;
        public byte[] readBuff;
        public int buffCount = 0;       // 当前读缓冲区的长度
        public byte[] lenBytes = new byte[sizeof(Int32)];
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
            try
            {
                return isUse ? socket.RemoteEndPoint.ToString() : "该Conn未使用，无法获取地址";
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("未知错误，无法获取地址\n" + ex.Message);
                return null;
            }
        }

        public void Send(ProtocolBase protocol)
        {
            ServerNet.Instance.Send(this, protocol);
        }

        public void Close()
        {
            if (isUse)
            {
                if (player != null)
                {
                    // 玩家退出处理
                    //DataManager.Instance.SavaPlayer(player);
                }
                Console.WriteLine("[断开连接]" + GetAddress());
                socket.Close();
                isUse = false;
                user = null;
                player = null;
            }
        }
    }
}
