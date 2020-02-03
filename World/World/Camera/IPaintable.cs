using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    interface IPaintable
    {
        event EventHandler changed;
        void Paint(Bitmap bitmap);
    }
}
