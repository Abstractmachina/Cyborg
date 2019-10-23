using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Core
{
    public class Plane
    {
        private Vec3 _origin;
        private Vec3 _x, _y, _z;


        public Vec3 Origin { get { return _origin; } set { _origin = value; } }
        public Vec3 X { get { return _x; } set { _x = value; } }
        public Vec3 Y { get { return _y; } set { _y = value; } }
        public Vec3 Z { get { return _z; } set { _z = value; } }

        public static readonly Plane World = new Plane();

        public Plane()
        {
            _origin = Vec3.Zero;
            _x = Vec3.XAxis;
            _y = Vec3.YAxis;
            _z = Vec3.ZAxis;
        }

        public Plane(Vec3 origin, Vec3 x, Vec3 y, Vec3 z)
        {
            Origin = origin;
            X = x;
            Y = y;
            Z = z;
        }
    }
}
