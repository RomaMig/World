using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    abstract class Maps<T>
    {
        public int Width { get; }
        public int Height { get; }
        public T[,] Map
        {
            get;
        }
        public T this[int i, int j]
        {
            get
            {
                return Map[i, j];
            }
            set
            {
                Map[i, j] = value;
            }
        }

        public Maps(int width, int height)
        {
            Width = width;
            Height = height;
            Map = new T[width, height];
        }
    }
}
