using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grasshopper;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;

namespace Cyborg.GH
{
    public class SandboxGH
    {
        void Main(Surface surface, List<Rhino.Geometry.Curve> curves, double tol)
        {

            var culPatt = new List<bool>();
            foreach (var c in curves)
            {
                bool isContained = false;
                var cp = c.ToNurbsCurve().Points;
                foreach (var p in cp)
                {
                    double u = 0;
                    double v = 0;
                    surface.ClosestPoint(p.Location, out u, out v);
                    if (surface.PointAt(u,v).DistanceTo(p.Location) < tol)
                    {
                        isContained = true;
                        break;
                    }
                }
                culPatt.Add(isContained);

            }
        }
    }
}