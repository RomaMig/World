using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using World.CameraSystem;
using World.Environment.Light;
using World.Environment.Map;
using World.Interfaces;

namespace World.EngineSystem.OptimizeSystem
{
    class Optimize : IChangeable
    {
        private readonly MapBrightnesses brightnesses;
        private readonly MapPaintables paintables;
        private Rectangle before;
        private Rectangle after;
        private volatile bool readyToUpdate;

        public Optimize(MapWorld world)
        {
            brightnesses = new MapBrightnesses(world.Width * Form1.CELL_SIZE, world.Height * Form1.CELL_SIZE);
            paintables = new MapPaintables(world.Width * Form1.CELL_SIZE, world.Height * Form1.CELL_SIZE);
            before = new Rectangle();
            for (int i = 0; i < world.Width; i++)
            {
                for (int j = 0; j < world.Height; j++)
                {
                    for (int n = 0; n < world[i, j].Width; n++)
                    {
                        for (int m = 0; m < world[i, j].Height; m++)
                        {
                            int cx = i * world[i, j].Width + n;
                            int cy = j * world[i, j].Height + m;
                            brightnesses[cx, cy] = world[i, j][n, m].Height as IBrightness;
                            paintables[cx, cy] = world[i, j][n, m].Height as IPaintable;
                        }
                    }
                }
            }
            Clear();
        }

        public void Ready(Rectangle after)
        {
            int offset = Form1.CELL_SIZE * 3;
            after.X = after.X - offset < 0 ? 0 : after.X - offset;
            after.Y = after.Y - offset < 0 ? 0 : after.Y - offset;
            after.Width = after.Right + offset * 2 > Form1.WORLD_SIZE ? Form1.WORLD_SIZE - after.X : after.Width + offset * 2;
            after.Height = after.Bottom + offset * 2 > Form1.WORLD_SIZE ? Form1.WORLD_SIZE - after.Y : after.Height + offset * 2;
            this.after = after;
            readyToUpdate = true;
        }

        public Task Update()
        {
            if (readyToUpdate)
            {
                readyToUpdate = false;
                return Task.Run(() =>
                {
                    if (before.Width != 0)
                    {
                        //Rectangle intersect = Rectangle.Intersect(before, after);
                        //Selection(after, intersect, Add);
                        //Selection(before, intersect, Remove);
                        Remove(before.X, before.Y, before.Width, before.Height);
                        Add(after.X, after.Y, after.Width, after.Height);
                    }
                    else
                    {
                        Add(after.X, after.Y, after.Width, after.Height);
                    }
                    before = after;
                });
            }
            return null;
        }

        private delegate void Dl(int x, int y, int w, int h);
        private void Selection(Rectangle rect, Rectangle intersect, Dl func)
        {
            if (intersect.Size != rect.Size)
            {
                if (rect.Top < intersect.Top)
                {
                    int dh = rect.Height - intersect.Height;
                    func(rect.X, rect.Y, rect.Width, dh);
                    if (rect.Left < intersect.Left)
                        func(rect.X, rect.Y + dh, rect.Width - intersect.Width, intersect.Height);
                    else
                    if (rect.Right > intersect.Right)
                        func(rect.X + intersect.Width, rect.Y + dh, rect.Width - intersect.Width, intersect.Height);
                }
                else
                if (rect.Bottom > intersect.Bottom)
                {
                    func(rect.X, intersect.Bottom, rect.Width, rect.Height - intersect.Height);
                    if (rect.Left < intersect.Left)
                        func(rect.X, rect.Y, rect.Width - intersect.Width, intersect.Height);
                    else
                    if (rect.Right > intersect.Right)
                        func(intersect.Right, rect.Y, rect.Width - intersect.Width, intersect.Height);
                }
                else
                if (rect.Left < intersect.Left)
                    func(rect.X, rect.Y, rect.Width - intersect.Width, rect.Height);
                else
                    func(intersect.Right, rect.Y, rect.Width - intersect.Width, rect.Height);
            }
        }

        private void Add(int x, int y, int w, int h)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    brightnesses[i, j].Infreez = true;
                    paintables[i, j].Infreez = true;
                }
            }
        }

        private void Remove(int x, int y, int w, int h)
        {
            for (int i = x; i < x + w; i++)
            {
                for (int j = y; j < y + h; j++)
                {
                    paintables[i, j].Infreez = brightnesses[i, j].Infreez = i % Form1.CELL_SIZE == 0 && j % Form1.CELL_SIZE == 0;

                }
            }
        }

        private void Clear()
        {
            for (int i = 0; i < brightnesses.Width; i++)
            {
                for (int j = 0; j < brightnesses.Height; j++)
                {
                    paintables[i, j].Infreez = brightnesses[i, j].Infreez = i % Form1.CELL_SIZE == 0 && j % Form1.CELL_SIZE == 0;

                }
            }
        }

        private class MapBrightnesses : Maps<IBrightness>
        {
            public MapBrightnesses(int width, int height) : base(width, height)
            {

            }
        }
        private class MapPaintables : Maps<IPaintable>
        {
            public MapPaintables(int width, int height) : base(width, height)
            {

            }
        }
    }
}
