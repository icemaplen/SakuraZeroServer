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

namespace SakuraZeroServer.Controller
{
    public class PlayerController:BaseController
    {
        public PlayerController()
        {
            requestCode = ERequestCode.Player;
        }

        public void KickOff(int id)
        {
            foreach (Conn c in ServerNet.Instance.conns)
            {
                if (c != null && c.isUse && c.player != null && c.player.ID == id)
                {
                    lock (c.player)
                    {
                        ProtocolBytes p = new ProtocolBytes(requestCode, EActionCode.KickOff, EReturnCode.None);
                        Send(c, p);
                        c.player.Logout();
                    }
                }
            }
        }

        public void GetRoles(Conn conn, ProtocolBase protocol)
        {
            int start = sizeof(Int32) * 3;
            //ProtocolBytes result=new ProtocolBytes(requestCode,)

        }
    }
}
