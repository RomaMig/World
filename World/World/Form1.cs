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
using World.CameraSystem;
using World.Environment.Map;
using World.Environment;
using World.Utilite;
using World.EngineSystem;
using World.EngineSystem.OptimizeSystem;

namespace World
{
    public partial class Form1 : Form
    {
        public const int WORLD_SIZE = 600;
        public const int MAX_SIZE = 500;
        public const int CELL_SIZE = 10;
        public static readonly int CURRENT_SIZE = WORLD_SIZE > MAX_SIZE ? MAX_SIZE : WORLD_SIZE;
        public readonly Point WORLD_LOCATION;
        private MapWorld world;
        private Optimize optimize;
        private DinamicCamera dinamincCamera;
        private ICamera miniCamera;
        private ICamera normalCamera;
        private ICamera tempCamera;
        private Task genTask;
        private Grid grid;
        private RadioButton[] buttonsOfType;
        private int type;
        private double averageTime;
        private long iterrations;

        public Form1()
        {
            InitializeComponent();
            WORLD_LOCATION = new Point(miniMaps_panel.Width, 0);
            iterrations = 0;
            averageTime = 0;
            initWorld();
            initForm();
            initComponents();
            GenerateWorld();
        }

        private void initWorld()
        {
            type = 0;
            dinamincCamera = new DinamicCamera(
                this,
                new Rectangle(0, 0, WORLD_SIZE, WORLD_SIZE),
                new Rectangle(WORLD_LOCATION.X, WORLD_LOCATION.Y, CURRENT_SIZE, CURRENT_SIZE),
                1);
            miniCamera = new Camera(
                mini_map,
                new Rectangle(0, 0, WORLD_SIZE, WORLD_SIZE),
                new Rectangle(0, 0, mini_map.Width, mini_map.Height),
                true);
            normalCamera = new Camera(
                normal_map,
                new Rectangle(0, 0, WORLD_SIZE, WORLD_SIZE),
                new Rectangle(0, 0, normal_map.Width, normal_map.Height),
                true);
            tempCamera = new Camera(
                temp_map,
                new Rectangle(0, 0, WORLD_SIZE / CELL_SIZE, WORLD_SIZE / CELL_SIZE),
                new Rectangle(0, 0, temp_map.Width, temp_map.Height),
                true);
            world = new MapWorld(new Size(WORLD_SIZE / CELL_SIZE, WORLD_SIZE / CELL_SIZE));
            world.Engine.AddPost(
                dinamincCamera,
                //miniCamera,
                //normalCamera,
                tempCamera
                );
        }

        private void initForm()
        {
            this.MouseWheel += zoom_Event;
            this.DoubleBuffered = true;
            this.Size = new Size(
                CURRENT_SIZE + control_panel.Width + miniMaps_panel.Width + 16,
                CURRENT_SIZE + 39);
            CenterToScreen();
        }

        private void initComponents()
        {
            buttonsOfType = new RadioButton[8]
            {
                continent,
                archipelago,
                mountains,
                hills,
                plains,
                sea,
                optional,
                randomly
            };
            progressBar1.Size = new Size(CURRENT_SIZE / 4, 23);
            progressBar1.Location = new Point(
                WORLD_LOCATION.X + (CURRENT_SIZE - progressBar1.Size.Width) / 2,
                WORLD_LOCATION.Y + (CURRENT_SIZE - progressBar1.Size.Height) / 2);
        }

        private void AddContentOnCamers()
        {
            for (int i = 0; i < world.Width; i++)
            {
                for (int j = 0; j < world.Height; j++)
                {
                    for (int n = 0; n < world[i, j].Width; n++)
                    {
                        for (int m = 0; m < world[i, j].Height; m++)
                        {
                            dinamincCamera.Add((IPaintable)world[i, j][n, m].Height);
                            miniCamera.Add((IPaintable)world[i, j][n, m].Height);
                            normalCamera.Add((IPaintable)world[i, j][n, m].Normal);
                        }
                    }
                    tempCamera.Add((IPaintable)world[i, j].Temperature);
                }
            }
            dinamincCamera.Update().Wait();
            miniCamera.Update().Wait();
            normalCamera.Update().Wait();
            tempCamera.Update().Wait();
            PaintCamera(dinamincCamera, this);
            PaintCamera(miniCamera, mini_map);
            PaintCamera(normalCamera, normal_map);
            PaintCamera(tempCamera, temp_map);
        }

