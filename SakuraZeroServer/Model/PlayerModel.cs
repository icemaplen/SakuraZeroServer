using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroCommon.Property;
using SakuraZeroCommon.Tool;

namespace SakuraZeroServer.Model
{
    class PlayerModel : RoleModel
    {

        public float CritRate { get; set; }     // 暴击率
        public float CritAttack { get; set; }   // 暴击伤害

        public EPlayerJob playerJob { get; set; }
    }


}
