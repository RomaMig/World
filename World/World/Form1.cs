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

        public const int WORK_AREA = 600;
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
            initWorld();
            GenerateWorld();
            initForm();
        }

        private void initWorld()
        {
            world = new World(this, new Size(WORK_AREA / CELL_SIZE, WORK_AREA / CELL_SIZE));
            camera = new Camera(world, this, new Rectangle(0, 0, WORK_AREA, WORK_AREA));
        }

        private void initForm()
        {
            this.MouseWheel += camera.zoom;
            this.DoubleBuffered = true;
            this.Size = new Size(WORK_AREA + control_panel.Width + 16, WORK_AREA + 39);
            CenterToScreen();
            progressBar1.Size = new Size(WORK_AREA / 4, 23);
            progressBar1.Location = new Point((WORK_AREA - progressBar1.Size.Width) / 2, (WORK_AREA - progressBar1.Size.Height) / 2);
        }

        private void GenerateWorld()
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.Enabled = true;
            KeyPreview = false;
            loop.Enabled = false;
            camera.collapse();
            progressBar1.Value = 1;
            Invalidate();
            progressBar1.Value = 2;
            new Thread(() =>
            {
                progressBar1.Value = 5;
                world.Generate(progressBar1);
                camera.Reset();
                progressBar1.Value = 100;
                Invalidate();
                progressBar1.Visible = false;
                progressBar1.Enabled = false;
                KeyPreview = true;
            }).Start();
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

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                camera.VectorY = (int)(1 * camera.scale);
                loop.Enabled = true;
            }
            if (e.KeyCode == Keys.A)
            {
                camera.VectorX = (int)(1 * camera.scale);
                loop.Enabled = true;
            }
            if (e.KeyCode == Keys.S)
            {
                camera.VectorY = (int)(-1 * camera.scale);
                loop.Enabled = true;
            }
            if (e.KeyCode == Keys.D)
            {
                camera.VectorX = (int)(-1 * camera.scale);
                loop.Enabled = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.S)
            {
                camera.VectorY = 0;
                if (camera.VectorX == 0 && camera.VectorY == 0)
                    loop.Enabled = false;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
            {
                camera.VectorX = 0;
                if (camera.VectorX == 0 && camera.VectorY == 0)
                    loop.Enabled = false;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            camera.Update();
            Invalidate();
        }

        private void Update_Map(object sender, EventArgs e)
        {
            GenerateWorld();
        }

        private void check_grid_CheckedChanged(object sender, EventArgs e)
        {
            camera.GridPaint = check_grid.Checked;
            Invalidate();
        }
    }
}