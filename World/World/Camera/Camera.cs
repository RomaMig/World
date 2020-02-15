using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace World
{
    class Camera
    {
        protected Rectangle original;
        protected List<IPaintable> queue;
        private Image img;
        public float Scale { get; set; }
        public Bitmap Bitmap { get; set; }
        public Rectangle Screen { get; set; }

        /*
         * Создание камеры(контекст, оригинальный прямоугольник, экран)
         * Add - добавление элементов на отрисовку
        */
        public Camera(Control control, Rectangle original, Rectangle screen)
        {
            control.Paint += Paint;
            this.original = original;
            Scale = 1;
            Screen = screen;
            Bitmap = new Bitmap(original.Width, original.Height);
            queue = new List<IPaintable>();
            img = Bitmap;
        }

        public void Add(params IPaintable[] p)
        {
            queue.AddRange(p);
            UpdatePaint(p);
        }

        public void Remove(params IPaintable[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                queue.Remove(p[i]);
                p[i].changed -= OnChanged;
            }
        }

        public void Clear(Control control)
        {
            queue.ForEach((IPaintable p) => { p.changed -= OnChanged; });
            queue.Clear();

            img = new Bitmap(Screen.Width, Screen.Height);
            control.Invalidate();
        }

        private void UpdatePaint(params IPaintable[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                p[i].Paint(Bitmap);
                p[i].changed += OnChanged;
            }
        }

        private void OnChanged(object sender, EventArgs e)
        {
            IPaintable p = (IPaintable)sender;
            p.Paint(Bitmap);
        }

        public void Repaint()
        {
            queue.ForEach((IPaintable p) => { p.Paint(Bitmap); });
        }

        public void UpdateImage()
        {
            img = Bitmap;
        }

        private void Paint(object sender, PaintEventArgs args)
        {
            Graphics g = args.Graphics;
            g.ScaleTransform(Screen.Width * Scale / original.Width, Screen.Height * Scale / original.Height);
            g.DrawImageUnscaledAndClipped(img, original);
        }
    }
}