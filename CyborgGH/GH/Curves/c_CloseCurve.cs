using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;


namespace Cyborg.GH.Curves
{
    public class CloseCurve : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the c_CloseCurve class.
        /// </summary>
        public CloseCurve()
          : base("Close Curve", "CloseCrv",
              "Close open curve with a line segment.",
              Strings.LIB_NAME, Strings.SUB_CURVE)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Input Curve", "C", "Input Curve.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Output Curve", "C", "Output Curve", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Curve c = null;

            if (!DA.GetData(0, ref c)) return;

            var outputCurves = new List<Curve>();
            if (c.IsClosed)
            {
                var closeCrv = new Line(c.PointAtStart, c.PointAtEnd);

                var inputCurves = new List<Rhino.Geometry.Curve>();
                inputCurves.Add(c);
                inputCurves.Add(closeCrv.ToNurbsCurve());

                outputCurves = Curve.JoinCurves(inputCurves).ToList();
                DA.SetData(0, outputCurves[0]);
            }

            else DA.SetData(0, c);
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
            get { return new Guid("2748d7f6-b0a5-410f-9193-3af9234737d5"); }
        }
    }
}