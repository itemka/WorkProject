namespace TVSender
{
    partial class OptionsSelectForm
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
            this.buttonAccept = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonOKClose = new System.Windows.Forms.Button();
            this.textBoxFileEnd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lblConfigPath = new System.Windows.Forms.Label();
            this.txtConfigPath = new System.Windows.Forms.TextBox();
            this.btnConfigPathSelect = new System.Windows.Forms.Button();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            ( (System.ComponentModel.ISupportInitialize)(this.numericUpDown1) ).BeginInit();
            this.SuspendLayout();
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(109, 134);
            this.buttonAccept.Name = "buttonAccept";
            this.buttonAccept.Size = new System.Drawing.Size(75, 23);
            this.buttonAccept.TabIndex = 0;
            this.buttonAccept.Text = "Применить";
            this.buttonAccept.UseVisualStyleBackColor = true;
            this.buttonAccept.Click += new System.EventHandler(this.buttonAccept_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Шрифт списка";
            // 
            // buttonOKClose
            // 
            this.buttonOKClose.Location = new System.Drawing.Point(11, 134);
            this.buttonOKClose.Name = "buttonOKClose";
            this.buttonOKClose.Size = new System.Drawing.Size(75, 23);
            this.buttonOKClose.TabIndex = 3;
            this.buttonOKClose.Text = "ОК";
            this.buttonOKClose.UseVisualStyleBackColor = true;
            this.buttonOKClose.Click += new System.EventHandler(this.buttonOKClose_Click);
            // 
            // textBoxFileEnd
            // 
            this.textBoxFileEnd.Location = new System.Drawing.Point(136, 41);
            this.textBoxFileEnd.Name = "textBoxFileEnd";
            this.textBoxFileEnd.Size = new System.Drawing.Size(47, 20);
            this.textBoxFileEnd.TabIndex = 7;
            this.textBoxFileEnd.Text = ".hz";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Расширение файлов";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "набора операций";
            // 
            // lblConfigPath
            // 
            this.lblConfigPath.Location = new System.Drawing.Point(11, 75);
            this.lblConfigPath.Name = "lblConfigPath";
            this.lblConfigPath.Size = new System.Drawing.Size(173, 13);
            this.lblConfigPath.TabIndex = 31;
            this.lblConfigPath.Text = "Сервер-папка конфигурации";
            // 
            // txtConfigPath
            // 
            this.txtConfigPath.Location = new System.Drawing.Point(11, 88);
            this.txtConfigPath.Name = "txtConfigPath";
            this.txtConfigPath.Size = new System.Drawing.Size(153, 20);
            this.txtConfigPath.TabIndex = 30;
            // 
            // btnConfigPathSelect
            // 
            this.btnConfigPathSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ( (byte)(204) ));
            this.btnConfigPathSelect.Location = new System.Drawing.Point(161, 87);
            this.btnConfigPathSelect.Name = "btnConfigPathSelect";
            this.btnConfigPathSelect.Size = new System.Drawing.Size(24, 22);
            this.btnConfigPathSelect.TabIndex = 32;
            this.btnConfigPathSelect.Text = "...";
            this.btnConfigPathSelect.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConfigPathSelect.UseVisualStyleBackColor = true;
            this.btnConfigPathSelect.Click += new System.EventHandler(this.btnConfigPathSelect_Click);
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(136, 9);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(48, 20);
            this.numericUpDown1.TabIndex = 33;
            // 
            // OptionsSelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(195, 167);
            this.Controls.Add(this.numericUpDown1);
            this.Controls.Add(this.btnConfigPathSelect);
            this.Controls.Add(this.lblConfigPath);
            this.Controls.Add(this.txtConfigPath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.textBoxFileEnd);
            this.Controls.Add(this.buttonOKClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAccept);
            this.Name = "OptionsSelectForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SelectOptionForm";
            this.Load += new System.EventHandler(this.SelectOptionForm_Load);
            ( (System.ComponentModel.ISupportInitialize)(this.numericUpDown1) ).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonOKClose;
        private System.Windows.Forms.TextBox textBoxFileEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblConfigPath;
        private System.Windows.Forms.TextBox txtConfigPath;
        private System.Windows.Forms.Button btnConfigPathSelect;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
    }
}