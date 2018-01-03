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
        public int TargetGenerationSize = 250;
        public int AutoGenerateThreshold = 4;
        public int AutoDegenerateThreshold = 3;

        public Tile TileSelectedForGenerationLastStep;
        public HashSet<Tile> TilesInitializedLastStep = new HashSet<Tile>();
        public HashSet<Tile> TilesUngeneratedLastStep = new HashSet<Tile>();
        public HashSet<Tile> TilesGenerateableLastStep = new HashSet<Tile>();
        public HashSet<Tile> TilesGeneratedLastStep = new HashSet<Tile>();

        public World()
        {
            this.GenerateTile(new Position());
        }

        public void FullGeneration()
        {
            while (this.GeneratedTiles.Count < this.TargetGenerationSize) { this.GenerationStep(); }
        }
        public void GenerationStep()
        {
            this.TileSelectedForGenerationLastStep = null;
            this.TilesGenerateableLastStep.Clear();
            this.TilesGeneratedLastStep.Clear();
            this.TilesInitializedLastStep.Clear();
            this.TilesUngeneratedLastStep.Clear();

            if (this.GeneratedTiles.Count < this.TargetGenerationSize)
            {
                if (this.GeneratableTiles.Count > 0)
                {
                    var tile = this.GeneratableTiles.ToList()[this.Rng.Next(this.GeneratableTiles.Count)];
                    this.TileSelectedForGenerationLastStep = tile;
                    this.GenerateTile(tile.Position);
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
                this.TilesInitializedLastStep.Add(tile);
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
                this.TilesGeneratedLastStep.Add(tile);
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
                                this.TilesGenerateableLastStep.Add(scanTile);
                            }
                        }
                        if (scanTile.GenerationWeight > this.AutoGenerateThreshold && !scanTile.IsGenerated) this.GenerateTile(scanTile.Position);
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
                this.TilesUngeneratedLastStep.Add(tile);
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
                        if (scanTile.GenerationWeight < this.AutoDegenerateThreshold && scanTile.IsGenerated) this.DegenerateTile(scanTile.Position);
                    }
                }
            }
        }
    }
}
