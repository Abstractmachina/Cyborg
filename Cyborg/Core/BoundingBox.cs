using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Core
{
    public class BoundingBox
    {

        private Vec3 _min;
        private Vec3 _max;

        public Vec3 Min
        {
            get { return _min; }
            set { _min = value; }
        }
        public Vec3 Max
        {
            get { return _max; }
            set { _max = value; }
        }


        /// <summary>
        /// set bounding box for a collection of points.
        /// </summary>
        /// <param name="points"></param>
        public BoundingBox(IEnumerable<Vec3> points)
        {
            var x = points.Select(p => p.X).ToList();
            x.Sort();
            var y = points.Select(p => p.Y).ToList();
            y.Sort();
            var z = points.Select(p => p.Z).ToList();
            z.Sort();

            Min = new Vec3(x[0], y[0], z[0]);
            Max = new Vec3(x[x.Count - 1], y[y.Count - 1], z[z.Count - 1]);
        }

        /// <summary>
        /// Set bounding box by specifying min and max corner.
        /// </summary>
        /// <param name="min"></param>
        /// <param name="max"></param>
        public BoundingBox(Vec3 min, Vec3 max)
        {
            Min = min;
            Max = max;
        }


    }
}
