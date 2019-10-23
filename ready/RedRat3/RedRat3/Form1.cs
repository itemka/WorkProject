using System;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Timers;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Reflection;
using System.Diagnostics;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using RedRat;
using RedRat.IR;
using RedRat.USB;
using RedRat.Util;
using RedRat.RedRat3;
using RedRat.RedRat3.USB;
using RedRat.AVDeviceMngmt;

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
            label1.Enabled = false;
            button3.Enabled = false;
            textBox1.Enabled = false;

            label4.Enabled = false;
            button7.Enabled = false;
            textBox3.Enabled = false;

            button2.Enabled = false;



            while (!Directory.Exists(RedRat3_Data)) { Directory.CreateDirectory(RedRat3_Data); }
            while (!Directory.Exists(modelsPath)) { Directory.CreateDirectory(modelsPath); }

            listView1.LargeImageList = imageList1;
            imageList1.Images.Add("Folder", Properties.Resources.folder5);
            imageList1.Images.Add("File", Properties.Resources.file5);
            listView1.View = View.Details;
            AddFoldersWithFileFromEnterPath(modelsPath);
        }

        public void Messages(string message = "") { textBox2.Text = "- " + message + Environment.NewLine + Environment.NewLine + textBox2.Text; }

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
            SearchRedRat SR = new SearchRedRat();
            RedRat3 = SR.FindRedRat();
            string newPath = pathClick + "\\" + listView1.FocusedItem.Text;

            if (File.Exists(newPath))//Checker is folder or file
            {
                //MessageBox.Show("File is choice!");
                //Process.Start(pathClick);
                OutputIR = SO.ConvertingBINARYorXMLtoIRsignal(newPath);
                if (OutputIR != null)
                {
                    Messages("Выбран файл: " + listView1.FocusedItem.Text);
                    label1.Enabled = true;
                    button3.Enabled = true;
                    textBox1.Enabled = true;

                    label4.Enabled = true;
                    button7.Enabled = true;
                    textBox3.Enabled = true;

                    button2.Enabled = true;
                }
            }
            else { AddFoldersWithFileFromEnterPath(newPath, false); }
        }
        //below
        // Кнопка добавления новой папки
        private void button6_Click(object sender, EventArgs e)
        {
            inputName IN = new inputName(); IN.ShowDialog();
            if ((IN.name != "") && (IN.name != " ") && (IN.name != "  "))
            {
                if (!Directory.Exists(pathClick + "\\" + IN.name))
                {
                    Directory.CreateDirectory(pathClick + "\\" + IN.name); Messages("Добавлена папка: " + IN.name);
                }
                else { Messages("Неверный формат имени. Попробуйте снова."); }
                AddFoldersWithFileFromEnterPath(pathClick);
            }
            else Messages("Папка не создана, введите корректно имя папки.");
        }
        private void button6_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button6, "Создать новую папку");
        }
        // Кнопка копировать
        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.FocusedItem.Focused)
                {
                    string newPath = pathClick + "\\" + listView1.FocusedItem.Text;
                    tempPathForCopyPast = newPath;
                    if (File.Exists(newPath)) { Messages("Файл: " + newPath + " скопирован."); }
                    else { Messages("Папка: " + newPath + " скопирована."); }
                    copy = true;
                }

            }
            catch (Exception) { Messages("Файл не выбран"); }
        }
        private void button4_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button4, "Копировать");
        }
        // Копия папки
        public static void CopyDirectory(DirectoryInfo sourceFolder, DirectoryInfo destinationFolder)
        {
            MessageBox.Show(sourceFolder.FullName);
            if (!destinationFolder.Exists) { destinationFolder.Create(); }
            // Copy all files.
            FileInfo[] files = sourceFolder.GetFiles();
            foreach (FileInfo file in files) { file.CopyTo(Path.Combine(destinationFolder.FullName, file.Name)); }
            // Process subdirectories.
            DirectoryInfo[] dirs = sourceFolder.GetDirectories();
            foreach (DirectoryInfo dir in dirs)
            {
                // Call CopyDirectory() recursively.
                CopyDirectory(dir, new DirectoryInfo(Path.Combine(destinationFolder.FullName, dir.Name)) /*Get destination directory.*/);
            }
        }
        // Кнопка вставить
        private void button10_Click(object sender, EventArgs e)
        {
            try
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
                            Messages("Файл вставлен");
                        }
                        else
                        {
                            //Directory.Move(tempPathForCopyPast, pathClick + "\\" + objectName);
                            DirectoryInfo sourceFolder = new DirectoryInfo(tempPathForCopyPast);
                            DirectoryInfo destinationFolder = new DirectoryInfo(pathClick + "\\" + ReverseStringAndDelete(tempPathForCopyPast));
                            if (pathClick.Contains(ReverseStringAndDelete(tempPathForCopyPast))) //change have/not have substring into path of 
                            {
                                string errorCopyText = "С целью предотвращения рекурсии - Копирование отменено" + Environment.NewLine + " Попытка скопировать в ту же папку.";
                                MessageBox.Show(errorCopyText);
                                Messages(errorCopyText);
                            }
                            else
                            {
                                CopyDirectory(sourceFolder, destinationFolder);
                                AddFoldersWithFileFromEnterPath(pathClick);
                                Messages("Папка вставлена.");
                            }
                        }
                    }
                }
                else { Messages("Файл не скопирован"); }
            }
            catch (Exception) { Messages("Файл не скопирован"); }
        }
        private void button10_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button10, "Вставить");
        }
        // Кнопка переименовать
        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                if (listView1.FocusedItem.Focused)
                {
                    string newPath = pathClick + "\\" + listView1.FocusedItem.Text;
                    inputName IN = new inputName(); IN.ShowDialog();
                    if (File.Exists(newPath))
                    {
                        File.Move(newPath, pathClick + "\\" + IN.name);
                        Messages("Файл " + listView1.FocusedItem.Text + " переименован на: " + IN.name);
                        AddFoldersWithFileFromEnterPath(pathClick);
                    }
                    else
                    {
                        Directory.Move(newPath, pathClick + "\\" + IN.name);
                        Messages("Папка " + listView1.FocusedItem.Text + " переименована на: " + IN.name);
                        AddFoldersWithFileFromEnterPath(pathClick);
                    }
                }
            }
            catch (Exception) { Messages("Файл не выбран"); }
        }
        private void button11_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button11, "Переименовать");
        }
        // Кнопка удаления выбранного папки или файла
        private void button9_Click(object sender, EventArgs e)
        {
            try
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
                            Messages("Файл: " + newPath + " ,был удален.");
                        }
                    }
                    else
                    {
                        var m2 = MessageBox.Show("Папка: " + newPath + " будет удалена.", "Удаление", MessageBoxButtons.YesNo);
                        if (m2 == DialogResult.Yes)
                        {
                            Directory.Delete(newPath, true);
                            AddFoldersWithFileFromEnterPath(pathClick);
                            Messages("Папка: " + newPath + " ,была удалена.");
                        }
                    }
                }
            }
            catch (Exception) { Messages("Файл не выбран"); }
        }
        private void button9_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button9, "Удалить");
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
                    case Keys.F1: button1.PerformClick(); break;
                    case Keys.Add: button2.PerformClick(); break;
                    case Keys.F5: button7.PerformClick(); break;
                    case Keys.F4: button3.PerformClick(); break;
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
        private void button14_Click(object sender, EventArgs e)
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
        private void button14_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button14, "Проверка подключения RedRat3");
        }
        // Кнопка Выбрать сигнал(F3)
        private void button13_Click(object sender, EventArgs e)
        {
            try
            {
                SignalOutput SO = new SignalOutput();
                OpenFileDialog OFD = new OpenFileDialog();
                OFD.InitialDirectory = modelsPath;
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    Messages("Выбран файл: " + OFD.FileName);
                    OutputIR = SO.ConvertingBINARYorXMLtoIRsignal(OFD.FileName);
                    if (OutputIR != null)
                    {
                        label1.Enabled = true;
                        button3.Enabled = true;
                        textBox1.Enabled = true;

                        label4.Enabled = true;
                        button7.Enabled = true;
                        textBox3.Enabled = true;

                        button2.Enabled = true;
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
        private void button13_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button13, "Выбрать файл для вывода через Проводник");
        }
        // Кнопка захвата одиночного сигнала
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                SearchRedRat SR3 = new SearchRedRat();
                RedRat3 = SR3.FindRedRat();
                if (RedRat3 != null)
                {
                    IRsignalTrainingMode IRSTM = new IRsignalTrainingMode();
                    var FTC = new FormTimerCapture(10, "Подайте сигнал с пульта");
                    var waitsignal2 = Task.Factory.StartNew(() => { FTC.ShowDialog(); });

                    var qwe = Task.Factory.StartNew(() =>
                    {
                        IRSTM.CaptureSignal();
                        OutputIR = IRSTM.GetSignal();
                        if (OutputIR != null)
                        {
                            label1.Enabled = true;
                            button3.Enabled = true;
                            textBox1.Enabled = true;

                            label4.Enabled = true;
                            button7.Enabled = true;
                            textBox3.Enabled = true;

                            button2.Enabled = true;

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
        private void button1_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button1, "F1");
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
        private void button2_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button2, "+");
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
                    if (interVal > 0 && interVal < 9999999 && interVal >= 400)
                    {
                        FormTimerOutputIRsignal FTOIRS = new FormTimerOutputIRsignal(RedRat3, interVal, OutputIR);
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
                        MessageBox.Show("Введенный интервал не соответствует (Ввод в милисекндах и не менее 400ms)", "Вывод сигнала", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button3_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button3, "F4");
        }
        // Кнопка сохранения драйверов
        private void button8_Click(object sender, EventArgs e)
        {
            try
            {
                File.WriteAllBytes(RedRat3_Data + "Драйвера_для_RedRat3.zip", Properties.Resources.Драйвера_для_RedRat3);
                Messages("Драйвера выгружены успешно. Путь: " + RedRat3_Data + "Драйвера_для_RedRat3.zip");
                //MessageBox.Show("Драйвера выгружены успешно.\nПуть: " + RedRat3_Data + "Драйвера_для_RedRat3.zip");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button8_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button8, "Выгрузить драйвера для RedRat3 по пути: " + RedRat3_Data);
        }
        // Кнопка вывода папки сигналов
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                string newPath = pathClick + "\\" + listView1.FocusedItem.Text;
                if (!File.Exists(newPath))
                {
                    DirectoryInfo directory = new DirectoryInfo(newPath);
                    DirectoryInfo[] dirs = directory.GetDirectories();
                    FileInfo[] fileInfo = directory.GetFiles();
                    if (dirs.Length == 0)
                    {
                        if (fileInfo.Length != 0)
                        {
                            interVal = Convert.ToInt32(textBox3.Text);
                            if (interVal > 0 && interVal < 9999999 && interVal >= 400) { MessageBox.Show("Введенный интервал не соответствует (Ввод в милисекндах и не менее 400ms)"); }
                            else { FormTimerOutputIRsignal FTOIRS = new FormTimerOutputIRsignal(RedRat3, Convert.ToInt32(textBox3.Text), null, true, newPath, fileInfo); }
                        }
                        else { Messages("Ошибка. Нет Файлов. Для вывода добавьте сигналы."); }
                    }
                    else { Messages("Ошибка. Присутствует папка. Для корректной работы удалите лишние папки. "); }
                }
                else { Messages("Ошибка. Выбран файл"); }
            }
            catch (Exception)
            {
                Messages("Ошибка, не выбрана папка. Попробуйте сново.");
            }

        }
        private void button7_MouseEnter(object sender, EventArgs e)
        {
            ToolTip TT = new ToolTip();
            TT.SetToolTip(button7, "F5");
        }
        #endregion



        private string pointDelete(string str)
        {
            string newStr = "";
            char[] arrayChar = str.ToCharArray();
            for (int i = 0; i < arrayChar.Length; i++)
            {
                if (arrayChar[i] == '.') newStr += "";
                else newStr += arrayChar[i];
            }
            char[] newString = newStr.ToCharArray();
            return new string(newString);
        }


        string signals;
        protected AVDeviceDB avDeviceDB;
        protected AVDevice avDevice;
        string avDeviceDBName = "";

        protected void LoadDB()
        {
            // Read signal database from XML file.
            var openFileDialog = new OpenFileDialog
            {
                Filter = "XML files (*.xml)|*.xml|All files (*.*)|*.*",
                RestoreDirectory = true
            };

            if (openFileDialog.ShowDialog() != DialogResult.OK) return;

            avDeviceDBName = openFileDialog.FileName;
            var fName = openFileDialog.FileName;
            var ser = new XmlSerializer(typeof(AVDeviceDB));
            FileStream fs = null;
            try
            {
                fs = new FileStream((new FileInfo(fName)).FullName, FileMode.Open);
                avDeviceDB = (AVDeviceDB)ser.Deserialize(fs); ;
                //Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            finally
            {
                if (fs != null) fs.Close();
            }
        }

        // Кнопка вызова пульта
        private void button12_Click(object sender, EventArgs e)
        {
            //RemoteController RC = new RemoteController();
            //RC.ShowDialog();

            LoadDB();
            AVDevice[] AVDevices = avDeviceDB.AVDevices;
            //Беру последнее имя и удаляю точку в имени
            string nameFolderAVDeviceDB = ReverseStringAndDelete(pointDelete(avDeviceDBName));
            if (!Directory.Exists(pathClick + "\\" + nameFolderAVDeviceDB))
            {
                Directory.CreateDirectory(pathClick + "\\" + nameFolderAVDeviceDB); //MessageBox.Show(pathClick + "\\" + nameFolderAVDeviceDB);
            }

            AddFoldersWithFileFromEnterPath(pathClick);

            foreach (AVDevice item in AVDevices)
            {
                signals += "________" + item.Name + Environment.NewLine;

                if (!Directory.Exists(pathClick + "\\" + nameFolderAVDeviceDB + "\\" + item.Name))
                {
                    Directory.CreateDirectory(pathClick + "\\" + nameFolderAVDeviceDB + "\\" + item.Name); //MessageBox.Show(pathClick + "\\" + nameFolderAVDeviceDB);
                }
                else { Messages("Такая папка существует."); }

                IRPacket[] Signals = item.Signals;
                foreach (IRPacket element in Signals)
                {
                    //MessageBox.Show(pathClick + "\\" + nameFolderAVDeviceDB + "\\" + item.Name + "\\" + element.Name + ".xml");
                    RRUtil.SerializePacketToXML(pathClick + "\\" + nameFolderAVDeviceDB + "\\" + item.Name + "\\" + element.Name + ".xml", element);
                    signals += "-" + element.Name + Environment.NewLine;
                }

            }
            MessageBox.Show(signals);
        }

    }
}