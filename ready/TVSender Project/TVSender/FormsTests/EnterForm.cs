using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;

namespace TVSender
{
    public partial class EnterForm : Form
    {
        public string Result
        {
            get { return result; }
        }
        string result = null;
        int time;
        string msg;
        string cancelmsg = "";
        public EnterForm(string _msg, string name, string image)
        {
            InitializeComponent();

            msg = _msg;
            PublicData.cancel_keyform = CloseForm;
            this.Text = name;
            label1.Text = _msg;
            String[] word = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            string PathImage = PublicData.Check.AddGetDirectory(PublicData.RootPath, PublicData.imageFolder);
            PathImage = Path.Combine(PathImage, image);            
            try
            {
                pictureBox1.Image = (File.Exists(PathImage) ) ? Image.FromFile(PathImage) : (Image)Properties.Resources.ResourceManager.GetObject(image);
            }
            catch { pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("white"); }
            int tryParse;
            if (word.Length > 1)
            {
                if (word[0].ToLower() == "ожидайте" & Int32.TryParse(word[1], out tryParse) )
                {
                    this.Text += "сек";
                    label1.Text = string.Format("Ожидайте ({0}): {1}", word[1], msg);
                    button1.Visible = false;
                    time = int.Parse(word[1]);
                    timer1.Start();
                }
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Enter)
            {
                if (!button2.Focused)
                {
                    button1.PerformClick();                    
                }
                return true;
            }

            if (keyData == Keys.Escape)
            {
                button2.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.result = textBoxEnter.Text;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            Close();
        }

        public void CloseForm()
        {
            timer1.Stop();            
            cancelmsg = "USB не найден.\nПовторить тест?";
            Close();        
        }

        private void AskForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {               
                string ask = "Вы отменили операцию.\nПовторить тест?";
                if (cancelmsg != "")
                {
                    ask = cancelmsg;
                    string hz = "Предупреждение";
                    var otvet = MessageBox.Show(ask, hz, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
                    if (otvet == DialogResult.Retry)
                        this.DialogResult = DialogResult.Retry;
                    else this.DialogResult = DialogResult.Cancel;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (time == 0)
            {
                this.DialogResult = DialogResult.OK;
                this.Close();
            }            
            time--;
            label1.Text = string.Format("Ожидайте ({0}): {1}", time.ToString(), msg); 
        }
    }
}
