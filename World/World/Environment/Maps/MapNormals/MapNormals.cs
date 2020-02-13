using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    class MapNormals
    {
        private int width;
        private int height;
        private Vector3[,] map;
        public Vector3 this[int i, int j]
        {
            get
            {
                return map[i, j];
            }
            set
            {
                map[i, j] = value;
            }
        }

        public MapNormals(int width, int height)
        {
            this.width = width;
            this.height = height;
            map = new Vector3[width, height];
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
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    map[i, j] = Vector3.Cross(xVector(i, j, heights), yVector(i, j, heights)).normalize();
                }
            }
        }

        private Vector3 xVector(int i, int j, double[,] heights)
        {
            int li = i - 1;
            int ri = i + 1;
            if (li < 0) li = 0;
            if (ri > width - 1) ri = width - 1;
            return new Vector3(1f / 63.75f, 0, (float)(heights[ri, j] - heights[li, j]));
        }
        private Vector3 yVector(int i, int j, double[,] heights)
        {
            int tj = j - 1;
            int bj = j + 1;
            if (tj < 0) tj = 0;
            if (bj > height - 1) bj = height - 1;
            return new Vector3(0, 1f / 63.75f, (float)(heights[i, bj] - heights[i, tj]));
        }
    }

    struct Vector3
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
        public float Lenght { get; set; }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            Lenght = (float)Math.Sqrt(x * x + y * y + z * z);
        }

        public Vector3 normalize()
        {
            X /= Lenght;
            Y /= Lenght;
            Z /= Lenght;
            Lenght = 1;
            return this;
        }

        public bool isNormal()
        {
            return Lenght == 1;
        }

        public static Vector3 Cross(Vector3 v1, Vector3 v2)
        {
            float x = v1.Y * v2.Z - v1.Z * v2.Y;
            float y = v1.Z * v2.X - v1.X * v2.Z;
            float z = v1.X * v2.Y - v1.Y * v2.X;
            return new Vector3(x, y, z);
        }
    }
}
