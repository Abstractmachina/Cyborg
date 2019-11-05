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
                if (value < 0) throw new ArgumentOutOfRangeException();
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

        public VerletSolverSettings()
        {
            _damping = 0;
            _timeStep = 1;
        }

        public VerletSolverSettings(double timeStep, double damping)
        {
            TimeStep = timeStep;
            Damping = damping;
        }

    }
}
