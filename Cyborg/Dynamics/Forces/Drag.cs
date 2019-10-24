
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyborg.Core;
using Cyborg.Dynamics.Interfaces;

namespace Cyborg.Dynamics.Forces
{
    public class Drag : Force, IConstraint
    {

        private int _i0;
        private Vec3 _delta;

        public Drag() { }



        public void Calculate(List<Particle> particles)
        {
            //F = -1 * 1r(density of liquid) * vel^2 * 1A(frontal Area) * c * vel.Unit
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
