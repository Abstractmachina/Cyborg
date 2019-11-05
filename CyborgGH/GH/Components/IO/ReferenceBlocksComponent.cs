using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using System.Linq;

namespace Cyborg.GH.Components.IO
{
    public class ReferenceBlocksComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ReferenceBlocksComponent class.
        /// </summary>
        public ReferenceBlocksComponent()
          : base("Reference Rhino Doc Blocks", "RefBlocks",
              "Reference blocks in current Rhino document.",
              Strings.LIB_NAME, Strings.SUB_IO)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("layers", "L", "Layer Filter", GH_ParamAccess.list);
            pManager[0].Optional = true;
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

            if (!DA.GetDataList(0, layers)) layers.Add(null);

            var tree = new GH_Structure<IGH_GeometricGoo>();


            if (layers[0] != null)
            {
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
            }
            else
            {
                int ind = 0;
                foreach (var o in RhinoDoc.ActiveDoc.Objects)
                {
                    if (o.ObjectType == Rhino.DocObjects.ObjectType.InstanceReference)
                    {
                        var pth = new GH_Path(ind++);
                        tree.AppendRange(o.GetSubObjects().Select(obj => GH_Convert.ToGeometricGoo(obj.Geometry)), pth);
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
            get { return new Guid("247daa54-df92-42cf-85e7-5dc5d509579e"); }
        }
    }
}