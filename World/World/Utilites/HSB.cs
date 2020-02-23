using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    struct HSB
    {
        public int H { get; set; }
        public int S { get; set; }
        public int B { get; set; }

        public HSB(int h, int s, int b)
        {
            H = h;
            S = s;
            B = b;
        }

        public static HSB toHSB(Color color)
        {
            return new HSB((int)color.GetHue(), (int)(color.GetSaturation() * 100), (int)(color.GetBrightness() * 100));
        }

        public static Color fromHSB(HSB hsb)
        {
            int h = hsb.H;
            int s = hsb.S;
            int b = hsb.B;

            double R, G, B;
            if (b <= 0)
            {
                R = G = B = 0;
            }
            else if (s <= 0)
            {
                R = G = B = b;
            }
            else
            {
                int Hi = ((int)(h / 60)) % 6;

                double Bmin = ((100f - s) * b) / 100f;
                double a = (b - Bmin) * (h % 60) / 60f;
                double Binc = Bmin + a;
                double Bdec = b - a;

                switch (Hi)
                {
                    case 1:
                        R = Bdec;
                        G = b;
                        B = Bmin;
                        break;
                    case 2:
                        R = Bmin;
                        G = b;
                        B = Binc;
                        break;
                    case 3:
                        R = Bmin;
                        G = Bdec;
                        B = b;
                        break;
                    case 4:
                        R = Binc;
                        G = Bmin;
                        B = b;
                        break;
                    case 5:
                        R = b;
                        G = Bmin;
                        B = Bdec;
                        break;
                    default:
                        R = b;
                        G = Binc;
                        B = Bmin;
                        break;
                }
            }
            return Color.FromArgb((int)Math.Round(R * 255 / 100), (int)Math.Round(G * 255 / 100), (int)Math.Round(B * 255 / 100));
        }
    }
}
