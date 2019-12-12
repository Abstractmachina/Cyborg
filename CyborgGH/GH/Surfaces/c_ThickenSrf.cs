using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cyborg.GH;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;

namespace Cyborg.CyborgGH.GH.Surfaces
{
    public class c_ThickenSrf : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the c_OffsetSolid class.
        /// </summary>
        public c_ThickenSrf()
          : base("Thicken Surface", "ThickenSrf",
              "Offset Surface as a solid",
              Strings.LIB_NAME, Strings.SUB_SRF)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "S", "Surface to be thickened", GH_ParamAccess.list);
            pManager.AddNumberParameter("Thickness", "T", "Offset Distance", GH_ParamAccess.item, 1.0);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Brep", "B", "Thickened Surface", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var surfaces = new List<GH_Surface>();
            double thickness = 1.0;

            if (!DA.GetDataList(0, surfaces)) return;
            if (!DA.GetData(1, ref thickness)) return;

            //instantiate dictionary
            var result = new ConcurrentDictionary<int, Brep[]>();
            for (int i = 0; i < surfaces.Count; i++) result[i] = null;

            Parallel.For(0, surfaces.Count, i =>
           {
               Brep[] outblends;
               Brep[] outwalls;
               //result[(int)i] = Brep.CreateOffsetBrep(Brep.CreateFromSurface(surfaces[(int) i]), thickness, true, true, 0.1, out outblends, out outwalls);

           });

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
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("b2d8ab9c-bdbe-4173-906c-b879bcd8cd49"); }
        }
    }
}