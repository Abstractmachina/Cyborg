using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Core
{
    public class Vec3
    {
        public Vec3()
        {
            X = 0;
            Y = 0;
            Z = 0;
        }
        public Vec3(double x, double y, double z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        /*
         * 
         * PROPERTIES
         * 
         * */

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public static readonly Vec3 Zero = new Vec3();


        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public Vec3 UnitVector
        {
            get { return new Vec3(X / this.Length, Y / this.Length, Z / this.Length); }
        }


        /*
         * 
         * OPERATORS
         * 
         * */
        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vec3 operator *(Vec3 a, double scalar)
        {
            return new Vec3(a.X * scalar, a.Y * scalar, a.Z * scalar);
        }

        public static double operator *(Vec3 a, Vec3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }


        /*
         * 
         * CONVERSIONS
         * 
         * */

        public static explicit operator Vec3(Vec2 vec2)
        {
            return new Vec3(vec2.X, vec2.Y, 0);
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
                Z *= l;
                return true;
            }
            return false;

        }

        /// <summary>
        /// Calculate cross product with other Vec3
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public Vec3 Cross(Vec3 other)
        {
            return new Vec3(this.Y * other.Z + this.Z * other.Y, this.Z * other.X + this.X * other.Z, this.X * other.Y + this.Y * other.Y);
        }


        /// <summary>
        /// Get the scalar distance to another Vector.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public double DistanceTo(Vec3 other)
        {
            return (other - this).Length;
        }
    }
}
