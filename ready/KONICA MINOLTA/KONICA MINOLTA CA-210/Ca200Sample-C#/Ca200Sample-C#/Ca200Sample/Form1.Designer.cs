namespace Ca200Sample
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.ButtonCalZero = new System.Windows.Forms.Button();
            this.ButtonMeasure = new System.Windows.Forms.Button();
            this.ButtonCancel = new System.Windows.Forms.Button();
            this.Labelduv = new System.Windows.Forms.Label();
            this.LabelT = new System.Windows.Forms.Label();
            this.Labely = new System.Windows.Forms.Label();
            this.Labelx = new System.Windows.Forms.Label();
            this.LabelLv = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ButtonCalZero
            // 
            this.ButtonCalZero.Location = new System.Drawing.Point(163, 92);
            this.ButtonCalZero.Name = "ButtonCalZero";
            this.ButtonCalZero.Size = new System.Drawing.Size(96, 26);
            this.ButtonCalZero.TabIndex = 38;
            this.ButtonCalZero.Text = "CalZero";
            this.ButtonCalZero.UseVisualStyleBackColor = true;
            this.ButtonCalZero.Click += new System.EventHandler(this.ButtonCalZero_Click);
            // 
            // ButtonMeasure
            // 
            this.ButtonMeasure.Location = new System.Drawing.Point(163, 59);
            this.ButtonMeasure.Name = "ButtonMeasure";
            this.ButtonMeasure.Size = new System.Drawing.Size(96, 26);
            this.ButtonMeasure.TabIndex = 37;
            this.ButtonMeasure.Text = "Measure";
            this.ButtonMeasure.UseVisualStyleBackColor = true;
            this.ButtonMeasure.Click += new System.EventHandler(this.ButtonMeasure_Click);
            // 
            // ButtonCancel
            // 
            this.ButtonCancel.Location = new System.Drawing.Point(163, 24);
            this.ButtonCancel.Name = "ButtonCancel";
            this.ButtonCancel.Size = new System.Drawing.Size(96, 26);
            this.ButtonCancel.TabIndex = 36;
            this.ButtonCancel.Text = "Cancel";
            this.ButtonCancel.UseVisualStyleBackColor = true;
            this.ButtonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // Labelduv
            // 
            this.Labelduv.AutoSize = true;
            this.Labelduv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Labelduv.Location = new System.Drawing.Point(57, 99);
            this.Labelduv.Name = "Labelduv";
            this.Labelduv.Size = new System.Drawing.Size(39, 14);
            this.Labelduv.TabIndex = 35;
            this.Labelduv.Text = "0.0000";
            // 
            // LabelT
            // 
            this.LabelT.AutoSize = true;
            this.LabelT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelT.Location = new System.Drawing.Point(57, 79);
            this.LabelT.Name = "LabelT";
            this.LabelT.Size = new System.Drawing.Size(31, 14);
            this.LabelT.TabIndex = 34;
            this.LabelT.Text = "0000";
            // 
            // Labely
            // 
            this.Labely.AutoSize = true;
            this.Labely.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Labely.Location = new System.Drawing.Point(57, 59);
            this.Labely.Name = "Labely";
            this.Labely.Size = new System.Drawing.Size(39, 14);
            this.Labely.TabIndex = 33;
            this.Labely.Text = "0.0000";
            // 
            // Labelx
            // 
            this.Labelx.AutoSize = true;
            this.Labelx.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Labelx.Location = new System.Drawing.Point(57, 39);
            this.Labelx.Name = "Labelx";
            this.Labelx.Size = new System.Drawing.Size(39, 14);
            this.Labelx.TabIndex = 32;
            this.Labelx.Text = "0.0000";
            // 
            // LabelLv
            // 
            this.LabelLv.AutoSize = true;
            this.LabelLv.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.LabelLv.Location = new System.Drawing.Point(57, 19);
            this.LabelLv.Name = "LabelLv";
            this.LabelLv.Size = new System.Drawing.Size(39, 14);
            this.LabelLv.TabIndex = 31;
            this.LabelLv.Text = "000.00";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(22, 99);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(25, 12);
            this.Label5.TabIndex = 30;
            this.Label5.Text = "duv:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(22, 79);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(14, 12);
            this.Label4.TabIndex = 29;
            this.Label4.Text = "T:";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(22, 59);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(13, 12);
            this.Label3.TabIndex = 28;
            this.Label3.Text = "y:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(22, 39);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(13, 12);
            this.Label2.TabIndex = 27;
            this.Label2.Text = "x:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(22, 19);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(19, 12);
            this.Label1.TabIndex = 26;
            this.Label1.Text = "Lv:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 141);
            this.Controls.Add(this.ButtonCalZero);
            this.Controls.Add(this.ButtonMeasure);
            this.Controls.Add(this.ButtonCancel);
            this.Controls.Add(this.Labelduv);
            this.Controls.Add(this.LabelT);
            this.Controls.Add(this.Labely);
            this.Controls.Add(this.Labelx);
            this.Controls.Add(this.LabelLv);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.Label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Button ButtonCalZero;
        internal System.Windows.Forms.Button ButtonMeasure;
        internal System.Windows.Forms.Button ButtonCancel;
        internal System.Windows.Forms.Label Labelduv;
        internal System.Windows.Forms.Label LabelT;
        internal System.Windows.Forms.Label Labely;
        internal System.Windows.Forms.Label Labelx;
        internal System.Windows.Forms.Label LabelLv;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
    }
}

