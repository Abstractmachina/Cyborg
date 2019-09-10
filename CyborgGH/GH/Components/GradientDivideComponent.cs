using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;


using Cyborg.Params;

namespace Cyborg.GH.Components
{
    public class GradientDivideComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the GradientDivideComponent class.
        /// </summary>
        public GradientDivideComponent()
          : base("Gradient Division", "GradDiv",
              "Discretized gradient division of linear parametric space with attractor points.",
              Strings.LIB_NAME, Strings.SUB_NURBS)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            //List<double> t0, double distMin, double distMax, double thres, int minBuffer, double factor

            pManager.AddNumberParameter("Center Points", "t0", "Center Points of gradient", GH_ParamAccess.list);
            pManager.AddNumberParameter("Minimum Distance", "D0", "Mininum Step Distance", GH_ParamAccess.item);
            pManager.AddNumberParameter("Maximum Distance", "D1", "Maximum Step Distance", GH_ParamAccess.item);
            pManager.AddNumberParameter("Merge Threshold", "TH", "Merge threshold of center points", GH_ParamAccess.item);
            pManager.AddNumberParameter("Buffer Size", "B", "buffer size before increment takes effect", GH_ParamAccess.item);
            pManager.AddNumberParameter("Factor", "F", "Scale factor of gradient step", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Parameters", "t", "Resulting gradient parameters", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var t0 = new List<double>();
            double distMin = 0;
            double distMax = 0;
            double thres = 0;
            double buffer = 0;
            double factor = 0;

            if (!DA.GetDataList(0, t0)) return;
            if (!DA.GetData(1, ref distMin)) return;
            if (!DA.GetData(2, ref distMax)) return;
            if (!DA.GetData(3, ref thres)) return;
            if (!DA.GetData(4, ref buffer)) return;
            if (!DA.GetData(5, ref factor)) return;

            if ( distMin > distMax) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Minimum distance can't be larger than maximum distance.");
            if ( factor <= 0) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Factor must be greater than 0");
            if ( thres < 0) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Merge threshold must be greater than or equal to 0");
            if ( buffer < 0) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Buffer must be greater than or equal to 0");


            var paramOut = ParamOperations.GradientDivide(t0, distMin, distMax, thres, (int) buffer, factor);


            DA.SetDataList(0, paramOut);
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
            get { return new Guid("dd22bce8-d2c8-4295-a8af-a10b544f8c36"); }
        }
    }
}