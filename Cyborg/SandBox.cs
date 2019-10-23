using Cyborg.Core;
using Cyborg.Dynamics;
using Cyborg.Dynamics.Forces;
using Cyborg.Dynamics.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Cyborg
{
    public class SandBox
    {



        public void Main()
        {
            var pts = new List<Particle>();
            var cs = new List<IConstraint>();

            for (int i = 0; i < 5; i++)
            {
                cs.Add(new Collision2(i));
            }

            var test = cs[0] as Collision2;
            Debug.WriteLine(test.debug);




        }


        class Collision2 : Force, IConstraint
        {
            private int _i0;
            private Vec3 _delta;


            public string debug;

            public Collision2() { }
            public Collision2(int i)
            {
                _i0 = i;
                _delta = new Vec3();
                debug = "inert";
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
                            debug = "hit!";
                            Vec3 normal = v;
                            var dot = normal * current.Vel;
                            var nProj = (dot / normal.Length) * normal.Unit;
                            var d = 2 * (nProj - p.Vel);
                            var vm = (p.Vel + d) * -1;
                            _delta = vm * Strength;
                        }
                        else debug = "not hit!";
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
}
