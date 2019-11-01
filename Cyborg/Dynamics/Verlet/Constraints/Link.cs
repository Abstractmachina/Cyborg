using Cyborg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet.Constraints
{
    public class Link : IVerletConstraint
    {

        private int _i0, _i1;
        private double _targetLength;
        private double _stiffness;


        public Link(int i0, int i1, double targetLength, double stiffness) {
            _i0 = i0;
            _i1 = i1;
            _targetLength = targetLength;
            _stiffness = stiffness;

        }


        public void Calculate(List<VerletParticle> particles)
        {
            var p0 = particles[_i0];
            var p1 = particles[_i1];

            var vel = p1.Pos - p0.Pos;
            var dist = vel.Length;
            if ( dist > _targetLength)
            {
                var diff = dist - _targetLength;
                p0.Delta += vel.Unit * diff / 2;
                p1.Delta += vel.Unit * diff / 2 * -1;
            }

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
                var itr = value.GetEnumerator();

                itr.MoveNext();
                _i0 = itr.Current;

                itr.MoveNext();
                _i1 = itr.Current;
            }
        }


    }
}
