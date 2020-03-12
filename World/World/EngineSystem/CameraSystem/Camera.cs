using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace World.CameraSystem
{
    class Camera : ICamera
    {
        protected Rectangle bound;
        protected Rectangle origin;
        protected Rectangle lins;
        protected Rectangle screen;
        protected List<IPaintable> paintableElements;
        private Queue<IPaintable> queue;
        private float scale;
        private Image img;
        public float Scale
        {
            get
            {
                return scale;
            }
            set
            {
                scale = value;
                if (scale < screen.Width * 1f / origin.Width) scale = screen.Width * 1f / origin.Width;
                //if (Scale > 4) Scale = 4;
                lins.X = (int)(screen.X / scale);
                lins.Y = (int)(screen.Y / scale);
                lins.Width = (int)(screen.Width / scale);
                lins.Height = (int)(screen.Height / scale);
            }
        }
        public Bitmap Bitmap { get; set; }
        public Rectangle Bound
        {
            get
            {
                return new Rectangle(lins.X-bound.X, lins.Y - bound.Y, lins.Width, lins.Height);
            }
        }

        private volatile bool readyToUpdate;

        /*
         * Создание камеры(контекст, оригинальный прямоугольник, экран)
         * Add - добавление элементов на отрисовку
        */
        public Camera(Control control, Rectangle origin, Rectangle screen, bool fullScreen)
        {
            control.Paint += Paint;
            this.origin = origin;
            this.screen = screen;
            bound = origin;
            lins = screen;
            if (fullScreen)
                Scale = 0;
            else
                Scale = origin.Width * 1f / screen.Width;
            bound.Location = lins.Location;
            Bitmap = new Bitmap(origin.Width, origin.Height);
            paintableElements = new List<IPaintable>();
            queue = new Queue<IPaintable>();
            readyToUpdate = false;
            newImage();
        }

        public void AddScale(float delta)
        {
            Scale += delta;
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

            img = new Bitmap(lins.Width, lins.Height);
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
            if (p.Infreez)
                AddToPaint(p);
        }

        private void AddToPaint(IPaintable p)
        {
            lock (queue)
            {
                queue.Enqueue(p);
                readyToUpdate = true;

                if (queue.Count > Form1.WORLD_SIZE * Form1.WORLD_SIZE)
                {
                    queue.Dequeue();
                }
            }
        }

        public Task Update()
        {
            if (readyToUpdate)
            {
                readyToUpdate = false;
                return Task.Run(() =>
                {
                    while (queue.Count > 0)
                        queue.Dequeue()?.Paint(Bitmap);
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
            g.ScaleTransform(scale, scale);
            g.DrawImageUnscaled(img, bound.X, bound.Y, lins.Width, lins.Height);
        }

        public virtual string Info()
        {
            return bound.ToString() + "\n" +
                lins.ToString() + "\n" +
                Scale + "\n";
        }
    }
}