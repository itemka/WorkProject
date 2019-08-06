using System;
using System.Data;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace RedRat3
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private string nF;
        public string nameFile
        {
            get
            {
                return nF;
            }
            set
            {
                nF = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == " " || textBox1.Text == "  ")
            {
                MessageBox.Show("Имя файла не заданно!", "Уведомление", MessageBoxButtons.OK);
                Close();
            }
            else
            {
                nF = textBox1.Text;
                Close();
            }
        }
    }
}