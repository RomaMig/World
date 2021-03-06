﻿using System;
using System.Drawing;

namespace World
{
    class Noise : MapHeights
    {
        private FloatVector[,] grid;
        private readonly Size size;
        private readonly double step;
        private readonly int resolution;

        public Noise(int width, int height, int resolution, double step) : base((int)(width / step), (int)(height / step))
        {
            size = new Size(width, height);
            this.step = step;
            this.resolution = resolution;
            max = defMax = .5 * Math.Sqrt(2) / resolution;
            min = defMin = -.5 * Math.Sqrt(2) / resolution;
            int w = width * resolution + 1;
            int h = height * resolution + 1;
            grid = new FloatVector[w, h];
            Random random = new Random();
            for (var i = 0; i < w; i++)
            {
                for (var j = 0; j < h; j++)
                {
                    double angle = random.NextDouble();
                    grid[i, j] = new FloatVector(Math.Cos(angle * 2 * Math.PI), Math.Sin(angle * 2 * Math.PI));
                }
            }
            Generate();
        }

        public Noise(int width, int height, int resolution, double step, int numOctaves) : this(width, height, resolution, step)
        {
            upOctaveTo(numOctaves);
        }

        private void Generate()
        {
            for (double x = 0; (int)Math.Round(x / step) < Width; x += step)
            {
                for (double y = 0; (int)Math.Round(y / step) < Height; y += step)
                {
                    x = ((double)((int)(10000 * x + 0.0001))) / 10000;
                    y = ((double)((int)(10000 * y + 0.0001))) / 10000;
                    double x0 = ((double)((int)(x * resolution))) / resolution;
                    double dx = (x - x0) * resolution;
                    double y0 = ((double)((int)(y * resolution))) / resolution;
                    double dy = (y - y0) * resolution;

                    int xg = (int)(x0 * resolution);
                    int yg = (int)(y0 * resolution);

                    var vx0y0 = grid[xg, yg];
                    var vx0y1 = grid[xg, yg + 1];
                    var vx1y0 = grid[xg + 1, yg];
                    var vx1y1 = grid[xg + 1, yg + 1];
                    var s = MultiplyVectors(vx0y0,
                        new FloatVector(dx, dy));
                    var t = MultiplyVectors(vx1y0,
                        new FloatVector(dx - 1, dy));
                    var u = MultiplyVectors(vx0y1,
                        new FloatVector(dx, dy - 1));
                    var v = MultiplyVectors(vx1y1,
                        new FloatVector(dx - 1, dy - 1));

                    double a = getWeightedAverage(s, t, dx);
                    double b = getWeightedAverage(u, v, dx);
                    double c = getWeightedAverage(a, b, dy);

                    map[(int)Math.Round(x / step), (int)Math.Round(y / step)] = c / Math.Pow(2, resolution);
                }
            }
        }

        private double MultiplyVectors(FloatVector v1, FloatVector v2)
        {
            return v1.X * v2.X + v1.Y * v2.Y;
        }

        private double getWeightedAverage(double a, double b, double weight)
        {
            return a + Fade(weight) * (b - a);
        }

        private double Fade(double t)
        {
            return t * t * t * (6 * t * t - 15 * t + 10);
        }

        public void upOctaveTo(int numOctaves)
        {
            double num = Math.Pow(2, numOctaves);
            for (int resolution = 2; resolution <= num && (1f / resolution) >= step; resolution *= 2)
            {
                upOctave(new Noise(size.Width, size.Height, resolution, step));
            }
        }

        private void upOctave(Noise noise)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    map[i, j] += noise[i, j];
                }
            }
        }

        public static void PrintMap(Noise noise)
        {
            char[] symbol = { ' ', '.', '`', '-', '^', '~', '+', '>', 'l', '1', 'I', '3', '9', '8', '0', 'Q' };
            noise.normalize(0, symbol.Length - 1);
            for (int i = 0; i < noise.Width; i++)
            {
                for (int j = 0; j < noise.Height; j++)
                {
                    Console.Write(symbol[(int)noise[i, j]]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        class FloatVector
        {
            public double X { get; set; }
            public double Y { get; set; }
            public FloatVector() { }
            public FloatVector(double x, double y)
            {
                this.X = x;
                this.Y = y;
            }
        }
    }
}