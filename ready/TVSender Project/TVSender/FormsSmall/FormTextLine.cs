using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVSender.FormsSmall
{
    public partial class FormTextLine : Form
    {
        public FormTextLine()
        {
            InitializeComponent();
            this.ShowIcon = false;
            this.MaximizeBox = false;
            //this.FormBorderStyle = FormBorderStyle.None;
            this.MaximizeBox = false;
            this.Text = "";
            this.ShowIcon = false;
            this.StartPosition = FormStartPosition.CenterScreen;                        
            label1.Text = "Сохранено в буфер обмена.\nМожете использовать ctrl + v";
        }

        public FormTextLine(string incStr) : this()
        {
            textBox1.Text = incStr ?? string.Empty;
            if (string.IsNullOrEmpty(incStr))
            { textBox1.Enabled = false; }
        }

        public FormTextLine(string incStr, string secondStr) : this(incStr)
        {
            richTextBox1.Text = secondStr ?? string.Empty;
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            if (keyData == Keys.F8)
            {
                RedRatCaptureSignal();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void textBox1_Click(object sender, EventArgs e)
        {
            textBox1.SelectAll();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void richTextBox1_Click(object sender, EventArgs e)
        {
            richTextBox1.SelectAll();
        }

        private async void RedRatCaptureSignal()
        {
            await Ex.Catch(RedratHelper.CapturedSignal());
        }
    }
}
