using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Reflection;

namespace TVSender
{
    public partial class KeyForm : Form
    {
        int time;
        string[] msg;
        Label[] mu;
        bool[] pass;
        Dictionary<string, int> ButtonNumber;

        public KeyForm(string _msg, string[] incArray, string name = "", bool color = false)
        {
            InitializeComponent();

            pass = new bool[incArray.Length];
            ButtonNumber = new Dictionary<string,int>();

            for (int i = 0; i < pass.Length; i++)
                pass[i] = false;

            mu = new Label[7];
            mu[0] = label2;
            mu[1] = label3;
            mu[2] = label4;
            mu[3] = label5;
            mu[4] = label6;
            mu[5] = label7;
            mu[6] = label8;

            for (int i = 0; i < mu.Length; i++)
            {
                mu[i].Visible = false;
            }

            for (int i = 0; i < incArray.Length; i++)
            {
                ButtonNumber.Add(incArray[i].Trim().ToLower().Replace(" ",""),i);
                mu[i].Text = incArray[i];
                mu[i].Visible = true;
            }

            label1.BackColor = Color.MediumSpringGreen;

            if (!color)
            {
                label9.Visible = false;
                label10.Visible = false;
                label11.Visible = false;
                label12.Visible = false;
                label13.Visible = false;
                label14.Visible = false;
                label15.Visible = false;
            }

            PublicData.set_key = new Action<string>(setStringKey);

            this.Text = name;
            label1.Text = _msg;
            String[] word = name.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (word[0] == "Ожидайте")
            {
                this.Text += "сек";
                msg = _msg.Split(':');
                label1.Text = msg[0] + " (" + word[1] + "):" + msg[1];
                button1.Visible = false;
                //button1.CanFocus = true;
                time = int.Parse(word[1]);
                timer1.Start();
            }            
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Enter)
            {
                if (!button2.Focused)
                {
                    button1.PerformClick();
                    return true;
                }
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
            foreach(var lbl in mu)
                if (lbl.BackColor != Color.Lime)
                {
                    label1.Text = "Не все кнопки нажаты";
                    label1.BackColor = Color.Maroon;
                    label1.ForeColor = Color.Red;
                    return;
                }
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();            
        }

        private void KeyForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != DialogResult.OK)
            {
                string ask = "Вы отменили операцию.\nПовторить тест?";
                string hz = "Предупреждение";
                var otvet = MessageBox.Show(ask, hz, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
                if (otvet == DialogResult.Retry)
                    this.DialogResult = DialogResult.Retry;
                else this.DialogResult = DialogResult.Cancel;
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
            label1.Text = msg[0] + " (" + time.ToString() + "):" + msg[1];
        }

        public void setkey(int n)
        {
            mu[n].BackColor = Color.Lime;
            pass[n] = true;
            foreach (bool s in pass)
                if (!s) return;

            Invoke(new EventHandler(delegate
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }) );
        }
        public void setStringKey(string inc)
        {
            if (inc == null) return;
            string button = inc.Trim().ToLower().Replace(" ", "");
            if (!ButtonNumber.ContainsKey(button) ) return;
            setkey(ButtonNumber[button]);
        }
    }
}
