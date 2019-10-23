using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cyborg.Core;

namespace Cyborg.Dynamics
{
    public class Particle
    {
        private Vec3 pos, vel, accel;
        private double mass;
        private double size;

        public Vec3 Pos { get { return pos; } set { pos = value; } }
        public Vec3 Vel { get { return vel; } set { vel = value; } }
        public Vec3 Accel { get { return accel; } set { accel = value; } }

        public double Mass
        {
            get { return mass; }
            set
            {
                if (value <= 0) mass = 1;
                else mass = value;
            }
        }

        public double Size
        {
            get { return size; }
            set { if (value <= 0) size = 1; else size = value; }
        }


        #region Constructors

        public Particle() : this(new Vec3(0, 0, 0))
        { }

        public Particle(Vec3 pos) : this(pos, new Vec3(0, 0, 0))
        {
            this.pos = pos;
        }

        public Particle(Vec3 pos, Vec3 vel)
        {
            this.pos = pos;
            this.vel = vel;
            this.accel = new Vec3(0, 0, 0);
            mass = 1;
            size = 1;
        }


        #endregion

        public void AddDelta(Vec3 delta)
        {
            accel += delta;
        }

        public void Update(double speedLimit = 0.01)
        {
            vel += accel;
            Limit(speedLimit);
            pos += vel;
            Clear();
        }

        private void Clear()
        {
            accel = Vec3.Zero;
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
