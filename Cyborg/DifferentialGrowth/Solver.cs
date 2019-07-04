using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Rhino.Geometry;
using Cyborg.Core;
using Cyborg.Simulation;

namespace Cyborg.DifferentialGrowth

{
    class Solver
    {

        /*
        private int counter;
        private Grid grid;

        public List<Particle> pts;

        #region CONSTRUCTORS
        public Solver() { }
        public Solver(List<Vec3> initPos, double size)
        {
            pts = new List<Particle>();
            //pts = initPos.Select(p => new Particle(p)).ToList();
            foreach (var v in initPos)
            {
                var p = new Particle(v, size);
                pts.Add(p);
            }

            counter = 0;

        }
        #endregion

        public void Step(double growthRate)
        {
            counter++;

            if (counter > 100)
            {
                Resample();
                counter = 0;
            }
            Grow(growthRate);
            Collision();
            foreach (var p in pts) p.Move();
        }

        private void Resample()
        {
            var c = Curve.CreateInterpolatedCurve(pts.Select(p => (Point3d)p.Loc), 3);
            double[] divParam = c.DivideByLength(den, true);
            var divPts = divParam.Select(t => (Vector3d)c.PointAt(t)).Select(p => new Particle(p, den / 2 * 0.9)).ToList();

            pts = divPts;

        }

        private void Collision()
        {
            foreach (var p0 in pts)
            {

                var accelVec = new Vec3();
                int numVecs = 0;

                foreach (var p1 in pts)
                {
                    if (p0 != p1)
                    {

                        var vec = p0.Loc - p1.Loc;
                        double dist = vec.Length;
                        if (dist < (p0.Size + p1.Size))
                        {
                            accelVec += vec / dist;
                            numVecs++;
                        }
                    }
                }

                if (numVecs != 0)
                {
                    accelVec /= numVecs;
                    p0.Accel += accelVec;
                }
            }

        }


        public void Grow(double growthRate)
        {
            foreach (var p in pts)
            {
                p.Size += growthRate;
            }
        }
    }

    */
    }
}
