using System;
using System.IO;
using System.Windows.Forms;

namespace RedRat3
{
    // Форма настроек(установка переодичности вывода сигнала)
    public partial class FormSetting : Form
    {
        private int mili_second;
        public int ms { get { return mili_second; } set { mili_second = value; } }

        public FormSetting()
        {
            this.KeyPreview = true;
            InitializeComponent();
            textBox1.Select();
            textBox1.Text = "";
        }

        // Кнопка установки переодичности вывода сигнала
        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox1.Text == " " || textBox1.Text == "0")
            {
                MessageBox.Show("Введите итервал(ms) в настройках.");
            }
            else
            {
                mili_second = Convert.ToInt32(textBox1.Text);
                Close();
            }
        }

        // Событие(ввод только цифр), которое происходит при фокусировке textBox1
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8)) { e.Handled = true; }
        }

        // Событие для горячих клавиш
        private void FormSetting_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) button1.PerformClick();
        }

        //Кнопка сохранения драйверов
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllBytes(Form1.path1 + "drivers.rar", Properties.Resources.drivers);
                MessageBox.Show("Драйвера выгружены успешно.");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}