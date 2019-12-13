using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Core
{
    public class Vec2
    {
        public Vec2():this(0, 0) { }
        public Vec2(double x, double y)
        {
            X = x;
            Y = y;
        }

        #region OPERATORS

        public static Vec2 operator +(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2 operator -(Vec2 a, Vec2 b)
        {
            return new Vec2(a.X - b.X, a.Y - b.Y);
        }

        /// <summary>
        /// Scale Vector
        /// </summary>
        public static Vec2 operator *(Vec2 a, double scalar)
        {
            return new Vec2(a.X * scalar, a.Y * scalar);
        }
        public static Vec2 operator *(double scalar, Vec2 a)
        {
            return new Vec2(a.X * scalar, a.Y * scalar);
        }

        /// <summary>
        /// Dot Product
        /// </summary>
        public static double operator *(Vec2 a, Vec2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        public static Vec2 operator /(Vec2 v, double scalar)
        {
            scalar = 1.0 / scalar;
            v.X *= scalar;
            v.Y *= scalar;
            return v;
        }
        public static Vec2 operator /(double scalar, Vec2 v)
        {
            v.X = scalar / v.X;
            v.Y = scalar / v.Y;
            return v;
        }
        #endregion

        #region PROPERTIES
        public double X { get; set; }

        public double Y { get; set; }

        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public Vec2 Unit
        {
            get { return new Vec2(X / this.Length, Y / this.Length); }
        }
        #endregion

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


        /// <summary>
        /// Get the scalar distance to another Vector.
        /// </summary>
        public double DistanceTo(Vec2 other)
        {
            return (other - this).Length;
        }

        /// <summary>
        /// Get projection vector of b on a.
        /// </summary>
        public Vec2 Projection(Vec2 other)
        {
            return ((this * other) / this.Length) * this.Unit;

        }

    }
}
