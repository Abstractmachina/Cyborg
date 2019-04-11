﻿using Grasshopper.Kernel;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Cyborg.RH
{
    public static class Surfaces
    {

        /// <summary>
        /// LoftSplit() with default options.
        /// </summary>
        /// <param name="crvs">Input Curves</param>
        /// <param name="outBreps">Loft Output as Breps</param>
        /// <returns></returns>
        public static bool LoftSplit(List<Curve> crvs, ref List<Brep> outBreps)
        {
            LoftOptions options = new LoftOptions(false, false, 0, 0, (int)LoftType.Straight);
            return LoftSplit(crvs, options, ref outBreps);
        }

        /// <summary>
        /// Split curves at tangency discontinuities and loft resulting segments. 
        /// </summary>
        /// <param name="inCrvs">Input curves</param>
        /// <param name="options">Additional Options</param>
        /// <param name="outBreps">Loft Output as Breps</param>
        /// <returns></returns>
        public static bool LoftSplit(List<Curve> inCrvs, LoftOptions options, ref List<Brep> outBreps)
        {

            var exCrvs = new List<List<Curve>>();

            //explode crv
            foreach (var c in inCrvs)
            {
                var L = new List<Curve>();
                if (!Curves.ExplodeCurveSegments(ref L, c, true)) return false;
                else exCrvs.Add(L);
            }

            if (exCrvs.Count == 0) return false;
            if (exCrvs == null) return false;

            int numberOfLofts = exCrvs[0].Count;

            for (int i = 0; i < numberOfLofts; i++)
            {
                Curve[] cr = new Curve[exCrvs.Count];
                for (int j = 0; j < cr.Length; j++)
                {
                    cr[j] = exCrvs[j][i];
                }

                outBreps.Add(Rhino.Geometry.Brep.CreateFromLoft(cr, Point3d.Unset, Point3d.Unset, LoftType.Straight, false)[0]);

            }

            return true;
        }

        

    }
}
