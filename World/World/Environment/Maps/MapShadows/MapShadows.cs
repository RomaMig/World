using System;
using World.Environment.Light;

namespace World.Environment.Map
{
    class MapShadows : Maps<bool>
    {
        private MapHeights heights;
        private LightSource source;
        public MapShadows(int width, int height, MapHeights heights, LightSource source) : base(width, height)
        {
            this.heights = heights;
            this.source = source;
        }

        public void Update(object sender, EventArgs e)
        {

        }
    }
}
