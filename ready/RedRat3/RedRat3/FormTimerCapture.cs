using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace RedRat3
{
    /// Форма Таймер захвата
    public partial class FormTimerCapture : Form
    {
        int time;
        string msgFormTimerCapture;

        /// Сама форма, с добавлением полей в метод
        public FormTimerCapture(int _time, string _msgFormTimerCapture)
        {
            InitializeComponent();
            msgFormTimerCapture = _msgFormTimerCapture;
            if (_time >= 0)
            {
                time = _time;
                timer1.Interval = 1000;
                timer1.Start();
            }
        }

        /// Таймер захвата
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (time < 0)
            {
                timer1.Stop();
                DialogResult = DialogResult.Cancel;
                Close();
                return;
            }
            else { label1.Text = "Timer (" + time.ToString() + "): " + msgFormTimerCapture; }
            time--;
        }
    }
}