using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System;

using System.Net;
using System.Net.Mail;

using System.DirectoryServices;
using System.DirectoryServices.ActiveDirectory;
using System.DirectoryServices.Protocols;

using Outlook = Microsoft.Office.Interop.Outlook;
using AddressEntries = Microsoft.Office.Interop.Outlook.AddressEntries;

namespace ZEBT
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            comboBox1.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox1.AutoCompleteSource = AutoCompleteSource.ListItems;
            comboBox2.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            comboBox2.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        #region MenuStrip

        private void обновитьСписокУчетныхЗаписейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdataListUsersHORIZONT updataListUsersHORIZONT = new UpdataListUsersHORIZONT();
            updataListUsersHORIZONT.Show();
        }

        #endregion

        #region raspredy
        // распреды \\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        // Кнопка отметки/снятия выполнения
        public bool SaveData1 = true;
        public bool SaveData2 = true;
        public bool SaveData3 = true;
        public bool SaveData4 = true;
        public bool SaveData5 = true;
        public bool SaveData6 = true;
        public bool SaveData7 = true;
        public bool SaveData8 = true;
        public bool SaveData9 = true;

        private void button1_Click(object sender, EventArgs e)
        {
            if (SaveData1)
            {
                textBox6.BackColor = Color.Lime;
                string dateTime = Convert.ToString(DateTime.Now);
                textBox1.Text = dateTime;
                SaveData1 = false;
            }
            else if(!SaveData1)
            {
                textBox6.BackColor = Color.Gray;
                textBox1.Text = "";
                SaveData1 = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button9_Click(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        // endраспреды\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\
        #endregion

        string nameUsers1 = @"D:\nameUsers1.txt";

        // testButton
        private void button37_Click(object sender, EventArgs e)
        {
            //using (StreamWriter sw = new StreamWriter(nameUsers1, true, System.Text.Encoding.Default))
            //{
            //}
                int k = 0;
                foreach (KeyValuePair<string,string> keyValue in Program.UpdataUsers)
                {
                        k++;
                        //sw.WriteLine(k + ": " + keyValue.Key + " - " + keyValue.Value);
                        comboBox1.Items.Add(keyValue.Key);
                        comboBox2.Items.Add(keyValue.Key);
                }
        }
        
        //Событие возникает при изменении индекса в comboBox2, в нем происходит поиск и сравнение по значениям Key <> Value
        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (KeyValuePair<string, string> item in Program.UpdataUsers)
            {
                if (comboBox2.Text == item.Key)
                {
                    //textBox2.Text = item.Value;
                    break;
                }
            }
        }

        //Отправка mail
        private void button4_Click(object sender, EventArgs e)
        {
            #region mail
            ////smtp сервер
            //string smtpHost = "mail.horizont.by";
            ////smtp порт
            //int smtpPort = 25;
            ////логин
            //string login = "Pavlenko@horizont.by";
            ////пароль
            //string pass = "Pav.789";

            ////создаем подключение
            //SmtpClient client = new SmtpClient(smtpHost);
            //client.Credentials = new NetworkCredential(login, pass);

            ////От кого письмо
            //string from = "Pavlenko@horizont.by";
            ////Кому письмо
            //string to = "itemka2503@yandex.by";
            ////Тема письма
            //string subject = "Письмо от item";
            ////Текст письма
            //string body = "Привет! \n\n\n Это тестовое письмо от item!";

            ////Создаем сообщение
            //MailMessage mess = new MailMessage(from, to, subject, body);

            //try
            //{
            //    client.Send(mess);
            //    MessageBox.Show("Message send");
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //}
            #endregion
        }

        private void отправитьПисьмоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendMessage sendMessage = new SendMessage();
            sendMessage.ShowDialog();
        }

        
    }
}