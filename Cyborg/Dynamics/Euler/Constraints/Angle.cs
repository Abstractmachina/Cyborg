
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cyborg.Core;
using Cyborg.Dynamics.Euler.Interfaces;

namespace Cyborg.Dynamics.Euler.Constraints
{
    public class Angle : Constraint, IConstraint
    {
        private int _i0, _i1;
        private Vec3 _delta;

        public void Calculate(List<Particle> particles)
        {
            throw new NotImplementedException();
        }

        public void Apply(List<Particle> particles)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<int> Indices
        {
            get
            {
                yield return _i0;
                yield return _i1;
            }
            set
            {
                var it = value.GetEnumerator();
                it.MoveNext();
                _i0 = it.Current;

                it.MoveNext();
                _i1 = it.Current;
            }
        }
    }
}
