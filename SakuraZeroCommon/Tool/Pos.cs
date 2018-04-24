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
