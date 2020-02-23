using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    struct Water : IPaintable, IBrightness, IElement<double>
    {
        public const double seaLevel = -.3;
        private double deep;
        private Color color;
        public Point Location { get; set; }
        public double Value
        {
            get
            {
                return deep;
            }
            set
            {
                if (value >= -1 && value <= seaLevel)
                {
                    deep = value;
                    color = BaseColor;
                    if (changed != null)
                        changed(this, null);
                }
            }
        }
        public Vector3 Normal { get; set; }
        public float Brightness { get; set; }
        public Color Color
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
                if (changed != null)
                    changed(this, null);
            }
        }
        public Color BaseColor
        {
            get
            {
                IElement<double> e = this;
                return e.getColor(Value);
            }
        }
        public Color ReflectColor { get; set; }

        public event EventHandler changed;

        public Water(int x, int y, double value)
        {
            Location = new Point(x, y);
            deep = value;
            color = Color.Red;
            changed = null;
            Brightness = .5f;
            Normal = new Vector3(0, 0, 1);
            ReflectColor = new Color();
            Value = value;
        }

        public void Paint(Bitmap bitmap)
        {
            bitmap.SetPixel(Location.X, Location.Y, color);
        }

        Color IElement<double>.getColor(double value)
        {
            if (value < -.4)
                return Color.FromArgb(
                    (int)Math.Round(-1f - 5f * value),
                    (int)Math.Round(800f / 3f + 575f / 3f * value),
                    (int)Math.Round(268f + 165f * value));
            if (-.4 < value && value < Water.seaLevel)
                return Color.FromArgb(
                    (int)Math.Round(857f + 2140f * value),
                    (int)Math.Round(322f + 330f * value),
                    (int)Math.Round(266f + 160f * value));
            return Color.FromArgb(0, 0, 255);
        }

        public void updateBrightness()
        {
            HSB hsb = HSB.toHSB(BaseColor);
            hsb.B = (int)(Brightness * 100);
            Color = Utilites.FilterColor(HSB.fromHSB(hsb), ReflectColor);
            //color = Color.FromArgb((int)(Brightness * 255), color.R, color.G, color.B);
        }
    }
}
