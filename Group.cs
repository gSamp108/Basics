using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class Group
    {
        public World World;
        public List<Token> Tokens { get; private set; }
        public int TickOrder { get; set; }
        public decimal CurrentTickMineralIncome { get; set; }
        public decimal LastTickMineralIncome { get; set; }

        public Group(World world)
        {
            this.World = world;
            this.Tokens = new List<Token>();
        }

        public void StartPhaseTick()
        {
            this.LastTickMineralIncome = this.CurrentTickMineralIncome;
            this.CurrentTickMineralIncome = 0;
        }
    }
}
