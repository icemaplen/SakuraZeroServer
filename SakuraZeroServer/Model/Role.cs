using SakuraZeroCommon.Property;
using SakuraZeroCommon.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroServer.Model
{
    public class Role
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ESex Sex { get; set; }
        public int Level { get; set; }
        public int Exp { get; set; }
        public int HP { get; set; }
        public int MP { get; set; }
        public int MapNum { get; set; }
        public Pos pos { get; set; }
        public int Attack { get; set; }
        public float HitRate { get; set; }      // 命中率
        public float Puncture { get; set; }     // 穿刺
        public float Defence { get; set; }      // 防御
        public float DodgeRate { get; set; }    // 闪避率
    }
}
