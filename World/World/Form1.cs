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
using World.Cameras;
using World.Environment.Map;
using World.Environment;
using World.Utilite;
using World.EngineSystem;

namespace World
{
    public partial class Form1 : Form
    {
        public const int WORLD_SIZE = 600;
        public const int MAX_SIZE = 700;
        public const int CELL_SIZE = 10;
        public readonly Point WORLD_LOCATION;
        private MapWorld world;
        private DinamicCamera dinamincCamera;
        private Camera miniCamera;
        private Camera normalCamera;
        private Camera tempCamera;
        private Grid grid;
        private RadioButton[] buttonsOfType;
        private int type;

        public Form1()
        {
            InitializeComponent();
            WORLD_LOCATION = new Point(miniMaps_panel.Width, 0);
            initWorld();
            GenerateWorld();
            initForm();
        }

        private void initWorld()
        {
            type = 0;
            dinamincCamera = new DinamicCamera(
                this,
                new Rectangle(WORLD_LOCATION.X, WORLD_LOCATION.Y, WORLD_SIZE, WORLD_SIZE),
                new Rectangle(WORLD_LOCATION.X, WORLD_LOCATION.Y, WORLD_SIZE > 700 ? MAX_SIZE : WORLD_SIZE, WORLD_SIZE > 700 ? MAX_SIZE : WORLD_SIZE),
                2);
            miniCamera = new Camera(
                mini_map,
                new Rectangle(0, 0, WORLD_SIZE, WORLD_SIZE),
                new Rectangle(0, 0, mini_map.Width, mini_map.Height));
            normalCamera = new Camera(
                normal_map,
                new Rectangle(0, 0, WORLD_SIZE, WORLD_SIZE),
                new Rectangle(0, 0, normal_map.Width, normal_map.Height));
            tempCamera = new Camera(
                temp_map,
                new Rectangle(0, 0, WORLD_SIZE / CELL_SIZE, WORLD_SIZE / CELL_SIZE),
                new Rectangle(0, 0, temp_map.Width, temp_map.Height));
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
                (WORLD_SIZE > 700 ? MAX_SIZE : WORLD_SIZE) + control_panel.Width + miniMaps_panel.Width + 16,
                (WORLD_SIZE > 700 ? MAX_SIZE : WORLD_SIZE) + 39);
            CenterToScreen();
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
            progressBar1.Size = new Size(WORLD_SIZE / 4, 23);
            progressBar1.Location = new Point(
                dinamincCamera.Screen.X + (dinamincCamera.Screen.Width - progressBar1.Size.Width) / 2,
                dinamincCamera.Screen.Y + (dinamincCamera.Screen.Height - progressBar1.Size.Height) / 2);
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
            Task.Run(() =>
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
                progressBar1.Value = 98;
                AddContentOnCamers();
                world.Engine.Start();
                progressBar1.Value = 99;
                world.Engine.Work(null,
                    () =>
                    {
                        PaintCamera(dinamincCamera, this);
                        PaintCamera(tempCamera, temp_map);
                        Text = world.Engine.Watch.ElapsedMilliseconds + "";
                    },
                    (Exception e) =>
                    {
                        MessageBox.Show(e.StackTrace+"\n"+e.InnerException);
                    });
                progressBar1.Value = 100;
                progressBar1.Visible = false;
                progressBar1.Enabled = false;
                KeyPreview = true;
            });
        }

        private void PaintCamera(Camera cam, Control con)
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
            if (e.KeyCode == Keys.W)
            {
                dinamincCamera.DirectionY = (int)(1 * dinamincCamera.Scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.A)
            {
                dinamincCamera.DirectionX = (int)(1 * dinamincCamera.Scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.S)
            {
                dinamincCamera.DirectionY = (int)(-1 * dinamincCamera.Scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.D)
            {
                dinamincCamera.DirectionX = (int)(-1 * dinamincCamera.Scale);
                move.Enabled = true;
            }
            if (e.KeyCode == Keys.Add)
            {
                zoom.Tag = new MouseEventArgs(MouseButtons.None, 0, WORLD_LOCATION.X + WORLD_SIZE / 2, WORLD_LOCATION.Y + WORLD_SIZE / 2, 10);
                zoom.Enabled = true;
            }
            if (e.KeyCode == Keys.Subtract)
            {
                zoom.Tag = new MouseEventArgs(MouseButtons.None, 0, WORLD_LOCATION.X + WORLD_SIZE / 2, WORLD_LOCATION.Y + WORLD_SIZE / 2, -10);
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
            GenerateWorld();
            check_grid.Checked = false;
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
            Invalidate();
        }

        private void move_Tick(object sender, EventArgs e)
        {
            dinamincCamera.Moving();
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
        {/*
            tempCamera.newImage();
            temp_map.Invalidate();*/
            for (int i = 0; i < world.Width; i++)
            {
                for (int j = 0; j < world.Height; j++)
                {
                    for (int n = 0; n < world[i, j].Width; n++)
                    {
                        for (int m = 0; m < world[i, j].Height; m++)
                        {
                            world[i, j][n, m].Height.Value += 0.1;
                        }
                    }
                }
            }
        }
    }
}