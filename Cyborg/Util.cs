using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg
{
    public class Util
    {

        public static double Constrain(double val, double min, double max)
        {
            if (val > max) val = max;
            else if (val < min) val = min;
            return val;
        }

    }
}
