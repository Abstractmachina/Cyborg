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

        public static double Map(double val, double min1, double max1, double min2, double max2)
        {
            return min2 + (val - min1) * (max2 - min2) / (max1 - min1);
        }

    }
}
