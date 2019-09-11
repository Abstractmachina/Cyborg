using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Constraints
{
    public abstract class Constraint
    {

        private double weight;
        public double Weight
        {
            get { return weight; }
            set
            {
                if (value < 0) weight = 1;
                else weight = value;
            }
        }

    }
}
