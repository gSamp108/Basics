using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public struct Position
    {
        public int X;
        public int Y;

        public override bool Equals(object obj)
        {
            return obj is Position && (((Position)obj).X == this.X && ((Position)obj).Y == this.Y);
        }
        public override int GetHashCode()
        {
            var result = 27;
            unchecked
            {
                result *= 31 * this.X;
                result *= 31 * this.Y;
            }
            return result;
        }
        public override string ToString()
        {
            return "(" + this.X + ", " + this.Y + ")";
        }
        public Position(int x, int y) { this.X = x; this.Y = y; }
    }
}
