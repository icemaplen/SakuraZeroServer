using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SakuraZeroCommon.Property;
using SakuraZeroCommon.Tool;

namespace SakuraZeroServer.Model
{
    public class Player : Role
    {
        public float CritRate { get; set; }     // 暴击率
        public float CritAttack { get; set; }   // 暴击伤害

        public EPlayerJob playerJob { get; set; }

        public Player(int playerid, string name, int userid, int sex, int playerJob, int level, int exp, int mapnum, string pox)
        {

        }

        /// <summary>
        /// 下线
        /// </summary>
        /// <returns></returns>
        public bool Logout()
        {
            // 事件处理
            return true;
        }
    }


}
