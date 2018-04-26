using SakuraZeroCommon.Core;
using SakuraZeroCommon.Protocol;
using SakuraZeroServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer.Controller
{
    public class SystemController:BaseController
    {
        public SystemController()
        {
            requestCode = ERequestCode.System;
        }

        public void HeartBeat(Conn conn,ProtocolBase protocol)
        {
            conn.lastTickTime = TimeStamp.GetTimeStamp();
        }
    }
}
