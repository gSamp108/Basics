using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class Tile
    {
        public World World { get; private set; }
        public Position Position { get; private set; }
        public bool MineralNode { get; private set; }

        public Tile(World world, Position position)
        {
            this.World = world;
            this.Position = position;
            if (this.World.Rng.Percentage() < this.World.MineralNodeSpawnChance) this.MineralNode = true;
        }
    }
}
