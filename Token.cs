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
    }
}
