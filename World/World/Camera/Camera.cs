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
        private CameraState state;
        public Bitmap Bitmap { get; set; }
        public Rectangle Screen { get; set; }

        public CameraState State
        {
            get
            {
                return state;
            }
        }

        public enum CameraState
        {
            TO_IMAGE,
            TO_SCREEN
        }

        /*
         * Создание камеры(контекст, оригинальный прямоугольник, экран)
         * Add - добавление элементов на отрисовку
         * Resize для получения картинки и последующей перерисовки
        */
        public Camera(Control control, Rectangle original, Rectangle screen, CameraState state)
        {
            control.Paint += Paint;
            this.original = original;
            Screen = screen;
            Bitmap = new Bitmap(original.Width, original.Height);
            queue = new List<IPaintable>();
            img = Bitmap;
            this.state = state;
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

        public void Resize(Control control)
        {
            Rectangle rect = new Rectangle();
            switch (state)
            {
                case CameraState.TO_IMAGE: rect = original; break;
                case CameraState.TO_SCREEN: rect = Screen; break;
            }
            if (rect.Width != 0 && rect.Height != 0)
            {
                if (Bitmap.Size != rect.Size)
                {
                    img = new Bitmap(Bitmap, rect.Width, rect.Height);
                }
                else
                {
                    img = Bitmap;
                }
            }
            control.Invalidate();
        }

        private void Paint(object sender, PaintEventArgs args)
        {
            args.Graphics.DrawImageUnscaled(img, original);
        }
        
        public virtual void newState(Control control, CameraState state)
        {
            this.state = state;
            Resize(control);
        }
    }
}