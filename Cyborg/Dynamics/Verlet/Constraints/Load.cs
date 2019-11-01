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
        private Vec3 _accel;


        public Vec3 Accel
        {
            get { return _accel; }
            set { _accel = value; }
        }

        public Load(int i0, Vec3 accel)
        {
            _i0 = i0;
            Accel = accel;
        }

        public void Calculate(List<VerletParticle> particles)
        {
            particles[_i0].AddAccel(_accel);
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
