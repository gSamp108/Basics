using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public static class Functions
    {
        public static int FromRange(this Random rng, Range range)
        {
            return rng.Next((range.Max + 1) - range.Min) + range.Min;
        }    
    }
}
