namespace TVSender
{
    partial class AskSoundForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AskSoundForm) );
            this.label1 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxTimeDomainLeft = new System.Windows.Forms.PictureBox();
            this.pictureBoxTimeDomainRight = new System.Windows.Forms.PictureBox();
            this.button3 = new System.Windows.Forms.Button();
            ( (System.ComponentModel.ISupportInitialize)(this.pictureBox1) ).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ( (System.ComponentModel.ISupportInitialize)(this.pictureBoxTimeDomainLeft) ).BeginInit();
            ( (System.ComponentModel.ISupportInitialize)(this.pictureBoxTimeDomainRight) ).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.MediumSpringGreen;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 23F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.label1.ForeColor = System.Drawing.Color.FromArgb( ((int)( ((byte)(227) )) ), ( (int)( ((byte)(4) )) ), ( (int)( ((byte)(71) )) ));
            this.label1.Location = new System.Drawing.Point(-3, -6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(643, 113);
            this.label1.TabIndex = 5;
            this.label1.Text = "LABEL проверье изображения скарт и наличие двух осциллограм блабла аблабла";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // button2
            // 
            this.button2.BackgroundImage = ( (System.Drawing.Image)(resources.GetObject("button2.BackgroundImage") ));
            this.button2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button2.Location = new System.Drawing.Point(483, 381);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(161, 135);
            this.button2.TabIndex = 0;
            this.button2.TabStop = false;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackgroundImage = ( (System.Drawing.Image)(resources.GetObject("button1.BackgroundImage") ));
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button1.Location = new System.Drawing.Point(-3, 381);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(161, 135);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(-3, 136);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(643, 377);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(-3, 100);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.pictureBoxTimeDomainLeft);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBoxTimeDomainRight);
            this.splitContainer1.Size = new System.Drawing.Size(643, 282);
            this.splitContainer1.SplitterDistance = 142;
            this.splitContainer1.TabIndex = 8;
            // 
            // pictureBoxTimeDomainLeft
            // 
            this.pictureBoxTimeDomainLeft.BackColor = System.Drawing.Color.White;
            this.pictureBoxTimeDomainLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTimeDomainLeft.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTimeDomainLeft.Name = "pictureBoxTimeDomainLeft";
            this.pictureBoxTimeDomainLeft.Size = new System.Drawing.Size(643, 142);
            this.pictureBoxTimeDomainLeft.TabIndex = 3;
            this.pictureBoxTimeDomainLeft.TabStop = false;
            // 
            // pictureBoxTimeDomainRight
            // 
            this.pictureBoxTimeDomainRight.BackColor = System.Drawing.Color.White;
            this.pictureBoxTimeDomainRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBoxTimeDomainRight.Location = new System.Drawing.Point(0, 0);
            this.pictureBoxTimeDomainRight.Name = "pictureBoxTimeDomainRight";
            this.pictureBoxTimeDomainRight.Size = new System.Drawing.Size(643, 136);
            this.pictureBoxTimeDomainRight.TabIndex = 6;
            this.pictureBoxTimeDomainRight.TabStop = false;
            // 
            // button3
            // 
            this.button3.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.button3.BackgroundImage = ( (System.Drawing.Image)(resources.GetObject("button3.BackgroundImage") ));
            this.button3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.button3.Font = new System.Drawing.Font("Romantic", 27F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.button3.Location = new System.Drawing.Point(240, 381);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(161, 135);
            this.button3.TabIndex = 10;
            this.button3.UseVisualStyleBackColor = false;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // AskSoundForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(641, 512);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.IsMdiContainer = true;
            this.Name = "AskSoundForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AskForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AskForm_FormClosing);
            this.Load += new System.EventHandler(this.AskSoundForm_Load);
            this.SizeChanged += new System.EventHandler(this.AskSoundForm_SizeChanged);
            ( (System.ComponentModel.ISupportInitialize)(this.pictureBox1) ).EndInit();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ( (System.ComponentModel.ISupportInitialize)(this.pictureBoxTimeDomainLeft) ).EndInit();
            ( (System.ComponentModel.ISupportInitialize)(this.pictureBoxTimeDomainRight) ).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBoxTimeDomainLeft;
        private System.Windows.Forms.PictureBox pictureBoxTimeDomainRight;
        private System.Windows.Forms.Button button3;
    }
}