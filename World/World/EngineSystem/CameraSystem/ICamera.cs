using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using World.Interfaces;

namespace World.CameraSystem
{
    interface ICamera : IChangeable
    {
        float Scale { get; set; }
        Rectangle Bound { get; }

        void Add(params IPaintable[] p);
        void Remove(params IPaintable[] p);
        void Clear(Control control);
        void Repaint();
        void newImage();
    }
}
