using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using System.Linq;
using GH_IO.Serialization;
using System.Windows.Forms;
using Cyborg.CyborgGH.Properties;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Cyborg.GH.Surfaces
{
    public class LoftSplit : GH_Component
    {
        /// <summary>
        /// 
        /// </summary>
        public LoftSplit()
          : base("Loft Split", "LoftSplit",
              "Loft and return result split at tangents.",
              Strings.LIB_NAME, Strings.SUB_SRF)
        {

        }

        /// <summary>
        /// 
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "Curves to loft.", GH_ParamAccess.list);

            var optionsID = new Guid("{A8DA9901-F5FB-49ec-9CD1-DFA7B788263E}");
            var options = Grasshopper.Instances.ComponentServer.EmitObject(optionsID) as IGH_Param;

            if (options == null) return;

            options.Name = "Loft Options";
            options.NickName = "O";
            options.Description = "Additional Options";
            options.Optional = true;
            options.Access = GH_ParamAccess.item;
            pManager.AddParameter(options);
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Loft Output", "L", "Lofted Breps.", GH_ParamAccess.list);
            
        }

        /// <summary>
        /// 
        /// </summary>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var sections = new List<Rhino.Geometry.Curve>();
            if (!DA.GetDataList("Curves", sections)) return;

            if (sections.Count < 2)
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Need at least 2 curves for loft.");
                return;
            }

            //check if curve structures match
            //get number of control points for all crvs in list
            var ptCount = sections.Select(c =>
            {
                var nc = c.ToNurbsCurve();
                return nc.Points.Count;
            }).ToList();

            if (ptCount.Any(o => o != ptCount[0]))
            {
                AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Curves must have identical structure.");
                return;
            }


            object options = null;
            bool useDefault = false;
            if (!DA.GetData(1, ref options)) useDefault = true;
            if (options == null) useDefault = true;

            var lofts = new List<Brep>();
            if (useDefault)
            {
                if (!Surfaces.LoftSplit(sections, ref lofts)) AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Loft has failed!");
            }
            else
            {
                var adjustSeams = (bool)Grasshopper.Utility.InvokeGetter(options, "AdjustSeams");
                var closedLoft = (bool)Grasshopper.Utility.InvokeGetter(options, "ClosedLoft");
                var rebuildCount = (int)Grasshopper.Utility.InvokeGetter(options, "RebuildCount");
                var refitTolerance = (double)Grasshopper.Utility.InvokeGetter(options, "RefitTolerance");
                var loftType = (int)Grasshopper.Utility.InvokeGetter(options, "LoftType");
                //var loftFit = (int)Grasshopper.Utility.InvokeGetter(options, "LoftFit");

                LoftOptions opt = new LoftOptions(adjustSeams, closedLoft, rebuildCount, refitTolerance, loftType);

                if (!Surfaces.LoftSplit(sections, opt, ref lofts)) AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Loft has failed!");
            }

            DA.SetDataList("Loft Output", lofts);

        }

        


        /// <summary>
        /// Provides an Icon for every component that will be visible in the User Interface.
        /// Icons need to be 24x24 pixels.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                // You can add image files to your project resources and access them like this:
                //return Resources.IconForThisComponent;
                return Resources.LoftSplit;
            }
        }


        /// <summary>
        /// Each component must have a unique Guid to identify it. 
        /// It is vital this Guid doesn't change otherwise old ghx files 
        /// that use the old ID will partially fail during loading.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("1b7823cd-430c-4856-86c5-7367aa252329"); }
        }
    }
}
