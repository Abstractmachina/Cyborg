using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyborg.Dynamics.Interfaces;

namespace Cyborg.Dynamics.Constraints
{
    public class Length : Constraint, IConstraint
    {
        private double targetLength;
        private int i0, i1;


        public Length(int i0, int i1, double targetLength, double weight)
        {

            this.i0 = i0;
            this.i1 = i1;
            this.targetLength = targetLength;
            this.Weight = weight;

        }



        public void Calculate(List<Particle> particles)
        {



        }
        public void Apply(List<Particle> particles)
        {

        }


        public IEnumerable<int> Indices { get; set; }

    }
}
