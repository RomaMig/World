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
        public double scale;
        private Point vector;
        private Rectangle rectangle;
        private Image img;
        private List<Cell> queue;
        private bool isGrid;
        public World World { get; set; }
        public Bitmap Bitmap { get; set; }
        public Rectangle Screen { get; set; }
        public int Velocity { get; set; }
        public bool GridPaint
        {
            set
            {
                isGrid = value;
                FullRepaint();
            }
        }
        public int VectorX
        {
            get
            {
                return vector.X;
            }
            set
            {
                vector.X = value * Velocity;
            }
        }
        public int VectorY
        {
            get
            {
                return vector.Y;
            }
            set
            {
                vector.Y = value * Velocity;
            }
        }

        public Camera(World world, Form1 form, Rectangle screen)
        {
            World = world;
            Screen = screen;
            Velocity = 2;
            scale = 1;
            vector = new Point(0, 0);
            form.paint += Paint;
            form.closing += Close;
            Init();
        }

        private void Init()
        {
            vector.X = 0;
            vector.Y = 0;
            rectangle = new Rectangle(vector, Screen.Size);
            scale = 1;
            Bitmap = new Bitmap(Screen.Width, Screen.Height);
            queue = new List<Cell>();
            img = Bitmap;
        }

        public void Reset()
        {
            Init();
            changeQueue();
            queue.ForEach((Cell c) => { c.Paint(Bitmap); });
            if (isGrid) DrawGrid();
            img = Bitmap;
        }

        public void collapse()
        {
            rectangle.Width = 0;
            rectangle.Height = 0;
            vector.X = 0;
            vector.Y = 0;
        }

        public void Update()
        {
            if (vector.X != 0 || vector.Y != 0)
            {
                move();
            }
        }

        private void FullRepaint()
        {
            if (rectangle.Width != 0 && rectangle.Height != 0)
            {
                queue.ForEach((Cell c) => { c.Paint(Bitmap); });
                if (isGrid) DrawGrid();
                img = new Bitmap(Bitmap, rectangle.Width, rectangle.Height);
            }
        }

        private void Repaint()
        {
            if (rectangle.Width != 0 && rectangle.Height != 0)
            {
                img = new Bitmap(Bitmap, rectangle.Width, rectangle.Height);
            }
        }

        public void Paint(Form1 form, PaintEventArgs args)
        {
            bool flag = false;
            Cell cell;
            while ((cell = queue.Find((Cell c) => { return c.Changed.Count > 0; })) != null)
            {
                flag = true;
                cell.Repaint(Bitmap);
            }
            if (flag) Repaint();
            args.Graphics.DrawImageUnscaled(img, rectangle);
        }

        public void Close(Form1 form, EventArgs args)
        {

        }

        public void zoom(Object sender, MouseEventArgs e)
        {
            Form1 form = (Form1)sender;
            if (!form.progressBar1.Visible && Screen.Contains(e.Location))
            {
                int bSize = rectangle.Width;
                scale += e.Delta / 1000f;
                if (scale < 1) scale = 1;
                if (scale > 4) scale = 4;
                vector.X = Math.Sign(vector.X) * (int)scale;
                vector.Y = Math.Sign(vector.Y) * (int)scale;
                int n = (int)(Screen.Width * scale);
                bSize = n - bSize;
                rectangle.Size = new Size(n, n);
                PointF p = e.Location;
                double propX = p.X / Screen.Size.Width;
                double propY = p.Y / Screen.Size.Height;
                rectangle.Offset((int)(-bSize * propX), (int)(-bSize * propY));
                if (Rectangle.Intersect(Screen, rectangle).Size != Screen.Size)
                {
                    if (rectangle.Left > Screen.Left) rectangle.X = 0;
                    if (rectangle.Top > Screen.Top) rectangle.Y = 0;
                    if (rectangle.Right < Screen.Right) rectangle.X = Screen.Right - rectangle.Width;
                    if (rectangle.Bottom < Screen.Bottom) rectangle.Y = Screen.Bottom - rectangle.Height;
                }
                changeQueue();
                Repaint();
                form.Invalidate();
            }
        }

        public void move()
        {
            move(vector);
        }

        public void move(Point vector)
        {
            move(vector.X, vector.Y);
        }

        public void move(int offsetX, int offsetY)
        {
            rectangle.Offset(offsetX, offsetY);
            if (Rectangle.Intersect(Screen, rectangle).Size != Screen.Size)
            {
                if (rectangle.Left > Screen.Left) rectangle.X = 0;
                if (rectangle.Top > Screen.Top) rectangle.Y = 0;
                if (rectangle.Right < Screen.Right) rectangle.X = Screen.Right - rectangle.Width;
                if (rectangle.Bottom < Screen.Bottom) rectangle.Y = Screen.Bottom - rectangle.Height;
            }
            changeQueue();
        }

        private void changeQueue()
        {
            bool flag = queue.Count == 0;
            Cell cell;
            while ((cell = queue.Find((Cell c) => { return Rectangle.Intersect(rectangle, c.Rect).IsEmpty; })) != null)
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
                        if (!queue.Contains(World.cells[i, j]) && rectangle.Contains(World.cells[i, j].Rect))
                            queue.Add(World.cells[i, j]);
                    }
                }
            }
        }

        private void DrawGrid()
        {
            draw_grid(Form1.WORK_AREA, Form1.WORK_AREA, Form1.CELL_SIZE, Color.LightGray);
            draw_grid(Form1.WORK_AREA, Form1.WORK_AREA, Form1.CELL_SIZE * 10, Color.Gray);
        }

        private void draw_grid(int w, int h, int indent, Color color)
        {
            int wid = w / indent;
            int hei = h / indent;
            for (int i = 0; i < wid; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    Bitmap.SetPixel(j, i * indent, color);
                }
            }
            for (int i = 0; i < hei; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    Bitmap.SetPixel(i * indent, j, color);
                }
            }
        }

    }
}