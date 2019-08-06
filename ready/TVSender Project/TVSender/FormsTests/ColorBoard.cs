using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace TVSender //менюшка переключения цветов для теста. на практике не использовалась, но если удалять, то возможно
                    //привязана к тестам, которые тоже не использовались
{
    public partial class ColorBoard : Form
    {        
        int colors;
        public ColorBoard()
        {
            colors = 0;            
            InitializeComponent();
            this.KeyPreview = true;
            //this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            //this.button6.Click += new System.EventHandler(this.button6_Click);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Enter)
            {
                if (!button7.Focused)
                {
                    button6.PerformClick();
                    return true;
                }
            }

            if (keyData == Keys.Escape)
            {
                button7.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.D1: case Keys.NumPad1:
                    button1.PerformClick();
                    press(button1);
                    e.Handled = true;
                    break;
                case Keys.D2: case Keys.NumPad2:
                    button2.PerformClick();
                    press(button2);
                    e.Handled = true;
                    break;
                case Keys.D3: case Keys.NumPad3:
                    button3.PerformClick();
                    press(button3);
                    e.Handled = true;
                    break;
                case Keys.D4: case Keys.NumPad4:
                    button4.PerformClick();
                    press(button4);
                    e.Handled = true;
                    break;
                case Keys.D5: case Keys.NumPad5:
                    button5.PerformClick();
                    press(button5);
                    e.Handled = true;
                    break;
            }
        }

        private void escape()
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }    

        private void ok()
        {
            if (colors == 6)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else label1.Text = (6 - colors).ToString() + " more colors!";
        }

        private void press(Button button)
        {            
            //if (button.Text != string.Empty) colors++;
            if (button.FlatStyle == FlatStyle.Flat) colors++;
            //button.Text = string.Empty;
            button.FlatStyle = FlatStyle.Flat;
            button.ForeColor = Color.Silver;
            label1.Text = colors.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            press(button2);
            //Code.blue();
        }

        private void ColorBoard_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                string ask = "repeat test?";
                string hz = "предупреждение";
                var otvet = MessageBox.Show(ask, hz, MessageBoxButtons.YesNo, MessageBoxIcon.Stop);
                if (otvet == DialogResult.Yes)
                    this.DialogResult = DialogResult.Retry;
                else this.DialogResult = DialogResult.Cancel;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ok();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Code.green();
            PublicData.CreatedTests["green"]();
            press(button1);            
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Code.red();
            PublicData.CreatedTests["red"]();
            press(button3);            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Code.white();
            PublicData.CreatedTests["white"]();
            press(button4);            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //Code.black();
            PublicData.CreatedTests["black"]();
            press(button5);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            //Code.black();
            PublicData.CreatedTests["grey"]();
            press(button8);
        }        
    }
}
