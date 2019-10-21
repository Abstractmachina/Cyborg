﻿using Cyborg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyborg.Dynamics.Interfaces;

namespace Cyborg.Dynamics
{
    public abstract class Solver
    {

        private int counter;
        private SolverSettings settings;


        public int Counter { get; set; }
        public SolverSettings Settings { get; set; }


        #region CONSTRUCTORS

        public Solver() { }

        public Solver(SolverSettings settings)
        {
            this.settings = settings;
            counter = 0;
        }

        //public Solver(List<Vec3> initPos, double size)
        //{
        //    pts = new List<Particle>();
        //    //pts = initPos.Select(p => new Particle(p)).ToList();
        //    foreach (var v in initPos)
        //    {
        //        var p = new Particle(v, size / 2 * 0.9);
        //        pts.Add(p);
        //    }


        //    //get dimensions for grid
        //    this.world = world;
        //    double xMax = world.Max.X;
        //    double yMax = world.Max.Y;
        //    double cellsize = size * 2;

        //    int xNum = (int)xMax / (int)cellsize;
        //    int yNum = (int)yMax / (int)cellsize;
        //    grid = new Grid(xNum, yNum, cellsize);


        //    counter = 0;

        //}
        #endregion

        public void Step()
        {
            UpdateParticles();
            counter++;
        }

        private void ApplyConstraints(List<Particle> particles, List<IConstraint> constraints)
        {
            foreach 
        }

        private void UpdateParticles(List<Particle> particles)
        {

            foreach (var p in particles)
            {
                p.Update();
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
