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

        public void Generate(ProgressBar progress)
        {
            int w = Size.Width;
            int h = Size.Height;
            Size sizeCell = new Size(Form1.CELL_SIZE, Form1.CELL_SIZE);
            progress.Value = 10;
            Map m1 = Map.createDiamondSquare(w, h, 0.01);
            progress.Value = 55;
            Map m2 = Map.createNoise(w, h, (double)((int)(100000f / Form1.CELL_SIZE)) / 100000, 5, 6);
            progress.Value = 95;
            Map heights = Map.createUnion((int i, int j, Map[] m) =>
                {
                    return (m[0][i, j] + m[1][i, j] * (1 + m[0][i, j])) * 2;
                },
                m1,
                m2);
            progress.Value = 96;
            heights.postprocessing();
            heights.normalizeWithExtention(0, 255, 0.98);
            progress.Value = 97;
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (cells[i, j] == null)
                        cells[i, j] = new Cell(i, j, sizeCell);
                    for (int k = 0; k < sizeCell.Width; k++)
                    {
                        for (int s = 0; s < sizeCell.Height; s++)
                        {
                            cells[i, j][k, s] = heights[i * sizeCell.Width + k, j * sizeCell.Height + s];
                        }
                    }
                }
            }
            progress.Value = 98;
        }
    }

    class Cell
    {
        public Heights[,] heights;
        public List<IEntity> Entities { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public double this[int i, int j]
        {
            get
            {
                return heights[i, j].Height;
            }
            set
            {
                if (value < 0 || value > 255)
                    heights[i, j].Height = 0;
                else
                    heights[i, j].Height = value;
            }
        }

        public Cell(int x, int y, Size size)
        {
            Point p;
            Size = size;
            Location = new Point(x * Size.Width, y * Size.Height);
            heights = new Heights[Size.Width, Size.Height];
            for (int i = 0; i < Size.Width; i++)
            {
                for (int j = 0; j < Size.Height; j++)
                {
                    heights[i, j] = new Heights(Location.X + i, Location.Y + j);
                }
            }
            Entities = new List<IEntity>();
        }

        public struct Heights : IPaintable
        {
            private double height;
            private Color color;
            public event EventHandler changed;
            public Point Location { get; set; }
            public double Height
            {
                get
                {
                    return height;
                }
                set
                {
                    height = value;
                    int c = (int)height;
                    color = Color.FromArgb(c, c, c);
                    if (changed != null)
                        changed(this, null);
                }
            }

            public Heights(int x, int y)
            {
                Location = new Point(x, y);
                height = 0;
                color = Color.Red;
                changed = null;
            }

            public void Paint(Bitmap bitmap)
            {
                bitmap.SetPixel(Location.X, Location.Y, color);
            }
        }
    }
}
