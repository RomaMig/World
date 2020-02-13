using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    interface IElement<T>
    {
        T Value { get; set; }
        Point Location { get; set; }

        Color getColor(T value);
    }
}
