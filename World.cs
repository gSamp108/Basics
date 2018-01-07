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
        public IEnumerable<Tile> Tiles { get { foreach (var tile in this.tiles.Values) { yield return tile; } } }
        public List<Token> Tokens { get; private set; }
        public List<Group> Groups { get; private set; }
        public int GroupSpawnRadius { get; private set; }
        public decimal GroupSpawnChance { get; private set; }
        public int MineralSpawnRadius { get; private set; }
        public decimal MineralSpawnChance { get; private set; }

        public World(WorldGeneration worldGeneration)
        {
            this.Initialize();
            foreach (var generatedTile in worldGeneration.GeneratedTiles)
            {
                var tile = new Tile(this, generatedTile.Position);
                this.tiles.Add(tile.Position, tile);
            }
            this.SpawnMinerals();
            this.SpawnGroups();
        }

        private void SpawnMinerals()
        {
            var open = new HashSet<Position>(this.tiles.Keys);
            while (open.Count > 0)
            {
                var tile = open.ToList()[this.Rng.Next(open.Count)];
                foreach (var excludedTile in tile.AdjacentInRange(this.MineralSpawnRadius))
                {
                    open.Remove(excludedTile);
                }
                if (this.Rng.Percentage() < this.MineralSpawnChance) this[tile].SpawnMineralNode();
            }
        }

        private void Initialize()
        {
            this.GroupSpawnRadius = 10;
            this.GroupSpawnChance = 0.5m;
            this.MineralSpawnRadius = 3;
            this.MineralSpawnChance = 0.1m;
            this.Tokens = new List<Token>();
            this.Groups = new List<Group>();
            this.Rng = new Random();
            this.tiles = new Dictionary<Position, Tile>();
        }

        private void SpawnGroups()
        {
            var open = new HashSet<Position>(this.tiles.Keys);
            while (open.Count > 0)
            {
                var tile = open.ToList()[this.Rng.Next(open.Count)];
                foreach (var excludedTile in tile.AdjacentInRange(this.GroupSpawnRadius))
                {
                    open.Remove(excludedTile);
                }
                if (this.Rng.Percentage() < this.GroupSpawnChance)
                {
                    var group = new Group(this);
                    this.AddGroup(group);
                    var token = new Token(this, tile, TokenTypes.Unit, group);
                    this.AddToken(token);
                }
            }            
        }

        private void AddToken(Token token)
        {
            this.Tokens.Add(token);
            token.Group.Tokens.Add(token);
            var tile = this[token.Position];
            tile.AddToken(token);
        }

        private void AddGroup(Group group)
        {
            this.Groups.Add(group);
        }
    }
}
