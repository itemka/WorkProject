﻿using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using WindowsInput;
using System.Drawing;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using WindowsInput.Native;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Automation = System.Windows.Automation;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.Drawing;
using CA200SRVRLib;


namespace ScanLogPanasonicGR300
{
    public partial class Form1 : Form
    {
        //propertisForWithBalans
        public static ICas m_ICas;
        public static ICa m_ICa;
        public static IProbe m_IProbe;
        public static IMemory m_IMemory;
        public static ICa200 m_ICa200; //= (ICa200)new Ca200Class();
        public static bool isConnectedMinolta = false;
        public static int timeAfterAutostart;

        #region Variables, Contsant for keyboard and Dll
        static string DirectoriOne24GR300 = "..\\ScanLogPanasonicGR300\\24GR300";
        static string DirectoriTwo32GR300 = "..\\ScanLogPanasonicGR300\\32GR300";
        static string DirectoriThree43GR300 = "..\\ScanLogPanasonicGR300\\43GR300";
        static string DirectoriModel = "..\\ScanLogPanasonicGR300\\";
        //static string DirectoriModel2 = "..\\ScanLogPanasonicGR300";
        public string PanasoncModel;
        static string FileWhitNameDate = "\\" + DateTime.Now.ToShortDateString() + ".log";

        public string pathToFileCreateToday;

