using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;

namespace Cyborg.GH.IO
{
    public class WriteToDWG : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the c_WriteToDWG class.
        /// </summary>
        public WriteToDWG()
          : base("Write To DWG", "DWG",
              "Export Geometry to DWG.",
              Strings.LIB_NAME, Strings.SUB_IO)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Write", "W", "Execute Write.", GH_ParamAccess.item, false);
            pManager.AddGeometryParameter("Geometry", "G", "Geometry to be exported.", GH_ParamAccess.tree);
            pManager.AddTextParameter("Layer", "L", "Set layer. Note: Does not support nesting.", GH_ParamAccess.list);
            pManager.AddColourParameter("Color", "C", "Set layer color.", GH_ParamAccess.list);
            pManager.AddTextParameter("File Name", "N", "Set file name", GH_ParamAccess.list);
            pManager.AddTextParameter("Path", "P", "Set directory path", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("debug", "debug", "debug", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            var debug = new List<string>();

            bool i_write = false;
            var i_geom = new GH_Structure<IGH_GeometricGoo>();
            var i_layers = new List<string>();
            var i_colors = new List<System.Drawing.Color>();
            var i_names = new List<string>();
            var i_paths = new List<string>();

            if (!DA.GetData(0, ref i_write)) return;
            if (!DA.GetDataTree(1, out i_geom)) return;
            if (!DA.GetDataList(2, i_layers)) return;
            if (!DA.GetDataList(3, i_colors)) return;
            if (!DA.GetDataList(4, i_names)) return;
            if (!DA.GetDataList(5, i_paths)) return;


            if (i_write)
            {
                if (i_geom.PathCount != i_names.Count) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Must specify name for each branch.");

                for (int i = 0; i <i_geom.PathCount; i++)
                {
                    var branch = i_geom.Branches[i];

                    var ids = new List<Guid>();
                    var oTable = Rhino.RhinoDoc.ActiveDoc.Objects;

                    string scheme = "R12 Lines & Arcs";
                    string outputPath = "";


                    //create layer if not exist
                    string tempLayerName = "";
                    if (i_layers.Count == 1) tempLayerName = i_layers[0];
                    else
                    {
                        if (i_layers.Count != i_geom.PathCount) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Must have one layer for each branch or single layer for all branches.");
                        else tempLayerName = i_layers[i];
                    }

                    int layerIndex = Rhino.RhinoDoc.ActiveDoc.Layers.FindByFullPath(tempLayerName, -1); 
                    if (layerIndex < 0) layerIndex = Rhino.RhinoDoc.ActiveDoc.Layers.Add(tempLayerName, System.Drawing.Color.Orange);
                    var attr = new Rhino.DocObjects.ObjectAttributes();
                    attr.LayerIndex = layerIndex;

                    //bake and export geometry
                    foreach (var o in branch)
                    {
                        if (o is Curve)
                        {
                            //oTable.AddCurve((Curve) o);
                            oTable.AddCurve((Curve)o, attr);
                            //Print(oTable.MostRecentObject().Id.ToString());
                            ids.Add(oTable.MostRecentObject().Id);

                        }
                    }

                    //create output path
                    if (i_paths.Count == 1) outputPath = i_paths[0] + i_names[i];
                    else
                    {
                        if (i_paths.Count != i_names.Count) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Must have path for every file or one path for all files.");
                        else outputPath = i_paths[i] + i_names[i];
                    }
                    outputPath += ".dwg";
                    string macro = String.Format("!_-export {0} Scheme {1} _Enter", outputPath, scheme);

                    //select baked objects and export 
                    foreach (var id in ids) oTable.FindId(id).Select(true);
                    Rhino.RhinoApp.RunScript(macro, false);
                    foreach (var id in ids) oTable.Delete(id, true);
                }
            }

            DA.SetDataList(0, debug);

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
            get { return new Guid("b01c6c27-2310-4e50-b4d0-06ccafad414c"); }
        }
    }
}