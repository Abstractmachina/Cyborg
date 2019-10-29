using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet
{
    public class VerletSolver
    {
        public int counter;

        public VerletSolver()
        {
            counter = 0;
        }

        public void Step(List<VerletParticle> particles, List<IVerletConstraint> constraints)
        {
            ApplyConstraints(particles, constraints);
            UpdateParticles(particles);
            counter++;
        }


        public void ApplyConstraints(List<VerletParticle> particles, List<IVerletConstraint> constraints)
        {
            foreach (var c in constraints) c.Apply(particles);
        }

        private void UpdateParticles(List<VerletParticle> particles)
        {
            foreach (var p in particles) p.Update(1.0);
        }
    }
}
