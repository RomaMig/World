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
            this.Settings_map = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.Map = new System.Windows.Forms.GroupBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.loop = new System.Windows.Forms.Timer(this.components);
            this.Settings_map.SuspendLayout();
            this.Map.SuspendLayout();
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
            this.UpdateMap.Click += new System.EventHandler(this.button1_Click);
            // 
            // Settings_map
            // 
            this.Settings_map.Controls.Add(this.button2);
            this.Settings_map.Controls.Add(this.UpdateMap);
            this.Settings_map.Location = new System.Drawing.Point(628, 12);
            this.Settings_map.Name = "Settings_map";
            this.Settings_map.Size = new System.Drawing.Size(220, 100);
            this.Settings_map.TabIndex = 1;
            this.Settings_map.TabStop = false;
            this.Settings_map.Text = "Настройки карты";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(115, 19);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(95, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Обновить";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // Map
            // 
            this.Map.Controls.Add(this.progressBar1);
            this.Map.Location = new System.Drawing.Point(12, 12);
            this.Map.Name = "Map";
            this.Map.Size = new System.Drawing.Size(610, 625);
            this.Map.TabIndex = 2;
            this.Map.TabStop = false;
            this.Map.Text = "Карта";
            this.Map.Visible = false;
            this.Map.BackgroundImageChanged += new System.EventHandler(this.Map_BackgroundImageChanged);
            // 
            // progressBar1
            // 
            this.progressBar1.Enabled = false;
            this.progressBar1.Location = new System.Drawing.Point(205, 301);
            this.progressBar1.MarqueeAnimationSpeed = 80;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(200, 23);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 0;
            this.progressBar1.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(628, 119);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 3;
            // 
            // loop
            // 
            this.loop.Interval = 15;
            this.loop.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 649);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Map);
            this.Controls.Add(this.Settings_map);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Form1_KeyPress);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyUp);
            this.Settings_map.ResumeLayout(false);
            this.Map.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button UpdateMap;
        private System.Windows.Forms.GroupBox Settings_map;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.GroupBox Map;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer loop;
    }
}

