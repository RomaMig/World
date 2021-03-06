﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;

namespace World
{
    class World : Maps<Cell>, ITask
    {
        public Control Control { get; set; }
        public Camera Camera { get; set; }
        public Light Light { get; }
        public bool Runnable { get; set; }
        public Task Task { get; set; }

        public World(Control control, Size size, Camera camera) : base(size.Width, size.Height)
        {
            Control = control;
            Camera = camera;
            Light = new Light();
        }

        public void Generate(ProgressBar progress, int type)
        {
            Stop();
            //progress.Value = 40;

            MapHeights heights = MapHeights.creates[type](Width, Height);
            //progress.Value = 70;

            int[,] distribution = Utilites.DFS<double>(heights.Map,
                    (double a) =>
                    {
                        return a < Water.seaLevel;
                    });
            //progress.Value = 80;

            MapNormals normals = new MapNormals(heights);
            //progress.Value = 90;

            initLight();
            //progress.Value = 92;

            initCells(new Size(Form1.CELL_SIZE, Form1.CELL_SIZE), heights, normals, Light, distribution);
            Light.FirstPass();
            //progress.Value = 95;
        }

        public void Start()
        {
            if (Light != null)
                Light.Start();
            Runnable = true;
            Task = Task.Run(() =>
                {
                    Stopwatch watch = new Stopwatch();
                    while (Runnable)
                    {
                        watch.Restart();
                        if (Light.readyToUpdate)
                        {
                            Light.readyToUpdate = false;
                            Task t = Light.Update();
                            t.Wait();
                        }
                        Camera.newImage();
                        watch.Stop();
                        //Thread.Sleep(Math.Max(0, (int)(15 - watch.ElapsedMilliseconds)));
                        Control.Text = (1000 / watch.ElapsedMilliseconds) + "";
                        Control.Invalidate();
                    }
                });
        }

        public void Stop()
        {
            if (Light != null)
                Light.Stop();
            Runnable = false;
        }

        private void initLight()
        {
            Light.Clear();
            DinamicLight dl = new DinamicLight(
                new Point3(200, 200, 1000),
                new Vector3(1, 1, -8).normalize(),
                Color.FromArgb(255, 250, 200),
                .65f,
                LightSource.FieldForm.UNIFORM);
            dl.AddBehavior(
                () =>
                {
                    Vector3 tmp = dl.Direction;
                    tmp.Rotate((float)(Math.PI / 720f), (float)(Math.PI / 360f), 0);
                    dl.Direction = tmp.normalize();
                }, 250);/*
            dl.AddBehavior(
                () =>
                {
                    Point3 tmp = dl.Location;
                    tmp.Z--;
                    dl.Location = tmp;
                }, 20);*/
             LightSource ls = new LightSource(
                new Point3(200, 200, 25),
                new Vector3(0, 0, -1).normalize(),
                Color.FromArgb(120, 125, 250),
                .35f,
                LightSource.FieldForm.UNIFORM);
            Light.AddSources(dl, ls);
        }

        private void initCells(Size cellSize, MapHeights heights, MapNormals normals, Light light, int[,] distribution)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    if (Map[i, j] == null)
                        Map[i, j] = new Cell(i, j, cellSize);
                    for (int k = 0; k < cellSize.Width; k++)
                    {
                        for (int s = 0; s < cellSize.Height; s++)
                        {
                            int x = Map[i, j].Location.X + k;
                            int y = Map[i, j].Location.Y + s;
                            int cx = i * cellSize.Width + k;
                            int cy = j * cellSize.Height + s;
                            switch (distribution[cx, cy] == 0 ? Cell.Element.GROUND : Cell.Element.WATER)
                            {
                                case Cell.Element.WATER:
                                    Map[i, j][k, s] =
                   new Cell.Property(
                       new Water(x, y, heights[cx, cy]),
                       new Normal(x, y, normals[cx, cy]));
                                    break;
                                case Cell.Element.GROUND:
                                    Map[i, j][k, s] =
                  new Cell.Property(
                      new Ground(x, y, heights[cx, cy], normals[cx, cy]),
                      new Normal(x, y, normals[cx, cy]));
                                    break;
                            }
                            if (Map[i, j][k, s].Height is IBrightness)
                                light.AddItems((IBrightness)Map[i, j][k, s].Height);
                        }
                    }
                }
            }
        }
    }
}
