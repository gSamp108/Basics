using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class World
    {
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
        public decimal MineralNodeSpawnChance { get; private set; }
        public IEnumerable<Tile> Tiles { get { foreach (var tile in this.tiles.Values) { yield return tile; } } }

        public World(WorldGeneration worldGeneration)
        {
            this.Rng = new Random();
            this.tiles = new Dictionary<Position, Tile>();
            this.MineralNodeSpawnChance = 0.01m;
            foreach (var generatedTile in worldGeneration.GeneratedTiles)
            {
                var tile = new Tile(this, generatedTile.Position);
                this.tiles.Add(tile.Position, tile);
            }
        }
    }
}
