using System;
using System.Collections.Generic;
using System.Linq;

//using Cyborg.Properties;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cyborg.GH.Components
{
    public class FilletExtend : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ExtendFilletComponent class.
        /// </summary>
        public FilletExtend()
          : base("Fillet Extend", "Fillet_E",
              "Fillet and extend/trim two curves",
              Strings.LIB_NAME, Strings.SUB_CURVES)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curve 1", "C1", "First Curve", GH_ParamAccess.item);
            pManager.AddCurveParameter("Curve 2", "C2", "Second Curve", GH_ParamAccess.item);
            pManager.AddNumberParameter("Fillet Radius", "R", "Radius of Fillet", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Fillet Output", "C", "Result", GH_ParamAccess.list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Curve c1 = null;
            Curve c2 = null;
            double radius = 0;

            if (!DA.GetData(0, ref c1)) return;
            if (!DA.GetData(1, ref c2)) return;
            if (!DA.GetData(2, ref radius)) return;

            var result = Curves.Fillet(c1, c2, radius);

            DA.SetDataList(0, result);
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
                //return Resources.FilletExtend;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("89adbd0d-93d4-4c52-84dc-d005fa68dde3"); }
        }
    }
}