using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace World
{
    class World
    {
        public Size Size { get; set; }
        private Cell[,] cells;
        public Cell this[int i, int j]
        {
            get
            {
                return cells[i, j];
            }
        }

        public World(Size size)
        {
            Size = size;
            int w = size.Width;
            int h = size.Height;
            cells = new Cell[w, h];
        }

        public void Generate(ProgressBar progress, int type)
        {
            int w = Size.Width;
            int h = Size.Height;
            progress.Value = 40;
            MapHeights heights = MapHeights.creates[type](w, h);
            progress.Value = 70;
            int[,] distribution = Utilites.DFS<double>(heights.Map,
                    (double a) =>
                    {
                        return a < Water.seaLevel;
                    });
            progress.Value = 80;
            MapNormals normals = new MapNormals(heights);
            progress.Value = 90;
            Light light = new Light(
                new LightSource(
                    new Point3(200, 200, 100),
                    new Vector3(1, 1, -4).normalize(),
                    Color.White,
                    1f,
                    LightSource.FieldForm.UNIFORM));
            initCells(w, h, new Size(Form1.CELL_SIZE, Form1.CELL_SIZE), heights, normals, light, distribution);
            light.Start();
            progress.Value = 95;
        }

        private void initCells(int w, int h, Size cellSize, MapHeights heights, MapNormals normals, Light light, int[,] distribution)
        {
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (cells[i, j] == null)
                        cells[i, j] = new Cell(i, j, cellSize);
                    for (int k = 0; k < cellSize.Width; k++)
                    {
                        for (int s = 0; s < cellSize.Height; s++)
                        {
                            int x = cells[i, j].Location.X + k;
                            int y = cells[i, j].Location.Y + s;
                            int cx = i * cellSize.Width + k;
                            int cy = j * cellSize.Height + s;
                            switch (distribution[cx, cy] == 0 ? Cell.Element.GROUND : Cell.Element.WATER)
                            {
                                case Cell.Element.WATER:
                                    cells[i, j][k, s] =
                   new Cell.Property(
                       new Water(x, y, heights[cx, cy]),
                       new Normal(x, y, normals[cx, cy]));
                                    break;
                                case Cell.Element.GROUND:
                                    cells[i, j][k, s] =
                  new Cell.Property(
                      new Ground(x, y, heights[cx, cy], normals[cx, cy]),
                      new Normal(x, y, normals[cx, cy]));
                                    break;
                            }
                            if (cells[i, j][k, s].Height is IBrightness)
                                light.AddItems((IBrightness)cells[i, j][k, s].Height);
                        }
                    }
                }
            }
        }
    }
}
