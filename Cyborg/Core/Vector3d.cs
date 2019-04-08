using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Core
{
    class Vector3d
    {

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }


        public Vector3d() {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Vector3d(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static Vector3d operator +(Vector3d a, Vector3d b)
        {
            Vector3d vec = new Vector3d();
            vec.X = a.X + b.X;
            vec.Y = a.Y + b.Y;
            vec.Z = a.Z + b.Z;
            return vec;
        }

        public static Vector3d operator -(Vector3d a, Vector3d b)
        {
            Vector3d vec = new Vector3d();
            vec.X = a.X - b.X;
            vec.Y = a.Y - b.Y;
            vec.Z = a.Z - b.Z;
            return vec;
        }

        /// <summary>
        /// Get vector length.
        /// </summary>
        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        /// <summary>
        /// Get unit vector.
        /// </summary>
        public Vector3d UnitVector
        {
            get { return new Vector3d(X / this.Length, Y / this.Length, Z / this.Length); }
        }


        /*
         * METHODS
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
                Z *= l;
                return true;
            }
            return false;

        }
    }
}
