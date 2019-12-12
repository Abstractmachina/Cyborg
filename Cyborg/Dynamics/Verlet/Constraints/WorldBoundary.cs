using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyborg.Core;

namespace Cyborg.Dynamics.Verlet.Constraints
{
    /// <summary>
    /// World boundary that is defined as a bounding box aligned to the world coordinate system. 
    /// </summary>
    public class WorldBoundary : IVerletConstraint
    {
        private int _i0;
        private BoundingBox _world;

        public WorldBoundary(int i0, BoundingBox world)
        {
            _i0 = i0;
            _world = world;
        }

        public void Calculate(List<VerletParticle> particles)
        {
            var p = particles[_i0];

            var vel = p.Pos - p.OldPos;
            if (p.Pos.X <= _world.Min.X || p.Pos.X >= _world.Max.X) p.Pos += new Vec3(vel.X * -2.0, 0, 0);
            if (p.Pos.Y <= _world.Min.Y || p.Pos.Y >= _world.Max.Y) p.Pos += new Vec3(0, vel.Y * -2.0, 0);
            if (p.Pos.X <= _world.Min.Z || p.Pos.Z >= _world.Max.Z) p.Pos += new Vec3(0, 0, vel.Z * -2.0);
        }

        public IEnumerable<int> Indices
        {
            get
            {
                yield return _i0;
            }
            set
            {
                var itr = value.GetEnumerator();

                itr.MoveNext();
                _i0 = itr.Current;
            }
        }
    }

}
