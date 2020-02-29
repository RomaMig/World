using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Utilite;

namespace World.Environment.Map
{
    class MapNormals : Maps<Vector3>
    {
        public MapNormals(int width, int height) : base(width, height)
        {

        }

        public MapNormals(int width, int height, double[,] heights) : this(width, height)
        {
            Build(heights);
        }

        public MapNormals(MapHeights heights) : this(heights.Width, heights.Height, heights.Map)
        {

        }

        public void Build(double[,] heights)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    this[i, j] = Vector3.Cross(xVector(i, j, heights), yVector(i, j, heights)).normalize();
                }
            }
        }

        private Vector3 xVector(int i, int j, double[,] heights)
        {
            int li = i - 1;
            int ri = i + 1;
            if (li < 0) li = 0;
            if (ri > Width - 1) ri = Width - 1;
            return new Vector3(1f / 63.75f, 0, (float)(heights[ri, j] - heights[li, j]));
        }
        private Vector3 yVector(int i, int j, double[,] heights)
        {
            int tj = j - 1;
            int bj = j + 1;
            if (tj < 0) tj = 0;
            if (bj > Height - 1) bj = Height - 1;
            return new Vector3(0, 1f / 63.75f, (float)(heights[i, bj] - heights[i, tj]));
        }
    }
}
