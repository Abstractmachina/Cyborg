using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Cyborg.CyborgGH.Properties;
using Cyborg.GH;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;

namespace Cyborg.CyborgGH.GH.IO
{
    public class c_ReferenceBlocksByLayer : GH_Component
    {
        private bool computeParallel;
        private bool includeHiddenObj;

        /// <summary>
        /// Initializes a new instance of the c_ReferenceBlocksByLayer class.
        /// </summary>
        public c_ReferenceBlocksByLayer()
          : base("Reference Block Geometry By Layer", "RefBlocks_L",
              "Reference block geometry in current Rhino document by geometry layer.",
              Strings.LIB_NAME, Strings.SUB_IO)
        {
            computeParallel = true;
            includeHiddenObj = true;
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Layer FIlter", "L", "Specify layers that geometry is in (not blocks)", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometry", "G", "Blocks as exploded geometry", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var layers = new List<string>();
            if (!DA.GetDataList(0, layers)) return;

            if (computeParallel) DA.SetDataTree(0, ProcessBlocks_Parallel(layers));
            else DA.SetDataTree(0, ProcessBlocks(layers));
        }



        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public GH_Structure<IGH_GeometricGoo> ProcessBlocks_Parallel(List<string> layers)
        {

            // var inputList = RhinoDoc.ActiveDoc.Objects.ToList();
            var inputList = new List<RhinoObject>();

            //settings for object enumeration, mainly to include hidden objects
            var settings = new ObjectEnumeratorSettings();
            if (includeHiddenObj) settings.HiddenObjects = true;
            else settings.HiddenObjects = false;
            settings.NormalObjects = true;
            settings.ActiveObjects = true;
            settings.DeletedObjects = false;
            settings.IncludeGrips = false;
            settings.IncludeLights = false;
            settings.IncludePhantoms = false;

            var objects = RhinoDoc.ActiveDoc.Objects.GetObjectList(settings).Where(o => o.ObjectType == ObjectType.InstanceReference);

            //for some reason, cant convert directly to list
            foreach (var o in objects) inputList.Add(o);

            //instantiate dictionary
            var result = new ConcurrentDictionary<int, IEnumerable<IGH_GeometricGoo>>(Environment.ProcessorCount, inputList.Count);
            for (int i = 0; i < inputList.Count; i++)
            {
                result[i] = new List<IGH_GeometricGoo>();
            }

            Parallel.For(0, inputList.Count, i =>
            {
                var o = inputList[i];

                var tempgeom = o.GetSubObjects().Select(oo => oo.Geometry).ToArray();
                var templayer = o.GetSubObjects().Select(oo => RhinoDoc.ActiveDoc.Layers.FindIndex(oo.Attributes.LayerIndex).FullPath).ToArray();

                var outList = new List<IGH_GeometricGoo>();
                for (int j = 0; j < tempgeom.Length; j++) if (layers.Contains(templayer[j])) outList.Add(GH_Convert.ToGeometricGoo(tempgeom[j]));
                result[i] = outList;
            }
            );

            var outGeom = new GH_Structure<IGH_GeometricGoo>();
            foreach (var branch in result)
            {
                var pth = new GH_Path(branch.Key);
                outGeom.AppendRange(branch.Value, pth);
            }

            return outGeom;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="layers"></param>
        /// <returns></returns>
        private GH_Structure<IGH_GeometricGoo> ProcessBlocks(List<string> layers)
        {
            var tree = new GH_Structure<IGH_GeometricGoo>();
            int ind = 0;
            foreach (var o in RhinoDoc.ActiveDoc.Objects)
            {
                if (o.ObjectType == Rhino.DocObjects.ObjectType.InstanceReference)
                {
                    var pth = new GH_Path(ind++);
                    var tempgeom = o.GetSubObjects().Select(oo => oo.Geometry).ToArray();
                    var templayer = o.GetSubObjects().Select(oo => RhinoDoc.ActiveDoc.Layers.FindIndex(oo.Attributes.LayerIndex).FullPath).ToArray();

                    for (int i = 0; i < tempgeom.Length; i++)
                    {
                        if (layers.Contains(templayer[i]))
                            tree.Append(GH_Convert.ToGeometricGoo(tempgeom[i]), pth);
                    }
                    //tree.AddRange(o.GetSubObjects().Select(oo => oo.Geometry), pth);
                    //layers.AddRange(o.GetSubObjects().Select(oo => RhinoDoc.ActiveDoc.Layers.FindIndex(oo.Attributes.LayerIndex).FullPath), pth);
                }
            }

            return tree;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="menu"></param>
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, "Parallel Computing", OnClick_Parallel, true, computeParallel);
            Menu_AppendItem(menu, "Include Hidden Objects", Onclick_HiddenObj, true, includeHiddenObj);
        }

        /// <summary>
        /// 
        /// </summary>
        private void Onclick_HiddenObj(object sender, EventArgs e)
        {
            includeHiddenObj = !includeHiddenObj;
            this.ExpireSolution(true);
        }

        /// <summary>
        /// 
        /// </summary>
        private void OnClick_Parallel(object sender, EventArgs e)
        {
            computeParallel = !computeParallel;
            this.ExpireSolution(true);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return Resources.ic_refBlocksByLayer;
                //return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("2dff09c3-bb0f-4208-ac25-3033ae11c31c"); }
        }
    }
}