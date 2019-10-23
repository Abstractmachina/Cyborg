using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyborg.Core;
using Cyborg.Dynamics.Interfaces;

namespace Cyborg.Dynamics.Forces
{
    public class Linear: Force, IConstraint
    {
        private Vec3 _delta;
        private int _i0;

        public Linear() { }
        public Linear(int index, Vec3 delta, double strength = 1.0)
        {
            _i0 = index;
            this._delta = delta;
            Strength = strength;
        }
        
        
        public void Calculate(List<Particle> particles)
        {
            _delta = _delta * Strength;
        }

        public void Apply(List<Particle> particles)
        {
            particles[_i0].AddDelta(_delta);

        }

        public IEnumerable<int> Indices {
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
