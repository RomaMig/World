using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{   
    struct Ground : IPaintable, IBrightness, IElement<double>
    {
        private double height;
        private Color color;
        public Point Location { get; set; }
        public event EventHandler changed;
        public double Value
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
                IElement<double> e = this;
                color = e.getColor(height);
                if (changed != null)
                    changed(this, null);
            }
        }
        public float Brightness { get; set; }
        public Vector3 Normal { get; set; }

        public Ground(int x, int y, double value, Vector3 normal)
        {
            Location = new Point(x, y);
            height = value;
            color = Color.Red;
            changed = null;
            Brightness = .5f;
            Normal = normal;
            Value = value;
        }

        public void Paint(Bitmap bitmap)
        {
            bitmap.SetPixel(Location.X, Location.Y, color);
        }

        Color IElement<double>.getColor(double value)
        {
            if (Water.seaLevel <= value && value <= -.1)
                return Color.FromArgb(
                    (int)Math.Round(259f + 90f * value),
                    (int)Math.Round(457f / 2f + 45f * value),
                    (int)Math.Round(415f / 2f + 45f * value));
            if (-.1 < value && value < .3)
                return Color.FromArgb(
                    (int)Math.Round(132f - 430f * value),
                    (int)Math.Round(679f / 4f - 325f / 2f * value),
                    (int)Math.Round(49f + 30f * value));
            if (.3 <= value)
                return Color.FromArgb(
                    (int)Math.Round(778f / 7f + 790f / 7f * value),
                    (int)Math.Round(537f / 7f + 940f / 7f * value),
                    (int)Math.Round(454f / 7f + 960f / 7f * value));
            return Color.FromArgb(0, 255, 0);
        }

        public void updateBrightness()
        {
            color = Color.FromArgb((int)(Brightness * 255), color.R, color.G, color.B);
        }
    }
}
