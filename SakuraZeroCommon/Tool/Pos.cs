using System;

namespace SakuraZeroCommon.Tool
{
    public class Pos
    {
        public int MapNum { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Pos(float x = 0, float y = 0, float z = 0)
        {
            MapNum = 1;
            X = x;
            Y = y;
            Z = z;
        }

        public Pos(int mapNum = 1, float x = 0, float y = 0, float z = 0)
        {
            MapNum = mapNum;
            X = x;
            Y = y;
            Z = z;
        }
    }
}
