using System;
using System.IO;
using System.Data;
using System.Linq;
using System.Text;
using System.Timers;
using System.Drawing;
using System.IO.Ports;
using System.Threading;
using System.Management;
using System.Windows.Forms;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using RedRat;
using RedRat.IR;
using RedRat.Util;
using RedRat.RedRat3;
using RedRat.RedRat3.USB;
using RedRat.AVDeviceMngmt;

namespace RedRat3
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //button1_Click(button1, null);

            //bool ArduinoPortFound = false;

            //try
            //{
            //    string[] ports = SerialPort.GetPortNames();
            //    foreach (string port in ports)
            //    {
            //        port1 = new SerialPort(port, 9600);
            //        if (ArduinoDetected())
            //        {
            //            ArduinoPortFound = true;
            //            break;
            //        }
            //        else
            //        {
            //            ArduinoPortFound = false;
            //        }
            //    }
            //}
            //catch { }

            //if (ArduinoPortFound == false) return;
            //System.Threading.Thread.Sleep(500); // немного подождем

            //port1.BaudRate = 9600;
            //port1.DataBits = 8;
            //port1.RtsEnable = false;
            //port1.DtrEnable = true;
            //port1.ReadTimeout = 1000;

            ////try
            ////{
            ////    port1.Open();
            ////}
            ////catch { }

        }
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
        }
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void button2_Click(object sender, EventArgs e)
        {
        }
        private void label2_Click(object sender, EventArgs e)
        {
        }
        private void label1_Click(object sender, EventArgs e)
        {
        }
        private void groupBox1_Enter(object sender, EventArgs e)
        {
        }


        //Поиск
        private void redRatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Search S = new Search();
            MessageBox.Show(S.OpenRedRat3(), @"RedRat3 Info.", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        //Вызов формы справки
        private void справкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 f3 = new Form3();
            f3.ShowDialog();
        }


        //_____________________________________________________________________________________________________________________________________________________________________Для Arduino

        SerialPort port1 = new System.IO.Ports.SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);

        //SerialPort port1;

        //private bool ArduinoDetected()
        //{
        //    try
        //    {
        //        port1.Open();
        //        System.Threading.Thread.Sleep(1000);
        //        // небольшая пауза, ведь SerialPort не терпит суеты

        //        string returnMessage = port1.ReadLine();
        //        port1.Close();

        //        // необходимо чтобы void loop() в скетче содержал код Serial.println("Info from Arduino");
        //        if (returnMessage.Contains("b") || returnMessage.Contains("a"))
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}




        public bool tim2 = true;     //Переменная для запуска и выключение таймера
        //Для того, чтобы в buuton1 сделать вывод данных из файла
        public void OutputFromFile()
        {
            //port1 = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
            
            if (label4.Text == "Имя файла")
            {
                MessageBox.Show("Файл не выбран!\nВыберите файл.", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else if (tim2 == true)
            {
                label4.ForeColor = Color.Chartreuse;
                button1.BackColor = Color.IndianRed;
                //timer2.Enabled = !timer2.Enabled;
                timer2.Enabled = true;  //Включаем таймер

                tim2 = false;
            }
            else if (tim2 == false)
            {
                label4.ForeColor = Color.White;
                button1.BackColor = Color.FromArgb(74, 74, 74);
                timer2.Enabled = false; //Выключаем таймер
                tim2 = true;
                //при выключении таймера выставляем k = 0
                k = 0;
            }
        }

        //Переменная для выполнения один раз вывода из файла
        public int k = 0;
        //переменая для исключения ошибки
        public bool isError;
        //Таймер2 для вывода из файла
        private void timer2_Tick(object sender, EventArgs e)
        {
            //try
            //{
                string returnMessage = "";

                serialPort1.RtsEnable = false;
                serialPort1.DtrEnable = true;
                //Если порт закрыт
                if (!port1.IsOpen)
                {
                    port1.Open();
                    Thread.Sleep(1000);
                    returnMessage = port1.ReadExisting();
                    port1.Close();
                }
                //port1.Open();
                
                
                //string returnMessage = port1.ReadExisting();

                //port1.Close();
                //MessageBox.Show(returnMessage);

                //Паузой, после того, как выполнелось один раз и не перешло в положение вертикально
                if (returnMessage[0] == 'a')
                {
                    button1.BackColor = Color.FromArgb(0, 192, 0);

                    k++;
                    //Если выполнело 1 раз
                    if (k == 1)
                    {

                        OutputFD();

                        

                        if (!port1.IsOpen)
                        {
                            //MessageBox.Show("1");
                            port1.Open();
                            Thread.Sleep(1000);
                            port1.Write("1");
                            Thread.Sleep(1000);
                            port1.Write("0");
                            port1.Close();
                        }
                        else
                        {
                            
                            port1.Write("1");
                            Thread.Sleep(1000);
                            port1.Write("0");
                        }
                    }
                    else if (k > 1)
                    {
                        return;
                    }
                }
                else if (returnMessage == null || returnMessage == "")
                {
                MessageBox.Show("3");
                }
                else
                {
                    button1.BackColor = Color.IndianRed;
                    k = 0;
                    //port1.Close();
                }

            //if (port1.IsOpen)
            //{
            //    port1.Close();
            //}
            //port1.Close();

            //}
            //catch (Exception)
            //{
            //    //MessageBox.Show(ex.Message);
            //    isError = true;
            //}
        }


        //_____________________________________________________________________________________________________________________________


        public string FilePath;
        public string FileName;
        //Переменная для выбора что именно выводить: NEC код или XML файлы
        public bool outputOfFileXML;

        //Кнопка вывода из файла
        private void button1_Click(object sender, EventArgs e)
        {
            OutputFromFile();
        }

        //Открывает диалоговое окно и записывает имя выбранного файла
        private void открытьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var messageFile = MessageBox.Show(
                                                "Yes - просмотр считанных команд с ПДУ\nNO - просмотр NEC-кода",
                                                "Выбор данных для просмотра и корректировки",
                                                MessageBoxButtons.YesNoCancel
                                             );
            if (messageFile == DialogResult.Yes)
            {
                OpenFileDialog OFD = new OpenFileDialog();
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    FilePath = OFD.FileName;
                    FileName = OFD.SafeFileName;
                    //MessageBox.Show(FilePath);
                    label4.Text = FileName;

                    //Переменная для выбора что именно выводить: NEC код или XML файлы
                    outputOfFileXML = true;

                    //Вывод сигналов в textBox2
                    textBox2.Text = File.ReadAllText(@"" + FilePath, Encoding.Default);
                }
            }
            else if (messageFile == DialogResult.No)
            {
                OpenFileDialog OFD = new OpenFileDialog();
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    FilePath = OFD.FileName;
                    FileName = OFD.SafeFileName;
                    //MessageBox.Show(FilePath);
                    label4.Text = FileName;

                    //Переменная для выбора что именно выводить: NEC код или XML файлы
                    outputOfFileXML = false;

                    //Вывод сигналов в textBox2
                    textBox2.Text = File.ReadAllText(@"" + FilePath, Encoding.Default);
                }
            }
        }

        //Создан для вывода данных файла по времени
        public void OutputFD()
        {
            if (outputOfFileXML == true)
            {
                string path = @"" + FilePath + "";
                OutputCapturedSignal(path);
            }
            else if (outputOfFileXML == false)
            {
                string path = @"" + FilePath + "";
                OutputOfFile OOF = new OutputOfFile();
                OOF.OutputFileData(path);
            }
        }


        //____________________________________________________________________________________________________________________________________________________Вывод единичного сигнала по имени, после ввода NEC кода


        //Переменная - путь до XML файлоа
        string pathToFileXML = "C:\\RedRat3\\OneXMLsignal.xml";
        //переменная для вывода сигнала по времени
        public static bool period = false;

        //Кнопка вывода сигнала один раз
        private void button4_Click(object sender, EventArgs e)
        {
            //port1.Close();

            new CustomSignal(Convert.ToString(textBox1.Text));
            SimpleSignalOutput SSO = new SimpleSignalOutput();
            SSO.SignalOutput(pathToFileXML);

            //Для таймера специально был сделан
            SSO.Output();
        }

        //Вызов формы задания ms
        private void button2_Click_1(object sender, EventArgs e)
        {
            button2.BackColor = Color.DarkOrange;
            Form2 f2 = new Form2();
            f2.ShowDialog();
            timer1.Interval = f2.ms;
            period = true;
        }

        //Кнопка вывода сигнала по времени
        private void button3_Click(object sender, EventArgs e)
        {
            if (period == true)
            {
                //Пуск/Пауза таймера
                timer1.Enabled = !timer1.Enabled;

                new CustomSignal(Convert.ToString(textBox1.Text));
                SimpleSignalOutput SSO = new SimpleSignalOutput();
                SSO.SignalOutput(pathToFileXML);

                //Для таймера специально был сделан
                SSO.Output();
            }
            else
            {
                MessageBox.Show("Задайте период", "Уведомление", MessageBoxButtons.OK);
            }
        }

        //Для таймера специально был сделан метод
        public void Output_Signal()
        {
            SimpleSignalOutput SSO = new SimpleSignalOutput();
            SSO.SignalOutput(pathToFileXML);
            SSO.Output();
        }

        //Таймер
        int i = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            i++;
            label2.Text = i.ToString();
            Output_Signal();
        }


        //___________________________________________________________________________________________________________________________________________________Вывод из файла, после считывания с пульта


        //|************************************************************************************************************|\\
        //|                                                                                                            |\\
        //|            Последовательность подключения: 1. RedRat3, 2. Arduino, 3. В сеть гидроопускатель               |\\
        //|            *********************************************************************************               |\\
        //|      1. При удалении файлов из папки C:\\RedRat3\\XMLsignal всех файлов, ничего не работает                |\\
        //|____________________________________________________________________________________________________________|\\


        public bool sigWriteXML = false;
        public int fileNumber = 1;
        public bool pathToFileOK = false;
        //Путь к папке C:\\RedRat3\\XMLsignal\\ для того, чтобы добавить в SignalOutput + имя из TXT файла
        public string pathToFolder = "C:\\RedRat3\\XMLsignal\\";
        //Путь к TXT файлу
        public string pathToFileTXT = @"C:\RedRat3\XMLsignal.txt";

        //Удаляет все файлы в папке по пути C:\\RedRat3\\XMLsignal и очистка файла XMLsignal.txt
        private void button8_Click(object sender, EventArgs e)
        {
            DeleteAllFileEndFolder();
        }

        //Кнопка захвата сигнала
        private void button6_Click(object sender, EventArgs e)
        {
            CapturedSignal();
        }

        //Кнопка сохранения захваченных сигналов в файл C:\RedRat3\XMLsignal.txt
        private void button9_Click(object sender, EventArgs e)
        {
            LoadingCapturedSignalInTXT();
        }

        //Кнопка вывода захваченных сигналов один раз из файла C:\RedRat3\XMLsignal.txt
        private void button7_Click(object sender, EventArgs e)
        {
            OutputCapturedSignal(pathToFileTXT);
        }


        //_____________________________________________________________Методы для XML файлов


        //Очищает файл C:\RedRat3\XMLsignal.txt и удаляет все файлы в папке C:\\RedRat3\\XMLsignal
        public void DeleteAllFileEndFolder()
        {
            var mess = MessageBox.Show("Стереть все захваченные сигналы?", "Подтверждение", MessageBoxButtons.YesNo);
            if (mess == DialogResult.Yes)
            {
                File.WriteAllText(pathToFileTXT, string.Empty);

                DirectoryInfo dirInfo = new DirectoryInfo("C:\\RedRat3\\XMLsignal");
                foreach (FileInfo file in dirInfo.GetFiles())
                {
                    file.Delete();
                }

                fileNumber = 1;
            }
        }

        //Метод каждый раз создает новый файл
        public string PathToFileNameXML()
        {
            string fileNameXML = "";
            pathToFileOK = true;
            if (pathToFileOK == true)
            {
                if (fileNumber > 0 && fileNumber < 10)
                {
                    fileNameXML = "C:\\RedRat3\\XMLsignal\\XMLsignal00" + fileNumber.ToString() + ".xml";
                    fileNumber++;
                }
                else if (fileNumber > 9 && fileNumber < 100)
                {
                    fileNameXML = "C:\\RedRat3\\XMLsignal\\XMLsignal0" + fileNumber.ToString() + ".xml";
                    fileNumber++;
                }
                else if (fileNumber > 99 && fileNumber < 1000)
                {
                    fileNameXML = "C:\\RedRat3\\XMLsignal\\XMLsignal" + fileNumber.ToString() + ".xml";
                    fileNumber++;
                }
                else if (fileNumber > 999)
                {
                    MessageBox.Show("Превышен лимит сигналов! Максимум: 999 сигналов.");
                    return fileNameXML;
                }
            }

            return fileNameXML;
        }

        //Метод захватывает сигнал и записывает его в папку C:\\RedRat3\\XMLsignal
        public void CapturedSignal()
        {
            SimpleSignalOutput SSO = new SimpleSignalOutput();
            if (SSO.FindRedRat3() != null)
            {
                var mess = MessageBox.Show("Ввод сигнала в RedRat3 осуществляется в течении 10с после подтверждения. \n\nВключить RedRat3 на прием?", "Подтверждение", MessageBoxButtons.YesNo);
                if (mess == DialogResult.Yes)
                {
                    IRsignalTrainingMode IRSTM = new IRsignalTrainingMode();
                    IRSTM.CaptureSignal(PathToFileNameXML());
                }
            }
            else
            {
                MessageBox.Show("Нет подключенных устройств RedRat3. Подключите RedRat3 и попробуйте снова.", "Проверка подключения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //Метод сохраняет имена файлов XML(захваченные сигналы) в файле C:\RedRat3\XMLsignal.txt
        public void LoadingCapturedSignalInTXT()
        {
            SimpleSignalOutput SSO = new SimpleSignalOutput();
            DirectoryInfo dirInfo = new DirectoryInfo("C:\\RedRat3\\XMLsignal");
            if (SSO.FindRedRat3() != null)
            {
                if (dirInfo.GetFiles().Length != 0)
                {
                    string pathToFileTXT = @"C:\RedRat3\XMLsignal.txt";
                    //Добавляем имена XML-файлов в TXT файл C:\RedRat3\XMLsignal.txt
                    StreamWriter sw = File.AppendText(pathToFileTXT);
                    foreach (FileInfo item in dirInfo.GetFiles())
                    {
                        sw.WriteLine(item.Name);
                    }
                    sw.Close();

                    //Вывод сигналов в textBox2
                    textBox2.Text = File.ReadAllText(pathToFileTXT, Encoding.Default);

                    label4.Text = "XMLsignal.txt";

                    MessageBox.Show("Захваченные сигналы сохранены в файл " + pathToFileTXT + "\n\nДля корректного вывода из файла добавьте паузу между сигналами!", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Папка сигналов C:\\RedRat3\\XMLsignal - пуста! Захватите сигнал.", "Ошибка", MessageBoxButtons.OK);
                }
            }
            else
            {
                MessageBox.Show("Нет подключенных устройств RedRat3. Подключите RedRat3 и попробуйте снова.", "Проверка подключения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        //Метод выводит все захваченные сигналы
        public void OutputCapturedSignal(string path)
        {
            StreamReader sr = new StreamReader(path);
            string line;
            while (!sr.EndOfStream)
            {
                line = sr.ReadLine();

                if (line[0] == '-')
                {
                    List<string> l = new List<string>();
                    foreach (var item in line)
                    {
                        l.Add(Convert.ToString(item));
                    }
                    l.RemoveAt(0);
                    string[] lArray = l.ToArray();
                    string time = string.Join("", lArray);

                    //Временно преостановить паток
                    Thread.Sleep(Convert.ToInt32(time));
                }
                else
                {
                    SimpleSignalOutput SSO = new SimpleSignalOutput();
                    SSO.SignalOutput(pathToFolder + line);
                    SSO.Output();
                }

                //Временно преостановить паток
                Thread.Sleep(200);
            }
            sr.Close();
        }


        //___________________________________________________________________________________________________________________________________________________Вывод текста из файла в textBox2 


        public string nameFileOfForm4;
        //Сохранение изменений TextBox в файл с заданным именем в папку C:\RedRat3\SavedSignals
        private void button10_Click(object sender, EventArgs e)
        {
            SimpleSignalOutput SSO = new SimpleSignalOutput();
            if (SSO.FindRedRat3() != null)
            {
                var mes = MessageBox.Show("Сохранить данные?", "Уведомление", MessageBoxButtons.YesNo);
                if (mes == DialogResult.Yes)
                {
                    Form4 f4 = new Form4();
                    f4.ShowDialog();
                    nameFileOfForm4 = @"C:\RedRat3\SavedSignals\" + f4.nameFile + ".txt";

                    if (f4.nameFile != "" || f4.nameFile != " " || f4.nameFile != "  ")
                    {
                        File.WriteAllText(nameFileOfForm4, string.Empty);

                        //Добавляем имена XML-файлов в TXT файл nameFileOfForm4
                        StreamWriter sw = File.AppendText(nameFileOfForm4);
                        foreach (string item in textBox2.Lines)
                        {
                            sw.WriteLine(item);
                        }
                        sw.Close();

                        textBox2.Text = "";
                        label4.Text = "Имя файла";
                        MessageBox.Show("Данные были сохранены в файл: " + nameFileOfForm4, "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                MessageBox.Show("Нет подключенных устройств RedRat3. Подключите RedRat3 и попробуйте снова.", "Проверка подключения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}