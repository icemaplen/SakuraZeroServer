using SakuraZeroServer.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using SakuraZeroCommon.Protocol;
using SakuraZeroCommon.Core;
using SakuraZeroServer.Controller;
using System.Reflection;

namespace SakuraZeroServer.Core
{
    public class ServerNet
    {
        public static ServerNet Instance;

        public Socket serverCocket;
        public Conn[] conns;
        public int maxConnCount = 10;       //最大连接数
        public ProtocolBase protocol;       //协议类型

        private System.Timers.Timer timer = new System.Timers.Timer(1000);      // 主定时器
        private long heartBeatTime = 10;
        private Dictionary<ERequestCode, BaseController> requestDict;


        public ServerNet()
        {
            Instance = this;
        }

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
            InitRequestDict();

            // 定时器，每隔1000ms执行一次
            timer.Elapsed += new System.Timers.ElapsedEventHandler(HandleMainTimer);
            timer.AutoReset = false;
            timer.Enabled = true;

            // 初始化连接池
            conns = new Conn[maxConnCount];
            for (int i = 0; i < maxConnCount; i++)
            {
                conns[i] = new Conn();
            }

            serverCocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            serverCocket.Bind(new IPEndPoint(IPAddress.Parse(host), port));
            serverCocket.Listen(maxConnCount);
            serverCocket.BeginAccept(AcceptCallback, null);
            Console.WriteLine("服务器启动成功...");
        }

        /// <summary>
        /// 初始化Request列表.
        /// </summary>
        private void InitRequestDict()
        {
            requestDict = new Dictionary<ERequestCode, BaseController>();
            requestDict.Add(ERequestCode.User, new UserController());
            requestDict.Add(ERequestCode.System, new SystemController());
            requestDict.Add(ERequestCode.Player, new PlayerController());
            requestDict.Add(ERequestCode.Inventory, new InventoryController());
            // TODO
        }

        /// <summary>
        /// 接收连接的回调方法
        /// </summary>
        /// <param name="ar"></param>
        private void AcceptCallback(IAsyncResult ar)
        {
            try
            {
                Socket socket = serverCocket.EndAccept(ar);
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
                    Console.WriteLine($"监听到客户端连接：[{address}] --- Conn池ID：{index}");
                    conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain, SocketFlags.None, ReceiveCallback, conn);
                    serverCocket.BeginAccept(AcceptCallback, null);
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine("Accept失败：" + ex.Message);
            }
        }

        /// <summary>
        /// 接收消息的回调方法
        /// </summary>
        /// <param name="ar"></param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            Conn conn = ar.AsyncState as Conn;
            try
            {
                int count = conn.socket.EndReceive(ar);
                if (count <= 0)
                {
                    conn.Close();
                    return;
                }

                conn.buffCount += count;
                ProcessData(conn);
                conn.socket.BeginReceive(conn.readBuff, conn.buffCount, conn.BuffRemain, SocketFlags.None, ReceiveCallback, conn);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"ReceiveCallback产生异常:" + ex);
                conn.Close();
            }
        }

        /// <summary>
        /// 粘包消息处理.
        /// </summary>
        /// <param name="conn"></param>
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
            ProtocolBase proto = protocol.Decode(conn.readBuff, sizeof(Int32), conn.msgLength);
            HandleMsg(conn, proto);
            // 清除已处理的消息
            int count = conn.buffCount - conn.msgLength - sizeof(Int32);
            Array.Copy(conn.readBuff, sizeof(Int32) + conn.msgLength, conn.readBuff, 0, count);
            conn.buffCount = count;
            if (conn.buffCount > 0)
            {
                ProcessData(conn);
            }
        }

        /// <summary>
        /// 消息分发
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="protocolBase"></param>
        private void HandleMsg(Conn conn, ProtocolBase protocolBase)
        {
            ERequestCode requestCode = protocolBase.RequestCode;
            EActionCode actionCode = protocolBase.ActionCode;
            Console.WriteLine($"收到来自[{conn.GetAddress()}]协议:[{ requestCode.ToString()}---{actionCode.ToString()}]");
            BaseController controller;
            if (!requestDict.TryGetValue(requestCode, out controller))
            {
                Console.WriteLine($"【警告】未找到Request:{requestCode.ToString()}对应的方法");
                return;
            }
            MethodInfo method = controller.GetType().GetMethod(actionCode.ToString());
            if (method == null)
            {
                Console.WriteLine($"【警告】未找到Action:{actionCode.ToString()}对应的方法");
                return;
            }
            object[] objs = new object[] { conn, protocolBase };
            method.Invoke(controller, objs);
        }

        /// <summary>
        /// 向客户端发送消息
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="protocol"></param>
        public void Send(Conn conn, ProtocolBase protocol)
        {
            byte[] bytes = protocol.Encode();
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

        /// <summary>
        /// 消息广播
        /// </summary>
        /// <param name="protocol"></param>
        public void Broadcast(ProtocolBase protocol)
        {
            foreach (Conn c in conns)
            {
                if (c != null && c.isUse)
                {
                    Send(c, protocol);
                }
            }
        }

        public void Close()
        {
            foreach (Conn c in conns)
            {
                if (c != null && c.isUse)
                {
                    lock (c)
                    {
                        c.Close();
                    }
                }
            }
        }

        private void HandleMainTimer(object sender, System.Timers.ElapsedEventArgs e)
        {
            // 处理心跳
            HeartBeat();
            timer.Start();
        }

        /// <summary>
        /// 心跳处理
        /// </summary>
        private void HeartBeat()
        {
            long timeNow = TimeStamp.GetTimeStamp();
            foreach (Conn c in conns)
            {
                if (c != null && c.isUse && c.lastTickTime < timeNow - heartBeatTime)
                {
                    Console.WriteLine("[心跳断开]:" + c.GetAddress());
                    lock (c)
                    {
                        c.Close();
                    }
                }
            }
        }
    }
}
