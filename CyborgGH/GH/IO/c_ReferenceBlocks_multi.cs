using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using System.Linq;
using System.Threading.Tasks;

namespace Cyborg.GH.IO
{
    public class ReferenceBlocks_multi : GH_TaskCapableComponent<ReferenceBlocks_multi.SolveResults>
    {
        /// <summary>
        /// Initializes a new instance of the ReferenceBlocksComponent class.
        /// </summary>
        public ReferenceBlocks_multi()
          : base("Reference Rhino Doc Blocks (multi-threaded)", "RefBlocks (m)",
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

        private static SolveResults ComputeBlocks(List<string> layers)
        {
            SolveResults results = new SolveResults();

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

            results.Value = tree;
            return results;
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            if (InPreSolve)
            {
                var layers = new List<string>();

                if (!DA.GetDataList(0, layers)) layers.Add(null);

                Task<SolveResults> task = Task.Run(() => ComputeBlocks(layers), CancelToken);
                TaskList.Add(task);
                return;
            }

            if (!GetSolveResults(DA, out SolveResults result))
            {
                var layers = new List<string>();

                if (!DA.GetDataList(0, layers)) layers.Add(null);

                result = ComputeBlocks(layers);
            }
            DA.SetDataTree(0, result.Value);

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
            get { return new Guid("247daa54-df92-42cf-85e7-5dc5d509579f"); }
        }

        public class SolveResults
        {
            public GH_Structure<IGH_GeometricGoo> Value { get; set; }
        }
    }

    
}