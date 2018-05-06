using SakuraZeroCommon.Core;
using SakuraZeroServer.Core;
using SakuraZeroCommon.Protocol;

namespace SakuraZeroServer.Controller
{
    public class BaseController
    {
        protected DataManager dataMgr = DataManager.Instance;
        protected ERequestCode requestCode;

        
        protected virtual void Send(Conn conn, ProtocolBase protocal)
        {
            conn.Send(protocal);
        }
    }

}
