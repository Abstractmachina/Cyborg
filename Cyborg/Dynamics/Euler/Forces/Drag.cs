
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cyborg.Core;
using Cyborg.Dynamics.Euler.Interfaces;

namespace Cyborg.Dynamics.Euler.Forces
{
    public class Drag : Force, IConstraint
    {

        private int _i0;
        private Vec3 _delta;


        public double DragCoefficient
        {
            get { return Strength; }
            set { Strength = value; }
        }


        public Drag() { }

        public Drag(int index, double dragCoefficient)
        {
            _i0 = index;
            Strength = dragCoefficient;
            _delta = Vec3.Zero;
        }



        public void Calculate(List<Particle> particles)
        {
            var p = particles[_i0];

            //F = -1 * 1r(density of liquid) * vel^2 * 1A(frontal Area) * c * vel.Unit

            if (p.Vel.Length != 0) _delta = -1 * (p.Vel.Length * p.Vel.Length) * Strength * p.Vel.Unit;
            else _delta = Vec3.Zero;

        }

        public void Apply(List<Particle> particles)
        {
            particles[_i0].AddDelta(_delta);
        }



        public IEnumerable<int> Indices
        {
            get
            {
                yield return _i0;
            }
            set
            {
                var itr = value.GetEnumerator();

                itr.MoveNext();
                _i0 = itr.Current;
            }
        }
    }
}
