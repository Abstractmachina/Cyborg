using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyborg.Core;
using Cyborg.Dynamics.Euler.Interfaces;

namespace Cyborg.Dynamics.Euler.Forces
{
    public class Spring : Force, IConstraint
    {
        private Vec3 _delta;
        private double _targetLength;
        private int _i0, _i1;

        public double TargetLength
        {
            get { return _targetLength; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Value must be >0");
                else _targetLength = value;
            }
        }

        public Spring(int i0, int i1, double targetLength, double springConstant)
        {

            _i0 = i0;
            _i1 = i1;
            _targetLength = targetLength;
            Strength = springConstant;

        }

        public void Calculate(List<Particle> particles)
        {

            var p0 = particles[_i0];
            var p1 = particles[_i1];

            Vec3 vec = p1.Pos - p0.Pos; //vector between particles
            double deltaLength = vec.Length - _targetLength;

            //f = -k * deltaX
            _delta = ((1 * Strength * deltaLength) / 2) * vec.Unit;

        }
        public void Apply(List<Particle> particles)
        {
            particles[_i0].AddDelta(_delta);
            particles[_i1].AddDelta(_delta * -1);
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
