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
        void Main()
        {
            var tree = new DataTree<Guid>();


            int index = 0;
            foreach (var g in RhinoDoc.ActiveDoc.Groups)
            {
                if (RhinoDoc.ActiveDoc.Objects.FindByGroup(g.Index).Length > 0)
                {
                    var p = new GH_Path(index++);
                    tree.AddRange(RhinoDoc.ActiveDoc.Objects.FindByGroup(g.Index).Select(o => o.Id), p);
                }
            }


            var result = tree;
        }
    }
}