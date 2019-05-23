using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cyborg.Core;

namespace Cyborg.Simulation
{
    class Particle
    {
        private Vec3 loc, vel, accel;

        public Vec3 Loc { get { return loc; } set { loc = value; } }
        public Vec3 Vel { get { return vel; } set { vel = value; } }
        public Vec3 Accel { get { return accel; } set { accel = value; } }


        private double size;
        public double Size {
            get { return size; }
            set
            {
                if (value <= 0) size = 1;
                else size = value;
            }
        }

        private double mass;
        public double Mass
        {
            get { return mass; }
            set
            {
                if (value <= 0) mass = 1;
                else mass = value;
            }
        }


        #region Constructors

        public Particle() : this(new Vec3(0, 0, 0))
        { }

        public Particle(Vec3 loc) : this(loc, new Vec3(0, 0, 0))
        {
            this.loc = loc;
        }

        public Particle(Vec3 loc, Vec3 vel)
        {
            this.loc = loc;
            this.vel = vel;
            this.accel = new Vec3(0, 0, 0);
        }

        public Particle(Vec3 loc, double size)
        {

            Loc = loc;
            Size = size;
            mass = 1;
            size = 1;

        }

        #endregion

        public void Move(double speedLimit = 0.01)
        {
            vel += accel;
            Limit(speedLimit);
            loc += vel;
            accel *= 0;
        }

        private void Limit(double maxSpeed)
        {
            if (vel.Length > maxSpeed)
            {
                vel.Unitize();
                vel *= maxSpeed;
            }
        }
    }
}
