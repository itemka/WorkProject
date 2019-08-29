using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System;

namespace ScanLogPanasonicGR300
{
    public partial class SearchScanToFile : Form
    {
        public string FileName { get { return label2.Text; } set { label2.Text = value; } }
        public string PathToFileName;
        public string _PathToFileName { get { return PathToFileName; } set { PathToFileName = value; } }

        public SearchScanToFile()
        {
            try
            {
                TopMost = true;
                InitializeComponent();
                this.KeyPreview = true;///Для горячих клавиш
            }
            catch (Exception ex) { MessageBox.Show("Ошибка:\n" + ex.Message, "ScanLog", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        private void SearchScanToFile_KeyDown(object sender, KeyEventArgs e) { if (e.KeyCode == Keys.Enter) { button1_Click(null, null); } }

        int z = 0;
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox2.Text = textBox2.Text.Replace(" ", string.Empty);
                if (textBox2.Text.Length == 14)
                {
                    string[] s = File.ReadAllLines(@PathToFileName);
                    for (int i = 0; i < s.Length; i++)
                    {
                        if (s[i].Contains(textBox2.Text))
                        {
                            z++;
                        }
                    }
                }
                textBox2.Text = "";
                label4.Text = z.ToString();
                z = 0;
            }
            catch (Exception ex) { MessageBox.Show("Ошибка:\n" + ex.Message, "ScanLog", MessageBoxButtons.OK, MessageBoxIcon.Exclamation); }
        }

        
    }
}
