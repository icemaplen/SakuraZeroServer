using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroServer.Model;
using SakuraZeroServer.DAO;
using SakuraZeroServer.Core;
using SakuraZeroCommon.Core;

namespace SakuraZeroServer.Controller
{
    public class PlayerController:BaseController
    {
        public PlayerController()
        {
            requestCode = ERequestCode.Player;
        }
    }
}
