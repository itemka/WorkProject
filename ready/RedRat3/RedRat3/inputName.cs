using System;
using System.Windows.Forms;

namespace RedRat3
{
    public partial class inputName : Form
    {
        public inputName()
        {
            InitializeComponent();
            this.KeyPreview = true;
        }

        public string nameSignal { get { return name; } set { name = value; } }
        private string name;

        private void button1_Click(object sender, EventArgs e)
        {
            name = textBox1.Text;
            Close();
        }

        // Событие для горячих клавиш
        private void inputName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
        }
    }
}