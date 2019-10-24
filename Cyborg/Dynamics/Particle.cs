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
        private double _radius;
        private bool _isLocked = false;
        private int _index;

        public Vec3 Pos { get { return _pos; } set { _pos = value; } }
        public Vec3 Vel { get { return _vel; } set { _vel = value; } }
        public Vec3 Accel { get { return _accel; } set { _accel = value; } }
        public int Index { get { return _index; } }

        public double Mass
        {
            get { return _mass; }
            set
            {
                if (value <= 0) _mass = 1;
                else _mass = value;
            }
        }

        public double Radius
        {
            get { return _radius; }
            set { if (value <= 0) _radius = 1; else _radius = value; }
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

        public Particle(Vec3 pos, double size) : this(pos, Vec3.Zero, Vec3.Zero, size)
        {
            Pos = pos;
            Radius = size;
        }
        public Particle(Vec3 pos, Vec3 vel, Vec3 accel): this(pos, vel, accel, 1.0d)
        {
            Pos = pos;
            Vel = vel;
            Accel = accel;
            _mass = 1;
            _radius = 1;
        }
        public Particle(Vec3 pos, Vec3 vel, Vec3 accel, double size)
        {
            Pos = pos;
            Vel = vel;
            Accel = accel;
            _mass = 1;
            _radius = size;
        }


        #endregion

        public void AddDelta(Vec3 delta)
        {
            _accel += delta;
        }

        public void Update(double speedLimit, double damping)
        {
            
            _vel += _accel;
            Limit(speedLimit);
            _vel *= (1.0 - damping);
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
