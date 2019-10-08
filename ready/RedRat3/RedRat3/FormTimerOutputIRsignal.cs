using System;
using RedRat.IR;
using System.IO;
using RedRat.RedRat3;
using System.Drawing;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RedRat3
{
    public partial class FormTimerOutputIRsignal : Form
    {
        public static IRedRat3 RedRat3;
        public static IRPacket OutputIR;
        public static int interval;
        public bool folderOutput;
        public string pathToFolder;
        public FileInfo[] fileInfos;
        SignalOutput SO = new SignalOutput();


        public FormTimerOutputIRsignal(IRedRat3 _RedRat3, Int32 _Interval, IRPacket _OutputIR = null, bool _folderOutput = false, string _pathToFolder = "", FileInfo[] _fileInfos = null)
        {
            InitializeComponent();
            RedRat3 = _RedRat3;
            interval = _Interval;
            OutputIR = _OutputIR;
            folderOutput = _folderOutput;
            pathToFolder = _pathToFolder;
            fileInfos = _fileInfos;

            if (folderOutput == false)
            {
                timer1.Interval = interval;
                timer1.Start();
            }
            else
            {   
                foreach (FileInfo file in fileInfos)
                {
                    OutputIR = SO.ConvertingBINARYorXMLtoIRsignal(pathToFolder + "\\" + file.Name);
                    if ((RedRat3 != null) && (OutputIR != null))
                    {
                        SO.OutputOneIRsignal(RedRat3, OutputIR);
                        Thread.Sleep(interval);
                    }
                    else
                    {
                        MessageBox.Show("Ошибка. Проверьте ввеленные значения интервала, или Подключение RedRat3, или плохо записан сигнал.");
                    }
                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (folderOutput == false)
                {
                    timer1.Stop();
                    Close();
                }
                else
                {
                    Close();
                }
            }
            catch (Exception)
            {
                timer1.Stop();
                Close();
            }
        }

        // Таймер вывода сигнала
        private void timer1_Tick(object sender, EventArgs e)
        {
            SO.OutputOneIRsignal(RedRat3, OutputIR);
        }
    }
}