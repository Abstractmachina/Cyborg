using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.ReactionDiffusion
{
    /// <summary>
    /// 
    /// </summary>
    struct Cell
    {
        public double A { get; set; }
        public double B { get; set; }

        public Cell(double a, double b)
        {
            A = a;
            B = b;
        }

    }
}
