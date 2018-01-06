using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Basics
{
    public class DecCurMax
    {
        private decimal current;
        private decimal maximum;

        public decimal Current
        {
            get { return this.current; }
            set
            {
                this.current = value;
                if (this.current < 0) this.current = 0;
                if (this.current > this.maximum) this.current = this.maximum;
            }
        }
        public decimal Maximum
        {
            get { return this.maximum; }
            set
            {
                this.maximum = value;
                if (this.maximum < 0) this.maximum = 0;
                if (this.current > this.maximum) this.current = this.maximum;
            }
        }
    }
}
