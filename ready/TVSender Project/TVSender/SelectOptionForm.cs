using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TVSender.Properties;
using System.IO;

namespace TVSender
{
    public partial class SelectOptionForm : Form
    {
        FormNewChoice mainForm;
        public SelectOptionForm(FormNewChoice _mainForm)
        {
            mainForm = _mainForm;
            InitializeComponent();
        }

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            if (textBoxListView.Text != "")
            {
                int size = Convert.ToInt32(textBoxListView.Text);
                mainForm.changeFontListView(size);
                Settings.Default.FontListView = size;
                Settings.Default.FileEndProg = "." + textBoxFileEnd.Text.Trim(' ','.','"');                


                Settings.Default.Save();
            }
        }

        private void LoadSettings()
        {
            textBoxListView.Text = Settings.Default.FontListView.ToString();
            textBoxFileEnd.Text = Settings.Default.FileEndProg;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                e.Handled = true;
        }

        private void SelectOptionForm_Load(object sender, EventArgs e)
        {
            LoadSettings();
        }

        private void buttonOKClose_Click(object sender, EventArgs e)
        {
            Save();
            this.Close();
        }

        private void buttonListViewPlus_MouseDown(object sender, MouseEventArgs e)
        {
            textBoxListView.Text = (Convert.ToInt32(textBoxListView.Text) + 1).ToString();
        }

        private void button_CreateFileBrak_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(PublicData.StartPath, "Забраковать" + textBoxFileEnd.Text);
            System.IO.File.CreateText(filePath).Close();
            IniFile ini = new IniFile();
            ini.Load(filePath);
            ini.AddSection("Брак");
            ini.Save(filePath);            
        }

        private void button_CreateFileTestSound_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(PublicData.StartPath, "Осциллограф" + textBoxFileEnd.Text);
            System.IO.File.CreateText(filePath).Close();
            IniFile ini = new IniFile();
            ini.Load(filePath);
            ini.AddSection("Осциллограф");
            ini.Save(filePath);
        }
    }
}
