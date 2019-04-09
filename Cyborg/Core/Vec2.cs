using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Core
{
    public class Vec2
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vec2(double x, double y)
        {
            X = x;
            Y = y;
        }

        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public Vec2 UnitVector
        {
            get { return new Vec2(X / this.Length, Y / this.Length); }
        }


        /*
         * 
         * METHODS
         * 
         * */

        /// <summary>
        ///Scale vector to its unit length.
        /// </summary>
        /// <returns>True on success</returns>
        public bool Unitize()
        {
            var l = this.Length;
            if (l > 0d)
            {
                l = 1 / this.Length;

                X *= l;
                Y *= l;
                return true;
            }
            return false;

        }
    }
}
