using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Euler.Constraints
{
    public abstract class Constraint
    {

        private double _weight;
        public double Weight
        {
            get { return _weight; }
            set
            {
                if (value < 0) _weight = 1;
                else _weight = value;
            }
        }

    }
}
