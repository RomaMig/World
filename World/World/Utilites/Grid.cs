using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace World
{
    class  Grid : IPaintable
    {
        public event EventHandler changed;

        public void Paint(Bitmap bitmap)
        {
            DrawGrid(bitmap, Form1.WORLD_SIZE, Form1.WORLD_SIZE, Form1.CELL_SIZE, Color.LightGray);
            DrawGrid(bitmap, Form1.WORLD_SIZE, Form1.WORLD_SIZE, Form1.CELL_SIZE * 10, Color.Gray);
        }

        private void DrawGrid(Bitmap bitmap, int w, int h, int indent, Color color)
        {
            int wid = w / indent;
            int hei = h / indent;
            for (int i = 0; i < wid; i++)
            {
                for (int j = 0; j < w; j++)
                {
                    bitmap.SetPixel(j, i * indent, color);
                }
            }
            for (int i = 0; i < hei; i++)
            {
                for (int j = 0; j < h; j++)
                {
                    bitmap.SetPixel(i * indent, j, color);
                }
            }
        }
    }

}
