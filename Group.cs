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

        public Group(World world)
        {
            this.World = world;
            this.Tokens = new List<Token>();
        }
    }
}
