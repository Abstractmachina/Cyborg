using System;

namespace Cyborg.Dynamics.Euler
{
    public class SolverSettings
    {

        private double _timeStep;
        private double _linearDamping;
        private double _maxSpeed;

        #region PROPERTIES
        public double LinearDamping
        {
            get { return _linearDamping; }
            set
            {
                if (value < 0 || value > 1) throw new ArgumentOutOfRangeException("Value must be between 0 and 1");
                else _linearDamping = value;
            }
        }

        public double MaxSpeed
        {
            get { return _maxSpeed; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Value must be > 0");
                else _maxSpeed = value;
            }
        }

        public double TimeStep
        {
            get { return _timeStep; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("Value must be > 0");
                else _timeStep = value;
            }
        }

        #endregion


        #region CONSTRUCTORS
        public SolverSettings() { }
        public SolverSettings(double linearDamping) : this(linearDamping, 1)
        {
            LinearDamping = linearDamping;
        }
        public SolverSettings(double linearDamping, double maxSpeed) : this(linearDamping, maxSpeed, 1)
        {
            MaxSpeed = maxSpeed;
            LinearDamping = linearDamping;
        }
        public SolverSettings(double linearDamping, double maxSpeed, double timeStep)
        {
            TimeStep = timeStep;
            MaxSpeed = maxSpeed;
            LinearDamping = linearDamping;
        }
        #endregion
    }
}