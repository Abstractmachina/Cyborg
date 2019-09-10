using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Cyborg.Dynamics;

namespace Cyborg.DifferentialGrowth
{
    class Grid
    {
        private int numCellsX;
        private int numCellsY;
        private double cellSize;
        private Particle[,] cells;

        /// <summary>
        /// 
        /// </summary>
        public Grid(int numCellsX, int numCellsY, double cellSize)
        {
            this.numCellsX = numCellsX;
            this.numCellsY = numCellsY;
            this.cellSize = cellSize;

            initGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        private void initGrid()
        {
            for (int x = 0; x < numCellsX; x++)
            {
                for (int y = 0; y < numCellsY; y++)
                {
                    cells[x, y] = null;
                }
            }
        }
    }
}
