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
        private Vec3 _accel;

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
        public Vec3 Accel
        {
            get { return _accel; }
            set { _accel = value; }
        }
        #endregion

        public VerletParticle(int i0) : this(i0, Vec3.Zero)
        {
            Index = i0;
        }

        public VerletParticle(int i0, Vec3 pos)
        {
            Index = i0;
            Pos = pos;
            _oldPos = _pos;
            Accel = Vec3.Zero;
        }

        public void AddAccel(Vec3 accel)
        {
            Accel = accel;
        }

        public void Update(double timeStep)
        {
            //Position = Position + (Position - OldPosition) + Accceleration * Timestep^2
            //OldPosition = Position
            Vec3 temp = Pos;
            Pos += Pos - OldPos + Accel * timeStep * timeStep;
            _oldPos = temp;

            Accel = Vec3.Zero;
        }






    }
}
