using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SakuraZeroCommon.Tool
{
    public class Pos
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Pos(string str)
        {
            string[] strs = str.Split(',');
            X = Int32.Parse(strs[0]);
            Y = Int32.Parse(strs[1]);
            Z = Int32.Parse(strs[2]);
        }

        public Pos(float x = 0, float y = 0, float z = 0)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public override string ToString()
        {
            return $"{X},{Y},{Z}";
        }
    }
}
