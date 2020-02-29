using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using World.Environment;
using World.Interfaces;

namespace World.Cameras
{
    class Camera : IChangeable
    {
        protected Rectangle original;
        protected List<IPaintable> paintableElements;
        private Queue<IPaintable> queue;
        private Image img;
        public float Scale { get; set; }
        public Bitmap Bitmap { get; set; }
        public Rectangle Screen { get; set; }

        private volatile bool readyToUpdate;

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
            paintableElements = new List<IPaintable>();
            queue = new Queue<IPaintable>();
            readyToUpdate = false;
            newImage();
        }

        public void Add(params IPaintable[] p)
        {
            paintableElements.AddRange(p);
            UpdatePaint(p);
        }

        public void Remove(params IPaintable[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                paintableElements.Remove(p[i]);
                p[i].repaint -= OnChanged;
            }
        }

        public void Clear(Control control)
        {
            paintableElements.ForEach((IPaintable p) => { p.repaint -= OnChanged; });
            paintableElements.Clear();
            queue.Clear();

            img = new Bitmap(Screen.Width, Screen.Height);
            control.Invalidate();
        }

        private void UpdatePaint(params IPaintable[] p)
        {
            for (int i = 0; i < p.Length; i++)
            {
                AddToPaint(p[i]);
                p[i].repaint += OnChanged;
            }
        }

        private void OnChanged(object sender, IPaintable p)
        {
            AddToPaint(p);
        }

        private void AddToPaint(IPaintable p)
        {
            queue.Enqueue(p);
            readyToUpdate = true;

            if (queue.Count > Form1.WORLD_SIZE * Form1.WORLD_SIZE)
            {
                queue.Dequeue();
            }

        }

        public Task Update()
        {
            if (readyToUpdate)
            {
                readyToUpdate = false;
                return Task.Run(() =>
                {
                    try
                    {
                        while (queue.Count != 0)
                            queue.Dequeue().Paint(Bitmap);
                    }
                    catch (NullReferenceException)
                    {

                    }
                });
            }
            return null;
        }

        public void Repaint()
        {
            paintableElements.ForEach((IPaintable p) => { p.Paint(Bitmap); });
        }

        public void newImage()
        {
            img = new Bitmap(Bitmap);
        }

        private void Paint(object sender, PaintEventArgs args)
        {
            Graphics g = args.Graphics;
            g.ScaleTransform(Screen.Width * Scale / original.Width, Screen.Height * Scale / original.Height);
            try
            {
                g.DrawImageUnscaledAndClipped(img, original);
            }
            catch (InvalidOperationException)
            {

            }
        }
    }
}