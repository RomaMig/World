using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace World.Cameras
{
    class DinamicCamera : Camera
    {
        private Point direction;
        public int Velocity { get; set; }
        public int DirectionX
        {
            get
            {
                return direction.X;
            }
            set
            {
                direction.X = value * Velocity;
            }
        }
        public int DirectionY
        {
            get
            {
                return direction.Y;
            }
            set
            {
                direction.Y = value * Velocity;
            }
        }

        public DinamicCamera(Control control, Rectangle original, Rectangle screen, int velocity) : base(control, original, screen)
        {
            Velocity = velocity;
            direction = new Point(0, 0);
        }

        public void Moving()
        {
            move();
        }

        public void zoom(MouseEventArgs e)
        {
            if (paintableElements.Count != 0 && Screen.Contains(e.Location))
            {
                int bSize = original.Width;
                Scale += e.Delta / 1000f;
                if (Scale < 1) Scale = 1;
                if (Scale > 4) Scale = 4;
                direction.X = Math.Sign(direction.X) * (int)Scale;
                direction.Y = Math.Sign(direction.Y) * (int)Scale;
                int n = (int)(Screen.Width * Scale);
                bSize = n - bSize;
                original.Size = new Size(n, n);
                PointF p = e.Location;
                double propX = (p.X - Screen.X) / Screen.Size.Width;
                double propY = (p.Y - Screen.Y) / Screen.Size.Height;
                move((int)(-bSize * propX), (int)(-bSize * propY));
            }
        }

        public void move()
        {
            move(direction);
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
    }
}