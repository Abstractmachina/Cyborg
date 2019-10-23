using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Forces
{
    public abstract class Force
    {
        private double strength;
        public double Strength
        {
            get { return strength; }
            set
            {
                if (value <= 0) strength = 1.0;
                else strength = value;
            }
        }

    }
}
