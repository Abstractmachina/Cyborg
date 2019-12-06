using System;
using System.Collections.Generic;
using System.Linq;
using Cyborg.GH;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.Geometry;

namespace Cyborg.CyborgGH.GH.IO
{
    public class c_ReferenceBlocksByLayer : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the c_ReferenceBlocksByLayer class.
        /// </summary>
        public c_ReferenceBlocksByLayer()
          : base("Reference Block Geometry By Layer", "RefBlocks_L",
              "Reference block geometry in current Rhino document by geometry layer.",
              Strings.LIB_NAME, Strings.SUB_IO)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("layers", "L", "Layer Filter", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "Blocks as exploded geometry", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var layers = new List<string>();

            if (!DA.GetDataList(0, layers)) return;

            var tree = new GH_Structure<IGH_GeometricGoo>();

            int ind = 0;
            foreach (var o in RhinoDoc.ActiveDoc.Objects)
            {
                if (o.ObjectType == Rhino.DocObjects.ObjectType.InstanceReference)
                {
                    var pth = new GH_Path(ind++);
                    var tempgeom = o.GetSubObjects().Select(oo => oo.Geometry).ToArray();
                    var templayer = o.GetSubObjects().Select(oo => RhinoDoc.ActiveDoc.Layers.FindIndex(oo.Attributes.LayerIndex).FullPath).ToArray();

                    for (int i = 0; i < tempgeom.Length; i++)
                    {
                        if (layers.Contains(templayer[i]))
                            tree.Append(GH_Convert.ToGeometricGoo(tempgeom[i]), pth);

                    }
                    //tree.AddRange(o.GetSubObjects().Select(oo => oo.Geometry), pth);
                    //layers.AddRange(o.GetSubObjects().Select(oo => RhinoDoc.ActiveDoc.Layers.FindIndex(oo.Attributes.LayerIndex).FullPath), pth);
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
            get { return new Guid("2dff09c3-bb0f-4208-ac25-3033ae11c31c"); }
        }
    }
}