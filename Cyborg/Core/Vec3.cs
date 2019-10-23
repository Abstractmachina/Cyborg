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


        #region PROPERTIES

        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public static readonly Vec3 Zero = new Vec3();
        public static readonly Vec3 XAxis = new Vec3(1, 0, 0);
        public static readonly Vec3 YAxis = new Vec3(0, 1, 0);
        public static readonly Vec3 ZAxis = new Vec3(0, 0, 1);



        public double Length
        {
            get { return Math.Sqrt(X * X + Y * Y + Z * Z); }
        }

        public Vec3 Unit
        {
            get { return new Vec3(X / this.Length, Y / this.Length, Z / this.Length); }
        }
        #endregion


        #region OPERATORS

        public static Vec3 operator +(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vec3 operator -(Vec3 a, Vec3 b)
        {
            return new Vec3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        /// <summary>
        /// Scale Vector
        /// </summary>
        public static Vec3 operator *(Vec3 a, double scalar)
        {
            return new Vec3(a.X * scalar, a.Y * scalar, a.Z * scalar);
        }
        public static Vec3 operator *(double scalar, Vec3 a)
        {
            return new Vec3(a.X * scalar, a.Y * scalar, a.Z * scalar);
        }

        /// <summary>
        /// Dot Product
        /// </summary>
        public static double operator *(Vec3 a, Vec3 b)
        {
            return a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        }
        #endregion


        #region CONVERSIONS

        override public string ToString()
        {
            return String.Format("{{{0},{1},{2}}}", this.X, this.Y, this.Z);
        }

        public static explicit operator string(Vec3 v)
        {
            return String.Format("{{{0},{1},{2}}}", v.X, v.Y, v.Z);
        }

        public static explicit operator Vec3(Vec2 vec2)
        {
            return new Vec3(vec2.X, vec2.Y, 0);
        }

        #endregion


        #region STATIC METHODS

        public static Vec3 Cross(Vec3 a, Vec3 b)
        {
            return new Vec3(a.Y * b.Z - a.Z * b.Y, a.Z * b.X - a.X * b.Z, a.X * b.Y - a.Y * b.X);
        }

        public static double Angle(Vec3 a, Vec3 b)
        {
            return Math.Acos(    (a*b) / (a.Length * b.Length)   );
        }

        /// <summary>
        /// Projection vector of b on a.
        /// </summary>
        public static Vec3 Projection(Vec3 a, Vec3 b)
        {
            return ((a * b) / a.Length) * a.Unit;
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
                Z *= l;
                return true;
            }
            return false;

        }

        /// <summary>
        /// Calculate cross product with other Vec3
        /// </summary>
        public Vec3 Cross(Vec3 other)
        {
            return new Vec3(this.Y * other.Z - this.Z * other.Y, this.Z * other.X - this.X * other.Z, this.X * other.Y - this.Y * other.X);
        }


        /// <summary>
        /// Get the scalar distance to another Vector.
        /// </summary>
        public double DistanceTo(Vec3 other)
        {
            return (other - this).Length;
        }

        /// <summary>
        /// Get projection vector of b on a.
        /// </summary>
        public Vec3 Projection(Vec3 other)
        {
            return ((this * other) / this.Length) * this.Unit;

        }

        
    }
}
