using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Verlet
{
    public interface IVerletConstraint
    {
        void Calculate(List<VerletParticle> particles);
        void Apply(List<VerletParticle> particles);
        IEnumerable<int> Indices { get; set; }
    }
}
