using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace World.CameraSystem
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

        public DinamicCamera(Control control, Rectangle original, Rectangle screen, int velocity) : base(control, original, screen, false)
        {
            Velocity = velocity;
            direction = new Point(0, 0);
        }

        public void zoom(MouseEventArgs e)
        {
            if (paintableElements.Count != 0 && screen.Contains(e.Location))
            {
                PointF p = e.Location;
                float pX = (p.X - screen.X) / screen.Width;
                float pY = (p.Y - screen.Y) / screen.Height;
                float x0 = pX * lins.Width + lins.X;
                float y0 = pY * lins.Height + lins.Y;
                AddScale(e.Delta / 2000f);
                float x1 = pX * lins.Width + lins.X;
                float y1 = pY * lins.Height + lins.Y;
                move((int)(x1 - x0), (int)(y1 - y0));
            }
        }

        public void Moving()
        {
            move();
        }

        private void move()
        {
            move(direction);
        }

        private void move(Point vector)
        {
            move(vector.X, vector.Y);
        }

        private void move(int offsetX, int offsetY)
        {
            bound.Offset(offsetX, offsetY);
                if (bound.Left > lins.Left) bound.X = lins.Left;
                if (bound.Top > lins.Top) bound.Y = lins.Top;
                if (bound.Right < lins.Right) bound.X = lins.Right - bound.Width;
                if (bound.Bottom < lins.Bottom) bound.Y = lins.Bottom - bound.Height;
        }

        public override string Info()
        {
            return base.Info();
        }
    }
}