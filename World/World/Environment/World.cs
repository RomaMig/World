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
            MapHeights heights = createMapHeights(progress, w, h);
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

        private MapHeights createMapHeights(ProgressBar progress, int w, int h)
        {
            MapHeights m1 = MapHeights.createDiamondSquare(w, h, 0.7);
            progress.Value = 55;
            MapHeights m2 = MapHeights.createNoise(w, h, (double)((int)(100000f / Form1.CELL_SIZE)) / 100000, 6, 15);
            progress.Value = 94;
            MapHeights heights = MapHeights.createUnion(
                (int i, int j, MapHeights[] m) =>
                {
                    return m[0][i, j] + m[1][i, j] * (1 + m[0][i, j]) / 2;
                },
                m1,
                m2);
            progress.Value = 95;
            heights.postprocessing();
            heights.normalizeWithExtention(-1, 1, 0.98);
            return heights;
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
                if (value < -1 || value > 1)
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
                    color = getColor(height);
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

            private Color getColor(double a)
            {
                if (a <= -.4)
                    return Color.FromArgb(
                        (int)Math.Round(-1f - 5f * a),
                        (int)Math.Round(800f / 3f + 575f / 3f * a),
                        (int)Math.Round(268f + 165f * a));
                if (-.4 < a && a < -.3)
                    return Color.FromArgb(
                        (int)Math.Round(857f + 2140f * a),
                        (int)Math.Round(322f + 330f * a),
                        (int)Math.Round(266f + 160f * a));
                if (-.3 <= a && a <= -.1)
                    return Color.FromArgb(
                        (int)Math.Round(259f + 90f * a),
                        (int)Math.Round(457f / 2f + 45f * a),
                        (int)Math.Round(415f / 2f + 45f * a));
                if (-.1 < a && a < .3)
                    return Color.FromArgb(
                        (int)Math.Round(132f - 430f * a),
                        (int)Math.Round(679f / 4f - 325f / 2f * a),
                        (int)Math.Round(49f + 30f * a));
                if (.3 <= a)
                    return Color.FromArgb(
                        (int)Math.Round(778f / 7f + 790f / 7f * a),
                        (int)Math.Round(537f / 7f + 940f / 7f * a),
                        (int)Math.Round(454f / 7f + 960f / 7f * a));
                return Color.FromArgb(0, 0, 0);
            }
        }
    }
}
