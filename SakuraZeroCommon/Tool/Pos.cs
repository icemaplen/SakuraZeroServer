using System;

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
    }
}
