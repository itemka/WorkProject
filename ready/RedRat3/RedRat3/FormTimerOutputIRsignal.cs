using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Timers;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Reflection;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using RedRat;
using RedRat.IR;
using RedRat.USB;
using RedRat.Util;
using RedRat.RedRat3;
using RedRat.RedRat3.USB;
using RedRat.AVDeviceMngmt;

namespace RedRat3
{
    public partial class FormTimerOutputIRsignal : Form
    {
        public static IRedRat3 RedRat3;
        public static IRPacket OutputIR;
        public static int interval;
        
        public FormTimerOutputIRsignal(IRedRat3 _RedRat3, IRPacket _OutputIR, Int32 _Interval)
        {
            InitializeComponent();
            RedRat3 = _RedRat3;
            OutputIR = _OutputIR;
            interval = _Interval;
            timer1.Interval = interval;
            timer1.Start();
        }

        //public bool w = true;
        private void button1_Click(object sender, EventArgs e)
        {
            //if (w)
            //{
            //w = false;
            timer1.Stop();
            Close();
            //}
            //else
            //{
            //    w = true;
            //    timer1.Stop();
            //    button1.Text = "Старт";
            //    button1.BackColor = Color.FromArgb(0, 192, 0);
            //}
        }

        SignalOutput SO = new SignalOutput();
        /// Таймер вывода сигнала
        private void timer1_Tick(object sender, EventArgs e)
        {
            SO.OutputOneIRsignal(RedRat3, OutputIR);
        }
    }
}
