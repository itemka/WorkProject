using System;
using RedRat.IR;
using System.IO;
using RedRat.RedRat3;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;

namespace RedRat3
{
    // Главная форма
    // Для добавления любого файла в проект ПРАВИЛЬНО: ->
    // Обозреватель решений -> Properties -> Resources.resx -> Добавить ресурс -> Добавить существующий файл...
    public partial class Form1 : Form
    {
        public static string RedRat3_Data = "C:\\RedRat3_Data\\";
        public static string modelsPath = "C:\\RedRat3_Data\\Models";
        public ListViewItem FocusedItem { get; }

        public IRedRat3 RedRat3;
        public static IRPacket OutputIR;
        public int interVal;
        public static string pathClick = modelsPath;
        public string tempPathForCopyPast;
        public bool copy = false;


        public Form1()
        {
            InitializeComponent();
            //Для горячих клавиш
            this.KeyPreview = true;
            button2.Enabled = false;
            button2.BackColor = Color.DimGray;
            button3.Enabled = false;
            button3.BackColor = Color.DimGray;
            textBox1.Text = "";

            while (!Directory.Exists(RedRat3_Data)) { Directory.CreateDirectory(RedRat3_Data); }
            while (!Directory.Exists(modelsPath)) { Directory.CreateDirectory(modelsPath); }

            listView1.LargeImageList = imageList1;
            imageList1.Images.Add("Folder", Properties.Resources.folder5);
            imageList1.Images.Add("File", Properties.Resources.file5);
            listView1.View = View.Details;
            AddFoldersWithFileFromEnterPath(modelsPath);
        }


        #region ListView
        // Send path and it returned name Directory where be this exe-file   
        string ReverseStringAndDelete(string s)
        {
            string str = "";

            //Переворачивает строку задом на перед
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);

            var b = false;
            for (int i = 0; i < arr.Length; i++)
            {
                if (b == false)
                {
                    if (arr[i] == '\\')
                    {
                        b = true;
                    }
                    else
                    {
                        str += arr[i];
                    }

                }
            }
            char[] rra = str.ToCharArray();
            Array.Reverse(rra);

            return new string(rra);
        }
        // Делаю путь на один переход назад
        string ShortPath(string s)
        {
            string str = "";

            //Переворачивает строку задом на перед
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);

            var b = false;
            for (int i = 0; i < arr.Length; i++)
            {
                if (b == false)
                {
                    if (arr[i] == '\\')
                    {
                        b = true;
                    }
                    else
                    {
                        //str += arr[i];
                    }
                }
                else
                {
                    str += arr[i];
                }
            }
            char[] rra = str.ToCharArray();
            Array.Reverse(rra);

            return new string(rra);
        }
        // Добавляем папки с файлами в ListView
        public void AddFoldersWithFileFromEnterPath(string path, bool back = false)
        {
            string _path = back == false ? path : ShortPath(path);

            listView1.Clear();
            listView1.Columns.Add(_path, -2, HorizontalAlignment.Left);
            
            foreach (var item in Directory.GetDirectories(_path))
            {
                if (item != null)
                {
                    pathClick = item;
                    string temp = ReverseStringAndDelete(item);
                    listView1.Items.Add(temp, 0);
                }
            }
            foreach (var item in Directory.GetFiles(_path))
            {
                if (item != null)
                {
                    pathClick = item;
                    string temp = ReverseStringAndDelete(item);
                    listView1.Items.Add(temp, 1);
                }
            }
            pathClick = _path;
            label2.Text = pathClick;
        }
        // Button return back
        private void button5_Click(object sender, EventArgs e)
        { if (pathClick.Length > modelsPath.Length) AddFoldersWithFileFromEnterPath(pathClick, true); }
        // Event for ItemActivate
        private void listView1_ItemActivate(object sender, EventArgs e)
        {
            SignalOutput SO = new SignalOutput();
            string newPath = pathClick + "\\" + listView1.FocusedItem.Text;
            
            if (File.Exists(newPath))//Checker is folder or file
            {
                //MessageBox.Show("File is choice!");
                //Process.Start(pathClick);
                SearchRedRat SR = new SearchRedRat();
                RedRat3 = SR.FindRedRat();
                OutputIR = SO.ConvertingBINARYtoIRsignal(newPath);
                if (OutputIR != null)
                {
                    label4.Text = listView1.FocusedItem.Text;
                    button2.Enabled = true;
                    button2.BackColor = Color.FromArgb(247, 98, 1);
                    button3.Enabled = true;
                    button3.BackColor = Color.FromArgb(19, 129, 214);
                }
            }
            else { AddFoldersWithFileFromEnterPath(newPath, false); }
        }
        //below
        // Кнопка добавления новой папки
        private void button6_Click(object sender, EventArgs e)
        {
            inputName IN = new inputName(); IN.ShowDialog();
            if (!Directory.Exists(pathClick + "\\" + IN.name)) { Directory.CreateDirectory(pathClick + "\\" + IN.name); }
            else { MessageBox.Show("Такая папка есть."); }
            AddFoldersWithFileFromEnterPath(pathClick);
        }
        // Кнопка копировать
        private void button4_Click(object sender, EventArgs e)
        {
            if (listView1.FocusedItem.Focused)
            {
                string newPath = pathClick + "\\" + listView1.FocusedItem.Text;
                tempPathForCopyPast = newPath;
                if (File.Exists(newPath))
                { var m1 = MessageBox.Show("Файл: " + newPath + " скопирован.", "Копирование", MessageBoxButtons.OK); }
                else { var m2 = MessageBox.Show("Папка: " + newPath + " скопирована.", "Копирование", MessageBoxButtons.OK); }
                copy = true;
            }
        }
        // Копия папки
