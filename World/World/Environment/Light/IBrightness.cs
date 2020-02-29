using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Utilite;

namespace World.Environment.Light
{
    interface IBrightness
    {
        Point Location { get; set; }
        Vector3 Normal { get; set; }
        float Brightness { get; set; }
        Color ReflectColor { get; set; }

        void updateBrightness();
        void updateBrightness(float R, float G, float B);
    }
}
