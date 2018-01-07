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
        public Token UnitLayer { get; private set; }
        public Token StructureLayer { get; private set; }

        public Tile(World world, Position position)
        {
            this.World = world;
            this.Position = position;
        }

        public void AddToken(Token token)
        {
            if (token.Type == TokenTypes.Unit)
            {
                if (this.UnitLayer == null) this.UnitLayer = token;
                else throw new Exception("Adding unit to tile with existing unit");
            }
            else
            {
                if (this.StructureLayer == null) this.StructureLayer = token;
                else throw new Exception("Adding structure to tile with existing structure");
            }
        }

        public void SpawnMineralNode()
        {
            this.MineralNode = true;
        }
    }
}
