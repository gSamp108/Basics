using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class RankedStat
    {
        public int Rank { get; set; }
        public int Curve { get; set; }
        public int Xp { get; set; }
        public int XpForIncrease { get { return ((this.Rank + 1) * this.Curve); } }

        public RankedStat() : this(1, 10) { }
        public RankedStat(int curve) : this(1, curve) { }
        public RankedStat(int rank, int curve)
        {
            this.Rank = rank;
            this.Curve = curve;
        }

        public void Gain(int amount)
        {
            this.Xp += amount;
            while (this.Xp >= this.XpForIncrease)
            {
                this.Xp -= this.XpForIncrease;
                this.Rank += 1;
            }
        }
    }
}
