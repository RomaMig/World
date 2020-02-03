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
    class DinamicCamera : Camera
    {
        public double scale;
        private Point vector;
        public int Velocity { get; set; }
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

        public DinamicCamera(Form1 form, Rectangle original, Rectangle screen, int velocity) : base(form, original, screen, CameraState.TO_IMAGE)
        {
            Velocity = velocity;
            vector = new Point(0, 0);
            scale = 1;
        }

        public override void collapse()
        {
            base.collapse();
            vector.X = 0;
            vector.Y = 0;
        }

        public void Update()
        {
            move();
        }

        public void zoom(Object sender, MouseEventArgs e)
        {
            Form1 form = (Form1)sender;
            if (!form.progressBar1.Visible && Screen.Contains(e.Location))
            {
                int bSize = original.Width;
                scale += e.Delta / 1000f;
                if (scale < 1) scale = 1;
                if (scale > 4) scale = 4;
                vector.X = Math.Sign(vector.X) * (int)scale;
                vector.Y = Math.Sign(vector.Y) * (int)scale;
                int n = (int)(Screen.Width * scale);
                bSize = n - bSize;
                original.Size = new Size(n, n);
                PointF p = e.Location;
                double propX = p.X / Screen.Size.Width;
                double propY = p.Y / Screen.Size.Height;
                move((int)(-bSize * propX), (int)(-bSize * propY));
                Resize(form);
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
            original.Offset(offsetX, offsetY);
            if (Rectangle.Intersect(Screen, original).Size != Screen.Size)
            {
                if (original.Left > Screen.Left) original.X = Screen.Left;
                if (original.Top > Screen.Top) original.Y = Screen.Top;
                if (original.Right < Screen.Right) original.X = Screen.Right - original.Width;
                if (original.Bottom < Screen.Bottom) original.Y = Screen.Bottom - original.Height;
            }
        }
        /*
        private void changeQueue()
        {
            bool flag = queue.Count == 0;
            Cell cell;
            while ((cell = queue.Find((Cell c) => { return Rectangle.Intersect(original, c.Rect).IsEmpty; })) != null)
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
                        if (!queue.Contains(World.cells[i, j]) && original.Contains(World.cells[i, j].Rect))
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
        */
    }
}