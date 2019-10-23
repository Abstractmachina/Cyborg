using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics.Interfaces
{
    public interface IConstraint
    {

        IEnumerable<int> Indices { get; set; }

        void Calculate(List<Particle> particles);

        void Apply(List<Particle> particles);
    }
}
