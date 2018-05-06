using SakuraZeroCommon.Core;
using SakuraZeroCommon.Protocol;
using SakuraZeroServer.Core;
using SakuraZeroServer.Tool;

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
