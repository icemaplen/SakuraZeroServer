using System;
using System.Collections.Generic;
using SakuraZeroServer.Model;
using SakuraZeroServer.Core;
using SakuraZeroCommon.Core;
using SakuraZeroCommon.Protocol;
using SakuraZeroCommon.Property;

namespace SakuraZeroServer.Controller
{
    public class PlayerController : BaseController
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
                result.AddInt(playerList.Count);
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
        public void CreateRole(Conn conn, ProtocolBase protocol)
        {
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = protocol as ProtocolBytes;
            string name = p.GetString(start, ref start);
            ProtocolBytes result;
            if (dataMgr.GetPlayerIDByName(name) != -1)
            {
                result = new ProtocolBytes(requestCode, EActionCode.CreateRole, EReturnCode.RepeatName);
            }
            else
            {
                string jobStr = p.GetString(start, ref start);
                EPlayerJob job = (EPlayerJob)Enum.Parse(typeof(EPlayerJob), jobStr);

                string characterStr = p.GetString(start, ref start);
                ECharacter character = (ECharacter)Enum.Parse(typeof(ECharacter), characterStr);

                bool isSuccess = dataMgr.CreateRole(conn.user.ID, name, job, character);
                EReturnCode returnCode = isSuccess ? EReturnCode.Success : EReturnCode.Failed;
                result = new ProtocolBytes(requestCode, EActionCode.CreateRole, returnCode);
            }
            Send(conn, result);
        }

        /// <summary>
        /// 角色上线
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="protocol"></param>
        public void PlayerLogin(Conn conn, ProtocolBase protocol)
        {
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = protocol as ProtocolBytes;
            int playerid = p.GetInt(start);
            Player player = dataMgr.GetPlayer(playerid);
            ProtocolBytes result;
            if (player.UserID != conn.user.ID)
            {
                Console.WriteLine($"【警告】Player[{player.UserID}]不属于User[{conn.user.ID}]");
                result = new ProtocolBytes(requestCode, EActionCode.PlayerLogin, EReturnCode.Failed);
            }
            else
            {
                conn.player = player;
                result = new ProtocolBytes(requestCode, EActionCode.PlayerLogin, EReturnCode.Success);
            }
            Send(conn, result);
        }

        /// <summary>
        /// 下线
        /// </summary>
        public void PlayerLogout(Conn conn, ProtocolBase protocol)
        {
            dataMgr.SavaPlayer(conn.player);
            conn.player = null;

            ProtocolBytes result = new ProtocolBytes(requestCode, EActionCode.PlayerLogout, EReturnCode.Success);
            Send(conn, result);
        }

        public void DeleteRole(Conn conn, ProtocolBase protocol)
        {
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = protocol as ProtocolBytes;
            int playerid = p.GetInt(start);
            Player player = dataMgr.GetPlayer(playerid);
            ProtocolBytes result;
            if (player.UserID != conn.user.ID)
            {
                Console.WriteLine($"【警告】Player[{player.UserID}]不属于User[{conn.user.ID}]");
                result = new ProtocolBytes(requestCode, EActionCode.DeleteRole, EReturnCode.Failed);
            }
            else
            {
                bool isSucceed = dataMgr.DeleteRole(playerid);
                EReturnCode returnCode = isSucceed ? EReturnCode.Success : EReturnCode.Failed;
                result = new ProtocolBytes(requestCode, EActionCode.DeleteRole, returnCode);
            }
            Send(conn, result);
        }
    }
}
