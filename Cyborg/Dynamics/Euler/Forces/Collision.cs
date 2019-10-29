using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cyborg.Dynamics.Euler;
using Cyborg.Dynamics.Euler.Interfaces;
using Cyborg.Core;

namespace Cyborg.Dynamics.Euler.Forces
{
    public class Collision : Force, IConstraint
    {
        private int _i0;
        private Vec3 _delta;
        private double _hardness;

        public double Hardness
        {
            get { return _hardness; }
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException("Value must be >0");
                else _hardness = value;
            }
        }

        public Collision() { }
        public Collision(int i) : this(i, 0.6d)
        {
            _i0 = i;
        }
        public Collision(int i, double strength) : this(i, strength, 1.0d)
        {
            _i0 = i;
            Strength = strength;
        }
        public Collision(int i, double strength, double hardness)
        {
            _i0 = i;
            _delta = new Vec3();
            Strength = strength;
            Hardness = hardness;
        }
        public void Calculate(List<Particle> particles)
        {

            var current = particles[_i0];
            int collisionCount = 0;
            Vec3 deltaSum = new Vec3();

            foreach (var p in particles)
            {
                if (p != current)
                {
                    var v = p.Pos - current.Pos;
                    var dist = v.Length;
                    if (dist <= current.Radius + p.Radius)
                    {
                        collisionCount++;
                        current.Pos = p.Pos + v  * -1; //make sure there are no negative values for dist
                        var hardnessFactor = 1 + ((current.Radius + p.Radius) - dist) * _hardness; 
                        //if current is moving
                        if (p.Vel.Length != 0) 
                        {
                            Vec3 normal = v;
                            var dot = normal * current.Vel;
                            var nProj = (dot / normal.Length) * normal.Unit;
                            var d = 2 * (nProj - p.Vel);
                            var vm = (p.Vel + d) * -1;
                            deltaSum += vm * Strength;
                        }
                        //if current is inert
                        else
                        {
                            deltaSum += -1 * v.Unit * Strength;
                        }
                    }
                }
            }
            if (collisionCount != 0)
            _delta = deltaSum * (1/collisionCount);
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

