using System;
using System.Drawing;

namespace World
{
    class Normal : IPaintable, IElement<Vector3>
    {
        private Vector3 normal;
        private Color color;
        public Vector3 Value
        {
            get
            {
                return normal;
            }
            set
            {
                normal = value;
                color = getColor(normal);
                if (changed != null)
                    changed(this, null);
            }
        }
        public Point Location { get; set; }
        public event EventHandler changed;

        public Normal(int x, int y, Vector3 value)
        {
            Location = new Point(x, y);
            normal = value;
            color = Color.Red;
            changed = null;
            Value = value;
        }

        public Color getColor(Vector3 value)
        {
            if (value.isNormal())
            {
                int r = (int)(127 * (value.X + 1));
                int g = (int)(127 * (value.Y + 1));
                int b = (int)(127 * (value.Z + 1));
                return Color.FromArgb(r, g, b);
            }
            return Color.Red;
        }

        public void Paint(Bitmap bitmap)
        {
            bitmap.SetPixel(Location.X, Location.Y, color);
        }
    }
}
