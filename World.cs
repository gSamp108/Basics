using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    class World
    {
        public Dictionary<Position, Tile> Tiles = new Dictionary<Position, Tile>();
        public HashSet<Tile> GeneratedTiles = new HashSet<Tile>();
        public HashSet<Tile> UngeneratedTiles = new HashSet<Tile>();
        public HashSet<Tile> GeneratableTiles = new HashSet<Tile>();
        public Random Rng = new Random();
        public int TargetGenerationSize = 100;

        public World()
        {
            this.GenerateTile(new Position());
        }

        public void GenerationStep()
        {
            if (this.GeneratedTiles.Count < this.TargetGenerationSize)
            {
                if (this.GeneratableTiles.Count > 0)
                {
                    var tile = this.GeneratableTiles.ToList()[this.Rng.Next(this.GeneratableTiles.Count)];
                }
            }
        }

        public void InitializeTile(Position position)
        {
            if (!this.Tiles.ContainsKey(position))
            {
                this.Tiles.Add(position, new Tile());
                var tile = this.Tiles[position];
                tile.World = this;
                tile.Position = position;
                tile.IsGenerated = false;
                tile.GenerationWeight = 0;
                this.UngeneratedTiles.Add(tile);
            }
        }
        public void GenerateTile(Position position)
        {
            if (!this.Tiles.ContainsKey(position)) this.InitializeTile(position);
            var tile = this.Tiles[position];
            if (!tile.IsGenerated)
            {
                this.UngeneratedTiles.Remove(tile);
                this.GeneratableTiles.Remove(tile);
                this.GeneratedTiles.Add(tile);
                tile.IsGenerated = true;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        var scanPosition = new Position(tile.Position.X + x, tile.Position.Y + y);
                        if (!this.Tiles.ContainsKey(scanPosition)) this.InitializeTile(scanPosition);
                        var scanTile = this.Tiles[scanPosition];
                        scanTile.GenerationWeight += 1;
                        if (!scanTile.IsGenerated)
                        {
                            if (x == 0 || y == 0)
                            {
                                this.GeneratableTiles.Add(scanTile);
                            }
                        }
                        if (scanTile.GenerationWeight > 4 && !scanTile.IsGenerated) this.GenerateTile(scanTile.Position);
                    }
                }
            }
        }
        public void DegenerateTile(Position position)
        {
            if (!this.Tiles.ContainsKey(position)) this.InitializeTile(position);
            var tile = this.Tiles[position];
            if (tile.IsGenerated)
            {
                this.GeneratedTiles.Remove(tile);
                this.GeneratableTiles.Add(tile);
                this.UngeneratedTiles.Add(tile);
                tile.IsGenerated = false;
                for (int x = -1; x <= 1; x++)
                {
                    for (int y = -1; y <= 1; y++)
                    {
                        var scanPosition = new Position(tile.Position.X + x, tile.Position.Y + y);
                        if (!this.Tiles.ContainsKey(scanPosition)) this.InitializeTile(scanPosition);
                        var scanTile = this.Tiles[scanPosition];
                        scanTile.GenerationWeight -= 1;
                        if (scanTile.GenerationWeight == 0) this.GeneratableTiles.Remove(scanTile);
                        if (scanTile.GenerationWeight < 3 && scanTile.IsGenerated) this.DegenerateTile(scanTile.Position);
                    }
                }
            }
        }


        public void GenerateStep(int size)
        {
            var rng = new Random();
            HashSet<Position> open = new HashSet<Position>();
            HashSet<Position> closed = new HashSet<Position>();
            Dictionary<Position, int> weight = new Dictionary<Position, int>();
            open.Add(new Position());

            Action<Position, int> weightChange = delegate(Position t, int amount)
            {
                if (!weight.ContainsKey(t)) weight.Add(t, 0);
                weight[t] += amount;
            };

            Action<Position> add = null;
            
            add = delegate(Position t)
            {
                if (!closed.Contains(t))
                {
                    open.Remove(t);
                    closed.Add(t);
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            var st = new Position(t.X + x, t.Y + y);
                            weightChange(st, 1);
                            if (!closed.Contains(st)) open.Add(st);
                            if (weight[st] > 4) add(st);
                        }
                    }
                }
            };

            Action<Position> remove = null;
            remove = delegate(Position t)
            {
                if (closed.Contains(t))
                {
                    open.Add(t);
                    closed.Remove(t);
                    for (int x = -1; x <= 1; x++)
                    {
                        for (int y = -1; y <= 1; y++)
                        {
                            var st = new Position(t.X + x, t.Y + y);
                            weightChange(st, -1);
                            if (weight[st] == 0) open.Remove(st);
                            if (weight[st] < 3 && closed.Contains(st)) remove(st);
                        }
                    }
                }
            };

            while (closed.Count < size)
            {
                var i = rng.Next(open.Count);
                var tile = open.ToList()[i];
                add(tile);
            }
        }
    }
}
