using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Cameras;
using World.Environment;
using World.Utilite;

namespace World.Environment.Elements
{
    class Temperature : Element<int>, IPaintable
    {
        private int temp;
        private Color color;
        public new int Value
        {
            get
            {
                return temp;
            }
            set
            {
                if (temp != value)
                {
                    if (value < 0)
                        temp = 0;
                    else
                    if (value > 270)
                        temp = 270;
                    else
                        temp = value;
                    Color = getColor(temp);
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

        public Temperature(int x, int y, int value) : base(x, y)
        {
            Value = value;
        }

        public override void Paint(Bitmap bitmap)
        {
            bitmap.SetPixel(Location.X, Location.Y, color);
        }

        protected override Color getColor(int value)
        {
            return HSB.fromHSB(new HSB(270 - value, 100, 100));
            /*
            if (value < 90) return Color.Blue;
            if (value > 180) return Color.Red;
            return Color.Green;
            */
        }
    }
}
