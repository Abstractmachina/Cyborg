using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Params
{
    public static class SetOperations
    {

        public static bool BuildDiagrid <T> (T[][] inputMatrix, bool flip, out List<List<T>>diagrid)
        {
            var groups = new List<List<T>>();

            int count0 = inputMatrix.GetLength(0);
            int count1 = inputMatrix.GetLength(1);

            //test for same length 
            for (int i = 0; i < count0; i++)
            {
                if (inputMatrix[i].Length != count1) 
                {
                    diagrid = new List<List<T>>();
                    return false; }
            }



            //full panels
            for (int i = 1; i < count0 - 1; i++)
            {
                var branch = inputMatrix[i];
                var group = new List<T>();
                if (i % 2 == 0)
                {
                    for (int j = 1; j < count1 - 2; j += 2)
                    {
                        group.Add(branch[j]);
                        group.Add(inputMatrix[i + 1][j + 1]);
                        group.Add(branch[j + 2]);
                        group.Add(inputMatrix[i - 1][j + 1]);
                    }
                }
                else
                {
                    for (int j = 0; j < count1 - 2; j += 2)
                    {
                        group.Add(branch[j]);
                        group.Add(inputMatrix[i + 1][j + 1]);
                        group.Add(branch[j + 2]);
                        group.Add(inputMatrix[i - 1][j + 1]);
                    }
                }

                groups.Add(group);
            }

            //half panels
            //columns
            var col1 = inputMatrix[0];
            for (int j = 1; j < count1 - 2; j += 2)
            {
                var group = new List<T>();
                group.Add(col1[j]);
                group.Add(inputMatrix[1][j + 1]);
                group.Add(col1[j + 2]);
                groups.Add(group);
            }

            //if uneven number of columns
            if (count0 % 2 != 0)
            {
                var colLast = inputMatrix[count0 - 1];
                for (int j = 1; j < count1 - 2; j += 2)
                {
                    var group = new List<T>();
                    group.Add(colLast[j]);
                    group.Add(inputMatrix[count0 - 2][j + 1]);
                    group.Add(colLast[j + 2]);
                    groups.Add(group);
                }
            }
            else
            {
                var colLast = inputMatrix[count0 - 1];

                for (int j = 0; j < count1 - 2; j += 2)
                {
                    var group = new List<T>();
                    group.Add(colLast[j]);
                    group.Add(inputMatrix[count0 - 2][j + 1]);
                    group.Add(colLast[j + 2]);
                    groups.Add(group);
                }
            }

            //rows
            for (int i = 1; i < count0 - 2; i += 2)
            {
                var group = new List<T>();

                group.Add(inputMatrix[i][0]);
                group.Add(inputMatrix[i + 1][1]);
                group.Add(inputMatrix[i + 2][0]);

                groups.Add(group);
            }


            if ((count0 % 2 != 0 && count1 % 2 == 0) || (count0 % 2 == 0 && count1 % 2 == 0))
            {
                for (int i = 0; i < count0 - 2; i += 2)
                {
                    var group = new List<T>();
                    group.Add(inputMatrix[i][count1 - 1]);
                    group.Add(inputMatrix[i + 1][count1 - 2]);
                    group.Add(inputMatrix[i + 2][count1 - 1]);

                    groups.Add(group);
                }
            }
            else
            {
                for (int i = 1; i < count0 - 2; i += 2)
                {
                    var group = new List<T>();
                    group.Add(inputMatrix[i][count1 - 1]);
                    group.Add(inputMatrix[i + 1][count1 - 2]);
                    group.Add(inputMatrix[i + 2][count1 - 1]);

                    groups.Add(group);
                }

            }

            //quarter panels
            var quarterGroup = new List<T>();
            quarterGroup.Add(inputMatrix[0][0]);
            quarterGroup.Add(inputMatrix[1][0]);
            quarterGroup.Add(inputMatrix[0][1]);
            groups.Add(quarterGroup);


            if (count0 % 2 != 0)
            {
                var group = new List<T>();

                int lastI = count0 - 1;
                group.Add(inputMatrix[lastI][0]);
                group.Add(inputMatrix[lastI][1]);
                group.Add(inputMatrix[lastI - 1][0]);

                groups.Add(group);
            }

            if ((count0 % 2 != 0 && count1 % 2 != 0) || (count0 % 2 == 0 && count1 % 2 == 0))
            {
                var group = new List<T>();

                group.Add(inputMatrix[count0 - 1][count1 - 2]);
                group.Add(inputMatrix[count0 - 2][count1 - 1]);
                group.Add(inputMatrix[count0 - 1][count1 - 1]);

                groups.Add(group);
            }

            if (count1 % 2 != 0)
            {
                var group = new List<T>();

                group.Add(inputMatrix[0][count1 - 2]);
                group.Add(inputMatrix[0][count1 - 1]);
                group.Add(inputMatrix[1][count1 - 1]);

                groups.Add(group);
            }

            diagrid =  groups;
            return true;
        }

    }
}
