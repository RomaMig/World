using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    interface IPaintable : IChangable
    {
        void Paint(Bitmap bitmap);
    }
}
