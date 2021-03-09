using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;


namespace Cyborg.GH.Meshes
{
    public class NGonBoundaries : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the MyComponent1 class.
        /// </summary>
        public NGonBoundaries()
          : base("NGon Boundaries", "NGB",
              "Get ngon boundaries as polyline.",
              Strings.LIB_NAME, Strings.SUB_MSH)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Mesh", "M", "input mesh", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddCurveParameter("Ngon Boundaries", "B", "Ngon Boundaries as polylines", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            Rhino.Geometry.Mesh mesh = new Rhino.Geometry.Mesh();

            DA.GetData(0, ref mesh);

            var boundaries = new List<Polyline>();

            for (int i = 0; i < mesh.Ngons.Count; i++)
            {

                var bound = mesh.Ngons.GetNgon(i).BoundaryVertexIndexList();
                var vertices = bound.Select(v => (Point3d)mesh.Vertices[(int)v]);
                boundaries.Add(new Polyline(vertices));

            }

            DA.SetDataList("0", boundaries);

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
            get { return new Guid("54defda3-13e3-41ea-9319-6bbf4c6c96f5"); }
        }
    }
}