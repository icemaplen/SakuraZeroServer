using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
