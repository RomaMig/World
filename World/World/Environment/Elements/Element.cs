using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Cameras;

namespace World.Environment.Elements
{
    abstract class Element<T> : IPaintable
    {
        public T Value { get; set; }
        public Point Location { get; set; }
        public Color Color { get; set; }

        public event EventHandler<IPaintable> repaint;

        public Element(int x, int y)
        {
            Location = new Point(x, y);
        }

        protected void Repaint(IPaintable sender)
        {
            if (repaint != null)
                repaint(this, sender);
        }

        protected abstract Color getColor(T value);
        public abstract void Paint(Bitmap bitmap);
    }
}
