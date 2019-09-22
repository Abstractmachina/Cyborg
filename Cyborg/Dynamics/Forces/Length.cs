using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyborg.Core;
using Cyborg.Dynamics.Interfaces;

namespace Cyborg.Dynamics.Forces
{
    public class Spring : Force, IConstraint
    {
        private Vec3 delta;
        private double targetLength;
        private int i0, i1;
        


        public Spring(int i0, int i1, double targetLength, double springConstant)
        {

            this.i0 = i0;
            this.i1 = i1;
            this.targetLength = targetLength;
            this.Strength = springConstant;

        }



        public void Calculate(List<Particle> particles)
        {

            var p0 = particles[i0];
            var p1 = particles[i1];

            Vec3 vec = p1.Pos - p0.Pos;
            double deltaLength = vec.Length;

            //f = -k * deltaX




        }
        public void Apply(List<Particle> particles)
        {

        }


        public IEnumerable<int> Indices {
            get
            {
                yield return i0;
                yield return i1;
            }
            set
            {
                var it = value.GetEnumerator();
                it.MoveNext();
                i0 = it.Current;

                it.MoveNext();
                i1 = it.Current;
            }
            }

    }
}
