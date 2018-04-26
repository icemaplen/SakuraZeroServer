using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroServer.DAO;
using SakuraZeroServer.Core;
using SakuraZeroCommon.Core;
using SakuraZeroCommon.Protocol;
using SakuraZeroServer.Model;

namespace SakuraZeroServer.Controller
{
    public class UserController:BaseController
    {
        public UserController()
        {
            requestCode = ERequestCode.User;
        }

        public void Login(Conn conn, ProtocolBase proto)
        {
            int start = sizeof(Int32) * 2;
            ProtocolBytes p = proto as ProtocolBytes;
            string username = p.GetString(start, ref start);
            string password = p.GetString(start, ref start);
            UserModel user = DataManager.Instance.VerifyUser(username, password);
            EReturnCode returnCode = user == null ? EReturnCode.Failed : EReturnCode.Success;
            p = new ProtocolBytes(requestCode, EActionCode.Login,returnCode);
            conn.Send(p);
        }

        public void Regisger()
        {

        }
    }
}
