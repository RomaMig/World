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
        public const int WORK_AREA = 600;
        public const int CELL_SIZE = 10;
        private World world;
        private DinamicCamera camera;

        public Form1()
        {
            InitializeComponent();
            initWorld();
            GenerateWorld();
            initForm();
        }

        private void initWorld()
        {
            world = new World(new Size(WORK_AREA / CELL_SIZE, WORK_AREA / CELL_SIZE));
            camera = new DinamicCamera(this, new Rectangle(0, 0, WORK_AREA, WORK_AREA), new Rectangle(0, 0, WORK_AREA, WORK_AREA), 2);
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

        private void AddHeights()
        {
            for (int i = 0; i < world.Size.Width; i++)
            {
                for (int j = 0; j < world.Size.Height; j++)
                {
                    for (int n = 0; n < world[i, j].Size.Width; n++)
                    {
                        for (int m = 0; m < world[i, j].Size.Height; m++)
                        {
                            camera.Add(world[i, j].heights[n, m]);
                        }
                    }
                }
            }
        }

        private void GenerateWorld()
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.Enabled = true;
            KeyPreview = false;
            move.Enabled = false;
            //camera.collapse();
            progressBar1.Value = 1;
            Invalidate();
            progressBar1.Value = 2;
            new Thread(() =>
            {
                progressBar1.Value = 5;
                world.Generate(progressBar1);
                AddHeights();
                progressBar1.Value = 100;
                Invalidate();
                progressBar1.Visible = false;
                progressBar1.Enabled = false;
                KeyPreview = true;
            }).Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W)
            {
                camera.VectorY = (int)(1 * camera.scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.A)
            {
                camera.VectorX = (int)(1 * camera.scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.S)
            {
                camera.VectorY = (int)(-1 * camera.scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.D)
            {
                camera.VectorX = (int)(-1 * camera.scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.Add)
            {
                zoom.Tag = new MouseEventArgs(MouseButtons.None, 0, WORK_AREA / 2, WORK_AREA / 2, 10);
                zoom.Enabled = true;
            }
            if (e.KeyCode == Keys.Subtract)
            {
                zoom.Tag = new MouseEventArgs(MouseButtons.None, 0, WORK_AREA / 2, WORK_AREA / 2, -10);
                zoom.Enabled = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.S)
            {
                camera.VectorY = 0;
                if (camera.VectorX == 0 && camera.VectorY == 0)
                    move.Enabled = false;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
            {
                camera.VectorX = 0;
                if (camera.VectorX == 0 && camera.VectorY == 0)
                    move.Enabled = false;
            }
            if (e.KeyCode == Keys.Add)
            {
                zoom.Enabled = false;
            }
            if (e.KeyCode == Keys.Subtract)
            {
                zoom.Enabled = false;
            }
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Update_Map(object sender, EventArgs e)
        {
            GenerateWorld();
        }

        private void check_grid_CheckedChanged(object sender, EventArgs e)
        {
            //camera.GridPaint = check_grid.Checked;
            //Invalidate();
        }

        private void zoom_Tick(object sender, EventArgs e)
        {
            camera.zoom(this, (MouseEventArgs)zoom.Tag);
        }

        private void move_Tick(object sender, EventArgs e)
        {
            camera.Update();
            Invalidate();
        }
    }
}