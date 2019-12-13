using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Core
{
    public class Vec2i
    {
        public Vec2i() : this(0, 0) { }
        public Vec2i(int x, int y)
        {
            X = x;
            Y = y;
        }

        #region OPERATORS

        public static Vec2i operator +(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.X + b.X, a.Y + b.Y);
        }

        public static Vec2i operator -(Vec2i a, Vec2i b)
        {
            return new Vec2i(a.X - b.X, a.Y - b.Y);
        }
        #endregion

        #region PROPERTIES
        public int X { get; set; }

        public int Y { get; set; }

        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y); }
        }

        public Vec2 Unit
        {
            get { return new Vec2((double) X / this.Length, (double) Y / this.Length); }
        }
        #endregion


        /// <summary>
        /// Get the scalar distance to another Vector.
        /// </summary>
        public double DistanceTo(Vec2i other)
        {
            return (other - this).Length;
        }
    }
}
