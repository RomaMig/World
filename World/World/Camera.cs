using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace World
{
    class Camera
    {
        Form1 form;
        private double scale;
        private List<Cell> queue;
        public Point vector;
        private Rectangle rect;
        public World World { get; set; }
        public Rectangle Screen { get; set; }
        private double Scale
        {
            get
            {
                return scale;
            }
            set
            {
                if (value >= 1 && value < Form1.WORK_AREA / Form1.CELL_SIZE)
                    scale = value;
            }
        }

        public Camera(World world, Form1 form, Point location, Size size, Rectangle screen, int scale)
        {
            World = world;
            Screen = screen;
            vector = new Point(0, 0);
            form.paint += Paint;
            form.closing += Close;
            this.form = form;
            rect = new Rectangle(location, size);
            queue = new List<Cell>();
            zoom(scale);
        }

        public void Update()
        {
            if (vector.X != 0 || vector.Y != 0)
            {
                move();
            }

        }

        public void Paint(Form1 form, PaintEventArgs args)
        {
            queue.ForEach((Cell c) => { c.Paint(this, args.Graphics); });
        }

        public void Paint(Graphics g, Rectangle r, Color color)
        {
            r.Offset(-rect.X, -rect.Y);
            if (Screen.Contains(r))
            {
                g.FillRectangle(new SolidBrush(color), r);
            }
            else
            {
                r = Rectangle.Intersect(r, Screen);
                if (r != null && r.Width != 0 && r.Height != 0)
                {
                    g.FillRectangle(new SolidBrush(color), r);
                }
            }
        }

        public void Close(Form1 form, EventArgs args)
        {

        }

        public void zoom(double scale)
        {
            Scale = scale;
            int n = (int)(rect.Width / scale);
            rect.Size = new Size(n, n);
            changeQueue();
        }

        public void move()
        {
            rect.Offset(vector);
            changeQueue();
        }

        public void move(Point vector)
        {
            rect.Offset(vector);
            changeQueue();
        }

        public void move(int offsetX, int offsetY)
        {
            rect.Offset(offsetX, offsetY);
            changeQueue();
        }

        private void changeQueue()
        {
            bool flag = queue.Count == 0;
            Cell cell;
            while ((cell = queue.Find((Cell c) => { return !rect.Contains(c.Rect); })) != null)
            {
                flag = true;
                queue.Remove(cell);
            }
            if (flag)
            {
                for (int i = 0; i < World.Size.Width; i++)
                {
                    for (int j = 0; j < World.Size.Height; j++)
                    {
                        if (!queue.Contains(World.cells[i, j]) && rect.Contains(World.cells[i, j].Rect))
                            queue.Add(World.cells[i, j]);
                    }
                }
            }
        }
    }
}
