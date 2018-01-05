using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class World
    {
        public class Resources
        {
            public Range Organics { get; set; }
            public Range Metallics { get; set; }
            public Range Conductives { get; set; }
            public Range Combustables { get; set; }
            public Range Chemicals { get; set; }
        }

        public Random Rng { get; private set; }
        public Tile this[Position position]
        {
            get
            {
                if (this.tiles.ContainsKey(position)) return this.tiles[position];
                else return null;
            }
        }
        private Dictionary<Position, Tile> tiles = new Dictionary<Position, Tile>();
        public Resources ResourceSpawnRate { get; private set; }
        public Resources ResourceSpawnDensitiy { get; private set; }

        public World(WorldGeneration worldGeneration)
        {
            foreach (var generatedTile in worldGeneration.GeneratedTiles)
            {
                var tile = new Tile(this, generatedTile.Position);
                this.tiles.Add(tile.Position, tile);
            }
        }
    }
}
