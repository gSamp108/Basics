using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class Tile
    {
        public class ResourceSettings
        {
            public RankedStat Organics { get; set; }
            public RankedStat Metallics { get; set; }
            public RankedStat Conductives { get; set; }
            public RankedStat Combustables { get; set; }
            public RankedStat Chemicals { get; set; }
        }

        public World World { get; private set; }
        public Position Position { get; private set; }
        public ResourceSettings Resources { get; private set; }

        public Tile(World world, Position position)
        {
            this.World = world;
            this.Position = position;

        }
    }
}
