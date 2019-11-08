using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet
{
    public class VerletSolver
    {
        private int _counter;
        private VerletSolverSettings _settings;

        public VerletSolver()
        {
            _counter = 0;
            _settings = new VerletSolverSettings();
        }

        public VerletSolver(VerletSolverSettings settings)
        {
            _counter = 0;
            _settings = settings;
        }


        public void Step(List<VerletParticle> particles, List<IVerletConstraint> constraints)
        {
            ApplyConstraints(particles, constraints);
            UpdateParticles(particles);
            _counter++;
        }


        public void ApplyConstraints(List<VerletParticle> particles, List<IVerletConstraint> constraints)
        {
            foreach (var c in constraints) c.Calculate(particles);
        }

        private void UpdateParticles(IEnumerable<VerletParticle> particles)
        {
            foreach (var p in particles) p.Update(1.0);
        }
    }
}
