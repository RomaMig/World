using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    class Union : MapHeights
    {
        public delegate double Rule(int i, int j, params MapHeights[] maps);
        

        public Union(int width, int height, Rule rules, params MapHeights[] maps) : base(width, height)
        {
            for (int i = 0; i < Width; i++)
            {
                for (int j = 0; j < Height; j++)
                {
                    map[i, j] = rules(i, j, maps);
                }
            }
            normalizeWithExtention(-1, 1);
        }
    }
}
