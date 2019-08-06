using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;
using System.Threading.Tasks;

namespace TVSender
{
    public partial class FormScaner : Form
    {
        int CountSymbols = 0;
        int BeforeCountSymbols = 0;
        //Thread CheckScan;
        Stopwatch timer = Stopwatch.StartNew();
        static string OfflinePath;
        private const string defaultPathReject = "\\\\1cby\\plant_work";
        public FormScaner()
        {
            InitializeComponent();
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("en-US") );
            MoveFiles();
            OfflinePath = GetOfflinePath();
            //CheckScan = new Thread(checkScan);
            //CheckScan.IsBackground = true;
            //CheckScan.Start();   
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Enter)
            {
                button1.PerformClick();
                return true;
            }

            if (keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }

            if (keyData == Keys.Up)
            {
                textBoxSN.Focus();
                return true;
            }

            if (keyData == Keys.Down)
            {
                textBoxDefect.Focus();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        public static void MoveFiles()
        {
            Ex.Try(() =>
            {
                string folder = GetPathRejectFromFile();
                foreach (var f in System.IO.Directory.GetFiles(OfflinePath))
                {
                    string filename = f.Substring(OfflinePath.Length + 1);
                    System.IO.File.Move(f, folder + "\\" + filename);
                }
            });
        }

        private static string GetOfflinePath()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = PublicData.Check.AddGetDirectory(path, "TVSender");
            path = PublicData.Check.AddGetDirectory(path, "logs Reject");
            return path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MoveFiles();
            labelDefectRed.Visible = false;
            YesBox2.Visible = false;
            NoBox2.Visible = false;

            labelSNRed.Visible = false;
            YesBox1.Visible = false;
            NoBox1.Visible = false;

            if (textBoxSN.Text.Length < 5)
            {
                textBoxSN.Focus();
                labelSNRed.Visible = true;
                labelDefectRed.Visible = false;
                NoBox1.Visible = true;
                YesBox1.Visible = false;
                textBoxSN.SelectAll();
                return;
            }
            YesBox1.Visible = true;
            if (textBoxDefect.Text.Length != 3)
            {
                textBoxDefect.Focus();
                labelDefectRed.Visible = true;
                NoBox2.Visible = true;
                YesBox2.Visible = false;
                textBoxDefect.SelectAll();
                return;
            }
            labelDefectRed.Visible = false;
            YesBox2.Visible = true;
            NoBox2.Visible = false;


            string pathReject = GetPathRejectFromFile();
            DirectoryInfo dir = new DirectoryInfo(pathReject);
            var filename = "#" + textBoxSN.Text + " " + System.DateTime.Now.ToString() + ".txt";
            filename = filename.Replace(':', '-');
            filename = filename.Replace('/', '.');
            var fullway = dir.FullName + "\\" + filename;             
            StreamWriter file;
            try
            {
                file = File.CreateText(fullway);
            }
            catch (DirectoryNotFoundException ex2)
            {
                PublicData.write_info(ex2.Message);
                try
                {
                    dir.Create();
                    file = System.IO.File.CreateText(fullway);
                }
                catch
                {
                    try
                    {
                        dir = new DirectoryInfo(OfflinePath);
                        dir.Create();
                        fullway = dir.FullName + "\\" + filename;
                        file = System.IO.File.CreateText(fullway);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("НЕ СОХРАНИЛОСЬ !\n" + ex.Message,
                            "Ошибка записи", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                        this.Close();
                        return;
                    }
                }
            }
            catch (UnauthorizedAccessException ex)
            {
                PublicData.write_info(ex.Message);
                try
                {
                    dir = new DirectoryInfo(OfflinePath);
                    fullway = dir.FullName + "\\" + filename;
                    file = System.IO.File.CreateText(fullway);
                }
                catch (Exception ex2)
                {
                    MessageBox.Show("НЕ СОХРАНИЛОСЬ !\n" + ex2.Message,
                        "Ошибка записи", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.Close();
                    return;
                }
            }
            catch (Exception ex)
            {
                PublicData.write_info(ex.Message);
                try
                {
                    dir = new DirectoryInfo(OfflinePath);
                    fullway = dir.FullName + "\\" + filename;
                    file = System.IO.File.CreateText(fullway);
                }
                catch (Exception ex2)
                {
                    MessageBox.Show("НЕ СОХРАНИЛОСЬ !\n" + ex.Message + '\n' + ex2.Message,
                        "Ошибка записи", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    this.Close();
                    return;
                }
            }
            if (file == null) return;

            string PlaceNumber = "999";
            IniFile comini = new IniFile();
            comini.Load("com_settings.ini");
            try
            {
                IniFile.IniSection portsection = comini.GetSection("ports");
                PlaceNumber = portsection.GetAddKey("PlaceNumber").Value;
            }
            catch { };

            string msg = textBoxSN.Text + "\t" + textBoxDefect.Text + PlaceNumber;
            file.WriteLine(msg);            
            file.Close();
            PublicData.write_info("сохранен " + filename);
            PublicData.brak_blink();
            this.Close();
        }

        public static string GetPathRejectFromFile()
        {
            IniFile ini = new IniFile();
            ini.Load(PublicData.configFile);
            var sect = ini.GetSection("brak");
            if (sect == null)
            {
                sect = ini.AddSection("brak");
                sect.AddKey(defaultPathReject);
            }
            if (sect.Keys.Count == 0) sect.AddKey(defaultPathReject);
            ini.SaveShowMessage();
            var keys = sect.Keys;

            string pathReject = defaultPathReject;
            foreach (IniFile.IniSection.IniKey key in keys)
            { pathReject = key.Name; break; }
            return pathReject;
        }

        private async void textBox1_TextChanged(object sender, EventArgs e)
        {            
            var time = timer.ElapsedMilliseconds;
            if (textBoxSN.Text.Length - BeforeCountSymbols > 6) textBoxDefect.Focus();
            if (time < 200)
            {
                if(textBoxSN.Text.Length > BeforeCountSymbols)
                    CountSymbols += 1;                
            }
            else
            {
                CountSymbols = 0;
            }
            BeforeCountSymbols = textBoxSN.Text.Length;
            timer.Reset();
            timer.Start();
            while (timer.ElapsedMilliseconds < 310)
            {
                if (CountSymbols > 6)
                {
                    if (timer.ElapsedMilliseconds > 200)
                    {
                        textBoxDefect.Focus();
                        labelSNRed.Visible = false;
                        YesBox1.Visible = true;
                        NoBox1.Visible = false;
                    }
                    //textBoxSN.Text = textBoxSN.Text.Substring(textBoxSN.TextLength - CountSymbols - 2);
                }
                await TaskEx.Delay(80);
            }            
        }

        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBoxDefect.SelectAll();
        }

        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBoxSN.SelectAll();
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxSN.SelectAll();
        }

        private void textBox2_MouseClick(object sender, MouseEventArgs e)
        {
            textBoxSN.SelectAll();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
