using System;
using System.Windows.Forms;

namespace RedRat3
{
    public partial class inputName : Form
    {
        private string _name = "";
        public string name { get { return _name; } set { _name = value; } }

        public inputName()
        {
            InitializeComponent();
            this.KeyPreview = true;
            textBox1.Text = name;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            _name = textBox1.Text;
            Close();
        }

        // Событие для горячих клавиш
        private void inputName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
        }
    }
}