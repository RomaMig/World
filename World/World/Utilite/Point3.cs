using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World.Utilite
{
    struct Point3
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public Point3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
    }
}
