using System;
using System.Drawing;
using World.Cameras;
using World.Environment;
using World.Utilite;

namespace World.Environment.Elements
{
    class Normal : Element<Vector3>, IPaintable
    {
        private Vector3 normal;
        private Color color;
        public new Vector3 Value
        {
            get
            {
                return normal;
            }
            set
            {
                normal = value;
                if (!normal.isNormal())
                    normal.normalize();
                Color = getColor(normal);
            }
        }
        public new Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                Repaint(this);
            }
        }

        public Normal(int x, int y, Vector3 value) : base(x, y)
        {
            Value = value;
        }

        public override void Paint(Bitmap bitmap)
        {
            bitmap.SetPixel(Location.X, Location.Y, color);
        }

        protected override Color getColor(Vector3 value)
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
    }
}
