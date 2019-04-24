using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.RH
{
    public static class Curves
    {


        /// <summary>
        /// Fillet 2 Curves. Result will be trimmed/extended.
        /// </summary>
        /// <param name="c1"></param>
        /// <param name="c2"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        public static List<Curve> Fillet(Curve c1, Curve c2, double radius)
        {
            return Curve.CreateFilletCurves(c1, c1.PointAtEnd, c2, c2.PointAtStart, radius, false, true, true, 0.1, 0.1).ToList();
        }

        /// <summary>
        /// Explode Curve into segments. Method provided by David Rutten.
        /// </summary>
        /// <param name="L">Exploded curve segments.</param>
        /// <param name="crv">Curve to explode</param>
        /// <param name="recursive"></param>
        /// <returns></returns>
        public static bool ExplodeCurveSegments(ref List<Curve> L, Curve crv, bool recursive)
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


    }
}
