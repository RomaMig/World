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
            this.settings_map_panel = new System.Windows.Forms.GroupBox();
            this.randomly = new System.Windows.Forms.RadioButton();
            this.optional = new System.Windows.Forms.RadioButton();
            this.sea = new System.Windows.Forms.RadioButton();
            this.plains = new System.Windows.Forms.RadioButton();
            this.hills = new System.Windows.Forms.RadioButton();
            this.mountains = new System.Windows.Forms.RadioButton();
            this.archipelago = new System.Windows.Forms.RadioButton();
            this.continent = new System.Windows.Forms.RadioButton();
            this.check_grid = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.move = new System.Windows.Forms.Timer(this.components);
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.control_panel = new System.Windows.Forms.GroupBox();
            this.mini_map_title = new System.Windows.Forms.GroupBox();
            this.mini_map = new System.Windows.Forms.Panel();
            this.zoom = new System.Windows.Forms.Timer(this.components);
            this.miniMaps_panel = new System.Windows.Forms.GroupBox();
            this.normal_map = new System.Windows.Forms.Panel();
            this.temp_map = new System.Windows.Forms.Panel();
            this.settings_map_panel.SuspendLayout();
            this.control_panel.SuspendLayout();
            this.mini_map_title.SuspendLayout();
            this.miniMaps_panel.SuspendLayout();
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
            // settings_map_panel
            // 
            this.settings_map_panel.Controls.Add(this.randomly);
            this.settings_map_panel.Controls.Add(this.optional);
            this.settings_map_panel.Controls.Add(this.sea);
            this.settings_map_panel.Controls.Add(this.plains);
            this.settings_map_panel.Controls.Add(this.hills);
            this.settings_map_panel.Controls.Add(this.mountains);
            this.settings_map_panel.Controls.Add(this.archipelago);
            this.settings_map_panel.Controls.Add(this.continent);
            this.settings_map_panel.Controls.Add(this.check_grid);
            this.settings_map_panel.Controls.Add(this.button2);
            this.settings_map_panel.Controls.Add(this.UpdateMap);
            this.settings_map_panel.Location = new System.Drawing.Point(12, 245);
            this.settings_map_panel.Name = "settings_map_panel";
            this.settings_map_panel.Size = new System.Drawing.Size(220, 186);
            this.settings_map_panel.TabIndex = 1;
            this.settings_map_panel.TabStop = false;
            this.settings_map_panel.Text = "Настройки карты";
            // 
            // randomly
            // 
            this.randomly.AutoSize = true;
            this.randomly.Location = new System.Drawing.Point(115, 119);
            this.randomly.Name = "randomly";
            this.randomly.Size = new System.Drawing.Size(78, 17);
            this.randomly.TabIndex = 10;
            this.randomly.Text = "Случайная";
            this.randomly.UseVisualStyleBackColor = true;
            this.randomly.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // optional
            // 
            this.optional.AutoSize = true;
            this.optional.Location = new System.Drawing.Point(115, 96);
            this.optional.Name = "optional";
            this.optional.Size = new System.Drawing.Size(50, 17);
            this.optional.TabIndex = 9;
            this.optional.Text = "Своя";
            this.optional.UseVisualStyleBackColor = true;
            this.optional.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // sea
            // 
            this.sea.AutoSize = true;
            this.sea.Location = new System.Drawing.Point(115, 73);
            this.sea.Name = "sea";
            this.sea.Size = new System.Drawing.Size(52, 17);
            this.sea.TabIndex = 8;
            this.sea.Text = "Море";
            this.sea.UseVisualStyleBackColor = true;
            this.sea.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // plains
            // 
            this.plains.AutoSize = true;
            this.plains.Location = new System.Drawing.Point(115, 49);
            this.plains.Name = "plains";
            this.plains.Size = new System.Drawing.Size(70, 17);
            this.plains.TabIndex = 7;
            this.plains.Text = "Равнины";
            this.plains.UseVisualStyleBackColor = true;
            this.plains.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // hills
            // 
            this.hills.AutoSize = true;
            this.hills.Location = new System.Drawing.Point(10, 119);
            this.hills.Name = "hills";
            this.hills.Size = new System.Drawing.Size(60, 17);
            this.hills.TabIndex = 6;
            this.hills.Text = "Холмы";
            this.hills.UseVisualStyleBackColor = true;
            this.hills.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // mountains
            // 
            this.mountains.AutoSize = true;
            this.mountains.Location = new System.Drawing.Point(10, 96);
            this.mountains.Name = "mountains";
            this.mountains.Size = new System.Drawing.Size(51, 17);
            this.mountains.TabIndex = 5;
            this.mountains.Text = "Горы";
            this.mountains.UseVisualStyleBackColor = true;
            this.mountains.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // archipelago
            // 
            this.archipelago.AutoSize = true;
            this.archipelago.Location = new System.Drawing.Point(10, 73);
            this.archipelago.Name = "archipelago";
            this.archipelago.Size = new System.Drawing.Size(78, 17);
            this.archipelago.TabIndex = 4;
            this.archipelago.Text = "Архипелаг";
            this.archipelago.UseVisualStyleBackColor = true;
            this.archipelago.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // continent
            // 
            this.continent.AutoSize = true;
            this.continent.Checked = true;
            this.continent.Location = new System.Drawing.Point(10, 49);
            this.continent.Name = "continent";
            this.continent.Size = new System.Drawing.Size(78, 17);
            this.continent.TabIndex = 3;
            this.continent.TabStop = true;
            this.continent.Text = "Континент";
            this.continent.UseVisualStyleBackColor = true;
            this.continent.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // check_grid
            // 
            this.check_grid.AutoSize = true;
            this.check_grid.Location = new System.Drawing.Point(10, 163);
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
            this.button2.Click += new System.EventHandler(this.button2_Click);
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
            this.control_panel.Controls.Add(this.mini_map_title);
            this.control_panel.Controls.Add(this.settings_map_panel);
            this.control_panel.Dock = System.Windows.Forms.DockStyle.Right;
            this.control_panel.Location = new System.Drawing.Point(640, 0);
            this.control_panel.Name = "control_panel";
            this.control_panel.Size = new System.Drawing.Size(244, 461);
            this.control_panel.TabIndex = 3;
            this.control_panel.TabStop = false;
            this.control_panel.Text = "Панель управления";
            // 
            // mini_map_title
            // 
            this.mini_map_title.Controls.Add(this.mini_map);
            this.mini_map_title.Location = new System.Drawing.Point(12, 19);
            this.mini_map_title.Name = "mini_map_title";
            this.mini_map_title.Size = new System.Drawing.Size(220, 220);
            this.mini_map_title.TabIndex = 2;
            this.mini_map_title.TabStop = false;
            this.mini_map_title.Text = "Мини карта";
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
            // miniMaps_panel
            // 
            this.miniMaps_panel.Controls.Add(this.temp_map);
            this.miniMaps_panel.Controls.Add(this.normal_map);
            this.miniMaps_panel.Dock = System.Windows.Forms.DockStyle.Left;
            this.miniMaps_panel.Location = new System.Drawing.Point(0, 0);
            this.miniMaps_panel.Name = "miniMaps_panel";
            this.miniMaps_panel.Size = new System.Drawing.Size(180, 461);
            this.miniMaps_panel.TabIndex = 4;
            this.miniMaps_panel.TabStop = false;
            this.miniMaps_panel.Text = "Карты";
            // 
            // normal_map
            // 
            this.normal_map.Location = new System.Drawing.Point(5, 16);
            this.normal_map.Name = "normal_map";
            this.normal_map.Size = new System.Drawing.Size(170, 170);
            this.normal_map.TabIndex = 0;
            // 
            // temp_map
            // 
            this.temp_map.Location = new System.Drawing.Point(5, 192);
            this.temp_map.Name = "temp_map";
            this.temp_map.Size = new System.Drawing.Size(170, 170);
            this.temp_map.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.miniMaps_panel);
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
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.settings_map_panel.ResumeLayout(false);
            this.settings_map_panel.PerformLayout();
            this.control_panel.ResumeLayout(false);
            this.mini_map_title.ResumeLayout(false);
            this.miniMaps_panel.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button UpdateMap;
        private System.Windows.Forms.GroupBox settings_map_panel;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Timer move;
        private System.Windows.Forms.GroupBox control_panel;
        public System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox check_grid;
        private System.Windows.Forms.Timer zoom;
        private System.Windows.Forms.GroupBox mini_map_title;
        private System.Windows.Forms.Panel mini_map;
        private System.Windows.Forms.RadioButton hills;
        private System.Windows.Forms.RadioButton mountains;
        private System.Windows.Forms.RadioButton archipelago;
        private System.Windows.Forms.RadioButton continent;
        private System.Windows.Forms.RadioButton randomly;
        private System.Windows.Forms.RadioButton optional;
        private System.Windows.Forms.RadioButton sea;
        private System.Windows.Forms.RadioButton plains;
        private System.Windows.Forms.GroupBox miniMaps_panel;
        private System.Windows.Forms.Panel normal_map;
        private System.Windows.Forms.Panel temp_map;
    }
}

