using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.ParametricSpace
{
    static class ParamOperations
    {

        /// <summary>
        /// Discretized gradient division of linear parametric space with attractor points.
        /// </summary>
        /// <param name="t0">Attractor pointw</param>
        /// <param name="distMin">minimum division distance</param>
        /// <param name="distMax">maximum division distance</param>
        /// <param name="thres">attractor merge threshold</param>
        /// <param name="minBuffer">bufferzone before scalar effect is applied to increment.</param>
        /// <param name="factor">scale factor of increment</param>
        /// <returns></returns>
        public static List<double> GradientDivide(List<double> t0, double distMin, double distMax, double thres, int minBuffer, double factor)
        {

            //SCALAR INCREMENT METHOD
            var paras = new List<double>();

            if (t0.Count == 0) return null;
            else if (t0.Count == 1) //single attractor
            {

                double t = t0[0];

                if (t == 0)
                {
                    var listA = GetEndDivisions(t, 1, distMin, distMax, minBuffer, factor);
                    paras.AddRange(listA);
                }
                else if (t == 1)
                {
                    var listA = GetEndDivisions(t, 0, distMin, distMax, minBuffer, factor);
                    paras.AddRange(listA);
                }
                else
                {
                    var listA = GetEndDivisions(t, 0, distMin, distMax, minBuffer, factor);
                    var listB = GetEndDivisions(t, 1, distMin, distMax, minBuffer, factor);
                    paras.Add(t);
                    paras.AddRange(listA);
                    paras.AddRange(listB);
                }
            }
            else //multi attractors
            {
                t0.Sort();//make sure params are in order

                double tA = 0;
                double tB = 0;

                //check for overlapping condition
                for (int i = 1; i < t0.Count; i++)
                {

                    tA = t0[i - 1];
                    tB = t0[i];

                    double range = tB - tA;
                    //if attractors are far enough apart, get points in between
                    if (range > thres)
                    {
                        var inbetween = GetCenterDivisions(tA, tB, distMin, distMax, minBuffer, factor);
                        paras.AddRange(inbetween);
                    }
                    //if attractors are close enough to merge
                    else
                    {
                        double center = (tB + tA) / 2;
                        //get beginning half
                        if (i - 2 < 1)
                        {
                            var listA = GetEndDivisions(center, 0, distMin, distMax, minBuffer, factor);
                            paras.AddRange(listA);
                        }
                        else
                        {
                            var listA = GetCenterDivisions(t0[i - 2], center, distMin, distMax, minBuffer, factor);
                            paras.AddRange(listA);
                        }
                        //get ending half
                        if (i + 1 == t0.Count)
                        {
                            var listB = GetEndDivisions(center, 1, distMin, distMax, minBuffer, factor);
                            paras.AddRange(listB);
                        }
                        else
                        {
                            var listB = GetCenterDivisions(center, t0[i + 1], distMin, distMax, minBuffer, factor);
                            paras.AddRange(listB);
                        }
                    }
                }

                if (paras.Count < 1) return null;
                //end conditions
                //start point
                paras.Sort();
                tA = paras[0];

                if (tA > distMin * 2)
                {
                    var startList = GetEndDivisions(tA, 0, distMin, distMax, minBuffer, factor);
                    paras.AddRange(startList);
                }

                //end point
                paras.Sort();
                tB = paras[paras.Count - 1];

                if (tB < 1 - distMin)
                {
                    var endList = GetEndDivisions(tB, 1, distMin, distMax, minBuffer, factor);
                    paras.AddRange(endList);
                }
            }
            paras.Add(0);
            paras.Add(1);
            paras.Sort();
            return paras;
        }

        /// <summary>
        ///support method for GradientDivide. Get condition at end of space.
        /// </summary>
        private static List<double> GetEndDivisions(double start, double end, double distMin, double distMax, int minBuffer, double f)
        {
            if (minBuffer <= 0) minBuffer = 1;

            var paras = new List<double>();

            double currentStep = start;
            int i = 0;

            if (end == 0) //means going in reverse to start
            {
                while (currentStep > end)
                {

                    if (i < minBuffer)
                    {
                        currentStep -= distMin;

                    }
                    else
                    {
                        double stepIncrease = GetScalarStep(distMin, 1 + f, i - minBuffer);

                        if (stepIncrease < distMax)
                        {
                            currentStep -= stepIncrease;
                        }
                        else
                        {
                            currentStep -= distMax;
                        }
                    }

                    if (currentStep > 0) paras.Add(currentStep);
                    i++;
                }
            }
            else if (end == 1) //means going forward
            {
                while (currentStep < end)
                {

                    if (i < minBuffer)
                    {
                        currentStep += distMin;

                    }
                    else
                    {
                        double stepIncrease = GetScalarStep(distMin, 1 + f, i - minBuffer);

                        if (stepIncrease < distMax)
                        {
                            currentStep += stepIncrease;
                        }
                        else
                        {
                            currentStep += distMax;
                        }
                    }

                    if (currentStep < 1) paras.Add(currentStep);
                    i++;
                }
            }

            paras.Sort();
            return paras;
        }

        /// <summary>
        ///support method for GradientDivide. Get condition between two attractors.
        /// </summary>
        private static List<double> GetCenterDivisions(double start, double end, double distMin, double distMax, int minBuffer, double f)
        {

            if (minBuffer <= 0) minBuffer = 1;

            var paras = new List<double>();

            double range = end - start;
            double center = start + range / 2;

            double currentStep = start;
            int i = 0;

            //start to center
            while (currentStep < center)
            {
                if (i < minBuffer)
                {
                    currentStep += distMin;

                }
                else
                {
                    double stepIncrease = GetScalarStep(distMin, 1 + f, i - (minBuffer + 1));
                    if (stepIncrease < distMax)
                    {
                        currentStep += stepIncrease;
                    }
                    else
                    {
                        currentStep += distMax;
                    }
                }

                if (currentStep < center) paras.Add(currentStep);
                i += 1;
            }



            //end to center
            currentStep = end;
            i = 0;
            while (currentStep > center)
            {
                if (i < minBuffer)
                {
                    currentStep -= distMin;

                }
                else
                {
                    double stepIncrease = GetScalarStep(distMin, 1 + f, i - (minBuffer + 1));
                    if (stepIncrease < distMax)
                    {
                        currentStep -= stepIncrease;
                    }
                    else
                    {
                        currentStep -= distMax;
                    }
                }

                if (currentStep > center) paras.Add(currentStep);
                i += 1;
            }


            paras.Sort();

            return paras;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="distMin"></param>
        /// <param name="factor"></param>
        /// <param name="steps"></param>
        /// <returns></returns>
        private static double GetScalarStep(double distMin, double factor, int steps)
        {
            return distMin * Math.Pow(factor, steps);
        }

    }
}
