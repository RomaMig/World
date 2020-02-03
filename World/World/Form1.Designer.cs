namespace World
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.UpdateMap = new System.Windows.Forms.Button();
            this.settings_map = new System.Windows.Forms.GroupBox();
            this.check_grid = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.move = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.control_panel = new System.Windows.Forms.GroupBox();
            this.mini_map_group = new System.Windows.Forms.GroupBox();
            this.mini_map = new System.Windows.Forms.Panel();
            this.zoom = new System.Windows.Forms.Timer(this.components);
            this.settings_map.SuspendLayout();
            this.control_panel.SuspendLayout();
            this.mini_map_group.SuspendLayout();
            this.SuspendLayout();
            // 
            // UpdateMap
            // 
            this.UpdateMap.Location = new System.Drawing.Point(10, 19);
            this.UpdateMap.Name = "UpdateMap";
            this.UpdateMap.Size = new System.Drawing.Size(95, 23);
            this.UpdateMap.TabIndex = 0;
            this.UpdateMap.Text = "Обновить";
            this.UpdateMap.UseVisualStyleBackColor = true;
            this.UpdateMap.Click += new System.EventHandler(this.Update_Map);
            // 
            // settings_map
            // 
            this.settings_map.Controls.Add(this.check_grid);
            this.settings_map.Controls.Add(this.button2);
            this.settings_map.Controls.Add(this.UpdateMap);
            this.settings_map.Location = new System.Drawing.Point(12, 245);
            this.settings_map.Name = "settings_map";
            this.settings_map.Size = new System.Drawing.Size(220, 100);
            this.settings_map.TabIndex = 1;
            this.settings_map.TabStop = false;
            this.settings_map.Text = "Настройки карты";
            // 
            // check_grid
            // 
            this.check_grid.AutoSize = true;
            this.check_grid.Location = new System.Drawing.Point(12, 49);
            this.check_grid.Name = "check_grid";
            this.check_grid.Size = new System.Drawing.Size(56, 17);
            this.check_grid.TabIndex = 2;
            this.check_grid.Text = "Сетка";
            this.check_grid.UseVisualStyleBackColor = true;
            this.check_grid.CheckedChanged += new System.EventHandler(this.check_grid_CheckedChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(115, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Кнопка";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // move
            // 
            this.move.Interval = 15;
            this.move.Tick += new System.EventHandler(this.move_Tick);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(242, 267);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(100, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 2;
            // 
            // control_panel
            // 
            this.control_panel.Controls.Add(this.mini_map_group);
            this.control_panel.Controls.Add(this.settings_map);
            this.control_panel.Dock = System.Windows.Forms.DockStyle.Right;
            this.control_panel.Location = new System.Drawing.Point(616, 0);
            this.control_panel.Name = "control_panel";
            this.control_panel.Size = new System.Drawing.Size(244, 649);
            this.control_panel.TabIndex = 3;
            this.control_panel.TabStop = false;
            this.control_panel.Text = "Панель управления";
            // 
            // mini_map_group
            // 
            this.mini_map_group.Controls.Add(this.mini_map);
            this.mini_map_group.Location = new System.Drawing.Point(12, 19);
            this.mini_map_group.Name = "mini_map_group";
            this.mini_map_group.Size = new System.Drawing.Size(220, 220);
            this.mini_map_group.TabIndex = 2;
            this.mini_map_group.TabStop = false;
            this.mini_map_group.Text = "Мини карта";
            // 
            // mini_map
            // 
            this.mini_map.Location = new System.Drawing.Point(10, 16);
            this.mini_map.Name = "mini_map";
            this.mini_map.Size = new System.Drawing.Size(200, 200);
            this.mini_map.TabIndex = 0;
            // 
            // zoom
            // 
            this.zoom.Interval = 15;
            this.zoom.Tick += new System.EventHandler(this.zoom_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 649);
            this.Controls.Add(this.control_panel);
            this.Controls.Add(this.progressBar1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.settings_map.ResumeLayout(false);
            this.settings_map.PerformLayout();
            this.control_panel.ResumeLayout(false);
            this.mini_map_group.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UpdateMap;
        private System.Windows.Forms.GroupBox settings_map;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer move;
        private System.Windows.Forms.GroupBox control_panel;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox check_grid;
        private System.Windows.Forms.Timer zoom;
        private System.Windows.Forms.GroupBox mini_map_group;
        private System.Windows.Forms.Panel mini_map;
    }
}

