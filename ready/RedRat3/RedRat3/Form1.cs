using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Timers;
using System.Drawing;
using System.IO.Ports;
using System.IO.Compression;
using System.Threading;
using System.Resources;
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
    /// Главная форма
    /// Для добавления любого файла в проект ПРАВИЛЬНО - Обозреватель решений -> Resources.resx -> Добавить ресурс -> Добавить существующий файл...
    public partial class Form1 : Form
    {
        public static string path1 = "C:\\RedRat3_Data\\";
        public Form1()
        {
            InitializeComponent();
            //Для горячих клавиш
            this.KeyPreview = true;
            button2.Enabled = false;
                button2.BackColor = Color.DimGray;
            button3.Enabled = false;
                button3.BackColor = Color.DimGray;

            while (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
        }

        public IRedRat3 RedRat3;
        public IRPacket OutputIR;
        public int interVal;
        
        /// Событие для горячих клавиш
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F1)
                    поискRedRat3ToolStripMenuItem.PerformClick();
                if (e.KeyCode == Keys.F2)
                    settingToolStripMenuItem.PerformClick();
                if (e.KeyCode == Keys.F3)
                    выбратьСигналF3ToolStripMenuItem.PerformClick();
                if (e.KeyCode == Keys.F5)
                    button1.PerformClick();
                if (e.KeyCode == Keys.F6)
                    button2.PerformClick();
                if (e.KeyCode == Keys.F7)
                    button3.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }  
        }

        /// Кнопка поиска RedRat3(F1)
        private void поискRedRat3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SearchRedRat SR3 = new SearchRedRat();
                RedRat3 = SR3.FindRedRatInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// Кнопка Настройки(F2)
        private void settingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                FormSetting FS = new FormSetting();
                FS.ShowDialog();
                interVal = FS.ms;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// Кнопка Выбрать сигнал(F3)
        private void выбратьСигналF3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SignalOutput SO = new SignalOutput();
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.InitialDirectory = path1;
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show(OFD.FileName);
                    OutputIR = SO.ConvertingBINARYtoIRsignal(OFD.FileName);
                    if (OutputIR != null)
                    {
                        button2.Enabled = true;
                        button2.BackColor = Color.FromArgb(247, 98, 1);
                        button3.Enabled = true;
                        button3.BackColor = Color.FromArgb(19, 129, 214);
                    }
                }
                поискRedRat3ToolStripMenuItem.PerformClick();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// Кнопка захвата сигнала
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SearchRedRat SR3 = new SearchRedRat();
                RedRat3 = SR3.FindRedRat();
                if (RedRat3 != null)
                {
                    IRsignalTrainingMode IRSTM = new IRsignalTrainingMode();
                    var FTC = new FormTimerCapture(10, "Подайте сигнал с пульта");
                    var waitsignal2 = Task.Factory.StartNew(() => { FTC.ShowDialog(); });

                    var qwe = Task.Factory.StartNew(() => {
                        IRSTM.CaptureSignal();
                        OutputIR = IRSTM.GetSignal();
                        if (OutputIR != null)
                        {
                            button2.Enabled = true;
                            button2.BackColor = Color.FromArgb(247, 98, 1);
                            button3.Enabled = true;
                            button3.BackColor = Color.FromArgb(19, 129, 214);
                            FTC.Close();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// Кнопка вывода сигнала
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((RedRat3 != null) && (OutputIR != null))
                {
                    SignalOutput SO = new SignalOutput();
                    SO.OutputOneIRsignal(RedRat3, OutputIR);
                }
                else if (OutputIR == null)
                {
                    MessageBox.Show("Захватите сигнал.", "Вывод сигнала", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// Кнопка вывода сигнала по таймеру
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (interVal > 0 && interVal < 9999999)
                {
                    FormTimerOutputIRsignal FTOIRS = new FormTimerOutputIRsignal(RedRat3, OutputIR, interVal);
                    if ((RedRat3 != null) && (OutputIR != null))
                    {
                        FTOIRS.ShowDialog();
                    }
                    else if (OutputIR == null)
                    {
                        MessageBox.Show("Захватите сигнал.", "Вывод сигнала", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                else
                {
                    MessageBox.Show("Задайте интервал в настройках.", "Вывод сигнала", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        } 
    }
}