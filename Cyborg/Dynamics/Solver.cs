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


        //public void Step(double growthRate)
        //{

        //    counter++;

        //    if (counter > 100)
        //    {
        //        Resample();
        //        counter = 0;
        //    }
        //    Grow(growthRate);
        //    Collision();
        //    foreach (var p in pts) p.Move();
        ////}


        //private void Resample()
        //{
        //    var c = Curve.CreateInterpolatedCurve(pts.Select(p => (Point3d)p.Loc), 3);
        //    double[] divParam = c.DivideByLength(den, true);
        //    var divPts = divParam.Select(t => (Vector3d)c.PointAt(t)).Select(p => new Particle(p, den / 2 * 0.9)).ToList();

        //    pts = divPts;

        //}

        //private void Collision()
        //{
        //    foreach (var p0 in pts)
        //    {

        //        var accelVec = new Vector3d();
        //        int numVecs = 0;

        //        foreach (var p1 in pts)
        //        {
        //            if (p0 != p1)
        //            {

        //                var vec = p0.Loc - p1.Loc;
        //                double dist = vec.Length;
        //                if (dist < (p0.Size + p1.Size))
        //                {
        //                    accelVec += vec / dist;
        //                    numVecs++;
        //                }
        //            }
        //        }

        //        if (numVecs != 0)
        //        {
        //            accelVec /= numVecs;
        //            p0.Accel += accelVec;
        //        }
        //    }

        //}


    }
}
