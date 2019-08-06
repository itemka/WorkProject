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
using System.Windows.Forms;
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
    static class Program
    {

        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Создание каталогов, если их нет
            string path1 = "C:\\RedRat3\\XMLsignal";
            string path2 = "C:\\RedRat3\\SavedSignals";
            while (!Directory.Exists(path1))
            {
                Directory.CreateDirectory(path1);
            }
            while (!Directory.Exists(path2))
            {
                Directory.CreateDirectory(path2);
            }

            if (File.Exists(@"C:\RedRat3\NECcod.txt") == false)
            {
                System.IO.File.AppendAllText("C:\\RedRat3\\NECcod.txt", "");
            }
            if (File.Exists(@"C:\RedRat3\XMLsignal.txt") == false)
            {
                System.IO.File.AppendAllText("C:\\RedRat3\\XMLsignal.txt", "");
            }

            Application.Run(new Form1());
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

    }

    //++++++++++++++++++++++++++++++++++++++++++++++

    public class Search : Form
    {

        public IRedRat3 redRat3;

        public string OpenRedRat3()
        {
            try
            {
                // Найти нет. Связанных с RedRat3
                var devices = RedRat3USBImpl.FindDevices();
                if (devices.Count > 0)
                {
                    // Возьмем первое найденное устройство.
                    redRat3 = (IRedRat3)devices[0].GetRedRat();
                    MessageBox.Show("Подключено устройство: \n\n\"" + redRat3.LocationInformation + "\"", "Проверка подключения", MessageBoxButtons.OK);
                }
                else
                    MessageBox.Show("Нет подключенных устройств RedRat3.", "Проверка подключения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в поиске RedRat.\n" + ex.Message, "Ошибка в поиске RedRat.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return RedRatInfo();
        }

        public string RedRatInfo()
        {
            // Получить и отобразить информацию о RedRat3.
            string msg;
            if (redRat3 != null)
            {
                var sb = new StringBuilder();

                // Физическая информация об оборудовании USB
                var devInfo = (USBDeviceInfo)redRat3.DeviceInformation;
                sb.Append(devInfo + "\n");
                sb.Append("Hardware Version: " + devInfo.ProductName + "." + devInfo.ProductDescriptor.Version);
                sb.Append("\nSerial Number: " + devInfo.SerialNumber);

                // Информация о версии прошивки читается напрямую с RedRat3
                sb.Append("\nFirmware Version: " + redRat3.FirmwareVersion);

                // Физическая позиция возвращается в объекте LocationInfo
                sb.Append("\nLocation: " + redRat3.LocationInformation);

                msg = sb.ToString();
            }
            else
            {
                msg = "No RedRat3 connected.";
            }

            return msg;
        }

    }

    public class TransformationCod : Form1
    {

        //Преобразует десятичное число в двоичное
        public static String ToBin(Int32 input)
        {
            String s = "";
            if (input > 0)
            {
                s += ToBin(input / 2) + (input % 2);
            }
            return s;
        }

        //Переворачивает строку задом на перед
        public static string ReverseString(string s)
        {
            char[] arr = s.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        //_____Д__40bf2fd0____К____40bf2f_____
        public string Transformation(string NEC, out string Data_REDRAT)
        {

            Data_REDRAT = "";

            try
            {

                if (NEC.Length == 6)
                {
                    string NEC_Short = NEC;

                    //________________________________________Короткое____________________________________________

                    //Переворачивает строку
                    string revNEC_short = ReverseString(NEC_Short);

                    //MessageBox.Show(revNEC_short);

                    //Разбивает стоку(код) на символы массива y
                    char[] y = new char[revNEC_short.Length];
                    for (int i = 0; i < revNEC_short.Length; i++)
                    {
                        y[i] = Convert.ToChar(revNEC_short[i]);
                    }

                    char el_5 = y[0];
                    char el_6 = y[1];
                    //MessageBox.Show(Convert.ToString( el_5 + " " + el_6));

                    //Преобразует символьный элемент в десятичное число
                    byte el_5_byte = Convert.ToByte(Convert.ToString(el_5), 16);
                    byte el_6_byte = Convert.ToByte(Convert.ToString(el_6), 16);
                    //MessageBox.Show("el_5_byte - " + Convert.ToString(el_5_byte) + "_____el_6_byte - " + Convert.ToString(el_6_byte));

                    //Преобразует байтовый элемент десятичный в текстовый двоичный
                    string el_5_str = ToBin(Convert.ToInt32(el_5_byte));
                    string el_6_str = ToBin(Convert.ToInt32(el_6_byte));

                    if (el_5_str.Length == 0) { el_5_str = "0000"; }
                    if (el_5_str.Length == 1) { el_5_str = "000" + el_5_str; }
                    if (el_5_str.Length == 2) { el_5_str = "00" + el_5_str; }
                    if (el_5_str.Length == 3) { el_5_str = "0" + el_5_str; }

                    if (el_6_str.Length == 0) { el_6_str = "0000"; }
                    if (el_6_str.Length == 1) { el_6_str = "000" + el_6_str; }
                    if (el_6_str.Length == 2) { el_6_str = "00" + el_6_str; }
                    if (el_6_str.Length == 3) { el_6_str = "0" + el_6_str; }

                    //MessageBox.Show("el_5_str - " + el_5_str + "_____el_6_str - " + el_6_str);

                    //Разбивает два элемента на элементы массива
                    string[] el_5_array = new string[el_5_str.Length];
                    string[] el_6_array = new string[el_6_str.Length];
                    for (int i = 0; i < el_5_str.Length; i++)
                    {
                        el_5_array[i] = Convert.ToString(el_5_str[i]);
                        el_6_array[i] = Convert.ToString(el_6_str[i]);
                        //MessageBox.Show(el_5_array[i] + "         " + el_6_array[i]);
                    }

                    //Инвентирует 5 элемент
                    for (int i = 0; i < el_5_array.Length; i++)
                    {
                        if (el_5_array[i] == "1")
                        {
                            el_5_array[i] = "0";
                        }
                        else if (el_5_array[i] == "0")
                        {
                            el_5_array[i] = "1";
                        }
                        //MessageBox.Show(el_5_array[i] + "     5");
                    }

                    //Инвентирует 6 элемент
                    for (int i = 0; i < el_6_array.Length; i++)
                    {
                        if (el_6_array[i] == "1")
                        {
                            el_6_array[i] = "0";
                        }
                        else if (el_6_array[i] == "0")
                        {
                            el_6_array[i] = "1";
                        }
                        //MessageBox.Show(el_6_array[i] + "     6");
                    }

                    //Объединяет элементы массивов в строку
                    string Elem5 = string.Join(null, el_5_array);
                    string Elem6 = string.Join(null, el_6_array);
                    //MessageBox.Show(Elem5 + "         " + Elem6);


                    //Перевод двоичного в десятичное
                    int z5 = Convert.ToInt32(Elem5, 2);
                    int z6 = Convert.ToInt32(Elem6, 2);
                    string res5 = Convert.ToString(z5);
                    string res6 = Convert.ToString(z6);
                    //MessageBox.Show(res5 + "         " + res6);

                    //Переводит десятичного в шестнадцатиричное
                    string result5 = Convert.ToString(z5, 16);
                    string result6 = Convert.ToString(z6, 16);
                    //MessageBox.Show(result5 + "         " + result6);

                    string two = result5 + result6;

                    //Переворачиваем элементы наоборот
                    string two5end6 = ReverseString(two);
                    //MessageBox.Show(two5end6);

                    //Добавление в конец короткого кода
                    string new_NEC_Short = NEC_Short + two5end6;
                    //MessageBox.Show(new_NEC_Short);


                    NEC = new_NEC_Short;

                    //________________________________________Длинное____________________________________________

                    //Разбивает по два символа строку
                    var s1 = NEC;
                    var chunkSize = 2;
                    var result = (from Match m in Regex.Matches(s1, @".{1," + chunkSize + "}") select m.Value).ToList();

                    //Преобразует в строку, перевернутую по два элемента
                    string[] NEC1 = new string[result.Count];
                    for (int i = 0; i < result.Count; i++)
                    {
                        NEC1[i] = ReverseString(result[i]);
                        //MessageBox.Show(NEC1[i]);
                    }

                    string RevNEC1 = string.Join(null, NEC1);
                    //MessageBox.Show(RevNEC1);


                    //Разбиваю стоку(код) на символы массива b
                    char[] b = new char[RevNEC1.Length];
                    for (int i = 0; i < RevNEC1.Length; i++)
                    {
                        b[i] = Convert.ToChar(RevNEC1[i]);

                    }

                    //Преобразует символьный элемент b в десятичныне числа
                    byte[] DEC = new byte[b.Length];
                    for (int i = 0; i < b.Length; i++)
                    {
                        DEC[i] = Convert.ToByte(Convert.ToString(b[i]), 16);
                        //MessageBox.Show("DEC - " + Convert.ToString(DEC[i]));
                    }

                    //Преобразует байтовый элемент DEC в текстовый BIN
                    string[] BIN = new string[DEC.Length];
                    for (int i = 0; i < DEC.Length; i++)
                    {
                        BIN[i] = ToBin(Convert.ToInt32(DEC[i]));

                        if (BIN[i].Length == 0) { BIN[i] = "0000"; }
                        if (BIN[i].Length == 1) { BIN[i] = "000" + BIN[i]; }
                        if (BIN[i].Length == 2) { BIN[i] = "00" + BIN[i]; }
                        if (BIN[i].Length == 3) { BIN[i] = "0" + BIN[i]; }

                        //MessageBox.Show("BIN - " + BIN[i]);
                    }

                    ////Переворачивает строку задом на перед
                    string[] REVER = new string[BIN.Length];
                    for (int i = 0; i < BIN.Length; i++)
                    {
                        REVER[i] = ReverseString(BIN[i]);
                        //MessageBox.Show("REVER - " + REVER[i]);
                    }

                    //Присваиваем элемент массива REVER[i] строке rev -> разбиваем rev на элементы массива two_three -> 
                    //меняем их на 22/23 -> объединяем элементы массива two_three -> обьединяем элементы массива TwoLong
                    string[] TwoLong = new string[8];
                    for (int i = 0; i < REVER.Length; i++)
                    {
                        string rev = REVER[i];
                        string[] two_three = new string[rev.Length];
                        for (int j = 0; j < rev.Length; j++)
                        {
                            two_three[j] = Convert.ToString(rev[j]);

                            if (two_three[j] == "0") { two_three[j] = "22"; }
                            if (two_three[j] == "1") { two_three[j] = "23"; }

                            //MessageBox.Show("two_three " + j+1 + " - " + two_three[j]);

                        }

                        string Long = string.Join(null, two_three);
                        //MessageBox.Show("Long " + i + " - " + Long);

                        TwoLong[i] = Long;
                    }
                    string InTwoLong = string.Join(null, TwoLong);
                    //MessageBox.Show(InTwoLong);

                    string DataREDRAT = "01" + InTwoLong + "2";

                    //MessageBox.Show(DataREDRAT);

                    Data_REDRAT = DataREDRAT;

                }
                else if (NEC.Length == 8)
                {

                    //________________________________________Длинное____________________________________________

                    //Разбивает по два символа строку
                    var s1 = NEC;
                    var chunkSize = 2;
                    var result = (from Match m in Regex.Matches(s1, @".{1," + chunkSize + "}") select m.Value).ToList();

                    //Преобразует в строку, перевернутую по два элемента
                    string[] NEC1 = new string[result.Count];
                    for (int i = 0; i < result.Count; i++)
                    {
                        NEC1[i] = ReverseString(result[i]);
                        //MessageBox.Show(NEC1[i]);
                    }

                    string RevNEC1 = string.Join(null, NEC1);
                    //MessageBox.Show(RevNEC1);


                    //Разбиваю стоку(код) на символы массива b
                    char[] b = new char[RevNEC1.Length];
                    for (int i = 0; i < RevNEC1.Length; i++)
                    {
                        b[i] = Convert.ToChar(RevNEC1[i]);

                    }

                    //Преобразует символьный элемент b в десятичныне числа
                    byte[] DEC = new byte[b.Length];
                    for (int i = 0; i < b.Length; i++)
                    {
                        DEC[i] = Convert.ToByte(Convert.ToString(b[i]), 16);
                        //MessageBox.Show("DEC - " + Convert.ToString(DEC[i]));
                    }

                    //Преобразует байтовый элемент DEC в текстовый BIN
                    string[] BIN = new string[DEC.Length];
                    for (int i = 0; i < DEC.Length; i++)
                    {
                        BIN[i] = ToBin(Convert.ToInt32(DEC[i]));

                        if (BIN[i].Length == 0) { BIN[i] = "0000"; }
                        if (BIN[i].Length == 1) { BIN[i] = "000" + BIN[i]; }
                        if (BIN[i].Length == 2) { BIN[i] = "00" + BIN[i]; }
                        if (BIN[i].Length == 3) { BIN[i] = "0" + BIN[i]; }

                        //MessageBox.Show("BIN - " + BIN[i]);
                    }

                    ////Переворачивает строку задом на перед
                    string[] REVER = new string[BIN.Length];
                    for (int i = 0; i < BIN.Length; i++)
                    {
                        REVER[i] = ReverseString(BIN[i]);
                        //MessageBox.Show("REVER - " + REVER[i]);
                    }

                    //Присваиваем элемент массива REVER[i] строке rev -> разбиваем rev на элементы массива two_three -> меняем их на 22/23 -> объединяем элементы массива two_three -> обьединяем элементы массива TwoLong
                    string[] TwoLong = new string[8];
                    for (int i = 0; i < REVER.Length; i++)
                    {
                        string rev = REVER[i];
                        string[] two_three = new string[rev.Length];
                        for (int j = 0; j < rev.Length; j++)
                        {
                            two_three[j] = Convert.ToString(rev[j]);

                            if (two_three[j] == "0") { two_three[j] = "22"; }
                            if (two_three[j] == "1") { two_three[j] = "23"; }

                            //MessageBox.Show("two_three " + j+1 + " - " + two_three[j]);

                        }

                        string Long = string.Join(null, two_three);
                        //MessageBox.Show("Long " + i + " - " + Long);

                        TwoLong[i] = Long;
                    }

                    string InTwoLong = string.Join(null, TwoLong);
                    //MessageBox.Show(InTwoLong);

                    string DataREDRAT = "01" + InTwoLong + "2";

                    //MessageBox.Show(DataREDRAT);

                    Data_REDRAT = DataREDRAT;
                }
                else
                {
                    MessageBox.Show("Введите правильно NEC код.");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Введите правильно NEC код.", MessageBoxButtons.OK);
            }

            //MessageBox.Show("Data_REDRAT" + Data_REDRAT);
            return Data_REDRAT;

        }

    }

    public class CustomSignal : Form1
    {

        public CustomSignal(string cod_NEC)
        {
            //CustomSignal: показывает, как создать пару ИК-сигналов, а затем сохранить их в стандартном формате XML-файла служебной программы Signal DB,
            //чтобы вы могли загрузить их в служебную программу Signal DB для дальнейшего использования.

            if (cod_NEC.Length == 6 || cod_NEC.Length == 8)
            {

                //Создаем объект декодированного сигнала и выводим полученные данные
                string Data_RR;
                TransformationCod tc = new TransformationCod();
                tc.Transformation(cod_NEC, out Data_RR);

                //________________________________________________________________Вывод декодированного сигнала
                //MessageBox.Show(Data_RR, "Декодированный сигнал");

                // == Создаем IR-сигналов ==
                var sigData = new byte[]
                {
                    0, 1, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 3, 2, 2, 2, 2, 2, 2, 2, 3, 2, 2, 2, 3, 2, 3, 2, 2, 2, 3, 2, 3, 2, 3, 2, 2, 2, 3, 2
                };

                //Засовываем в массив char
                char[] Data_RR_char = Data_RR.ToCharArray();

                //Проверяем символы на 0, 1, 2, 3 и присваиваем массиву sig соответствующие значения - ПРИДУМАТЬ ПО ДРУГОМУ ПОТОМ
                Int32[] sig = new Int32[Data_RR.Length];
                for (int i = 0; i < Data_RR.Length; i++)
                {
                    if (Data_RR_char[i] == '0')
                    {
                        sig[i] = 0;
                    }
                    else if (Data_RR_char[i] == '1')
                    {
                        sig[i] = 1;
                    }
                    else if (Data_RR_char[i] == '2')
                    {
                        sig[i] = 2;
                    }
                    else if (Data_RR_char[i] == '3')
                    {
                        sig[i] = 3;
                    }
                }

                //Конвертируем в byte
                for (int i = 0; i < sigData.Length; i++)
                {
                    sigData[i] = Convert.ToByte(sig[i]);
                }

                var sig1 = CreateSignal("New Signal", sigData);

                // == Создаем объект AV-устройства, чтобы мы могли хранить данные сигнала в файле XML.
                var avDeviceDB = new AVDeviceDB();

                var avDevice = new AVDevice("Sample Device", AVDevice.AVDeviceType.SET_TOP_BOX);
                avDeviceDB.AddAVDevice(avDevice);

                // Добавить сигналы в «Sample Device».
                avDevice.AddSignal(sig1, false);

                // Храните это как файл XML ...
                var ser = new XmlSerializer(typeof(AVDeviceDB));
                TextWriter writer = new StreamWriter((new FileInfo("C:\\RedRat3\\OneXMLsignal.xml")).FullName);
                ser.Serialize(writer, avDeviceDB);
                writer.Close();

                //_______________________________MessageBox.Show("Декодированный сигнал сохранен в XML-файл.", "Сохранение в файл");
                return;

            }
            else
            {
                if (cod_NEC == "")
                {
                    MessageBox.Show("Код ПДУ пуст. Введите код ПДУ.");
                }
                else if (cod_NEC.Length != 6 || cod_NEC.Length != 8)
                {
                    MessageBox.Show("Введите правильно NEC код.");
                }
            }

        }

        protected ModulatedSignal CreateSignal(string name, byte[] mainSignal)
        {
            // Создание объекта сигнала - укажите его имя и частоту модуляции / несущей.
            var modSig = new ModulatedSignal
            {
                Name = name,
                ModulationFreq = 38000
            };

            // Создать длины - ms.
            var lengths = new double[5];
            lengths[0] = 8.897;
            lengths[1] = 4.515;
            lengths[2] = 0.558;
            lengths[3] = 1.539;
            lengths[4] = 1.671;
            modSig.Lengths = lengths;

            // Время паузы между основными и повторяющимися сигналами
            modSig.IntraSigPause = 0; // 40ms

            // Нет повторений раздела повтора
            modSig.NoRepeats = 0;

            // Данные сигнала
            if (mainSignal == null)
            {
                throw new Exception("Основные сигналы не могут быть нулевыми");
            }
            var sigData = new byte[mainSignal.Length + 2];
            var sigDataCount = 0;

            // Добавление основных сигналов
            for (var i = 0; i < mainSignal.Length; i++, sigDataCount++)
            {
                sigData[sigDataCount] = mainSignal[i];
            }

            // Отметить конец основного сигнала
            sigData[sigDataCount++] = ModulatedSignal.EOS_MARKER;

            modSig.SigData = sigData;

            //MessageBox.Show("modSig " + Convert.ToString(modSig.SigData), "Сигнал", MessageBoxButtons.OK);

            return modSig;
        }

    }

    public class SimpleSignalOutput : Form1
    {

        public IRedRat3 redRat3;
        public string[] deviceName;
        public string[] signalName;
        public IRPacket signal;


        public void Output()
        {
            if (FindRedRat3() != null)
            {
                FindRedRat3().OutputModulatedSignal(signal);
            }
            else
            {
                MessageBox.Show("Нет подключенных устройств RedRat3. Подключите RedRat3 и попробуйте снова.", "Проверка подключения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        public void SignalOutput(string pathToFile)
        {

            try
            {
                //if (FindRedRat3() != null)
                //{

                using (var RR3 = FindRedRat3())
                {
                    var signalDB = LoadSignalDB(pathToFile);
                    string deviceN = string.Join(null, deviceName);
                    //MessageBox.Show("Device - " + deviceN);

                    //Задаем дивайс, чтобы вытащить имя сигнала
                    var Device = signalDB.GetAVDevice(deviceN);
                    signalName = Device.GetSignalNames();

                    string signalN = string.Join(null, signalName);
                    //MessageBox.Show("Signal - " + signalN);
                    signal = GetSignal(signalDB, deviceN, signalN);
                    //RR3.OutputModulatedSignal(signal);
                    //MessageBox.Show("Вывод сигнала " + deviceN + " -> " + signalN + ". \n");
                }

                //}
                //else
                //{
                //    MessageBox.Show("Нет подключенных устройств RedRat3. Подключите RedRat3 и попробуйте снова.", "Проверка подключения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in signal upload", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        /// <summary>
        /// Просто находит первый RedRat3, подключенный к этому компьютеру.
        /// </summary>
        public IRedRat3 FindRedRat3()
        {
            try
            {
                var devices = RedRat3USBImpl.FindDevices();

                if (devices.Count > 0)
                {
                    //Возьмем первое найденное устройство.
                    redRat3 = (IRedRat3)devices[0].GetRedRat();
                    //MessageBox.Show("Подключено устройство: \n\n\"" + redRat3.LocationInformation + "\"", "Проверка подключения", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в поиске RedRat.\n" + ex.Message, "Ошибка в поиске RedRat.", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return redRat3;
        }

        /// <summary>
        /// Загружает XML-файл базы данных сигналов.
        /// </summary>
        private AVDeviceDB LoadSignalDB(string path)
        {
            //Чтение базы данных сигналов из файла XML.
            AVDeviceDB newAVDeviceDB = null;

            var serializer = new XmlSerializer(typeof(AVDeviceDB));
            //var fileInfo = new FileInfo(openFileDialog.FileName);
            FileStream fs = null;

            try
            {
                fs = new FileStream((new FileInfo(path)).FullName, FileMode.Open);
                newAVDeviceDB = (AVDeviceDB)serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка открытия файла: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                if (fs != null) fs.Close();
            }

            var dev = newAVDeviceDB.AVDevices;

            deviceName = newAVDeviceDB.GetAVDeviceNames();

            return newAVDeviceDB;
        }

        /// <summary>
        /// Возвращает объект ИК-сигнала из файла DB сигнала, используя имя deviceName и signalName для поиска.
        /// </summary>
        private IRPacket GetSignal(AVDeviceDB signalDB, string deviceName, string signalName)
        {
            var device = signalDB.GetAVDevice(deviceName);
            if (device == null)
            {
                throw new Exception(string.Format("В базе данных сигналов нет устройства с именем '{0}'.", deviceName));
            }
            var signal = device.GetSignal(signalName);
            if (signal == null)
            {
                throw new Exception(string.Format("Нет сигнала с именем '{0}', найденным для устройства '{1}' в базе данных сигналов.", signalName, deviceName));
            }
            return signal;
        }

    }

    //++++++++++++++++++++++++++++++++++++++++++++++

    public class OutputOfFile : Form1
    {
        ////////////////////////////////////////////////////////////////////////Для NEC кода
        //Путь к OneXMLsignal.xml
        public string pathToFile = "C:\\RedRat3\\OneXMLsignal.xml";

        //Создан вывод кода один раз
        public void OutputLine(string NECCOD)
        {
            new CustomSignal(NECCOD);
            SimpleSignalOutput SSO = new SimpleSignalOutput();
            SSO.SignalOutput(pathToFile);
            SSO.Output();
        }

        //Вывод данных из файла
        public void OutputFileData(string path)
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
                    OutputLine(line);
                }

                //Временно преостановить паток
                Thread.Sleep(200);
            }
            sr.Close();
        }
    }

    //++++++++++++++++++++++++++++++++++++++++++++++

    class Argument
    {
        public Argument(string name, string optionName)
        {
            Name = name;
            OptionName = optionName;
        }

        public string Name { get; private set; }

        public string OptionName { get; private set; }

        public bool RequiresOption
        {
            get
            {
                return !string.IsNullOrEmpty(OptionName);
            }
        }

        public string OptionValue { get; set; }

        public bool Present { get; set; }
    }
    class ArgumentParser
    {
        Dictionary<string, Argument> arguments = new Dictionary<string, Argument>(StringComparer.InvariantCultureIgnoreCase);

        public static string BlinkArg = "-blink";
        public static string CaptureArg = "-capture";
        public static string OutputArg = "-output";
        public static string PrintArg = "-print";
        public static string DeviceArg = "-device";
        public static string SignalArg = "-signal";
        public static string XmlArg = "-xml";

        public ArgumentParser()
        {
            AddArgument(new Argument(BlinkArg, null));
            AddArgument(new Argument(CaptureArg, "filename"));
            AddArgument(new Argument(OutputArg, "filename"));
            AddArgument(new Argument(PrintArg, null));
            AddArgument(new Argument(DeviceArg, "devicename"));
            AddArgument(new Argument(SignalArg, "signalname"));
            AddArgument(new Argument(XmlArg, null));
        }

        void AddArgument(Argument arg)
        {
            arguments.Add(arg.Name, arg);
        }

        public bool ParseArgs(string[] args, out string message)
        {
            var argCount = args.Length;
            if (argCount < 2)
            {
                message = string.Format("Require at least 2 arguments, got {0}", argCount);
                return false;
            }

            RedRatName = args[0];

            for (var pos = 1; pos < argCount; ++pos)
            {
                var argName = args[pos];
                if (argName.StartsWith("-"))
                {
                    Argument arg;
                    if (arguments.TryGetValue(argName, out arg))
                    {
                        if (arg.RequiresOption)
                        {
                            if (pos < argCount - 1)
                            {
                                ++pos;
                                arg.OptionValue = args[pos];
                            }
                            else
                            {
                                message = string.Format("Argument {0} requires {1} option", argName, arg.OptionName);
                                return false;
                            }
                        }
                        arg.Present = true;
                    }
                    else
                    {
                        message = "Unknown argument " + argName;
                        return false;
                    }
                }
                else
                {
                    message = "Unexpected option " + argName;
                    return false;
                }
            }

            message = string.Empty;
            return true;
        }

        public string RedRatName { get; private set; }

        public bool HaveArg(string argName)
        {
            return arguments[argName].Present;
        }

        public bool HavePrintArg
        {
            get
            {
                return HaveArg(PrintArg);
            }
        }

        public bool ShouldBlink
        {
            get
            {
                return HaveArg(BlinkArg);
            }
        }

        public bool ShouldCapture
        {
            get
            {
                return HaveArg(CaptureArg);
            }
        }

        public bool ShouldOutput
        {
            get
            {
                return HaveArg(OutputArg);
            }
        }

        public string GetArgOption(string argName)
        {
            return arguments[argName].OptionValue;
        }
    }
    class XmlSignalLoader
    {
        public static IRPacket LoadSignal(string fileName, string deviceName, string signalName, out string message)
        {
            /*
            * xml-файл может быть одной из четырех вещей:
              * 1. IRPacket (скорее всего, поскольку предыдущие версии только понимали этот формат)
              * 2. AVDeviceDB (требуется устройство и параметры сигнала)
              * 3. AVDevice (требуется опция сигнала)
              * 4. Некоторые другие XML-формат
             */
            var result = GenericXmlSerializer<IRPacket>.FromFile(fileName);
            if (result.Successful)
            {
                message = string.Empty;
                return result.Object;
            }
            else
            {
                var haveDeviceName = !string.IsNullOrEmpty(deviceName);
                var haveSignalName = !string.IsNullOrEmpty(signalName);

                if (result.EncounteredType == "AVDeviceDB")
                {
                    if (haveDeviceName && haveSignalName)
                    {
                        var db = GenericXmlSerializer<AVDeviceDB>.FromFile(fileName).Object;
                        var device = db.AVDevices.FirstOrDefault(d => string.Equals(d.Name, deviceName, StringComparison.InvariantCultureIgnoreCase));
                        if (device != null)
                        {
                            var signal = device.GetSignal(signalName);
                            message = signal == null ? "Couldn't find signal " + signalName : string.Empty;
                            return signal;
                        }
                        else
                        {
                            message = "Couldn't find device " + deviceName;
                        }
                    }
                    else
                    {
                        message = "Вложенный XML-файл содержит набор устройств и сигналов. Вы должны указать параметры -device и -signal для идентификации используемого сигнала";
                    }
                }
                else if (result.EncounteredType == "AVDevice")
                {
                    if (haveSignalName)
                    {
                        var device = GenericXmlSerializer<AVDevice>.FromFile(fileName).Object;
                        var signal = device.GetSignal(signalName);
                        message = signal == null ? "Couldn't find signal " + signalName : string.Empty;
                        return signal;
                    }
                    else
                    {
                        message = "Вложенный файл xml содержит набор сигналов. Вы должны указать параметр -signal для определения используемого сигнала";
                    }
                }
                else
                {
                    message = string.Format("Не удалось загрузить " + fileName + ". Файлы xml должны содержать AVDeviceDB, AVDevice или IRPacket. Найдено " + result.EncounteredType);
                }
            }

            return null;
        }
    }

    /// <summary>
    /// Захват сигнала
    /// </summary>
    public class IRsignalTrainingMode : Form //ИК-СИГНАЛ - РЕЖИМ ОБУЧЕНИЯ
    {

        protected IRedRat3 redRat3;
        IRPacket irPacket = null;
        ArgumentParser argParser = new ArgumentParser();
        //Установите истинный один раз сигнал (или ex)
        protected bool haveSignal = false;
        // == Создаем объект AV-устройства, чтобы мы могли хранить данные сигнала в файле XML.
        private AVDeviceDB avDeviceDB;

        private IRedRat3 FindRedRat3()
        {
            try
            {
                var devices = RedRat3USBImpl.FindDevices();

                if (devices.Count > 0)
                {
                    //Возьмем первое найденное устройство.
                    redRat3 = (IRedRat3)devices[0].GetRedRat();
                    //MessageBox.Show("Подключено устройство: \n\n\"" + redRat3.LocationInformation + "\"", "Проверка подключения", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error in search", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

            return redRat3;
        }

        //Проверяет, xml это или нет
        bool IsXmlFile(string fileName)
        {
            var extension = fileName.Substring(startIndex: fileName.Length - 3);
            return string.Equals(extension, "xml", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Обрабатывает входной сигнал (или ошибку) из RedRat.
        /// </summary>
        public void SignalDataHandler(object sender, EventArgs e)
        {
            if (e is SignalEventArgs)
            {
                var siea = (SignalEventArgs)e;

                switch (siea.Action)
                {
                    //case SignalEventAction.EXCEPTION:
                    //    MessageBox.Show(siea.Exception.Message);
                    //    break;

                    case SignalEventAction.MODULATED_SIGNAL:
                        //MessageBox.Show("Есть ИК-данные...");
                        irPacket = siea.ModulatedSignal;
                        break;

                    //case SignalEventAction.IRDA_PACKET:
                    //    MessageBox.Show("Have IR data IRDA_PACKET...");
                    //    irPacket = siea.IrDaPacket;
                    //    break;


                    default:
                        MessageBox.Show("Неизвестный возврат ...");
                        break;
                }

                haveSignal = true;
            }
            else
            {
                MessageBox.Show("Event of unknown type....");
            }
        }

        //Захват сигнала
        public void CaptureSignal(string fileName)
        {
            if (fileName == "")
            {
                MessageBox.Show("Имя файла отсутствует.", "Ошибка", MessageBoxButtons.OK);
                return;
            }
            else
            {
                FindRedRat3();

                redRat3.LearningSignalIn += SignalDataHandler;
                //Ввод сигнала в RedRat3 осуществляется в течении 10с
                redRat3.GetModulatedSignal(10000);

                haveSignal = false;
                while (!haveSignal)
                {
                    Thread.Sleep(10);
                }

                if (irPacket != null)
                {
                    //Создает новый файл XML для irPacket
                    //XmlTextWriter xmlDoc = new XmlTextWriter("C:\\RedRat3_signalDB\\BD - irPacket.xml", Encoding.UTF8);
                    //xmlDoc.WriteStartDocument();
                    //var fileName = "C:\\RedRat3\\XMLsignal\\XMLsignal.xml";

                    //var fileName = argParser.GetArgOption(ArgumentParser.CaptureArg);
                    if (IsXmlFile(fileName))
                    {
                        //RRUtil.SerializePacketToXML(fileName, irPacket);

                        //Создаем объект AV-устройства, чтобы мы могли хранить данные сигнала в файле XML.
                        var avDeviceDB = new AVDeviceDB();

                        var avDevice = new AVDevice("Sample Device", AVDevice.AVDeviceType.SET_TOP_BOX);
                        avDeviceDB.AddAVDevice(avDevice);

                        irPacket.Name = "New Signal";

                        //Добавить сигналы в «Sample Device».
                        avDevice.AddSignal(irPacket, false);

                        //Храните это как файл XML...
                        var ser = new XmlSerializer(typeof(AVDeviceDB));
                        TextWriter writer = new StreamWriter((new FileInfo(fileName)).FullName);
                        ser.Serialize(writer, avDeviceDB);
                        writer.Close();

                        MessageBox.Show("ИК-данные c ПДУ сохранены в формате XML в файл:\n" + fileName);
                    }
                    else
                    {
                        RRUtil.SerializePacketToBinary(fileName, irPacket);
                        MessageBox.Show("Saved in binary format to file: " + fileName);
                    }

                    //xmlDoc.WriteEndDocument();
                    //xmlDoc.Close();
                }
                else
                {
                    MessageBox.Show("Сигнал пуст.");
                    return;
                }

            }

        }

    }
}