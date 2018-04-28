using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroServer.Model;
using SakuraZeroServer.DAO;
using SakuraZeroServer.Core;
using SakuraZeroCommon.Core;
using SakuraZeroCommon.Protocol;
using SakuraZeroCommon.Property;

namespace SakuraZeroServer.Controller
{
    public class PlayerController:BaseController
    {
        public PlayerController()
        {
            requestCode = ERequestCode.Player;
        }

        public void GetRoles(Conn conn, ProtocolBase protocol)
        {
            ProtocolBytes result;
            List<Player> playerList = dataMgr.GetPlayers(conn.user.ID);
            if (playerList != null)
            {
                result = new ProtocolBytes(requestCode, EActionCode.GetRoles, EReturnCode.Success);
                foreach (Player p in playerList)
                {
                    result.AddString(p.ToString());
                }
            }
            else
            {
                result = new ProtocolBytes(requestCode, EActionCode.GetRoles, EReturnCode.Failed);
            }
            Send(conn, result);
        }
        
        /// <summary>
        /// 返回值为Player
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="protocol"></param>
        public void CreateRole(Conn conn,ProtocolBase protocol)
        {
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = protocol as ProtocolBytes;
            string name = p.GetString(start, ref start);
            string jobStr = p.GetString(start, ref start);
            EPlayerJob job = (EPlayerJob)Enum.Parse(typeof(EPlayerJob), jobStr);
            bool isSuccess = dataMgr.CreateRole(conn.user.ID, name, job);
            ProtocolBytes result;
            if (isSuccess)
            {
                result = new ProtocolBytes(requestCode, EActionCode.CreateRole, EReturnCode.Success);
                int roleid = dataMgr.GetPlayerIDByName(name);
                Player player = dataMgr.GetPlayer(roleid);
                result.AddString(player.ToString());
            }
            else
            {
                result = new ProtocolBytes(requestCode, EActionCode.CreateRole, EReturnCode.Failed);
            }
            Send(conn, result);
        }

        /// <summary>
        /// 角色上线
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="protocol"></param>
        public void Login(Conn conn, ProtocolBase protocol)
        {
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = protocol as ProtocolBytes;
            int playerid = p.GetInt(start);
            Player player = dataMgr.GetPlayer(playerid);
            ProtocolBytes result;
            if (player.UserID != conn.user.ID)
            {
                Console.WriteLine($"【警告】Player[{player.UserID}]不属于User[{conn.user.ID}]");
                result = new ProtocolBytes(requestCode, EActionCode.Login, EReturnCode.Failed);
            }
            else
            {
                conn.player = player;
                result = new ProtocolBytes(requestCode, EActionCode.Login, EReturnCode.Success);
            }
            Send(conn, result);
        }

        /// <summary>
        /// 下线
        /// </summary>
        public void Logout(Conn conn, ProtocolBase protocol)
        {
            dataMgr.SavaPlayer(conn.player);
            conn.player = null;

            ProtocolBytes result= new ProtocolBytes(requestCode, EActionCode.Logout, EReturnCode.Success);
            Send(conn, result);
        }
    }
}
