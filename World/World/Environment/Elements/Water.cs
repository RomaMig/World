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
    class Water : Element<double>, IBrightness
    {
        public const double seaLevel = -.3;
        private double deep;
        private Color baseColor;
        public new double Value
        {
            get
            {
                return deep;
            }
            set
            {
                if (deep != value)
                {
                    if (value < -1)
                        deep = -1;
                    else
                    if (value >= seaLevel)
                        deep = seaLevel - 0.01;
                    else
                        deep = value;
                    baseColor = getColor(deep);
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

        public Water(int x, int y, double value) : base(x, y)
        {
            Brightness = .5f;
            Normal = new Vector3(0, 0, 1);
            ReflectColor = new Color();
            Value = value;
        }

        protected override Color getColor(double value)
        {
            if (value < -.4)
                return Color.FromArgb(
                    (int)Math.Round(-1f - 5f * value),
                    (int)Math.Round(800f / 3f + 575f / 3f * value),
                    (int)Math.Round(268f + 165f * value));
            if (-.4 < value && value < seaLevel)
                return Color.FromArgb(
                    (int)Math.Round(857f + 2140f * value),
                    (int)Math.Round(322f + 330f * value),
                    (int)Math.Round(266f + 160f * value));
            return Color.FromArgb(0, 0, 255);
        }

        public void updateBrightness()
        {
            Color = Utilites.FilterColor(baseColor, ReflectColor);

            //HSB hsb = HSB.toHSB(BaseColor);
            //hsb.B = (int)(Brightness * 100);
            //Color = Utilites.FilterColor(HSB.fromHSB(hsb), ReflectColor);
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
