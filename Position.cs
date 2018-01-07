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
        public IEnumerable<Position> Adjacent
        {
            get
            {
                yield return new Position(this.X - 1, this.Y + 0);
                yield return new Position(this.X + 0, this.Y - 1);
                yield return new Position(this.X + 1, this.Y + 0);
                yield return new Position(this.X + 0, this.Y + 1);
            }
        }
        public IEnumerable<Position> Nearby
        {
            get
            {
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        if ((x != 0) || (y != 0)) yield return new Position(this.X + x, this.Y + y);
                    }
                }
            }
        }
        public IEnumerable<Position> AdjacentInRange(int range)
        {
            var results = new HashSet<Position>();
            var currentRing = new HashSet<Position>();
            currentRing.Add(this);

            for (int i = 0; i < range; i++)
            {
                var nextRing = new HashSet<Position>();
                foreach (var position in currentRing)
                {
                    foreach (var adjacent in position.Adjacent)
                    {
                        if (!results.Contains(adjacent) && !currentRing.Contains(adjacent)) nextRing.Add(adjacent);
                    }
                    results.Add(position);
                }
                currentRing = nextRing;
            }

            return results;
        }
    }
}
