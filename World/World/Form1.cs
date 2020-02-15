﻿using System;
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
        public const int MAX_AREA = 700;
        public const int CELL_SIZE = 10;
        private World world;
        private DinamicCamera dinamincCamera;
        private Camera miniCamera;
        private Camera normalCamera;
        private Grid grid;
        private RadioButton[] buttonsOfType;
        private int type;

        public Form1()
        {
            InitializeComponent();
            initWorld();
            GenerateWorld();
            initForm();
        }

        private void initWorld()
        {
            type = 0;
            world = new World(new Size(WORK_AREA / CELL_SIZE, WORK_AREA / CELL_SIZE));
            dinamincCamera = new DinamicCamera(
                this,
                new Rectangle(miniMaps_panel.Width, 0, WORK_AREA, WORK_AREA),
                new Rectangle(miniMaps_panel.Width, 0, WORK_AREA > 700 ? MAX_AREA : WORK_AREA, WORK_AREA > 700 ? MAX_AREA : WORK_AREA),
                2);
            miniCamera = new Camera(
                mini_map,
                new Rectangle(0, 0, WORK_AREA, WORK_AREA),
                new Rectangle(0, 0, mini_map.Width, mini_map.Height));
            normalCamera = new Camera(
                normalMap,
                new Rectangle(0, 0, WORK_AREA, WORK_AREA),
                new Rectangle(0, 0, normalMap.Width, normalMap.Height));
        }

        private void initForm()
        {
            this.MouseWheel += dinamincCamera.zoom;
            this.DoubleBuffered = true;
            this.Size = new Size(
                (WORK_AREA > 700 ? MAX_AREA : WORK_AREA) + control_panel.Width + miniMaps_panel.Width + 16,
                (WORK_AREA > 700 ? MAX_AREA : WORK_AREA) + 39);
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
            progressBar1.Size = new Size(WORK_AREA / 4, 23);
            progressBar1.Location = new Point(
                dinamincCamera.Screen.X + (dinamincCamera.Screen.Width - progressBar1.Size.Width) / 2,
                dinamincCamera.Screen.Y + (dinamincCamera.Screen.Height - progressBar1.Size.Height) / 2);
        }

        private void AddContentOnCamers()
        {
            for (int i = 0; i < world.Size.Width; i++)
            {
                for (int j = 0; j < world.Size.Height; j++)
                {
                    for (int n = 0; n < world[i, j].Size.Width; n++)
                    {
                        for (int m = 0; m < world[i, j].Size.Height; m++)
                        {
                            dinamincCamera.Add((IPaintable)world[i, j][n, m].Height);
                            miniCamera.Add((IPaintable)world[i, j][n, m].Height);
                            normalCamera.Add((IPaintable)world[i, j][n, m].Normal);
                        }
                    }
                }
            }
            dinamincCamera.UpdateImage();
            miniCamera.UpdateImage();
            normalCamera.UpdateImage();
            this.Invalidate();
            mini_map.Invalidate();
            normalMap.Invalidate();
        }

        private void GenerateWorld()
        {
            progressBar1.Value = 0;
            progressBar1.Visible = true;
            progressBar1.Enabled = true;
            KeyPreview = false;
            move.Enabled = false;
            progressBar1.Value = 2;
            dinamincCamera.Clear(this);
            miniCamera.Clear(mini_map);
            normalCamera.Clear(normalMap);
            progressBar1.Value = 5;
            new Thread(() =>
            {
                progressBar1.Value = 10;
                world.Generate(progressBar1, type);
                progressBar1.Value = 98;
                AddContentOnCamers();
                progressBar1.Value = 100;
                progressBar1.Visible = false;
                progressBar1.Enabled = false;
                KeyPreview = true;
            }).Start();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            //e.Graphics.FillRectangle(Brushes.Black, 0, 0, 700, 700);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {

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
            try
            {
                GenerateWorld();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
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
            this.Invalidate();
        }

        private void zoom_Tick(object sender, EventArgs e)
        {
            dinamincCamera.zoom(this, (MouseEventArgs)zoom.Tag);
        }

        private void move_Tick(object sender, EventArgs e)
        {
            dinamincCamera.Update();
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

        private class Grid : IPaintable
        {
            public event EventHandler changed;

            public void Paint(Bitmap bitmap)
            {
                DrawGrid(bitmap, Form1.WORK_AREA, Form1.WORK_AREA, Form1.CELL_SIZE, Color.LightGray);
                DrawGrid(bitmap, Form1.WORK_AREA, Form1.WORK_AREA, Form1.CELL_SIZE * 10, Color.Gray);
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

        private void button2_Click(object sender, EventArgs e)
        {
        }
    }
}