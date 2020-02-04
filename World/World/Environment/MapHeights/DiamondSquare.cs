using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    class DiamondSquare : MapHeights
    {
        private double[,] grid;
        private int size;
        private double roug;
        private Random rnd;

        public DiamondSquare(int width, int height, double roug) : base(width, height)
        {
            int powWid = (int)Math.Ceiling(Math.Log(width, 2));
            int powHei = (int)Math.Ceiling(Math.Log(height, 2));
            this.roug = roug;

            int w = (int)Math.Pow(2, powWid) + 1;
            int h = (int)Math.Pow(2, powHei) + 1;
            size = Math.Max(w, h);
            grid = new double[size, size];

            rnd = new Random();
            Generate();
        }

        private void Generate()
        {
            grid[0, 0] = rnd.NextDouble();
            grid[0, size - 1] = rnd.NextDouble();
            grid[size - 1, 0] = rnd.NextDouble();
            grid[size - 1, size - 1] = rnd.NextDouble();
            for (int i = size - 1; i > 0; i /= 2)
            {
                SquareStep(i);
                DiamondStep(i);
            }
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    map[i, j] = grid[i + 1, j + 1];
                }
            }
            //multiply(2);
            normalizeWithExtention(-1, 1);
        }

        private void SquareStep(int s)
        {
            for (int i = 0; i < size - 1; i += s)
            {
                for (int j = 0; j < size - 1; j += s)
                {
                    int offset = s / 2;
                    grid[i + offset, j + offset] = newHeight(s,
                        grid[i, j],
                        grid[i + s, j],
                        grid[i, j + s],
                        grid[i + s, j + s]);

                }
            }
        }

        private void DiamondStep(int s)
        {
            int offset = s / 2;
            double img;

            for (int i = 0; i < size - 1; i += s)
            {
                for (int j = 0; j < size - 1; j += s)
                {
                    if (j - offset < 0) img = 0;
                    else img = grid[i + offset, j - offset];
                    grid[i + offset, j] = newHeight(s,
                        img,
                        grid[i, j],
                        grid[i + s, j],
                        grid[i + offset, j + offset]);

                    if (i - offset < 0) img = 0;
                    else img = grid[i - offset, j + offset];
                    grid[i, j + offset] = newHeight(s,
                        grid[i, j],
                        img,
                        grid[i + offset, j + offset],
                        grid[i, j + s]);
                    /*
                    if (i + s + offset >= size) img = 0;
                    else img = grid[i + s + offset, j + offset];
                    grid[i + s, j + offset] = newHeight(s,
                        grid[i + s, j],
                        grid[i + offset, j + offset],
                        img,
                        grid[i + s, j + s]);

                    if (j + s + offset >= size) img = 0;
                    else img = grid[i + offset, j + s + offset];
                    grid[i + offset, j] = newHeight(s,
                        grid[i + offset, j + offset],
                        grid[i, j + s],
                        grid[i + s, j + s],
                        img);
                        */
                }
            }
        }

        private double newHeight(int l, params double[] heights)
        {
            double sum = 0;
            for (int i = 0; i < heights.Length; i++)
            {
                sum += heights[i];
            }
            sum /= heights.Length;
            sum += (rnd.NextDouble() * 2 - 1) * roug * l;
            return sum;
        }
    }
}
