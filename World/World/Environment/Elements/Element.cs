using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.CameraSystem;

namespace World.Environment.Elements
{
    abstract class Element<T> : IPaintable
    {
        protected Color color;
        public T Value { get; set; }
        public Point Location { get; set; }
        public Color Color { get; set; }
        public bool Infreez { get; set; }

        public event EventHandler<IPaintable> repaint;

        public Element(int x, int y)
        {
            Location = new Point(x, y);
            Infreez = true;
        }

        protected void Repaint(IPaintable sender)
        {
            if (repaint != null)
                repaint(this, sender);
        }

        public virtual void Paint(Bitmap bitmap)
        {
            bitmap.SetPixel(Location.X, Location.Y, color);
        }

        protected abstract Color getColor(T value);
    }
}
