using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    class Cell : Maps<Cell.Property>
    {
        public List<IEntity> Entities { get; set; }
        public Point Location { get; set; }

        public Cell(int x, int y, Size size) : base(size.Width, size.Height)
        {
            Location = new Point(x * Width, y * Height);
            Entities = new List<IEntity>();
        }

        public struct Property
        {
            public IElement<double> Height { get; set; }
            public Normal Normal { get; set; }

            public Property(IElement<double> height, Normal normal)
            {
                Height = height;
                Normal = normal;
            }
        }

        public enum Element
        {
            WATER,
            GROUND
        }
    }
}
