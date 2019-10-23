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
        private Vec3 _pos, _vel, _accel;
        private double _mass;
        private double _size;
        private bool _isLocked = false;

        public Vec3 Pos { get { return _pos; } set { _pos = value; } }
        public Vec3 Vel { get { return _vel; } set { _vel = value; } }
        public Vec3 Accel { get { return _accel; } set { _accel = value; } }

        public double Mass
        {
            get { return _mass; }
            set
            {
                if (value <= 0) _mass = 1;
                else _mass = value;
            }
        }

        public double Size
        {
            get { return _size; }
            set { if (value <= 0) _size = 1; else _size = value; }
        }


        #region Constructors

        public Particle() : this(new Vec3(0, 0, 0))
        { }

        public Particle(Vec3 pos) : this(pos, new Vec3(0, 0, 0))
        {
            Pos = pos;
        }

        public Particle(Vec3 pos, Vec3 vel) : this(pos, vel, new Vec3(0, 0, 0))
        {
            Pos = pos;
            Vel = vel;
        }

        public Particle(Vec3 pos, Vec3 vel, Vec3 accel)
        {
            Pos = pos;
            Vel = vel;
            Accel = accel;
            _mass = 1;
            _size = 1;
        }


        #endregion

        public void AddDelta(Vec3 delta)
        {
            _accel += delta;
        }

        public void Update(double speedLimit, double damping)
        {
            var dampingForce = _vel * (1.0 - damping);

            _vel += _accel - dampingForce;
            Limit(speedLimit);
            _pos += _vel;
            Clear();
        }

        private void Clear()
        {
            _accel = Vec3.Zero;
        }

        private void Limit(double maxSpeed)
        {
            if (_vel.Length > maxSpeed)
            {
                _vel.Unitize();
                _vel *= maxSpeed;
            }
        }
    }
}
