namespace TVSender
{
    partial class SelectOptionForm
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
            if (disposing && (components != null))
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
            this.textBoxListView = new System.Windows.Forms.TextBox();
            this.buttonOKClose = new System.Windows.Forms.Button();
            this.textBoxFileEnd = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.button_CreateFileBrak = new System.Windows.Forms.Button();
            this.button_CreateFileTestSound = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // buttonAccept
            // 
            this.buttonAccept.Location = new System.Drawing.Point(135, 183);
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
            this.label1.Location = new System.Drawing.Point(11, 38);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Шрифт списка";
            // 
            // textBoxListView
            // 
            this.textBoxListView.Location = new System.Drawing.Point(135, 35);
            this.textBoxListView.Name = "textBoxListView";
            this.textBoxListView.Size = new System.Drawing.Size(47, 20);
            this.textBoxListView.TabIndex = 2;
            this.textBoxListView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox1_KeyPress);
            // 
            // buttonOKClose
            // 
            this.buttonOKClose.Location = new System.Drawing.Point(11, 183);
            this.buttonOKClose.Name = "buttonOKClose";
            this.buttonOKClose.Size = new System.Drawing.Size(75, 23);
            this.buttonOKClose.TabIndex = 3;
            this.buttonOKClose.Text = "ОК";
            this.buttonOKClose.UseVisualStyleBackColor = true;
            this.buttonOKClose.Click += new System.EventHandler(this.buttonOKClose_Click);
            // 
            // textBoxFileEnd
            // 
            this.textBoxFileEnd.Location = new System.Drawing.Point(136, 68);
            this.textBoxFileEnd.Name = "textBoxFileEnd";
            this.textBoxFileEnd.Size = new System.Drawing.Size(47, 20);
            this.textBoxFileEnd.TabIndex = 7;
            this.textBoxFileEnd.Text = ".hz";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(11, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(111, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Расширение файлов";
            // 
            // button_CreateFileBrak
            // 
            this.button_CreateFileBrak.Location = new System.Drawing.Point(11, 122);
            this.button_CreateFileBrak.Name = "button_CreateFileBrak";
            this.button_CreateFileBrak.Size = new System.Drawing.Size(197, 23);
            this.button_CreateFileBrak.TabIndex = 8;
            this.button_CreateFileBrak.Text = "Создать операцию брака";
            this.button_CreateFileBrak.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_CreateFileBrak.UseVisualStyleBackColor = true;
            this.button_CreateFileBrak.Click += new System.EventHandler(this.button_CreateFileBrak_Click);
            // 
            // button_CreateFileTestSound
            // 
            this.button_CreateFileTestSound.Location = new System.Drawing.Point(11, 151);
            this.button_CreateFileTestSound.Name = "button_CreateFileTestSound";
            this.button_CreateFileTestSound.Size = new System.Drawing.Size(197, 23);
            this.button_CreateFileTestSound.TabIndex = 9;
            this.button_CreateFileTestSound.Text = "Создать операцию осциллографа";
            this.button_CreateFileTestSound.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button_CreateFileTestSound.UseVisualStyleBackColor = true;
            this.button_CreateFileTestSound.Click += new System.EventHandler(this.button_CreateFileTestSound_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "набора операций";
            // 
            // SelectOptionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(220, 213);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button_CreateFileTestSound);
            this.Controls.Add(this.button_CreateFileBrak);
            this.Controls.Add(this.textBoxFileEnd);
            this.Controls.Add(this.buttonOKClose);
            this.Controls.Add(this.textBoxListView);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAccept);
            this.Name = "SelectOptionForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "SelectOptionForm";
            this.Load += new System.EventHandler(this.SelectOptionForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAccept;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxListView;
        private System.Windows.Forms.Button buttonOKClose;
        private System.Windows.Forms.TextBox textBoxFileEnd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button_CreateFileBrak;
        private System.Windows.Forms.Button button_CreateFileTestSound;
        private System.Windows.Forms.Label label3;
    }
}