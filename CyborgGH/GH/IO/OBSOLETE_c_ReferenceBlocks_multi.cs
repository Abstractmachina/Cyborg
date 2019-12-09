using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino;
using Rhino.Geometry;
using Grasshopper.Kernel.Types;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Rhino.DocObjects;

namespace Cyborg.GH.IO
{

    //OBSOLETE

    //public class ReferenceBlocks_multi : GH_Component
    //{
    //    /// <summary>
    //    /// Initializes a new instance of the ReferenceBlocksComponent class.
    //    /// </summary>
    //    public ReferenceBlocks_multi()
    //      : base("Reference Rhino Doc Blocks (multi-threaded)", "RefBlocks (m)",
    //          "Reference blocks in current Rhino document.",
    //          Strings.LIB_NAME, Strings.SUB_IO)
    //    {
    //    }

    //    /// <summary>
    //    /// Registers all the input parameters for this component.
    //    /// </summary>
    //    protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
    //    {
    //        pManager.AddTextParameter("layers", "L", "Layer Filter", GH_ParamAccess.list);
    //    }

    //    /// <summary>
    //    /// Registers all the output parameters for this component.
    //    /// </summary>
    //    protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
    //    {
    //        pManager.AddGeometryParameter("Geometry", "G", "Blocks as exploded geometry", GH_ParamAccess.tree);
    //        pManager.AddTextParameter("debug", "debug", "debug", GH_ParamAccess.list);
    //    }


    //    /// <summary>
    //    /// This is the method that actually does the work.
    //    /// </summary>
    //    /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
    //    protected override void SolveInstance(IGH_DataAccess DA)
    //    {

    //        var debug = new List<string>();

    //        var layers = new List<string>();

    //        if (!DA.GetDataList(0, layers)) return;

    //        var tree = new GH_Structure<IGH_GeometricGoo>();
    //       // var inputList = RhinoDoc.ActiveDoc.Objects.ToList();
    //        var inputList = new List<RhinoObject>();


    //        foreach (var o in RhinoDoc.ActiveDoc.Objects)
    //        {
    //            inputList.Add(o);
    //        }
            
    //        debug.Add("inputList count: " + inputList.Count.ToString());
    //        //foreach (var o in inputList) debug.Add("object: " + o.ToString());

    //        var result = new ConcurrentDictionary<int, IEnumerable<IGH_GeometricGoo>>(Environment.ProcessorCount, inputList.Count);
    //        for (int i = 0; i < inputList.Count; i++)
    //        {
    //            result[i] = new List<IGH_GeometricGoo>();
    //        }


    //        Parallel.For(0, inputList.Count, i =>
    //         {
    //             var o = inputList[i];
    //             if (o.ObjectType == Rhino.DocObjects.ObjectType.InstanceReference)
    //             {
    //                 var tempgeom = o.GetSubObjects().Select(oo => oo.Geometry).ToArray();
    //                 var templayer = o.GetSubObjects().Select(oo => RhinoDoc.ActiveDoc.Layers.FindIndex(oo.Attributes.LayerIndex).FullPath).ToArray();

    //                 var outList = new List<IGH_GeometricGoo>();
    //                 for (int j = 0; j < tempgeom.Length; j++)
    //                 {
    //                     if (layers.Contains(templayer[j]))

    //                     {
    //                         //tree.Append(GH_Convert.ToGeometricGoo(tempgeom[i]), pth);
    //                         outList.Add(GH_Convert.ToGeometricGoo(tempgeom[j]));
    //                     }

    //                 }
    //                 result[i] = outList;

    //             }

    //         }
    //        );

    //        foreach (var branch in result)
    //        {
    //            debug.Add("index: " + branch.Key.ToString() + " || Value: " + branch.Value.ToString());
    //            var pth = new GH_Path(branch.Key);
    //            tree.AppendRange(branch.Value, pth);
    //        }

    //        DA.SetDataTree(0, tree);
    //        DA.SetDataList(1, debug);

    //    }

    //    /// <summary>
    //    /// Provides an Icon for the component.
    //    /// </summary>
    //    protected override System.Drawing.Bitmap Icon
    //    {
    //        get
    //        {
    //            //You can add image files to your project resources and access them like this:
    //            // return Resources.IconForThisComponent;
    //            return null;
    //        }
    //    }

    //    /// <summary>
    //    /// Gets the unique ID for this component. Do not change this ID after release.
    //    /// </summary>
    //    public override Guid ComponentGuid
    //    {
    //        get { return new Guid("247daa54-df92-42cf-85e7-5dc5d509579f"); }
    //    }

    //}


}