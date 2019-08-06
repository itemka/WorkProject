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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }

        public string text;
        private int mili_second;
        public int ms
        {
            get
            {
                return mili_second;
            }
            set
            {
                mili_second = value;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == "0")
            {
                text = "1";
                mili_second = Convert.ToInt32(text);
                Close();
            }
            else
            {
                text = textBox1.Text;
                mili_second = Convert.ToInt32(text);
                Close();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}