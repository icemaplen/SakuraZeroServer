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
        public int UserID { get; set; }
        public int Gold { get; set; }
        public float CritRate { get; set; }     // 暴击率
        public float CritAttack { get; set; }   // 暴击伤害

        public EPlayerJob PlayerJob { get; set; }

        public Player(int playerid, string name, int userid, ESex sex, EPlayerJob playerJob, int gold, int level, int exp, int mapnum, Pos pos)
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

        public override string ToString()
        {
            string str = $"{ID}-{Name}-{Sex.ToString()}-{PlayerJob.ToString()}-{Gold}-{Level}-{Exp}-{MapNum}-{Pos.ToString()}";
            return str;
        }
    }


}
