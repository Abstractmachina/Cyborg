using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Euler.Forces
{
    public abstract class Force
    {
        private double _strength;
        public double Strength
        {
            get { return _strength; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Value must be > 0");
                else _strength = value;
            }
        }

    }
}
