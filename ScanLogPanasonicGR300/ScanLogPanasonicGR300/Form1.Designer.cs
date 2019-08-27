namespace ScanLogPanasonicGR300
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьПапкуСВыбраннойМодельюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.открытьФайлСозданныйСегодняToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.поискСканаВВСегоднешнемФайлеToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(251, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.открытьПапкуСВыбраннойМодельюToolStripMenuItem,
            this.открытьФайлСозданныйСегодняToolStripMenuItem,
            this.поискСканаВВСегоднешнемФайлеToolStripMenuItem});
            this.toolsToolStripMenuItem.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F);
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // открытьПапкуСВыбраннойМодельюToolStripMenuItem
            // 
            this.открытьПапкуСВыбраннойМодельюToolStripMenuItem.Name = "открытьПапкуСВыбраннойМодельюToolStripMenuItem";
            this.открытьПапкуСВыбраннойМодельюToolStripMenuItem.Size = new System.Drawing.Size(320, 22);
            this.открытьПапкуСВыбраннойМодельюToolStripMenuItem.Text = "Открыть папку с выбранной моделью";
            this.открытьПапкуСВыбраннойМодельюToolStripMenuItem.Click += new System.EventHandler(this.открытьПапкуСВыбраннойМодельюToolStripMenuItem_Click);
            // 
            // открытьФайлСозданныйСегодняToolStripMenuItem
            // 
            this.открытьФайлСозданныйСегодняToolStripMenuItem.Name = "открытьФайлСозданныйСегодняToolStripMenuItem";
            this.открытьФайлСозданныйСегодняToolStripMenuItem.Size = new System.Drawing.Size(320, 22);
            this.открытьФайлСозданныйСегодняToolStripMenuItem.Text = "Открыть файл созданный сегодня";
            this.открытьФайлСозданныйСегодняToolStripMenuItem.Click += new System.EventHandler(this.открытьФайлСозданныйСегодняToolStripMenuItem_Click);
            // 
            // поискСканаВВСегоднешнемФайлеToolStripMenuItem
            // 
            this.поискСканаВВСегоднешнемФайлеToolStripMenuItem.Name = "поискСканаВВСегоднешнемФайлеToolStripMenuItem";
            this.поискСканаВВСегоднешнемФайлеToolStripMenuItem.Size = new System.Drawing.Size(320, 22);
            this.поискСканаВВСегоднешнемФайлеToolStripMenuItem.Text = "Поиск скана в в сегоднешнем файле";
            this.поискСканаВВСегоднешнемФайлеToolStripMenuItem.Click += new System.EventHandler(this.поискСканаВВСегоднешнемФайлеToolStripMenuItem_Click);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.SystemColors.Control;
            this.textBox1.Font = new System.Drawing.Font("MS Reference Sans Serif", 9F);
            this.textBox1.Location = new System.Drawing.Point(-1, 53);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(253, 279);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.AcceptsReturn = true;
            this.textBox2.Font = new System.Drawing.Font("MS Reference Sans Serif", 9.75F);
            this.textBox2.Location = new System.Drawing.Point(12, 27);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(227, 23);
            this.textBox2.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.BackColor = System.Drawing.Color.DimGray;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.button1.Font = new System.Drawing.Font("Arial Rounded MT Bold", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(150, 0);
            this.button1.Margin = new System.Windows.Forms.Padding(0);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(101, 24);
            this.button1.TabIndex = 3;
            this.button1.Text = "(F4) Auto Return";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 331);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ScanLog";
            this.Deactivate += new System.EventHandler(this.Form1_Deactivate);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьПапкуСВыбраннойМодельюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem открытьФайлСозданныйСегодняToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem поискСканаВВСегоднешнемФайлеToolStripMenuItem;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button1;
    }
}

