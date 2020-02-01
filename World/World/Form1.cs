using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace World
{
    public partial class Form1 : Form
    {
        public delegate void EventPaint(Form1 f1, PaintEventArgs args);
        public delegate void EventClosing(Form1 f1, EventArgs args);

        public const int WORK_AREA = 500;
        public const int CELL_SIZE = 10;
        private World world;
        private Camera camera;
        public event EventPaint paint;
        public event EventClosing closing;

        public class EventBitmap : EventArgs
        {
            public Bitmap Bitmap { get; set; }

            public EventBitmap(Bitmap bitmap)
            {
                Bitmap = bitmap;
            }
        }

        public Form1()
        {
            InitializeComponent();
            init();
        }

        private void init()
        {
            Settings_map.Location = new Point(WORK_AREA + 18, 12);
            this.DoubleBuffered = true;
            this.Size = new Size(WORK_AREA + Settings_map.Width + 46, WORK_AREA + 63);
            CenterToScreen();
            progressBar1.Location = new Point((WORK_AREA - progressBar1.Size.Width) / 2, (WORK_AREA - progressBar1.Size.Height) / 2);
            world = new World(this, new Size(WORK_AREA / CELL_SIZE, WORK_AREA / CELL_SIZE));
            world.Generate();
            camera = new Camera(world, this, new Point(0, 0), new Size(WORK_AREA, WORK_AREA), new Rectangle(0, 0, WORK_AREA, WORK_AREA), 1);
        }

        private void draw_grid(Graphics g, Pen pen, int w, int h, int indent)
        {
            int wid = w / indent;
            int hei = h / indent;
            for (int i = 0; i <= wid; i++)
            {
                g.DrawLine(pen, 0, i * indent, w, i * indent);
            }
            for (int j = 0; j <= hei; j++)
            {
                g.DrawLine(pen, j * indent, 0, j * indent, h);
            }
        }

        private void drawGrid(Graphics g, int w, int h, int indent)
        {
            draw_grid(g, Pens.LightGray, WORK_AREA, WORK_AREA, CELL_SIZE);
            draw_grid(g, Pens.Gray, WORK_AREA, WORK_AREA, CELL_SIZE * 10);
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (paint != null)
                paint(this, e);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (closing != null)
                closing(this, e);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            progressBar1.Visible = true;
            progressBar1.Enabled = true;
            new Thread(() =>
            {
                world.Generate();
                progressBar1.Visible = false;
                progressBar1.Enabled = false;
            }).Start();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                camera.vector.Y = -1;
                loop.Enabled = true;
            }
            if (e.KeyCode == Keys.A)
            {
                camera.vector.X = -1;
                loop.Enabled = true;
            }
            if (e.KeyCode == Keys.S)
            {
                camera.vector.Y = 1;
                loop.Enabled = true;
            }
            if (e.KeyCode == Keys.D)
            {
                camera.vector.X = 1;
                loop.Enabled = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.S)
            {
                camera.vector.Y = 0;
                if (camera.vector.X == 0 && camera.vector.Y == 0)
                    loop.Enabled = false;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
            {
                camera.vector.X = 0;
                if (camera.vector.X == 0 && camera.vector.Y == 0)
                    loop.Enabled = false;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Map_BackgroundImageChanged(object sender, EventArgs e)
        {
            //Map.Invalidate();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            camera.Update();
            Invalidate();
        }
    }
}