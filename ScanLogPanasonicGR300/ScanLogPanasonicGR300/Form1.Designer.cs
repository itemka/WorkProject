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
            this.minoltaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.minoltaToolStripMenuItem});
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
            // minoltaToolStripMenuItem
            // 
            this.minoltaToolStripMenuItem.Name = "minoltaToolStripMenuItem";
            this.minoltaToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.minoltaToolStripMenuItem.Text = "Minolta";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(251, 331);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "ScanLog";
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
        private System.Windows.Forms.ToolStripMenuItem minoltaToolStripMenuItem;
    }
}

