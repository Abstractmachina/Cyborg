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
        private Vec3 delta;
        private int i0;

        public Linear() { }
        public Linear(int index, Vec3 delta, double strength = 1.0)
        {
            i0 = index;
            this.delta = delta;
            Strength = strength;
        }
        
        
        public void Calculate(List<Particle> particles)
        {

            //particles[i0].AddDelta(delta);

        }

        public void Apply(List<Particle> particles)
        {
            particles[i0].AddDelta(delta);

        }

        public IEnumerable<int> Indices {
            get
            {
                yield return i0;
            }
            set
            {
                var itr = value.GetEnumerator();

                itr.MoveNext();
                i0 = itr.Current;
            }
        }
    }
}
