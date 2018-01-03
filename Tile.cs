using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    class Tile
    {
        public World World;
        public Position Position;
        public int GenerationWeight;
        public bool IsGenerated;


        public int Tier;
        public int Development;
    }
}
