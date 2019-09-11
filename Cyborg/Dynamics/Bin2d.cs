using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics
{
    public class Bin2d<T>
    {
        private int numCellsX;
        private int numCellsY;
        private double cellSize;
        private T[,] cells;

        /// <summary>
        /// 
        /// </summary>
        public Bin2d(int numCellsX, int numCellsY, double cellSize)
        {
            this.numCellsX = numCellsX;
            this.numCellsY = numCellsY;
            this.cellSize = cellSize;

            InitGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitGrid()
        {
            for (int x = 0; x < numCellsX; x++)
            {
                for (int y = 0; y < numCellsY; y++)
                {
                    cells[x, y] = default(T);
                }
            }
        }
    }
}
