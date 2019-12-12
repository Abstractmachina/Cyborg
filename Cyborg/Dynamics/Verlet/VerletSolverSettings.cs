using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet
{
    public class VerletSolverSettings
    {

        private double _damping;
        private double _timeStep;

        #region PROPERTIES
        public double Damping
        {
            get { return _damping; }
            set
            {
                if (value < 0 && value > 1) throw new ArgumentOutOfRangeException("Damping value must be between 0 and 1.");
                else _damping = value;
            }
        }

        public double TimeStep
        {
            get { return _timeStep; }
            set {
                if (value <= 0) throw new ArgumentOutOfRangeException();
                else _timeStep = value;
            }
        }

        #endregion

        public VerletSolverSettings() : this(1.0, 0.0)
        {
        }

        public VerletSolverSettings(double timeStep) : this(timeStep, 0.0)
        {
        }

        public VerletSolverSettings(double timeStep, double damping)
        {
            TimeStep = timeStep;
            Damping = damping;
        }

    }
}
