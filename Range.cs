using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class Range
    {
        public int Min { get;  set; }
        public int Max { get;  set; }
        public Range(int min, int max)
        {
            this.Min = min;
            this.Max = max;
        }
    }
}