        ///Constant for keyboard
        const int VK_LBUTTON = 0x01;///mous
        const int VK_RETURN = 0x0D;///enter
        const int VK_SPACE = 0x20;///spase
        const int VK_NUMPAD0 = 0x60;///0
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x0101;
            #region DLL
            [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Auto)]
            public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string className, string windowName);
            [DllImport("USER32.DLL")]
            public static extern bool SetForegroundWindow(IntPtr hWnd);

            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);

            [DllImport("user32.dll")]
            public static extern IntPtr GetForegroundWindow();
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            public static extern bool PostMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
            [DllImport("user32.dll")]
            static extern int LoadKeyboardLayout(string pwszKLID, uint Flags);

            [DllImport("user32.dll", SetLastError = true)]
            public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);
        #endregion
        #endregion

        public Form1()
        {
            if (true)
            {

            }
            Connect_Minolta();
            Thread.Sleep(1000);
            Disconnect_CA210();

            ///ищем окно по имени и классу
            IntPtr ktcTV = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "WindowsForms10.Window.8.app.0.141b42a_r6_ad1", "KTC TV WBAA Tool V19 (2019.5)");
            /// ищем в окне поле для ввода password Administrator
            IntPtr fieldEnrty = FindWindowEx(ktcTV, IntPtr.Zero, "WindowsForms10.RichEdit20W.app.0.141b42a_r6_ad1", "");
            /// ищем в окне кнопку класса Button с подписью Lock
            IntPtr buttonUnlock = FindWindowEx(ktcTV, IntPtr.Zero, "WindowsForms10.BUTTON.app.0.141b42a_r6_ad1", "Lock");

            TopMost = true;
            PostMessage(GetForegroundWindow(), 0x50, 1, LoadKeyboardLayout("00000409", 1));///English install

            //Send path and it returned name Directory where be this exe-file   
            string ReverseStringAndDelete(string s)
            {
                string str = "";

                //Переворачивает строку задом на перед
                char[] arr = s.ToCharArray();
                Array.Reverse(arr);

                var b = false;
                for (int i = 0; i < arr.Length; i++)
                {
                    if (b == false)
                    {
                        if (arr[i] == '\\')
                        {
                            b = true;
                        }
                        else
                        {
                            str += arr[i];
                        }

                    }
                }
                char[] rra = str.ToCharArray();
                Array.Reverse(rra);

                return new string(rra);
            }
            //MessageBox.Show(ReverseStringAndDelete(Directory.GetCurrentDirectory()));

            PanasoncModel = ReverseStringAndDelete(Directory.GetCurrentDirectory());

            pathToFileCreateToday = DirectoriModel + PanasoncModel + FileWhitNameDate;
            DirectoriScanLog_Panasonic();
            if (File.Exists("ColorSystemTools.exe"))
            {
                Process.Start("ColorSystemTools.exe");
                //Thread.Sleep(1000);
                //SetForegroundWindow(ktcTV);
                //Thread.Sleep(1000);
                //SetForegroundWindow(fieldEnrty);
                //Thread.Sleep(1000);
                //SendMessage(fieldEnrty,Convert.ToInt32("k123"), 0, 0);
                //Thread.Sleep(1000);
                //SetForegroundWindow(buttonUnlock);
                //SendKeys.Send("{ENTER}");
            }
            InitializeComponent();
            this.KeyPreview = true;///Для горячих клавиш
            textBox1.ReadOnly = true;
            textBox1.MaxLength = 999999999;
            textBox2.Select();
        }

        ///Hotkey
        private void Form1_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { ProcessingDataScan(); } }
        ///Catalog for scan log

        public void DirectoriScanLog_Panasonic()
        {
            ///Create directory if their no
            while (!Directory.Exists(DirectoriOne24GR300) && !Directory.Exists(DirectoriTwo32GR300) && !Directory.Exists(DirectoriThree43GR300))
            {
                Directory.CreateDirectory(DirectoriOne24GR300);
                Directory.CreateDirectory(DirectoriTwo32GR300);
                Directory.CreateDirectory(DirectoriThree43GR300);
            }
            if (!File.Exists(pathToFileCreateToday))
            {
                //MessageBox.Show(pathToFileCreateToday);
                File.Create(pathToFileCreateToday);
            }

            #region Delete file < sevenDays
            ///// Delete file < sevenDays
            //string[] fileNameDate = Directory.GetFiles(DirectoriModel);
            //if (fileNameDate.Length != 0)
            //{
            //    foreach (string item in fileNameDate)
            //    {
            //        FileInfo f1 = new FileInfo(item);
            //        if (f1.LastAccessTime < DateTime.Now.AddDays(-7))
            //            f1.Delete();
            //    }
            //}
            #endregion
        }

        ///Send Space
        public void ScanStart()
        {
            try
            {
                IntPtr ScanLog = FindWindow(null, "ScanLog");
                ///ищем окно по имени и классу
                IntPtr ktcTV = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "WindowsForms10.Window.8.app.0.141b42a_r6_ad1", "KTC TV WBAA Tool V19 (2019.5)");
                ///ищем в окне кнопку класса Button с подписью Start
                IntPtr buttonStart = FindWindowEx(ktcTV, IntPtr.Zero, "WindowsForms10.BUTTON.app.0.141b42a_r6_ad1", "Start");
                if (ktcTV != IntPtr.Zero)
                {
                    //InputSimulator inputS = new InputSimulator();
                    //Thread.Sleep(500);
                    SetForegroundWindow(ktcTV);
                    Thread.Sleep(300);
                    SetForegroundWindow(buttonStart);
                    Thread.Sleep(300);
                    SendKeys.Send("{ENTER}");
                    //PostMessage(NMWnd, WM_KEYDOWN, VK_SPACE, 0);
                    //PostMessage(NMWnd, WM_KEYUP, VK_SPACE, 0);
                    //inputS.Keyboard.KeyPress(VirtualKeyCode.NUMPAD0);
                    Thread.Sleep(500);
                    SetForegroundWindow(ScanLog);
                }
                ///else { MessageBox.Show("Network Machine не найдена!"); }
            }
            catch (Exception ex) { MessageBox.Show("Ошибка:\n" + ex.Message, "ScanLog", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        public void ProcessingDataScan()
        {
            textBox2.Text = textBox2.Text.Replace(" ", string.Empty);
            if (textBox2.Text.Length == 14)
            {
                ScanStart();
                File.AppendAllText(pathToFileCreateToday, DateTime.Now.ToLongTimeString() + " | " + textBox2.Text + Environment.NewLine);
                textBox1.Text = DateTime.Now.ToLongTimeString() + " | " + textBox2.Text + Environment.NewLine + textBox1.Text;
            }
            textBox2.Text = "";
            //textBox2.Select();
        }

        private void открытьПапкуСВыбраннойМодельюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { Process.Start(@DirectoriModel + PanasoncModel); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void открытьФайлСозданныйСегодняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(pathToFileCreateToday))
                Process.Start(pathToFileCreateToday);
        }

        private void поискСканаВВСегоднешнемФайлеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(pathToFileCreateToday))
            {
                string[] b = new string[FileWhitNameDate.Length];
                int z = 0;
                for (int i = 0; i < FileWhitNameDate.Length; i++)
                {
                    if (z == i)
                    {

                    }
                    else
                    {
                        b[i - 1] = Convert.ToString(FileWhitNameDate[i]);
                    }
                }
                string FileName = string.Join(null, b);
                SearchScanToFile SSTF = new SearchScanToFile();
                SSTF.FileName = FileName;
                SSTF.PathToFileName = pathToFileCreateToday;
                SSTF.Show();
            }
        }


        #region White Balance
        public static void Connect_Minolta()
        {
            try
            {
                m_ICa.Measure(1);
            }
            catch (NullReferenceException)
            {
                Connect_Minolta_0();
                return;
            }
            catch (Exception)
            {
                try
                {
                    m_ICa.RemoteMode = 1;
                }
                catch (Exception)
                {
                    Connect_Minolta_0();
                }
            }
            isConnectedMinolta = true;
        }

        private static void Connect_Minolta_0()
        {
            m_ICa200 = new Ca200();

            if (m_ICa200 == null)
            {
                MessageBox.Show("Не установлен драйвер для Minolta CA-210.\nБаланс белого не будет доступен.",
                    "Driver Minolta CA210 not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isConnectedMinolta = false;
                return;
            }
            //write_info("Подключение к Minolta. Ожидайте...");
            try
            {
                m_ICa200.AutoConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка! Проверьте USB соединение с Минолтой.\n\n\n" + ex,
                    "Не удается подключить к Минолте по USB ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.buttonConnectUSB.Enabled = true;
                isConnectedMinolta = false;
                return;
            }
            Thread.Sleep(50);
            isConnectedMinolta = true;
            m_ICas = (ICas)m_ICa200.Cas;
            m_ICa = (ICa)m_ICas.get_ItemOfNumber(1);
            m_IProbe = (IProbe)m_ICa.SingleProbe;
            m_IMemory = (IMemory)m_ICa.Memory;
            Thread.Sleep(50);
            loopInit_CA210();
            //write_info("Подключение Minolta завершенно.");
        }
        
        private static void loopInit_CA210()
        {
            try
            {
                CalibrateZero();
                //Init_CA210();
            }
            catch (Exception ex)
            {
                string error1 = "SDK Command Error\n--measurement fail\n--check probe/display_setting";
                if (ex.Message.Contains(error1))
                {
                    CalibrateZero();
                    Thread.Sleep(50);
                    loopInit_CA210();
                    return;
                }
                m_ICa.RemoteMode = 0;
                isConnectedMinolta = false;
                MessageBox.Show("Error! Try again." + ex.Message, "Can't connect USB CA210",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void Init_CA210()
        {
            m_IMemory.ChannelNO = 15;
            Thread.Sleep(50);
            m_ICa.SetAnalogRange(2.5f, 2.5f);
            //if (m_ICa.DisplayMode != 0)
            //    m_ICa.DisplayMode = 0;
            //m_IMemory.SetChannelID(" ");
            //if (m_IMemory.ChannelID != "WB AutoAdj")
            //    m_IMemory.SetChannelID("WB AutoAdj");
            //m_ICa.Measure(1);

        }

        public static void CalibrateZero(string mes = "")
        {
            if (MessageBox.Show("CalZero?", "CalZero", MessageBoxButtons.OKCancel) == DialogResult.Cancel)
            {
                return;
            }

            try
            {
                m_ICa.CalZero();
            }
            catch (Exception exCal)
            {
                MessageBox.Show("Колибровка не выполнена.\n\n" + exCal);
                string error2 = "CA Command Error\n--too bright\n--block light";
                if (exCal.Message == error2)
                {
                    CalibrateZero("Слишком ярко!");
                }
            }
        }

        public static void Disconnect_CA210()
        {
            try
            {
                m_ICa.RemoteMode = 0;
                
                m_ICa = null;
                m_ICa200 = null;
                m_IProbe = null;
                m_ICas = null;
                m_IProbe = null;
            }
            catch { }
        }
        #endregion
    }
}