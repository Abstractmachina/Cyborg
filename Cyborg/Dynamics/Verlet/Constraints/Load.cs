using Cyborg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet.Constraints
{
    public class Load : IVerletConstraint
    {

        private int _i0;
        private Vec3 _delta;


        public Vec3 Delta
        {
            get { return _delta; }
            set { _delta = value; }
        }

        public Load(int i0, Vec3 delta)
        {
            _i0 = i0;
            Delta = delta;
        }

        public void Calculate(List<VerletParticle> particles)
        {
            particles[_i0].Pos += _delta;
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
