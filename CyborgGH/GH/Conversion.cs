using Cyborg.Core;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.GH
{
    public static class Conversion
    {
        #region To Cyborg
        public static Vec3 Vec3(Vector3d v) { return new Vec3(v.X, v.Y, v.Z); }
        public static Vec2 Vec2(Vector2d v) { return new Vec2(v.X, v.Y); }
        #endregion

        #region To Rhino
        public static Vector3d Vector3d(Vec3 v) { return new Vector3d(v.X, v.Y, v.Z); }
        public static Vector2d Vector2d(Vec2 v) { return new Vector2d(v.X, v.Y); }
        #endregion
    }
}
