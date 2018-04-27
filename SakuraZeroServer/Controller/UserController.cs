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
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = proto as ProtocolBytes;
            string username = p.GetString(start, ref start);
            string password = p.GetString(start, ref start);
            User user = dataMgr.VerifyUser(username, password);
            ProtocolBytes result;
            if (user != null)
            {
                // 登录成功，返回用户id和用户名
                conn.user = user;
                result = new ProtocolBytes(requestCode, EActionCode.Login, EReturnCode.Success);
                result.AddInt(user.ID);
                result.AddString(user.Username);

            }
            else
            {
                result = new ProtocolBytes(requestCode, EActionCode.Login, EReturnCode.Failed);
            }

            Send(conn, result);
        }

        public void Register(Conn conn, ProtocolBase proto)
        {
            int start = sizeof(Int32) * 3;
            ProtocolBytes p = proto as ProtocolBytes;
            string username = p.GetString(start, ref start);
            ProtocolBytes result;
            if (dataMgr.CanGetUser(username))
            {
                result = new ProtocolBytes(requestCode, EActionCode.Register, EReturnCode.RepeatName);
            }
            else
            {
                string password = p.GetString(start, ref start);
                bool createResult = dataMgr.CreateUser(username, password);
                // 有可能会创建失败，返回未知错误编号
                EReturnCode returnCode = createResult ? EReturnCode.Success : EReturnCode.None;
                result = new ProtocolBytes(requestCode, EActionCode.Register, returnCode);
            }
            Send(conn, result);
        }
    }
}
