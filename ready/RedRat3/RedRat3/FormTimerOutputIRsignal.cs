using System;
using RedRat.IR;
using RedRat.RedRat3;
using System.Windows.Forms;

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

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            Close();
        }

        SignalOutput SO = new SignalOutput();
        // Таймер вывода сигнала
        private void timer1_Tick(object sender, EventArgs e)
        {
            SO.OutputOneIRsignal(RedRat3, OutputIR);
        }
    }
}
