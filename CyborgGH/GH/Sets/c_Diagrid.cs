using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

using Cyborg.Params;

namespace Cyborg.GH.Sets
{
    public class BuildDiagrid : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DiagridComponent class.
        /// </summary>
        public BuildDiagrid()
          : base("Build diagrid structure", "Diagrid",
              "Build diagrid from 2D Matrix of any type.",
              Strings.LIB_NAME, Strings.SUB_SET)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Matrix", "M", "2D Matrix. Columns must be equal in length.", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("Alternate", "A", "Alternate panel arrangement.", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Diagrid", "D", "Matrix sorted into panels.", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool alt = false;
            var inputMatrix = new GH_Structure<IGH_Goo>();
            if (!DA.GetDataTree("Matrix", out inputMatrix)) return;
            if (!DA.GetData("Alternate", ref alt)) return;


            //convert gh tree to [][] dddd
            var processMatrix = inputMatrix.Branches.ToList();
            var processMatrix2 = new IGH_Goo[processMatrix.Count][];
            for (int i = 0; i < processMatrix.Count; i++) processMatrix2[i] = processMatrix[i].ToArray();

            //build diagrid
            var diagrid = new List<List<IGH_Goo>>();
            if (!SetOperations.BuildDiagrid<IGH_Goo>(processMatrix2, false, out diagrid)) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Operation failed. Make sure the matrix is uniform.");

            ////convert list<list<T>> to gh tree
            //var outDiagrid = new GH_Structure<IGH_Goo>();
            //for (int i = 0; i < diagrid.Count; i++)
            //{
            //    var pth = new GH_Path(i);
            //    outDiagrid.AppendRange(diagrid[i], pth);
            //}

            //DA.SetDataTree(0, outDiagrid);

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
            get { return new Guid("5388e424-30da-46d3-a44e-348d0f19a61f"); }
        }
    }
}