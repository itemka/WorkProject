namespace TVSender
{
    partial class FormScaner
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null) )
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormScaner) );
            this.textBoxSN = new System.Windows.Forms.TextBox();
            this.textBoxDefect = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelSNRed = new System.Windows.Forms.Label();
            this.labelDefectRed = new System.Windows.Forms.Label();
            this.YesBox1 = new System.Windows.Forms.PictureBox();
            this.YesBox2 = new System.Windows.Forms.PictureBox();
            this.NoBox1 = new System.Windows.Forms.PictureBox();
            this.NoBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            ( (System.ComponentModel.ISupportInitialize)(this.YesBox1) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)(this.YesBox2) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)(this.NoBox1) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)(this.NoBox2) ).BeginInit();
            this.SuspendLayout();
            // 
            // textBoxSN
            // 
            this.textBoxSN.BackColor = System.Drawing.Color.White;
            this.textBoxSN.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.textBoxSN.Location = new System.Drawing.Point(166, 110);
            this.textBoxSN.Name = "textBoxSN";
            this.textBoxSN.Size = new System.Drawing.Size(233, 29);
            this.textBoxSN.TabIndex = 0;
            this.textBoxSN.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox1_MouseClick);
            this.textBoxSN.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBoxSN.Enter += new System.EventHandler(this.textBox1_Enter);
            // 
            // textBoxDefect
            // 
            this.textBoxDefect.BackColor = System.Drawing.Color.White;
            this.textBoxDefect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.textBoxDefect.Location = new System.Drawing.Point(202, 209);
            this.textBoxDefect.Name = "textBoxDefect";
            this.textBoxDefect.Size = new System.Drawing.Size(197, 30);
            this.textBoxDefect.TabIndex = 1;
            this.textBoxDefect.MouseClick += new System.Windows.Forms.MouseEventHandler(this.textBox2_MouseClick);
            this.textBoxDefect.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            this.textBoxDefect.Enter += new System.EventHandler(this.textBox2_Enter);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.button1.Location = new System.Drawing.Point(177, 281);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(103, 38);
            this.button1.TabIndex = 2;
            this.button1.TabStop = false;
            this.button1.Text = "Enter";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.label1.Location = new System.Drawing.Point(19, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(146, 31);
            this.label1.TabIndex = 3;
            this.label1.Text = "Штрих-код";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.label2.Location = new System.Drawing.Point(19, 206);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(179, 31);
            this.label2.TabIndex = 4;
            this.label2.Text = "Код дефекта";
            // 
            // labelSNRed
            // 
            this.labelSNRed.AutoSize = true;
            this.labelSNRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.labelSNRed.ForeColor = System.Drawing.Color.Red;
            this.labelSNRed.Location = new System.Drawing.Point(19, 76);
            this.labelSNRed.Name = "labelSNRed";
            this.labelSNRed.Size = new System.Drawing.Size(351, 31);
            this.labelSNRed.TabIndex = 5;
            this.labelSNRed.Text = "Отсканируйте штрих-код";
            this.labelSNRed.Visible = false;
            // 
            // labelDefectRed
            // 
            this.labelDefectRed.AutoSize = true;
            this.labelDefectRed.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.labelDefectRed.ForeColor = System.Drawing.Color.Red;
            this.labelDefectRed.Location = new System.Drawing.Point(19, 169);
            this.labelDefectRed.Name = "labelDefectRed";
            this.labelDefectRed.Size = new System.Drawing.Size(306, 31);
            this.labelDefectRed.TabIndex = 6;
            this.labelDefectRed.Text = "Введите код дефекта";
            this.labelDefectRed.Visible = false;
            // 
            // YesBox1
            // 
            this.YesBox1.Image = ( (System.Drawing.Image)(resources.GetObject("YesBox1.Image") ));
            this.YesBox1.InitialImage = ( (System.Drawing.Image)(resources.GetObject("YesBox1.InitialImage") ));
            this.YesBox1.Location = new System.Drawing.Point(405, 104);
            this.YesBox1.Name = "YesBox1";
            this.YesBox1.Size = new System.Drawing.Size(40, 35);
            this.YesBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.YesBox1.TabIndex = 7;
            this.YesBox1.TabStop = false;
            this.YesBox1.Visible = false;
            // 
            // YesBox2
            // 
            this.YesBox2.Image = ( (System.Drawing.Image)(resources.GetObject("YesBox2.Image") ));
            this.YesBox2.InitialImage = ( (System.Drawing.Image)(resources.GetObject("YesBox2.InitialImage") ));
            this.YesBox2.Location = new System.Drawing.Point(405, 203);
            this.YesBox2.Name = "YesBox2";
            this.YesBox2.Size = new System.Drawing.Size(40, 35);
            this.YesBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.YesBox2.TabIndex = 8;
            this.YesBox2.TabStop = false;
            this.YesBox2.Visible = false;
            // 
            // NoBox1
            // 
            this.NoBox1.Image = ( (System.Drawing.Image)(resources.GetObject("NoBox1.Image") ));
            this.NoBox1.InitialImage = null;
            this.NoBox1.Location = new System.Drawing.Point(401, 104);
            this.NoBox1.Name = "NoBox1";
            this.NoBox1.Size = new System.Drawing.Size(46, 41);
            this.NoBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.NoBox1.TabIndex = 9;
            this.NoBox1.TabStop = false;
            this.NoBox1.Visible = false;
            // 
            // NoBox2
            // 
            this.NoBox2.Image = ( (System.Drawing.Image)(resources.GetObject("NoBox2.Image") ));
            this.NoBox2.InitialImage = null;
            this.NoBox2.Location = new System.Drawing.Point(401, 203);
            this.NoBox2.Name = "NoBox2";
            this.NoBox2.Size = new System.Drawing.Size(46, 41);
            this.NoBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.NoBox2.TabIndex = 10;
            this.NoBox2.TabStop = false;
            this.NoBox2.Visible = false;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb( ((int)( ((byte)(191) )) ), ( (int)( ((byte)(0) )) ), ( (int)( ((byte)(0) )) ));
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(-2, -1);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(476, 64);
            this.label5.TabIndex = 11;
            this.label5.Text = "Забраковать";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // FormScaner
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(471, 338);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.YesBox2);
            this.Controls.Add(this.YesBox1);
            this.Controls.Add(this.labelDefectRed);
            this.Controls.Add(this.labelSNRed);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxDefect);
            this.Controls.Add(this.textBoxSN);
            this.Controls.Add(this.NoBox1);
            this.Controls.Add(this.NoBox2);
            this.Name = "FormScaner";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Забраковать";
            ( (System.ComponentModel.ISupportInitialize)(this.YesBox1) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)(this.YesBox2) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)(this.NoBox1) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)(this.NoBox2) ).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxSN;
        private System.Windows.Forms.TextBox textBoxDefect;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelSNRed;
        private System.Windows.Forms.Label labelDefectRed;
        private System.Windows.Forms.PictureBox YesBox1;
        private System.Windows.Forms.PictureBox YesBox2;
        private System.Windows.Forms.PictureBox NoBox1;
        private System.Windows.Forms.PictureBox NoBox2;
        private System.Windows.Forms.Label label5;
    }
}