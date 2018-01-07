using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class World
    {
        public enum Phases { Start, ResourceGeneration, Planning, ResourceAllocation, Movement, Combat, End }

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
        public Phases CurrentPhase { get; private set; }

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

        public void Tick()
        {
            if (this.CurrentPhase == Phases.Start) this.StartPhaseTick();
            else if (this.CurrentPhase == Phases.ResourceGeneration) this.ResourceGenerationPhaseTick();
            else if (this.CurrentPhase == Phases.Planning) this.PlanningPhaseTick();
            else if (this.CurrentPhase == Phases.ResourceAllocation) this.ResourceAllocationPhaseTick();
            else if (this.CurrentPhase == Phases.Movement) this.MovementPhaseTick();
            else if (this.CurrentPhase == Phases.Combat) this.CombatPhaseTick();
            else if (this.CurrentPhase == Phases.End) this.EndPhaseTick();


            var groupTickOrder = this.Groups.OrderBy(o => o.TickOrder).ToList();
            this.PlanningPhaseTick();
        }

        private void StartPhaseTick()
        {
            foreach (var group in this.Groups)
            {
                group.StartPhaseTick();
            }
            foreach (var token in this.Tokens)
            {
                token.StartPhaseTick();
            }
            this.CurrentPhase = Phases.ResourceAllocation;
        }
        private void ResourceGenerationPhaseTick()
        {
            foreach (var token in this.Tokens.Where(o => o.Type == TokenTypes.Extractor))
            {
                var minerals = token.Ability;
                token.Group.CurrentTickMineralIncome += minerals;
            }
            this.CurrentPhase = Phases.Planning;
        }
        private void PlanningPhaseTick()
        {
            this.CurrentPhase = Phases.ResourceAllocation;
        }
        private void ResourceAllocationPhaseTick()
        {
            this.CurrentPhase = Phases.Movement;
        }
        private void MovementPhaseTick()
        {
            this.CurrentPhase = Phases.Combat;
        }
        private void CombatPhaseTick()
        {
            this.CurrentPhase = Phases.End;
        }
        private void EndPhaseTick()
        {
            this.CurrentPhase = Phases.Start;
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
            this.CurrentPhase = Phases.Start;
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
            group.TickOrder = this.Groups.Count;
        }
    }
}
