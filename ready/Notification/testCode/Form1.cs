using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace testCode
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private NotifyIcon NI = new NotifyIcon();
        private void button1_Click(object sender, EventArgs e)
        {
            NI.BalloonTipText = "Это твой убогий компьютер!";
            NI.BalloonTipTitle = "Привет! Как оно?";
            NI.BalloonTipIcon = ToolTipIcon.Info;
            NI.Icon = this.Icon;
            NI.Visible = true;
            NI.ShowBalloonTip(500);
        }

        private void NI_BalloonTipClosed(Object sender, EventArgs e)
        {
            NI.Visible = false;
        }
    }
}
