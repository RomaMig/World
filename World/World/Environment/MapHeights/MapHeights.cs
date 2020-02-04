using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    abstract class MapHeights
    {
        public readonly int Width;
        public readonly int Height;
        protected double[,] map;
        protected double max, min;
        protected double defMax, defMin;

        public double this[int i, int j]
        {
            get
            {
                if (i >= 0 && j >= 0 && i < Width && j < Height)
                    return map[i, j];
                else
                    return 0;
            }
            set
            {
                if (i >= 0 && j >= 0 && i < Width && j < Height)
                    map[i, j] = value;
            }
        }

        public MapHeights(int width, int height)
        {
            Width = width;
            Height = height;
            map = new double[Width, Height];
        }

        public static Noise createNoise(int w, int h, double step, int scale, int numOctaves)
        {
            Noise heights = new Noise(w / scale, h / scale, 1, step / scale, numOctaves);
            return heights;
        }

        public static DiamondSquare createDiamondSquare(int w, int h, double roug)
        {
            DiamondSquare heights = new DiamondSquare(w * Form1.CELL_SIZE, h * Form1.CELL_SIZE, roug);
            return heights;
        }

        public static Union createUnion(Union.Rule rules, params MapHeights[] maps)
        {
            int maxH = -1, maxW = -1;
            for (int i = 0; i < maps.Length; i++)
            {
                maps[i].normalize(-1, 1);
                maxH = Math.Max(maxH, maps[i].Height);
                maxW = Math.Max(maxW, maps[i].Width);
            }
            return new Union(maxW, maxH, rules, maps);
            
        }

        public void postprocessing()
        {
            Smooth(0.4, 1);
            Smooth(-.3, -.1, 2);
            Loosening(0.4, 1, 0.08, 2);
            Loosening(0.7, 1, 0.3, 4);
            Smooth(0.4, 1, 1);
        }

        public void multiply(int power)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    map[i, j] = Math.Pow(map[i, j], power);
                }
            }
        }

        public void normalize()
        {
            if (max == defMax && min == defMin)
            {
                return;
            }
            normalize(defMin, defMax);
        }

        public void normalize(double from, double to)
        {
            double range = (max - min) / 2;
            double center = min + range;
            double newRange = (to - from) / 2;
            double newCenter = from + newRange;

            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    map[i, j] = (map[i, j] - center) / range * newRange + newCenter;
                }
            }
            max = to;
            min = from;
        }

        public void normalizeWithExtention(double from, double to)
        {
            UpdateMaxMin();
            normalize(from, to);
        }
        /*
         * powExt = 0 - сжимает шум до 0;
         * powExt = 1 - растягивает шум до предельных значений max и min
         */
        public void normalizeWithExtention(double from, double to, double powExt)
        {
            double tmax = max;
            double tmin = min;

            Scale(powExt);
            max = tmax;
            min = tmin;
            normalize(from, to);
        }

        protected void normalizeAfteChanges()
        {
            double tmax = max;
            double tmin = min;

            UpdateMaxMin();
            if (max <= tmax && min >= tmin)
            {
                max = tmax;
                min = tmin;
                return;
            }
            else
            {
                if (max - min <= tmax - tmin)
                {
                    normalize(tmin, tmax);
                }
                else
                {
                    normalizeWithExtention(tmin, tmax);
                }
            }
        }

        public void Scale(double scale)
        {
            double range = (max - min) / 2;
            double center = min + range;
            normalizeWithExtention(center - range * scale, center + range * scale);
        }

        private void UpdateMaxMin()
        {
            max = min = map[0, 0];
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (min > map[i, j]) min = map[i, j];
                    if (max < map[i, j]) max = map[i, j];
                }
            }
        }

        public void Loosening(double range, double roug, int power)
        {
            Loosening(-range, range, roug, power);
        }
        public void Loosening(double l, double r, double roug, int power)
        {
            Random rnd = new Random();
            for (int p = 0; p < power; p++)
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        if (map[i, j] > l && map[i, j] < r)
                        {
                            map[i, j] += (rnd.NextDouble() - 0.4999) * roug;
                        }
                    }
                }
            }
            normalizeAfteChanges();
        }

        public void Smooth(double range, int power)
        {
            Smooth(-range, range, power);
        }

        public void Smooth(double l, double r, int power)
        {
            for (int p = 0; p < power; p++)
            {
                for (int i = 0; i < Width; i++)
                {
                    for (int j = 0; j < Height; j++)
                    {
                        if (map[i, j] > l && map[i, j] < r)
                        {
                            double sum = 0;
                            int count = 0;
                            for (int n = -1; n < 2; n++)
                            {
                                for (int m = -1; m < 2; m++)
                                {
                                    int k = i + n, s = j + m;
                                    if (k >= 0 && s >= 0 && k < Width && s < Height)
                                    {
                                        sum += map[k, s];
                                        count++;
                                    }
                                }
                            }
                            map[i, j] = sum / count;
                        }
                    }
                }
            }
        }
    }
}
