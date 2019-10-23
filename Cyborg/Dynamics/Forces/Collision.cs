using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cyborg.Dynamics.Interfaces;
using Cyborg.Core;

namespace Cyborg.Dynamics.Forces
{
    public class Collision : Force, IConstraint
    {
        private int _i0;
        private Vec3 _delta;


        public Collision() { }
        public Collision(int i)
        {
            _i0 = i;
        }

        public void Calculate(List<Particle> particles)
        {
            var current = particles[_i0];

            foreach (var p in particles)
            {
                if (p != current)
                {
                    var v = current.Pos - p.Pos;
                    var dist = v.Length;
                    if (dist <= current.Size + p.Size)
                    {
                        Vec3 normal = v;
                        var dot = normal * current.Vel;
                        var nProj = (dot / normal.Length) * normal.Unit;
                        var d = 2 * (nProj - p.Vel);
                        var vm = (p.Vel + d) * -1;
                        _delta = vm * Strength;
                    }
                }
            }

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

