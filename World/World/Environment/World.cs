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
            Size sizeCell = new Size(Form1.CELL_SIZE, Form1.CELL_SIZE);
            progress.Value = 50;
            MapHeights heights = MapHeights.creates[type](w, h);
            progress.Value = 97;
            int[,] distribution = Utilites.DFS<double>(heights.Map,
                    (double a) =>
                    {
                        return a < Water.seaLevel;
                    });
            MapNormals normals = new MapNormals(heights);
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
                            int x = cells[i, j].Location.X + k;
                            int y = cells[i, j].Location.Y + s;
                            int cx = i * sizeCell.Width + k;
                            int cy = j * sizeCell.Height + s;
                            switch (distribution[cx, cy] == 0 ? Cell.Element.GROUND : Cell.Element.WATER)
                            {
                                case Cell.Element.WATER: cells[i, j][k, s] = 
                                        new Cell.Property(
                                            new Water(x, y, heights[cx, cy]),
                                            new Normal(x, y, normals[cx, cy]));
                                    break;
                                case Cell.Element.GROUND: cells[i, j][k, s] =
                                        new Cell.Property(
                                            new Ground(x, y, heights[cx, cy]),
                                            new Normal(x, y, normals[cx, cy])); 
                                    break;
                            }
                        }
                    }
                }
            }
            progress.Value = 98;
        }
    }

    class Cell
    {
        private Property[,] properties;
        public List<IEntity> Entities { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public Property this[int i, int j]
        {
            get
            {
                return properties[i, j];
            }
            set
            {
                properties[i, j] = value;
            }
        }

        public Cell(int x, int y, Size size)
        {
            Size = size;
            Location = new Point(x * Size.Width, y * Size.Height);
            properties = new Property[Size.Width, Size.Height];
            Entities = new List<IEntity>();
        }

        public struct Property
        {
            public IElement<double> Height { get; set; }
            public Normal Normal { get; set; }

            public Property(IElement<double> height, Normal normal)
            {
                Height = height;
                Normal = normal;
            }
        }

        public enum Element
        {
            WATER,
            GROUND
        }
    }
}
