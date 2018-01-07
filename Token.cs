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
        public Group Group { get; private set; }
        public TokenTypes Type { get; private set; }

        public decimal Constitution { get; private set; }
        public decimal Stamina { get; private set; }
        public decimal Ability { get; private set; }
        public decimal Reach { get; private set; }
        public decimal Vision { get; private set; }

        public decimal MaxHealth { get { return this.Constitution * 10m; } }
        public decimal MaxMovement { get { return this.Stamina; } }

        public decimal Rank
        {
            get { return this.rank; }
            set
            {
                this.rank = value;
                var rankIncreaseRewards = new List<Action>();
                rankIncreaseRewards.Add(delegate { this.Stamina += 1; });
                rankIncreaseRewards.Add(delegate { this.Constitution += 1; });
                rankIncreaseRewards.Add(delegate { this.Ability += 1; });
                rankIncreaseRewards.Add(delegate { this.Reach += 1; });
                rankIncreaseRewards.Add(delegate { this.Vision += 1; });
                var reward = rankIncreaseRewards[this.World.Rng.Next(rankIncreaseRewards.Count)];
                for (int i = 0; i < this.rank; i++) { reward.Invoke(); }
            }
        }
        public decimal Xp
        {
            get { return this.xp; }
            set
            {
                this.xp = value;
                while (this.xp >= (this.XpForNextRank))
                {
                    this.xp -= (this.XpForNextRank);
                    this.Rank += 1;
                }
            }
        }
        public decimal Health
        {
            get { return this.health; }
            set
            {
                this.health = value;
                if (this.health < 0) this.health = 0;
                if (this.health > this.MaxHealth) this.health = this.MaxHealth;
            }
        }
        public decimal Movement
        {
            get { return this.movement; }
            set
            {
                this.movement = value;
                if (this.movement < 0) this.movement = 0;
                if (this.movement > this.MaxMovement) this.movement = this.MaxMovement;
            }
        }
        public decimal XpForNextRank { get { return this.rank * 10m; } }

        private decimal movement;
        private decimal health;
        private decimal rank;
        private decimal xp;


        public Token(World world, Position position, TokenTypes type, Group group)
        {
            this.World = world;
            this.Position = position;
            this.Group = group;
            this.Stamina = 1;
            this.Ability = 1;
            this.Reach = 2;
            this.Vision = 2;
            this.Health = this.MaxHealth;
            this.Type = type;
        }

        public void StartPhaseTick()
        {
            if (this.Type == TokenTypes.Unit)
            {
                if (this.Rank > 0) this.Movement = this.MaxMovement;
            }
        }
    }
}
