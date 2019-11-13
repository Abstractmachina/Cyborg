using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Cyborg.GH;
using Rhino;
using System.Linq;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

namespace Cyborg.GH.IO
{
    public class ReferenceGroups : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ReferenceGroupsComponent class.
        /// </summary>
        public ReferenceGroups()
          : base("Reference Rhino Doc Groups", "RefGroups",
              "Reference all groups of the current Rhino document.",
              Strings.LIB_NAME, Strings.SUB_IO)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Layers", "L", "Filter geometry by layer (optional).", GH_ParamAccess.list);
            pManager[0].Optional = true;
            
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("GUID", "G", "GUID of group members", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string noLayerSel = "all";
            var layers = new List<string>();

            if (!DA.GetDataList(0, layers))
            {
                layers.Add(noLayerSel);
            }

            var tree = new GH_Structure<GH_Guid>();

            int index = 0;
            foreach (var g in RhinoDoc.ActiveDoc.Groups)
            {
                if (RhinoDoc.ActiveDoc.Objects.FindByGroup(g.Index).Length > 0)
                {
                    var p = new GH_Path(index++);

                    if (layers[0] == noLayerSel) tree.AppendRange(RhinoDoc.ActiveDoc.Objects.FindByGroup(g.Index).Select(o => new GH_Guid(o.Id)), p);
                    else
                    {
                        var guids = RhinoDoc.ActiveDoc.Objects.FindByGroup(g.Index).Select(o => o.Id);
                        tree.AppendRange(guids.Where(id => layers.Contains(RhinoDoc.ActiveDoc.Layers.FindIndex(RhinoDoc.ActiveDoc.Objects.FindId(id).Attributes.LayerIndex).FullPath))
                            .Select(id => new GH_Guid(id)), p);
                    }
                }
            }

            DA.SetDataTree(0, tree);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("08178af3-7604-4d04-8baf-496a20821be7"); }
        }
    }
}