//______________________Доделать
        // Кнопка вставить
        private void button10_Click(object sender, EventArgs e)
        {
            if (copy)
            {
                if (File.Exists(tempPathForCopyPast) || Directory.Exists(tempPathForCopyPast))
                {
                    string objectName = ReverseStringAndDelete(tempPathForCopyPast);
                    if (File.Exists(tempPathForCopyPast))
                    {
                        FileInfo fileInfo = new FileInfo(tempPathForCopyPast);
                        fileInfo.CopyTo(pathClick + "\\" + objectName, true);
                        AddFoldersWithFileFromEnterPath(pathClick);
                    }
                    else
                    {
//______________________Доделать
                        Directory.Move(tempPathForCopyPast, pathClick + "\\" + objectName);
                        AddFoldersWithFileFromEnterPath(pathClick);
                    }
                }
            }
        }
        // Кнопка переименовать
        private void button11_Click(object sender, EventArgs e)
        {
            if (listView1.FocusedItem.Focused)
            {
                string newPath = pathClick + "\\" + listView1.FocusedItem.Text;
                inputName IN = new inputName(); IN.ShowDialog();
                if (File.Exists(newPath))
                {
                    File.Move(newPath, pathClick + "\\" + IN.name);
                    AddFoldersWithFileFromEnterPath(pathClick);
                }
                else
                {
                    Directory.Move(newPath, pathClick + "\\" + IN.name);
                    AddFoldersWithFileFromEnterPath(pathClick);
                }
            }
        }
        // Кнопка удаления выбранного папки или файла
        private void button9_Click(object sender, EventArgs e)
        {
            if (listView1.FocusedItem.Focused)
            {
                string newPath = pathClick + "\\" + listView1.FocusedItem.Text;
                if (File.Exists(newPath))
                {
                    var m1 = MessageBox.Show("Файл: " + newPath + " будет удален.", "Удаление", MessageBoxButtons.YesNo);
                    if (m1 == DialogResult.Yes)
                    {
                        File.Delete(newPath);
                        AddFoldersWithFileFromEnterPath(pathClick);
                    }
                }
                else
                {
                    var m2 = MessageBox.Show("Папка: " + newPath + " будет удалена.", "Удаление", MessageBoxButtons.YesNo);
                    if (m2 == DialogResult.Yes)
                    {
                        Directory.Delete(newPath, true);
                        AddFoldersWithFileFromEnterPath(pathClick);
                    }
                }
            }
        }
        #endregion



        #region Events
        // Событие для горячих клавиш
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                switch (e.KeyCode)
                {
                    //case Keys.F1: поискRedRat3ToolStripMenuItem.PerformClick(); break;
                    //case Keys.F2: settingToolStripMenuItem.PerformClick(); break;
                    //case Keys.F3: выбратьСигналF3ToolStripMenuItem.PerformClick(); break;
                    //case Keys.F5: button1.PerformClick(); break;
                    //case Keys.F6: button2.PerformClick(); break;
                    //case Keys.F7: button3.PerformClick(); break;
                    //case Keys.F8: button4.PerformClick(); break;
                    default: break;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Событие(ввод только цифр), которое происходит при фокусировке textBox1
        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != Convert.ToChar(8)) { e.Handled = true; }
        }
        #endregion
        #region Buttons
        // Кнопка поиска RedRat3(F1)
        private void поискRedRat3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SearchRedRat SR3 = new SearchRedRat();
                RedRat3 = SR3.FindRedRatInfo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Кнопка Выбрать сигнал(F3)
        private void dshfnmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SignalOutput SO = new SignalOutput();
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.InitialDirectory = RedRat3_Data;
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    MessageBox.Show(OFD.FileName);
                    OutputIR = SO.ConvertingBINARYtoIRsignal(OFD.FileName);
                    if (OutputIR != null)
                    {
                        button2.Enabled = true;
                        button2.BackColor = Color.FromArgb(247, 98, 1);
                        button3.Enabled = true;
                        button3.BackColor = Color.FromArgb(19, 129, 214);
                    }
                }
                SearchRedRat SR = new SearchRedRat();
                RedRat3 = SR.FindRedRat();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Кнопка захвата одиночного сигнала
        private void button1_Click(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button1, "Сигнал сохранится по пути левее");
            try
            {
                SearchRedRat SR3 = new SearchRedRat();
                RedRat3 = SR3.FindRedRat();
                if (RedRat3 != null)
                {
                    IRsignalTrainingMode IRSTM = new IRsignalTrainingMode();
                    var FTC = new FormTimerCapture(10, "Подайте сигнал с пульта");
                    var waitsignal2 = Task.Factory.StartNew(() => { FTC.ShowDialog(); });

                    var qwe = Task.Factory.StartNew(() => {
                        IRSTM.CaptureSignal();
                        OutputIR = IRSTM.GetSignal();
                        if (OutputIR != null)
                        {
                            button2.Enabled = true;
                            button2.BackColor = Color.FromArgb(247, 98, 1);
                            button3.Enabled = true;
                            button3.BackColor = Color.FromArgb(19, 129, 214);
                            AddFoldersWithFileFromEnterPath(pathClick);
                            FTC.Close();
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Кнопка вывода одиночного сигнала
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if ((RedRat3 != null) && (OutputIR != null))
                {
                    SignalOutput SO = new SignalOutput();
                    SO.OutputOneIRsignal(RedRat3, OutputIR);
                }
                else if (OutputIR == null)
                {
                    MessageBox.Show("Захватите сигнал.", "Вывод сигнала", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Кнопка вывода сигнала по таймеру 
        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" || textBox1.Text == " " || textBox1.Text == "0")
                {
                    MessageBox.Show("Задайте интервал.", "Вывод сигнала", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    interVal = Convert.ToInt32(textBox1.Text); // Установка переодичности вывода сигнала
                    if (interVal > 0 && interVal < 9999999)
                    {
                        FormTimerOutputIRsignal FTOIRS = new FormTimerOutputIRsignal(RedRat3, OutputIR, interVal);
                        if ((RedRat3 != null) && (OutputIR != null))
                        {
                            FTOIRS.ShowDialog();
                        }
                        else if (OutputIR == null)
                        {
                            MessageBox.Show("Захватите сигнал.", "Вывод сигнала", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Заданный интервал не входит в диапазон от 0 до 9999999.", "Вывод сигнала", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Кнопка вызова пульта
        private void пультToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoteController RC = new RemoteController();
            RC.ShowDialog();
        }
        // Кнопка сохранения драйверов
        private void выгрузитьДрайвераДляRedRat3ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllBytes(RedRat3_Data + "Драйвера_для_RedRat3.zip", Properties.Resources.Драйвера_для_RedRat3);
                MessageBox.Show("Драйвера выгружены успешно.\nПуть: " + RedRat3_Data + "Драйвера_для_RedRat3.zip");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        // Кнопка вывода папки сигналов
        private void button7_Click(object sender, EventArgs e)
        {
            MessageBox.Show(tempPathForCopyPast);
        }
        #endregion
    }
}