using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Core
{
    public class Vector2d
    {
        public double X { get; set; }
        public double Y { get; set; }

        public Vector2d(double x, double y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Get vector length.
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        /// <summary>
        /// Get unit vector.
        /// </summary>
        public Vector2d UnitVector
        {
            get { return new Vector2d(X / this.Length, Y / this.Length); }
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
