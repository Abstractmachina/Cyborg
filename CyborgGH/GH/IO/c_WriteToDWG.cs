using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Special;
using System.Drawing;
using Cyborg.CyborgGH.Properties;

namespace Cyborg.GH.IO
{
    public class WriteToDWG : GH_Component
    {

        private bool _schemeExists;

        /// <summary>
        /// Initializes a new instance of the c_WriteToDWG class.
        /// </summary>
        public WriteToDWG()
          : base("Write To DWG", "DWG",
              "Export Geometry to DWG.",
              Strings.LIB_NAME, Strings.SUB_IO)
        {
            _schemeExists = false;
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
            pManager.AddTextParameter("Scheme", "S", "Set export scheme", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("debug", "debug", "debug", GH_ParamAccess.list);
        }


        

        protected override void BeforeSolveInstance()
        {

            var component = this;
            var gh_doc = OnPingDocument();

            if (!_schemeExists && component.Params.Input[6].Sources.Count == 0)
            {

                //if (gh_doc == null) return;

                var valList = new GH_ValueList();

                valList.CreateAttributes();
                valList.Attributes.Pivot = new PointF(component.Attributes.Pivot.X - 200, (float)component.Attributes.Pivot.Y + 50);
                valList.Name = "Scheme";
                valList.NickName = "Scheme";
                valList.ListItems.Clear();

                var item0 = new GH_ValueListItem("R12 Lines & Arcs", "\"R12 Lines & Arcs\"");
                var item1 = new GH_ValueListItem("Default", "\"Default\"");
                var item2 = new GH_ValueListItem("2007 Lines", "\"2007 Lines\"");
                var item3 = new GH_ValueListItem("2007 Natural", "\"2007 Natural\"");
                var item4 = new GH_ValueListItem("2007 Polylines", "\"2007 Polylines\"");
                var item5 = new GH_ValueListItem("2007 Solids", "\"2007 Solids\"");
                var item6 = new GH_ValueListItem("CAM Imperial", "\"CAM Imperial\"");
                var item7 = new GH_ValueListItem("CAM Metric", "\"CAM Metric\"");
                var item8 = new GH_ValueListItem("R12 Natural", "\"R12 Natural\"");

                valList.ListItems.Add(item0);
                valList.ListItems.Add(item1);
                valList.ListItems.Add(item2);
                valList.ListItems.Add(item3);
                valList.ListItems.Add(item4);
                valList.ListItems.Add(item5);
                valList.ListItems.Add(item6);
                valList.ListItems.Add(item7);
                valList.ListItems.Add(item8);
                gh_doc.AddObject(valList, false);
                component.Params.Input[6].AddSource(valList);

                _schemeExists = true;
            }

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
            string i_scheme = "";

            if (!DA.GetData(0, ref i_write)) return;
            if (!DA.GetDataTree(1, out i_geom)) return;
            if (!DA.GetDataList(2, i_layers)) return;
            if (!DA.GetDataList(3, i_colors)) return;
            if (!DA.GetDataList(4, i_names)) return;
            if (!DA.GetDataList(5, i_paths)) return;
            if (!DA.GetData(6, ref i_scheme)) return;

            foreach(var g in i_geom.AllData(false))debug.Add("input geom: " + g);
            foreach (var l in i_layers) debug.Add("layer: " + l);
            foreach (var c in i_colors) debug.Add("color: " + c);
            foreach (var n in i_names) debug.Add("file name: " + n);
            foreach (var p in i_paths) debug.Add("file path: " + p);
            debug.Add("scheme: " + i_scheme);

            if (i_write)
            {
                if (i_geom.PathCount != i_names.Count) AddRuntimeMessage(GH_RuntimeMessageLevel.Error, "Must specify name for each branch.");

                for (int i = 0; i <i_geom.PathCount; i++)
                {
                    var branch = i_geom.Branches[i];

                    var ids = new List<Guid>();
                    var oTable = Rhino.RhinoDoc.ActiveDoc.Objects;

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

                        debug.Add(o.GetType().ToString());
                        if (o is GH_Curve)
                        {
                            Curve c = null;
                            GH_Convert.ToCurve(o, ref c, GH_Conversion.Both);
                            oTable.AddCurve(c, attr);
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
                    string macro = String.Format("!_-export {0} Scheme {1} _Enter", outputPath, i_scheme);

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
                //return null;

                return Resources.ic_dwg;
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