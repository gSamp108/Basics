using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class Token
    {
        public World World { get; private set; }
        public Position Position { get; private set; }
        public Tile Tile { get { return this.World[this.Position]; } }
        public decimal Health { get; private set; }
        public int Rank { get; private set; }
        public int Xp { get; private set; }
        public decimal Stamina { get; private set; }
        public decimal Ability { get; private set; }
        public Group Group { get; private set; }
        public decimal MaxHealth { get { return this.Stamina * 10m; } }
        public int Movement { get; private set; }
        public TokenTypes Type { get; private set; }

        public Token(World world, Position position, TokenTypes type, Group group)
        {
            this.World = world;
            this.Position = position;
            this.Group = group;
            this.Rank = 1;
            this.Stamina = 1;
            this.Ability = 1;
            this.Health = this.MaxHealth;
            this.Type = type;
        }
    }
}
