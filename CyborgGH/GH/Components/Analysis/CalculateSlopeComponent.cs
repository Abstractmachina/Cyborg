using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cyborg.GH.Components.Analysis
{
    public class CalculateSlopeComponent : GH_Component
    {
        public CalculateSlopeComponent()
          : base("Slope Calculator", "Slope",
              "Get slope of a given line",
              Strings.LIB_NAME, Strings.SUB_ANA)
        {
        }

        string inputLine = "Line";

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddLineParameter(inputLine, "Line", "Line describing slope section", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("Ratio", "Ratio", "Slope returned as denominator.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Angle", "Angle", "Slope returned as angle in degrees.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Grade", "Grade", "Slope returned as grade percentage.", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Line l = new Line();

            if (!DA.GetData(inputLine, ref l)) return;

            NurbsCurve c = l.ToNurbsCurve();

            Point3d[] endpts = new Point3d[2];

            //align curves by Z val
            if (c.PointAtStart.Z > c.PointAtEnd.Z) c.Reverse();

            Plane pl = new Plane(c.PointAtStart, Vector3d.ZAxis);
            var projection = Rhino.Geometry.Curve.ProjectToPlane(c, pl);

            var v1 = c.PointAtEnd - c.PointAtStart;
            var v2 = projection.PointAtEnd - projection.PointAtStart;
            var angle = Vector3d.VectorAngle(v1, v2);



            //calc ratio
            /*
          angle____ a ____
               \          |
                \         |
                 \        |
                  \       |
                   c      b
                    \     |
                     \    |
                      \   |
                       \  |
                        \ |
            */

            var cLength = c.GetLength();
            var b = Math.Sin(angle) * cLength;
            var a = Math.Cos(angle) * cLength;

            double factor = (double)1 / b;
            var ratio = Math.Round((a * factor), 2);
            if (ratio > 200) ratio = 200;

            string s = String.Format("1:{0}", ratio);
            DA.SetData("Ratio", ratio);
            DA.SetData("Angle", angle);
            DA.SetData("Grade", (1 / ratio));
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
            get { return new Guid("96b4ef3a-4332-4004-88bf-61ea6a8d99e3"); }
        }
    }
}