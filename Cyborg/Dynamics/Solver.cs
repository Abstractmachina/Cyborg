using Cyborg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyborg.Dynamics.Interfaces;

namespace Cyborg.Dynamics
{
    public class Solver
    {

        //TODO convergence check
        //TODO damping
        //TODO timestep

        private int _stepCounter;
        private SolverSettings _settings;
        


        public int StepCounter { get { return _stepCounter; } }
        public SolverSettings Settings { get { return _settings; } set { _settings = value; } }


        #region CONSTRUCTORS

        public Solver() { }

        public Solver(SolverSettings settings)
        {
            Settings = settings;
            _stepCounter = 0;
        }

        #endregion

        public void Step(List<Particle> particles, List<IConstraint> constraints)
        {
            ApplyConstraints(particles, constraints);
            UpdateParticles(particles);
            _stepCounter++;
        }

        private void ApplyConstraints(List<Particle> particles, List<IConstraint> constraints)
        {
            foreach( var c in constraints)
            {
                c.Calculate(particles);
            }

            foreach (var c in constraints)
            {
                c.Apply(particles);
            }
        }

        private void UpdateParticles(List<Particle> particles)
        {

            foreach (var p in particles)
            {
                p.Update(_settings.MaxSpeed, _settings.LinearDamping);
            }

        }

        #region STATIC METHODS

        /// <summary>
        /// Get unique indices of particle list.
        /// </summary>
        /// <param name="particles"></param>
        /// <returns></returns>
        public static SortedSet<int> GetAllUniqueIDs(List<Particle> particles)
        {
            return new SortedSet<int>(particles.Select(p => p.Index));
        }
        #endregion

    }
}
