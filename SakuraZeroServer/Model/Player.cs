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
        public ECharacter Character { get; set; }

        public Player(int playerid, string name, int userid, ESex sex, EPlayerJob playerJob, ECharacter character, int gold, int level, int exp, int mapnum, Pos pos)
        {
            ID = playerid;
            Name = name;
            UserID = userid;
            Sex = sex;
            PlayerJob = playerJob;
            Character = character;
            Gold = gold;
            Level = level;
            Exp = exp;
            MapNum = mapnum;
            Pos = pos;
        }
        

        public override string ToString()
        {
            string str = $"{ID}-{Name}-{Sex.ToString()}-{PlayerJob.ToString()}-{Character.ToString()}-{Gold}-{Level}-{Exp}-{MapNum}-{Pos.X}-{Pos.Y}-{Pos.Z}";
            return str;
        }
    }


}
