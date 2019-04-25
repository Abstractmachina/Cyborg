using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cyborg.ReactionDiffusion
{
    public class ReactionDiffuser
    {
        //world holds no geometry. pure relative location + chemical ratio
        private Cell[,] grid;
        private Cell[,] nextGrid;

        private int xRes, yRes;
        private int counter;

        public int XRes
        {
            get { return xRes; }
            set
            {
                if (value < 2) throw new Exception("Value must be > 2");
                else xRes = value;
            }
        }
        public int YRes
        {
            get { return yRes; }
            set
            {
                if (value < 2) throw new Exception("Value must be > 2");
                else yRes = value;
            }
        }

        //diffusion parameters

        private double dA = 1;
        private double dB = 0.3;
        private double feed = 0.055;
        private double kill = 0.062;

        public double DA
        {
            get { return dA; }
            set { dA = value; }
        }
        public double DB
        {
            get { return dB; }
            set { dB = value; }
        }

        public double Feed
        {
            get { return feed; }
            set { feed = value; }
        }

        public double Kill
        {
            get { return kill; }
            set { kill = value; }
        }


        

        public ReactionDiffuser(int xRes, int yRes, double dA, double dB, double feed, double kill)
        {
            XRes = xRes;
            YRes = yRes;
            this.dA = dA;
            this.dB = dB;
            this.feed = feed;
            this.kill = kill;
            CreateGrid();
            nextGrid = grid;
        }

        private void CreateGrid()
        {
            grid = new Cell[xRes, yRes];
            for (int i = 0; i < xRes; i++)
            {
                for (int j = 0; j < yRes; j++)
                {
                    double ca = 1;
                    double cb = 0;
                    Cell c = new Cell(ca, cb);
                    grid[i, j] = c;
                }
            }
        }

        public void Update(double deltaTime)
        {
            counter++;
            ReactionDiffuse(deltaTime);
            SwapGrid();
        }

        private void SwapGrid()
        {
            Cell[,] tempGrid = grid;
            grid = nextGrid;
            nextGrid = tempGrid;
        }

        private void ReactionDiffuse(double deltaTime)
        {
            for (int x = 1; x < nextGrid.GetLength(0) - 1; x++)
            {
                for (int y = 1; y < nextGrid.GetLength(1) - 1; y++)
                {
                    double a = grid[x, y].A;
                    double b = grid[x, y].B;

                    double rA = a + dA * LaPlaceA(x, y) - a * b * b + feed * (1 - a) * deltaTime;
                    double rB = b + dB * LaPlaceB(x, y) + a * b * b - (kill + feed) * b * deltaTime;

                    rA = Util.Constrain(rA, 0, 1d);
                    rB = Util.Constrain(rB, 0, 1d);

                    nextGrid[x, y].A = rA;
                    nextGrid[x, y].B = rB;
                }
            }
        }

        private double LaPlaceA(int x, int y)
        {
            double sumA = 0;
            sumA += grid[x, y].A * -1;
            sumA += grid[x - 1, y].A * 0.2;
            sumA += grid[x + 1, y].A * 0.2;
            sumA += grid[x, y + 1].A * 0.2;
            sumA += grid[x, y - 1].A * 0.2;
            sumA += grid[x - 1, y - 1].A * 0.05;
            sumA += grid[x + 1, y - 1].A * 0.05;
            sumA += grid[x + 1, y + 1].A * 0.05;
            sumA += grid[x - 1, y + 1].A * 0.05;
            return sumA;
        }
        private double LaPlaceB(int x, int y)
        {
            double sumB = 0;
            sumB += grid[x, y].B * -1;
            sumB += grid[x - 1, y].B * 0.2;
            sumB += grid[x + 1, y].B * 0.2;
            sumB += grid[x, y + 1].B * 0.2;
            sumB += grid[x, y - 1].B * 0.2;
            sumB += grid[x - 1, y - 1].B * 0.05;
            sumB += grid[x + 1, y - 1].B * 0.05;
            sumB += grid[x + 1, y + 1].B * 0.05;
            sumB += grid[x - 1, y + 1].B * 0.05;
            return sumB;
        }


        /*
        [Obsolete()]
        public void Seed(int xMin, int xMax, int yMin, int yMax)
        {
            for (int i = xMin; i < xMax; i++)
            {
                for (int j = yMin; j < yMax; j++)
                {
                    grid[i, j].A = 0;
                    grid[i, j].B = 1;
                }
            }
        }
        */
    }
}