        private void GenerateWorld()
        {
            genTask = Task.Run(() =>
            {
                KeyPreview = false;
                move.Enabled = false;

                world.Engine.Stop();

                dinamincCamera.Clear(this);
                miniCamera.Clear(mini_map);
                normalCamera.Clear(normal_map);
                tempCamera.Clear(temp_map);

                progressBar1.Value = 0;
                progressBar1.Visible = true;
                progressBar1.Enabled = true;
                progressBar1.Value = 8;
                world.Generate(progressBar1, type);
                optimize = new Optimize(world);
                optimize.Ready(dinamincCamera.Bound);
                world.Engine.AddPre(optimize);
                progressBar1.Value = 98;
                AddContentOnCamers();
                world.Engine.Start();
                progressBar1.Value = 99;
                world.Engine.Work(null,
                    () =>
                    {
                        PaintCamera(dinamincCamera, this);
                        PaintCamera(tempCamera, temp_map);
                        long time = world.Engine.Watch.ElapsedMilliseconds;
                        if (iterrations != 0)
                            averageTime = (averageTime * iterrations + time) * 1.0 / ++iterrations;
                        else
                            ++iterrations;
                        Text = "crrnt tm: " + time + " avrg tm: " + averageTime;
                    },
                    (Exception e) =>
                    {
                        MessageBox.Show(e.StackTrace + "\n" + e.InnerException);
                    });
                progressBar1.Value = 100;
                progressBar1.Visible = false;
                progressBar1.Enabled = false;
                KeyPreview = true;
            });
        }

        private void PaintCamera(ICamera cam, Control con)
        {
            cam.newImage();
            con.Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(Brushes.Black, 0, 0, 700, 700);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            world.Engine.Stop();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.I)
            {
                MessageBox.Show(dinamincCamera.Info());
            }
            if (e.KeyCode == Keys.W)
            {
                dinamincCamera.DirectionY = 1;//(int)(1 * dinamincCamera.Scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.A)
            {
                dinamincCamera.DirectionX = 1;// (int)(1 * dinamincCamera.Scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.S)
            {
                dinamincCamera.DirectionY = -1;//(int)(-1 * dinamincCamera.Scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.D)
            {
                dinamincCamera.DirectionX = -1;//(int)(-1 * dinamincCamera.Scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.Add)
            {
                zoom.Tag = new MouseEventArgs(MouseButtons.None, 0, WORLD_LOCATION.X + CURRENT_SIZE / 2, WORLD_LOCATION.Y + CURRENT_SIZE / 2, 10);
                zoom.Enabled = true;
            }
            if (e.KeyCode == Keys.Subtract)
            {
                zoom.Tag = new MouseEventArgs(MouseButtons.None, 0, WORLD_LOCATION.X + CURRENT_SIZE / 2, WORLD_LOCATION.Y + CURRENT_SIZE / 2, -10);
                zoom.Enabled = true;
            }
        }

        private void Form1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.W || e.KeyCode == Keys.S)
            {
                dinamincCamera.DirectionY = 0;
                if (dinamincCamera.DirectionX == 0 && dinamincCamera.DirectionY == 0)
                    move.Enabled = false;
            }
            if (e.KeyCode == Keys.A || e.KeyCode == Keys.D)
            {
                dinamincCamera.DirectionX = 0;
                if (dinamincCamera.DirectionX == 0 && dinamincCamera.DirectionY == 0)
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
            if (genTask == null || genTask.Status != TaskStatus.Running || genTask.Status == TaskStatus.RanToCompletion)
            {
                GenerateWorld();
                check_grid.Checked = false;
            }
        }

        private void check_grid_CheckedChanged(object sender, EventArgs e)
        {
            if (check_grid.Checked)
            {
                if (grid == null) grid = new Grid();
                dinamincCamera.Add(grid);
            }
            else
            {
                dinamincCamera.Remove(grid);
                dinamincCamera.Repaint();
            }
            Invalidate();
        }

        private void zoom_Tick(object sender, EventArgs e)
        {
            zoom_Event(this, (MouseEventArgs)zoom.Tag);
        }

        private void zoom_Event(object sender, MouseEventArgs e)
        {
            dinamincCamera.zoom(e);
            optimize.Ready(dinamincCamera.Bound);
            Invalidate();
        }

        private void move_Tick(object sender, EventArgs e)
        {
            dinamincCamera.Moving();
            optimize.Ready(dinamincCamera.Bound);
            Invalidate();
        }
        private void Form1_Resize(object sender, EventArgs e)
        {

        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            type = Array.IndexOf(buttonsOfType, radioButton);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}