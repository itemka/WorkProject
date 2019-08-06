using System;
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

namespace ScanLog
{
    public partial class Form1 : Form
    {
        #region Variables, Contsant for keyboard and Dll
            static string DirectoriOne24FR250 = "ScanLog_Panasonic\\24FR250";
            static string DirectoriTwo32FSR400 = "ScanLog_Panasonic\\32FSR400";
            static string DirectoriThree43FSR400 = "ScanLog_Panasonic\\43FSR400";
            static string DirectoriModel = "ScanLog_Panasonic\\";
            static string DirectoriModel2 = "ScanLog_Panasonic";
            public string PanasoncModel;
            static string FileWhitNameDate = "\\" + DateTime.Now.ToShortDateString() + ".log";

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
            TopMost = true;
            PostMessage(GetForegroundWindow(), 0x50, 1, LoadKeyboardLayout("00000409", 1));///English install
                SelectModel SM = new SelectModel();
                SM.ShowDialog();
            PanasoncModel = SM.model;
            DirectoriScanLog_Panasonic();
                if (File.Exists(PanasoncModel + "\\NetworkMachine2015.exe"))
                {
                    Process.Start(PanasoncModel + "\\NetworkMachine2015.exe");
                }
            InitializeComponent();
            this.KeyPreview = true;///Для горячих клавиш
            textBox1.ReadOnly = true;
            textBox1.MaxLength = 999999999;
            textBox2.Select();
        }
        ///Hotkey
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { ProcessingDataScan(); }
        }
        ///Catalog for scan log
        public void DirectoriScanLog_Panasonic()
        {
            ///Create directory if their no
            while (!Directory.Exists(DirectoriOne24FR250) && !Directory.Exists(DirectoriTwo32FSR400) && !Directory.Exists(DirectoriThree43FSR400))
            {
                Directory.CreateDirectory(DirectoriOne24FR250);
                Directory.CreateDirectory(DirectoriTwo32FSR400);
                Directory.CreateDirectory(DirectoriThree43FSR400);

                File.Create(DirectoriModel + PanasoncModel + FileWhitNameDate);
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
                IntPtr NMWnd = FindWindowEx(IntPtr.Zero, IntPtr.Zero, "LVDChild", "Network Machine 2016-07-11");
                ///ищем в окне кнопку класса Button с подписью Start
                IntPtr button = FindWindowEx(NMWnd, IntPtr.Zero, "Buttom", "Start");
                if (NMWnd != IntPtr.Zero)
                {
                    //InputSimulator inputS = new InputSimulator();
                    //Thread.Sleep(500);
                    SetForegroundWindow(NMWnd);
                    Thread.Sleep(500);
                    SendKeys.SendWait("                                      ");
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
                File.AppendAllText(DirectoriModel + PanasoncModel + FileWhitNameDate, DateTime.Now.ToLongTimeString() + " | " + textBox2.Text + Environment.NewLine);
                textBox1.Text = DateTime.Now.ToLongTimeString() + " | " + textBox2.Text + Environment.NewLine + textBox1.Text;
            }
            textBox2.Text = "";
            //textBox2.Select();
        }

        private void openDirectoriToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { Process.Start(@DirectoriModel + PanasoncModel); }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
        private void openFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(DirectoriModel + PanasoncModel + FileWhitNameDate))
                Process.Start(DirectoriModel + PanasoncModel + FileWhitNameDate);
        }


        private void searchToFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (File.Exists(DirectoriModel + PanasoncModel + FileWhitNameDate))
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
                        b[i-1] = Convert.ToString(FileWhitNameDate[i]);
                    }
                }
                string FileName = string.Join(null, b);
                SearchScanToFile SSTF = new SearchScanToFile();
                SSTF.FileName = FileName;
                SSTF.PathToFileName = DirectoriModel + PanasoncModel + FileWhitNameDate;
                SSTF.Show();
            }
        }
    }
}