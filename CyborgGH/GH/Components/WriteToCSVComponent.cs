using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cyborg.GH.Components
{
    public class WriteToCSVComponent : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the WriteToCSVComponent class.
        /// </summary>
        public WriteToCSVComponent()
          : base("Write To CSV", "writeCSV",
              "Write a list of points to CSV format",
              Strings.LIB_NAME, Strings.SUB_IO)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {

            pManager.AddTextParameter("Output path", "P", "Full path to output directory.", GH_ParamAccess.item);
            pManager.AddTextParameter("File name", "N", "File name", GH_ParamAccess.item);
            pManager.AddPointParameter("Point list", "PL", "List of points.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Write", "W", "Execute write to file", GH_ParamAccess.item, false);

        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            string path = "";
            string name = "";
            var pts = new List<Point3d>();
            bool write = false;

            if (!DA.GetData(0, ref path)) return;
            if (!DA.GetData(1, ref name)) return;
            if (!DA.GetDataList(2, pts)) return;
            if (!DA.GetData(3, ref write)) return;


            if (path == null || name == null || pts == null)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "One or more inputs missing.");
                return;
            }


            string outputPath;
            outputPath = System.String.Format("{0}{1}.csv", path, name);



            if (write)
            {

                string content = string.Empty;
                foreach (Point3d p in pts)
                {
                    content += string.Format("{0},{1},{2}\n", p.X, p.Y, p.Z);
                }

                System.IO.File.WriteAllText(outputPath, content);
            }

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
            get { return new Guid("0de72e7a-e7ac-4c21-92ea-55a43801fb6c"); }
        }
    }
}