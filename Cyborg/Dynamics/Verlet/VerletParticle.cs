using Cyborg.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet
{
    public class VerletParticle
    {
        private int _index;
        private Vec3 _pos;
        private Vec3 _oldPos;
        private double _radius;

        #region PROPERTIES
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        public Vec3 Pos
        {
            get { return _pos; }
            set { _pos = value; }
        }
        public Vec3 OldPos
        {
            get { return _oldPos; }
            
        }


        public double Radius
        {
            get { return _radius; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException();
                else _radius = value;
            }
        }
        #endregion

        #region CONSTRUCTORS
        public VerletParticle(int i0) : this(i0, Vec3.Zero)
        {
            Index = i0;
        }

        public VerletParticle(int i0, Vec3 pos) : this(i0, pos, 1.0)
        {
            Index = i0;
            Pos = pos;
        }

        public VerletParticle(int i0, Vec3 pos, double radius)
        {
            Index = i0;
            Pos = pos;
            Radius = radius;
            _oldPos = _pos;
        }

        #endregion


        public void Update(double timeStep)
        {
            //Position = Position + (Position - OldPosition) + Accceleration * Timestep^2

            //temp pos storage
            Vec3 temp = Pos;

            Vec3 vel = _pos - _oldPos;

            //this is where all the constraints are calculated
            //_pos += vel + _accel * timeStep * timeStep;
            _pos += vel  * timeStep * timeStep;

            //record new position. 
            _oldPos = temp;
        }






    }
}
