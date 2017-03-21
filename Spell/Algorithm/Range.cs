using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spell.Algorithm
{
    public class Range
    {
        public string Text { get; set; }
        public int Start { get; set; }
        public int End { get; set; }
        public Range()
        {
            Text = "";
            Start = 0;
            End = 0;
        }
    }
}
