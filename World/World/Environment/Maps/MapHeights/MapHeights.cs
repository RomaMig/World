using System;
using System.Collections.Generic;
using System.Drawing;
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
        public double[,] Map 
        {
            get
            {
                return map;
            }
        }
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
        public delegate MapHeights Creates(int w, int h);
        public static Creates[] creates = {
            createContinent,
            createArchipelago,
            createMountains,
            createHills,
            createPlains,
            createSeabed,
            createLandscape
        };
        public MapHeights(int width, int height)
        {
            Width = width;
            Height = height;
            map = new double[Width, Height];
        }

        public static PerlinNoise createPerlinNoise(int w, int h, double step, int scale, int numOctaves)
        {
            PerlinNoise heights = new PerlinNoise(w / scale, h / scale, 1, step / scale, numOctaves);
            return heights;
        }

        public static DiamondSquare createDiamondSquare(int w, int h, double roug)
        {
            DiamondSquare heights = new DiamondSquare(w * Form1.CELL_SIZE, h * Form1.CELL_SIZE, roug);
            return heights;
        }

        public static Union createUnion(bool isNormal, Union.Rule rules, params MapHeights[] maps)
        {
            int maxH = -1, maxW = -1;
            for (int i = 0; i < maps.Length; i++)
            {
                maxW = Math.Max(maxW, maps[i].Width);
                maxH = Math.Max(maxH, maps[i].Height);
            }
            return new Union(maxW, maxH, isNormal, rules, maps);
            
        }

        public static MapHeights createMountains(int w, int h)
        {
            MapHeights perlin = MapHeights.createPerlinNoise(w, h, (double)((int)(100000f / Form1.CELL_SIZE)) / 100000, 10, 25);
            MapHeights diamond = MapHeights.createDiamondSquare(w, h, .9);
            perlin.normalizeWithExtention(0, 1, .95);
            diamond.normalizeWithExtention(-.1, .4, .95);
            MapHeights union = createUnion(
                true,
                (int i, int j, MapHeights[] maps) =>
                {
                    return maps[0][i, j] + maps[1][i, j] * (maps[0][i, j] + 1.1) / 2.1f;
                },
                perlin,
                diamond);
            union.SimpleTransform(1);
            union.Loosening(0.4, 1, 0.03, 1);
            union.Loosening(0.7, 1, 0.08, 2);
            union.Smooth(0.4, 1, 1);
            union.normalizeWithExtention(-.5, 1, .97);
            return union;
        }

        public static MapHeights createHills(int w, int h)
        {
            MapHeights perlin = MapHeights.createPerlinNoise(w, h, (double)((int)(100000f / Form1.CELL_SIZE)) / 100000f, 5, 35);
            perlin.SimpleTransform(1);
            perlin.Smooth(0, .1, 8);
            perlin.Smooth(0, .2, 2);
            perlin.normalizeWithExtention(-.1, .38, .9);
            return perlin;
        }

        public static MapHeights createPlains(int w, int h)
        {
            MapHeights perlin = MapHeights.createPerlinNoise(w, h, (double)((int)(100000f / Form1.CELL_SIZE)) / 100000f, 10, 2);
            perlin.normalize(-.2, .3);
            perlin.Smooth(-.2, .3, 7);
            perlin.Smooth(.1, .3, 5);
            perlin.Smooth(-.2, .1, 3);
            perlin.Loosening(-.2, .3, 0.009, 1);
            return perlin;
        }

        public static MapHeights createSeabed(int w, int h)
        {
            MapHeights perlin = MapHeights.createPerlinNoise(w, h, (double)((int)(100000f / Form1.CELL_SIZE)) / 100000f, 20, 2);
            perlin.normalize(-1, -.6);
            perlin.Smooth(-1, -.6, 8);
            perlin.Smooth(-1, -.8, 5);
            return perlin;
        }

        public static MapHeights createLandscape(int w, int h)
        {
            MapHeights landscape;
            MapHeights mountains = createMountains(w, h);
            MapHeights hills = createHills(w, h);
            int[,] heights = Utilites.DFS<double>(mountains.Map, (double a) =>
                {
                    return a < .3;
                });
            mountains = createUnion(
                false,
                (int i, int j, MapHeights[] maps) =>
                {
                    if (heights[i, j] > 0)
                    {
                        return (maps[0][i, j] * (.3 - maps[1][i, j]) + maps[1][i, j] * (.5 + maps[1][i, j])) * 10f / 8f;
                    }
                    else
                    {
                        return maps[1][i, j];
                    }
                }, hills, mountains);
            Bitmap layoutM = new Bitmap(Bitmap.FromFile(@"C:\Users\Роман\source\repos\World\World\images\layout_mountains.bmp"), mountains.Width, mountains.Height);
            MapHeights plains = createPlains(w, h);
            landscape = createUnion(
                false,
                (int i, int j, MapHeights[] maps) =>
                {
                    double intensive = (layoutM.GetPixel(i, j).R / 255f);
                    return maps[0][i, j] * intensive + maps[1][i, j] * (1 - intensive);
                },
                plains, mountains);
            MapHeights seabed = createSeabed(w, h);
            Bitmap layoutS = new Bitmap(Bitmap.FromFile(@"C:\Users\Роман\source\repos\World\World\images\layout_sea.bmp"), seabed.Width, seabed.Height);
            landscape = createUnion(
                false,
                (int i, int j, MapHeights[] maps) =>
                {
                    double intensive = 1 - (layoutS.GetPixel(i, j).R / 255f);
                    return maps[0][i, j] * intensive + maps[1][i, j] * (1 - intensive);
                },
                seabed, landscape);
            landscape.max = 1;
            landscape.min = -1;
            
            //landscape.postprocessing();
            return landscape;
        }

        public static MapHeights createContinent(int w, int h)
        {
            MapHeights m1 = MapHeights.createDiamondSquare(w, h, 0.7);
            MapHeights m2 = MapHeights.createPerlinNoise(w, h, (double)((int)(100000f / Form1.CELL_SIZE)) / 100000, 6, 15);
            MapHeights heights = MapHeights.createUnion(
                true,
                (int i, int j, MapHeights[] m) =>
                {
                    return m[0][i, j] + m[1][i, j] * (1 + m[0][i, j]) / 2;
                },
                m1,
                m2);
            heights.postprocessing();
            heights.normalizeWithExtention(-1, 1, 0.98);
            return heights;
        }

        public static MapHeights createArchipelago(int w, int h)
        {
            MapHeights m1 = MapHeights.createDiamondSquare(w, h, 0.08);
            MapHeights m2 = MapHeights.createPerlinNoise(w, h, (double)((int)(100000f / Form1.CELL_SIZE)) / 100000, 5, 9);
            MapHeights heights = MapHeights.createUnion(
                true,
                (int i, int j, MapHeights[] m) =>
                {
                    return m[0][i, j] + m[1][i, j] * (1 + m[0][i, j]) / 1.9f;
                },
                m2,
                m1);
            heights.postprocessing();
            heights.normalizeWithExtention(-1, 1, 0.98);
            return heights;
        }

        public void SimpleTransform(double pow)
        {
            normalize(0, 1);
            Smooth(0, .1, (int)Math.Round(4 * pow));
            Smooth(0, .3, (int)Math.Round(3 * pow));
            Loosening(.31, 1, .008 * pow, 2);
            multiply(pow > .55 ? .55 : .95);
            Smooth(0, .2, (int)Math.Round(2 * pow));
        }

        public void postprocessing()
        {
            Smooth(-.4, 1);
            Smooth(-.3, -.1, 2);
            Loosening(0.4, 1, 0.05, 1);
            Loosening(0.7, 1, 0.15, 2);
            Smooth(0.4, 1, 1);
        }

        public void multiply(double power)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    map[i, j] = Math.Pow(map[i, j], power);
                }
            }
        }

        public void sqrt()
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    map[i, j] = Math.Sqrt(map[i, j]);
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
            if (max == min && max == 0)
                UpdateMaxMin();
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
