using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace World
{
    class Cell
    {
        private double[,] heights;
        public List<IEntity> Entities { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public Rectangle Rect { get; }
        public double this[int i, int j]
        {
            get
            {
                return heights[i, j];
            }
            set
            {
                //if (value < 0 || value > 255) value = 0;
                heights[i, j] = value;
            }
        }

        public Cell(Form1 form, int x, int y, Size size)
        {
            form.closing += Close;
            Size = size;
            Location = new Point(x * Size.Width, y * Size.Height);
            Rect = new Rectangle(Location, Size);
            heights = new double[Size.Width, Size.Height];
            Entities = new List<IEntity>();
        }

        public void Paint(Camera cam)
        {
            for (int i = 0; i < Size.Width; i++)
            {
                for (int j = 0; j < Size.Height; j++)
                {
                    int c = (int)Math.Round(heights[i, j]);
                    cam.Paint(Location.X + i, Location.Y + j, 1, 1, Color.FromArgb(c, c, c));
                }
            }
        }

        public void Close(Form1 form, EventArgs args)
        {

        }
    }

    class World
    {
        private Form1 context;
        public Size Size { get; set; }
        public Cell[,] cells;

        public World(Form1 form, Size size)
        {
            context = form;
            Size = size;
            int w = size.Width;
            int h = size.Height;
            cells = new Cell[w, h];
        }
        public void Generate()
        {
            int w = Size.Width;
            int h = Size.Height;
            Size sizeCell = new Size(Form1.CELL_SIZE, Form1.CELL_SIZE);
            Map heights = Map.createUnion((int i, int j, Map[] m) => 
                { 
                    return (m[0][i, j] + m[1][i, j] * (1 + m[0][i, j])) * 2;
                }, 
                Map.createDiamondSquare(w, h, 0.01), 
                Map.createNoise(w, h, (double)((int)(100000f / Form1.CELL_SIZE)) / 100000, 5, 6));
            heights.postprocessing();
            heights.normalizeWithExtention(0, 255, 0.98);
            for (int i = 0; i < w; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    if (cells[i, j] == null)
                        cells[i, j] = new Cell(context, i, j, sizeCell);
                    for (int k = 0; k < sizeCell.Width; k++)
                    {
                        for (int s = 0; s < sizeCell.Height; s++)
                        {
                            cells[i, j][k, s] = heights[i * sizeCell.Width + k, j * sizeCell.Height + s];
                        }
                    }
                }
            }
        }
    }
}
