﻿using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using System.Linq;
using GH_IO.Serialization;
using System.Windows.Forms;
using Cyborg.Properties;

// In order to load the result of this wizard, you will also need to
// add the output bin/ folder of this project to the list of loaded
// folder in Grasshopper.
// You can use the _GrasshopperDeveloperSettings Rhino command for that.

namespace Cyborg.RH.NURBS
{
    public class LoftSplitComponent : GH_Component
    {
        /// <summary>
        /// 
        /// </summary>
        public LoftSplitComponent()
          : base("Loft Split", "LoftSplit",
              "Loft and split at tangents.",
              "Cyborg", "NURBS")
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
            var sections = new List<Curve>();
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
            if (useDefault) lofts = LoftSplit(sections);
            else
            {
                var adjustSeams = (bool)Grasshopper.Utility.InvokeGetter(options, "AdjustSeams");
                var closedLoft = (bool)Grasshopper.Utility.InvokeGetter(options, "ClosedLoft");
                var rebuildCount = (int)Grasshopper.Utility.InvokeGetter(options, "RebuildCount");
                var refitTolerance = (double)Grasshopper.Utility.InvokeGetter(options, "RefitTolerance");
                var loftType = (int)Grasshopper.Utility.InvokeGetter(options, "LoftType");
                //var loftFit = (int)Grasshopper.Utility.InvokeGetter(options, "LoftFit");

                LoftOptions opt = new LoftOptions(adjustSeams, closedLoft, rebuildCount, refitTolerance, loftType);

                lofts = LoftSplit(sections, opt);
            }

            DA.SetDataList("Loft Output", lofts);

        }

        /// <summary>
        /// Main loftsplit method with default options.
        /// </summary>
        /// <param name="crvs"></param>
        /// <returns></returns>
        internal List<Brep> LoftSplit(List<Curve> crvs)
        {
            LoftOptions options = new LoftOptions(false, false, 0, 0, (int) LoftType.Straight);
            return LoftSplit(crvs, options);
        }

        /// <summary>
        /// Main method. Split curves at tangency discontinuities and loft resulting segments. 
        /// </summary>
        /// <param name="crvs"></param>
        /// <returns></returns>
        public List<Brep> LoftSplit(List<Curve> crvs, LoftOptions options)
        {
            
            var exCrvs = new List<List<Curve>>();

            //explode crv
            foreach (var c in crvs)
            {
                var L = new List<Curve>();
                if (!ExplodeCurveSegments(ref L, c, true)) AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Explode has failed.");
                else exCrvs.Add(L);
            }

            if (exCrvs.Count == 0) return null;
            if (exCrvs == null) return null;

            int numberOfLofts = exCrvs[0].Count;


            var lofts = new List<Brep>();

            for (int i = 0; i < numberOfLofts; i++)
            {
                Curve[] cr = new Curve[exCrvs.Count];
                for (int j = 0; j < cr.Length; j++)
                {
                    cr[j] = exCrvs[j][i];
                }

                lofts.Add(Rhino.Geometry.Brep.CreateFromLoft(cr, Point3d.Unset, Point3d.Unset, LoftType.Straight, false)[0]);

            }

            return lofts;
        }

        /// <summary>
        /// Explode Curve into segments. Method provided by David Rutten.
        /// </summary>
        /// <param name="L">Exploded curve segments.</param>
        /// <param name="crv">Curve to explode</param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        protected bool ExplodeCurveSegments(ref List<Curve> L, Curve crv, bool recursive)
        {
            if (crv == null) { return false; }

            PolyCurve polycurve = crv as PolyCurve;
            if (polycurve != null)
            {
                if (recursive) { polycurve.RemoveNesting(); }

                Curve[] segments = polycurve.Explode();

                if (segments == null) { return false; }
                if (segments.Length == 0) { return false; }

                if (recursive)
                {
                    foreach (Curve S in segments)
                    {
                        ExplodeCurveSegments(ref L, S, recursive);
                    }
                }
                else
                {
                    foreach (Curve S in segments)
                    {
                        L.Add(S.DuplicateShallow() as Curve);
                    }
                }

                return true;
            }

            PolylineCurve polyline = crv as PolylineCurve;
            if (polyline != null)
            {
                if (recursive)
                {
                    for (int i = 0; i < (polyline.PointCount - 1); i++)
                    {
                        L.Add(new LineCurve(polyline.Point(i), polyline.Point(i + 1)));
                    }
                }
                else
                {
                    L.Add(polyline.DuplicateCurve());
                }
                return true;
            }

            Polyline p;
            if (crv.TryGetPolyline(out p))
            {
                if (recursive)
                {
                    for (int i = 0; i < (p.Count - 1); i++)
                    {
                        L.Add(new LineCurve(p[i], p[i + 1]));
                    }
                }
                else
                {
                    L.Add(new PolylineCurve(p));
                }
                return true;
            }

            //Maybe it's a LineCurve?
            LineCurve line = crv as LineCurve;
            if (line != null)
            {
                L.Add(line.DuplicateCurve());
                return true;
            }

            //It might still be an ArcCurve...
            ArcCurve arc = crv as ArcCurve;
            if (arc != null)
            {
                L.Add(arc.DuplicateCurve());
                return true;
            }

            //Nothing else worked, lets assume it's a nurbs curve and go from there...
            NurbsCurve nurbs = crv.ToNurbsCurve();
            if (nurbs == null) { return false; }

            double t0 = nurbs.Domain.Min;
            double t1 = nurbs.Domain.Max;
            double t;

            int LN = L.Count;

            do
            {
                if (!nurbs.GetNextDiscontinuity(Continuity.C1_locus_continuous, t0, t1, out t)) { break; }

                Interval trim = new Interval(t0, t);
                if (trim.Length < 1e-10)
                {
                    t0 = t;
                    continue;
                }

                Curve M = nurbs.DuplicateCurve();
                M = M.Trim(trim);
                if (M.IsValid) { L.Add(M); }

                t0 = t;
            } while (true);

            if (L.Count == LN) { L.Add(nurbs); }

            return true;
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
