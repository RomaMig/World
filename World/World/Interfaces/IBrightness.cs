using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    interface IBrightness : IChangable
    {
        Point Location { get; set; }
        Vector3 Normal { get; set; }
        float Brightness { get; set; }
        Color ReflectColor { get; set; }


        void updateBrightness();
    }
}
