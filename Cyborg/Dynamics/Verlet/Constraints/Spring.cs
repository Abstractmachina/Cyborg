using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet.Constraints
{
    public class Spring : IVerletConstraint
    {
        private int _i0, _i1;
        private double _targetLength, _stiffness;

        public double TargetLength
        {
            get { return _targetLength; }
            set { _targetLength = value; }
        }
        public double Stiffness
        {
            get { return _stiffness; }
            set { _stiffness = value; }
        }

        public Spring(int i0, int i1, double targetLength, double stiffness)
        {
            _i0 = i0;
            _i1 = i1;
            TargetLength = targetLength;
            Stiffness = stiffness;

        }

        public void Calculate(List<VerletParticle> particles)
        {
            var p0 = particles[_i0];
            var p1 = particles[_i1];

            var vec = p1.Pos - p0.Pos;
            var length = vec.Length;

            var diff = (length - _targetLength) / length * _stiffness;

            p0.Pos += vec * (diff / 2);
            p1.Pos -= vec * (diff / 2);

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

        public override string ToString()
        {
            return String.Format("{{ i0:{0} , i1:{1} , targetLength:{2} , stiffness:{3} }}", _i0, _i1, _targetLength, _stiffness);
        }
    }
}
