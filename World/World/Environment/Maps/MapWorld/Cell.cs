using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.Environment.Elements;
using World.Environment;
using World.Environment.Light;
using World.CameraSystem;

namespace World.Environment.Map
{
    class Cell : Maps<Cell.Property>
    {
        private Temperature temp;
        public List<IEntity> Entities { get; set; }
        public Point Location { get; set; }
        public Temperature Temperature
        {
            get
            {
                return temp;
            }
        }
        public new Property this[int i, int j]
        {
            get
            {
                return Map[i, j];
            }
            set
            {
                Map[i, j] = value;
                if (i == 0 && j == 0 && Map[i, j].Height is IBrightness)
                {
                    Map[i, j].Height.repaint += Update;
                }
            }
        }

        public Cell(int x, int y, Size size) : base(size.Width, size.Height)
        {
            Location = new Point(x * Width, y * Height);
            temp = new Temperature(x, y, 0);
            Entities = new List<IEntity>();
        }

        public void Update(object sender, IPaintable e)
        {
            temp.Value = (int)(270 * ((IBrightness)Map[0, 0].Height).Brightness);
        }

        public struct Property
        {
            public Element<double> Height { get; set; }
            public Normal Normal { get; set; }

            public Property(Element<double> height, Normal normal)
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
