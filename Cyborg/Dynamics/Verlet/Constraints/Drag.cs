using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet.Constraints
{
    public class Drag : IVerletConstraint
    {
        private int _i0;
        private double _dragCoefficient;


        public double DragCoefficicent
        {
            get { return _dragCoefficient; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Value must be > 0");
                else _dragCoefficient = value;
            }
        }

        public Drag(int index) : this(index, 1.0)
        {
            _i0 = index;
        }

        public Drag(int index, double dragCoefficient)
        {
            _i0 = index;
            DragCoefficicent = dragCoefficient;
        }

        public void Calculate(List<VerletParticle> particles)
        {
            var p = particles[_i0];

            //F = -1 * 1r(density of liquid) * vel^2 * 1A(frontal Area) * c * vel.Unit
            
            var vel = p.Pos - p.OldPos;
            if (vel.Length != 0) p.Pos += vel.Unit * _dragCoefficient * vel.Length * vel.Length * -1;
            
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

        public override string ToString()
        {
            return String.Format("Drag {0} {{ drag coefficient: {1} }}", _i0, _dragCoefficient);
        }
    }
}
