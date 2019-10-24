using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.Dynamics
{
    public class Bin2d<T>
    {
        private int _numCellsX;
        private int _numCellsY;
        private double _cellSize;
        private T[,] _cells;

        /// <summary>
        /// 
        /// </summary>
        public Bin2d(int numCellsX, int numCellsY, double cellSize)
        {
            this._numCellsX = numCellsX;
            this._numCellsY = numCellsY;
            this._cellSize = cellSize;

            InitGrid();
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitGrid()
        {
            for (int x = 0; x < _numCellsX; x++)
            {
                for (int y = 0; y < _numCellsY; y++)
                {
                    _cells[x, y] = default(T);
                }
            }
        }
    }
}
