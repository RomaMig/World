using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World.Cameras
{
    interface IPaintable
    {
        event EventHandler<IPaintable> repaint;

        void Paint(Bitmap bitmap);
    }
}
