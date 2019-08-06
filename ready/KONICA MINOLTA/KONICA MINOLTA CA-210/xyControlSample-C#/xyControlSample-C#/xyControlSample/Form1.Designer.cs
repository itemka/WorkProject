namespace xyControlSample
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
            this.colorSpaceControlWrapper1 = new ColorSpaceControl.ColorSpaceControlWrapper();
            this.button_Measure = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // colorSpaceControlWrapper1
            // 
            this.colorSpaceControlWrapper1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.colorSpaceControlWrapper1.Location = new System.Drawing.Point(21, 17);
            this.colorSpaceControlWrapper1.Name = "colorSpaceControlWrapper1";
            this.colorSpaceControlWrapper1.Size = new System.Drawing.Size(289, 252);
            this.colorSpaceControlWrapper1.TabIndex = 0;
            // 
            // button_Measure
            // 
            this.button_Measure.Location = new System.Drawing.Point(326, 34);
            this.button_Measure.Name = "button_Measure";
            this.button_Measure.Size = new System.Drawing.Size(116, 42);
            this.button_Measure.TabIndex = 1;
            this.button_Measure.Text = "Measure";
            this.button_Measure.UseVisualStyleBackColor = true;
            this.button_Measure.Click += new System.EventHandler(this.button_Measure_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(468, 288);
            this.Controls.Add(this.button_Measure);
            this.Controls.Add(this.colorSpaceControlWrapper1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private ColorSpaceControl.ColorSpaceControlWrapper colorSpaceControlWrapper1;
        private System.Windows.Forms.Button button_Measure;
    }
}

