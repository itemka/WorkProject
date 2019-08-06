using System;
using System.Text;
using System.Linq;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;

namespace ScanLog
{
    public partial class SelectModel : Form
    {
        public SelectModel()
        {
            TopMost = true;
            InitializeComponent();
            this.KeyPreview = true;///Для горячих клавиш
        }

        private string name_model;
        public string model { get { return name_model; } set { name_model = value; } }

        private void SelectModel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) { button1.PerformClick(); }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked == true)
                name_model = "24FR250";
            else if (radioButton2.Checked == true)
                name_model = "32FSR400";
            else
                name_model = "43FSR400";
            Close();
        }
    }
}