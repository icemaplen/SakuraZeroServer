using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer
{
    class Server
    {
        public Socket listenfd;
        public Conn[] conns;
        public int maxConnCount = 50;       //最大连接数

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

        public void AcceptCallback(IAsyncResult ar)
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
                Console.WriteLine("Accept失败："+ex.Message);
            }
        }

        public void ReceiveCallback(IAsyncResult ar)
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
                string str = Encoding.UTF8.GetString(conn.readBuff, 0, count);
                Console.WriteLine($"收到[{conn.GetAddress()}]的数据：{str}");
                str = $"{conn.GetAddress()}:{str}";
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                for (int i = 0; i < conns.Length; i++)
                {
                    if (conns[i] == null || conns[i].isUse == false || conns[i] == conn)
                    {
                        continue;
                    }
                    Console.WriteLine("将消息传播给 "+conns[i].GetAddress());
                    conns[i].socket.Send(bytes);
                }

                conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain, SocketFlags.None, ReceiveCallback, conn);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"收到[{conn.GetAddress()}]断开连接");
                conn.Close();
            }
        }
    }
}
