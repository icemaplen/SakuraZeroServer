using SakuraZeroServer.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroServer.Tool;

namespace SakuraZeroServer.Core
{
    public class ServerNet
    {
        #region 单例模式
        private static ServerNet instance;
        public static ServerNet Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ServerNet();
                }
                return instance;
            }
        }
        #endregion

        public Socket listenfd;
        public Conn[] conns;
        public int maxConnCount = 50;       //最大连接数

        private System.Timers.Timer timer = new System.Timers.Timer(1000);      // 主定时器
        public long heartBeatTime = 10;

        /// <summary>
        /// 获得连接池索引.
        /// </summary>
        /// <returns></returns>
        public int NewIndex()
        {
            if (conns == null)
            {
                return -1;
            }

            for (int i = 0; i < conns.Length; i++)
            {
                if (conns[i] == null)
                {
                    conns[i] = new Conn();
                    return i;
                }
                else if (conns[i].isUse == false)
                {
                    return i;
                }
            }

            return -1;
        }

        public void Start(string host, int port)
        {
            // 定时器
            timer.Elapsed += new System.Timers.ElapsedEventHandler(HandleMainTimer);
            timer.AutoReset = false;
            timer.Enabled = true;

            conns = new Conn[maxConnCount];
            for (int i = 0; i < maxConnCount; i++)
            {
                conns[i] = new Conn();
            }

            listenfd = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listenfd.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            listenfd.Listen(maxConnCount);
            listenfd.BeginAccept(AcceptCallback, null);
            Console.WriteLine("服务器启动成功...");
        }

        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = listenfd.EndAccept(ar);
                int index = NewIndex();
                if (index < 0)
                {
                    socket.Close();
                    Console.WriteLine("[警告]连接已满");
                }
                else
                {
                    Conn conn = conns[index];
                    conn.Init(socket);
                    string address = conn.GetAddress();
                    Console.WriteLine($"客户端连接[{address}] conn池ID：{index}");
                    conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain, SocketFlags.None, ReceiveCallback, conn);
                    listenfd.BeginAccept(AcceptCallback, null);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Accept失败：" + ex.Message);
            }
        }

        private void ReceiveCallback(IAsyncResult ar)
        {
            Conn conn = ar.AsyncState as Conn;
            try
            {
                int count = conn.socket.EndReceive(ar);
                if (count <= 0)
                {
                    Console.WriteLine($"收到[{conn.GetAddress()}]断开连接");
                    conn.Close();
                    return;
                }

                conn.buffCount += count;
                ProcessData(conn);
                //string str = Encoding.UTF8.GetString(conn.readBuff, 0, count);
                //Console.WriteLine($"收到[{conn.GetAddress()}]的数据：{str}");
                //str = $"{conn.GetAddress()}:{str}";
                //byte[] bytes = Encoding.UTF8.GetBytes(str);
                //for (int i = 0; i < conns.Length; i++)
                //{
                //    if (conns[i] == null || conns[i].isUse == false || conns[i] == conn)
                //    {
                //        continue;
                //    }
                //    Console.WriteLine("将消息传播给 " + conns[i].GetAddress());
                //    conns[i].socket.Send(bytes);
                //}

                conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain, SocketFlags.None, ReceiveCallback, conn);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"收到[{conn.GetAddress()}]断开连接:" + ex);
                conn.Close();
            }
        }

        private void ProcessData(Conn conn)
        {
            if (conn.buffCount < sizeof(Int32))
            {
                return;
            }

            Array.Copy(conn.readBuff, conn.lenBytes, sizeof(Int32));
            conn.msgLength = BitConverter.ToInt32(conn.lenBytes, 0);
            if (conn.buffCount < conn.msgLength + sizeof(Int32))
            {
                return;
            }

            // 处理消息
            string str = Encoding.UTF8.GetString(conn.readBuff, sizeof(Int32), conn.msgLength);
            Console.WriteLine($"收到来自[{conn.GetAddress()}]的消息{str}");
            // Send(conn,str);
            // 清除已处理的消息
            int count = conn.buffCount - conn.msgLength - sizeof(Int32);
            Array.Copy(conn.readBuff, sizeof(Int32) + conn.msgLength, conn.readBuff, 0, count);
            conn.buffCount = count;
            if (conn.buffCount > 0)
            {
                ProcessData(conn);
            }
        }


        public void Send(Conn conn, string str)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(str);
            byte[] length = BitConverter.GetBytes(bytes.Length);
            byte[] sendBuff = length.Concat(bytes).ToArray();

            try
            {
                conn.socket.BeginSend(sendBuff, 0, sendBuff.Length, SocketFlags.None, null, null);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"[发送消息]{conn.GetAddress()}:{ex.Message}");
            }
        }

        public void Close()
        {
            foreach (Conn c in conns)
            {
                if (c != null && c.isUse == true)
                {
                    lock (c)
                    {
                        c.Close();
                    }
                }
            }
        }

        public void HandleMainTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 处理心跳
            HeartBeat();
            timer.Start();
        }

        public void HeartBeat()
        {
            Console.WriteLine("主定时器执行");
            long timeNow = TimeStamp.GetTimeStamp();
            foreach (Conn c in conns)
            {
                if (c != null && c.isUse && c.lastTickTime<timeNow-heartBeatTime)
                {
                    Console.WriteLine("[心跳断开]:"+c.GetAddress());
                    lock (c)
                    {
                        c.Close();
                    }
                }
            }
        }
    }
}
