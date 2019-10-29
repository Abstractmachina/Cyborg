using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet.Constraints
{
    public class Link
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
            var p0 = particles[_i0].Pos;
            var p1 = particles[_i1].Pos;

            var v = p1 - p0;
            var dist = v.Length;
            if ( dist > _targetLength)
            {
                var diff = dist - _targetLength;
                throw new NotImplementedException();
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
