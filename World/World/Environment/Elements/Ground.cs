using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Environment;
using World.Environment.Light;
using World.Utilite;

namespace World.Environment.Elements
{
    class Ground : Element<double>, IBrightness
    {
        private double height;
        private Color color;
        private Color baseColor;
        public new double Value
        {
            get
            {
                return height;
            }
            set
            {
                if (height != value)
                {
                    if (value < Water.seaLevel)
                        height = Water.seaLevel;
                    else
                    if (value > 1)
                        height = 1;
                    else
                        height = value;
                    baseColor = getColor(height);
                    updateBrightness();
                }
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
        public Color BaseColor
        {
            get
            {
                return baseColor;
            }
        }
        public Color ReflectColor { get; set; }
        public Vector3 Normal { get; set; }
        public float Brightness { get; set; }

        public Ground(int x, int y, double value, Vector3 normal) : base(x, y)
        {
            Brightness = .5f;
            Normal = normal;
            ReflectColor = new Color();
            Value = value;
        }

        public override void Paint(Bitmap bitmap)
        {
            bitmap.SetPixel(Location.X, Location.Y, color);
        }

        protected override Color getColor(double value)
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
            return Color.Green;
        }

        public void updateBrightness()
        {
            Color = Utilites.FilterColor(baseColor, ReflectColor);

            //HSB hsb = HSB.toHSB(BaseColor);
            //hsb.B = (int)(Brightness * 100);
            //Color = Utilites.FilterColor(HSB.fromHSB(hsb), ReflectColor);
            //Color = HSB.fromHSB(hsb);
            //color = Color.FromArgb((int)(Brightness * 255), color.R, color.G, color.B);
        }

        public void updateBrightness(float R, float G, float B)
        {
            Color = Color.FromArgb(
                (int)(baseColor.R * R / 255f),
                (int)(baseColor.G * G / 255f),
                (int)(baseColor.B * B / 255f));
        }
    }
}
