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

        public DinamicCamera(Control control, Rectangle original, Rectangle screen, int velocity) : base(control, original, screen, CameraState.TO_IMAGE)
        {
            Velocity = velocity;
            vector = new Point(0, 0);
            scale = 1;
        }

        public void Update()
        {
            move();
        }

        public void zoom(Object sender, MouseEventArgs e)
        {
            Control control = (Control)sender;
            if (queue.Count != 0 && Screen.Contains(e.Location) && State != CameraState.TO_SCREEN)
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
                Resize(control);
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

        public override void newState(Control control, CameraState state)
        {
            if (state == CameraState.TO_SCREEN)
            {
                original = Screen;
            }
            base.newState(control, state);
        }
    }
}