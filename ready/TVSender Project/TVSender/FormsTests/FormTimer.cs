using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.IO;
using NLog;

namespace TVSender //менюшка подтверждения успешности теста(используется в тестах)
{
    public partial class FormTimer : Form
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        int time;
        string msg;
        string msgAfter;
        string cancelmsg = "";
        public FormTimer(string _msg, string image="white", int timer = 0, string _msgAfter = "")
        {
            InitializeComponent();

            msg = _msg;
            msgAfter = _msgAfter;
            PublicData.cancel_keyform = CloseForm;
            this.Text = "Timer";
            label1.Text = _msg;

            string PathImage = PublicData.Check.AddGetDirectory(PublicData.RootPath, PublicData.imageFolder);
            PathImage = Path.Combine(PathImage, image);            
            try
            {
                pictureBox1.Image = (File.Exists(PathImage) ) ? Image.FromFile(PathImage) : (Image)Properties.Resources.ResourceManager.GetObject(image);
            }
            catch { pictureBox1.Image = (Image)Properties.Resources.ResourceManager.GetObject("white"); }
            if (timer > 0)
            {
                    this.Text += "сек";
                    label1.Text = $"{msg} ({timer}): {msgAfter}";
                    button1.Visible = false;
                    time = timer;
                    timer1.Start();
            }
            button1.Visible = false;
            button2.Visible = false;
            button3.Visible = false;
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

            if (keyData == Keys.Back)
            {
                button3.PerformClick();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
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
                string ask = "Вы отменили операцию.\nПовторить?";
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
            if (time <= 0)
            {
                timer1.Stop();
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }            
            time--;
            //label1.Text = $"Ожидайте ({time.ToString()}): {msg}";
            Ex.Try( () =>
            {
                Invoke(new MethodInvoker(() =>
                {
                    label1.Text = $"{msg} ({time.ToString()}): {msgAfter}";
                }));
            });
        }
    }
}
