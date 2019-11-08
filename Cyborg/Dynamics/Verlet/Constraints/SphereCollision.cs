using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet.Constraints
{
    public class SphereCollision : IVerletConstraint
    {

        private int _i0;
        private double _stiffness;

        public double Stiffness
        {
            get { return _stiffness; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Value must be > 0");
                else _stiffness = value;
            }
        }

        #region CONSTRUCTORS
        public SphereCollision(int index) : this(index, 1.0)
        {
            _i0 = index;
        }

        public SphereCollision(int index, double stiffness)
        {
            _i0 = index;
            Stiffness = stiffness;
        }
        #endregion

        public void Calculate(List<VerletParticle> particles)
        {

            var p0 = particles[_i0];

            foreach (var p in particles)
            {
                if (p != p0)
                {
                    var vec = p.Pos - p0.Pos;
                    var length = vec.Length;
                    if (length < p.Radius + p0.Radius)
                    {
                        p0.Pos -= vec.Unit * 0.5 * _stiffness;
                    }
                }
            }
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
            return String.Format("SCollision {0} {{ stiffness: {1} }}", _i0, _stiffness);
        }

    }
}
