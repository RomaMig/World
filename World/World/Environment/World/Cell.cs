using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    class Cell
    {
        public List<IEntity> Entities { get; set; }
        public Point Location { get; set; }
        public Size Size { get; set; }
        public Property[,] Properties { get; }
        public Property this[int i, int j]
        {
            get
            {
                return Properties[i, j];
            }
            set
            {
                Properties[i, j] = value;
            }
        }

        public Cell(int x, int y, Size size)
        {
            Size = size;
            Location = new Point(x * Size.Width, y * Size.Height);
            Properties = new Property[Size.Width, Size.Height];
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
