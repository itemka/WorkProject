using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TVSender
{
    public static class ChromaCOM
    {
        private readonly static string n = "\n";
        private static MeasureStruct probeMeasure;
        private static string LastPortAnswer;
        //public static SerialPort portWB;
        public static bool isConnected;
        public static bool measureSuccess = false;
        public static MeasureStruct ProbeMeasure
        { get { return probeMeasure; } }

        static ChromaCOM()
        {
            //portWB = new SerialPort("COM1");          
        }
        public static void ConnectCA()
        {
            try
            {
                byte[] send = StrToByte("46 31 0D 0A 4D 30 0D 0A 49 44 4E 3F 0D 0A");
                SendPort(send);                
                //Task.Delay(400);//200
                Thread.Sleep(400);
                //string inc = portWB.ReadExisting();
                var inc = ReadPortIncomes();
                if ( inc.Hex.Contains("43 48 52 4F 4D 41 20".Replace(" ", "").ToLower()) ) //Chroma 37 31 32 33
                { }
                else
                {
                    isConnected = false;
                    LastPortAnswer = inc.Txt + "\n" + inc.Hex;
                    return;
                }
            }
            catch (Exception ex)
            {
                ex.Show();
                isConnected = false;
            }
            if (CheckReadyToWork() && Sets() )
            {
                isConnected = true;
            }
        }

        private static bool CheckReadyToWork()
        {
            bool repeat = true;
            while (repeat)
            {
                repeat = false;
                //TaskEx.Delay(300);
                Thread.Sleep(300);
                Ex.Try( () =>
                {
                    Measure();
                });
                if (!measureSuccess)
                {
                    if (LastPortAnswer == "") //no responce, cuz wait for calibration
                    {
                        MessageBox.Show("Нет ответа. Возможно нужно провести калибровку.\n\nПереключите пробник в режим \"0-CAL\"",
                          "", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (Cal0() )
                        {
                            repeat = true;
                            continue;
                        }
                    }
                    if (LastPortAnswer.Contains("4532320d") ) //E22 Too Dark, 0-Cal enabled
                    {
                        DialogResult answer = MessageBox.Show("Слишком темно. Переключите пробник в режим измерения \"MEAS\"",
                            "Ошибка яркости", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (answer == DialogResult.OK)
                        { repeat = true; }
                    }
                    if (LastPortAnswer.Contains("4f4b30300d")) //ok. 
                    {
                        DialogResult answer = MessageBox.Show("Переключите пробник в режим измерения \"MEAS\"",
                            "Ошибка режима измерения", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (answer == DialogResult.OK)
                        { repeat = true; }
                    }
                }
                else
                { return true; }
            }
            return false;
        }
        public static bool Cal0()
        {
            HexString inc=null;
            bool repeat = true;
            while (repeat)
            {
                repeat = false;
                try
                {
                    byte[] send = StrToByte("5A 52 43 0D 0A"); //ZRC
                    SendPort(send);
                    for (int i = 0; i < 60; i++)
                    {
                        //TaskEx.Delay(500);
                        Thread.Sleep(500);
                        inc = ReadPortIncomes();
                        //inc.bin_answer = "45523231";
                        if (inc?.Hex == "") { continue; }
                        LastPortAnswer = inc.Txt + "\n" + inc.Hex;
                        if (inc.Hex.Contains("4F4B3030".ToLower() )) //OK
                        {
                            MessageBox.Show("Переключите пробник в режим измерения \"MEAS\"", "Успешно",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return true;
                        }
                        if (inc.Hex.Contains("45523231") ) //ER21 = Too Bright
                        {
                            var re = MessageBox.Show("Переключите пробник в режим \"0-CAL\"\n\nСлишком светло!",
                               "Слишком светло!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                            if (re == DialogResult.OK) { repeat = true; }
                            else { repeat = false; }
                        }
                        if (inc.Hex.Contains("4532320d")) //E22 Too Dark, 0-Cal enabled - калибровка не нужна
                        {
                            MessageBox.Show("Переключите пробник в режим измерения \"MEAS\"",
                                "Калибровка не нужна", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return true;
                        }
                        break;
                    }
                }
                catch (Exception ex)
                {
                    LastPortAnswer = inc?.Hex;
                    MessageBox.Show("Ошибка калибровки Cal-0.\n" + ex.Message,
                          "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            MessageBox.Show("Время калибровки вышло (30с): ответ не получен => Нет соединения.",
                  "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }
        public static bool Sets()
        {
            HexString inc=null;
            //TaskEx.Delay(100);
            Thread.Sleep(100);
            try
            {
                byte[] send = StrToByte( //MCH 2..CSD 1..CS
                    "4D 43 48 20 32 0D 0A 43 53 44 20" +
                    " 31 " + //1
                    "0D 0A 43 53 44 3F 0D 0A"
                    );
                SendPort(send);
                send = Fold(Encoding.Default.GetBytes("AVG 3"), StrToByte("0D 0A") );
                SendPort(send);
                //TaskEx.Delay(200);
                Thread.Sleep(200);
                inc = ReadPortIncomes();
            }
            catch (Exception ex)
            {
                measureSuccess = false;
                LastPortAnswer = inc?.Hex;
                //if (!SilentException)
                { MessageBox.Show(ex.Message); }
            }
            //await TaskEx.Delay(100);
            Thread.Sleep(100);
            return true;
        }
        public static void Measure(bool SilentException = false)
        {
            string answer = string.Empty;
            HexString inc = new HexString();
            try
            {
                byte[] send = StrToByte("4D 53 3F 0D"); //MS?
                SendPort(send);
                Thread.Sleep(200);
                inc = ReadPortIncomes();
                answer = inc.Hex;
                if (string.IsNullOrEmpty(inc.Hex) )
                {
                    throw new Exception($"Нет ответа от {Code.genr.PortName}-порта.");
                }
                //string inc = " 50 31 20 34 35 34 30 3B 33 39 34 33 3B 20 31 2E 37 37 0D";
                string xyLvValues = funcReadBetween(inc.Hex, "50 31 20", "0D", true);
                if (xyLvValues == null)
                {
                    throw new Exception(string.Format("Пришел неверный ответ по {0}-порту:\r\n{1}",
                        Code.genr.PortName, answer.ToUpper() ));
                }
                string[] values = HexStrToStringStr(xyLvValues).Replace(".", ",").Split(';');
                probeMeasure.x = float.Parse(values[0]);
                probeMeasure.y = float.Parse(values[1]);
                probeMeasure.Lv = float.Parse(values[2]);
                measureSuccess = true;
            }
            catch(Exception ex)
            {
                measureSuccess = false;
                LastPortAnswer = string.IsNullOrEmpty(answer) ? answer : inc.Txt +n+ answer;
                ex.Throw();
            }
        }
        private static void SendPort(byte[] send)
        {
            if (PublicData.isPort2)
            {
                if (!Code.genr.IsOpen)
                {
                    try { Code.genr.Open(); }
                    catch (Exception ex)
                    {
                        ex.Show("\n(попытка открыть закрытый порт провалилась)");
                    }
                }
                try
                {
                    Code.genr.Write(send, 0, send.Length);
                }
                catch (Exception) { throw; }
            }
        }
        private static byte[] StrToByte(string input)
        {
            SoapHexBinary hexBinary = null;
            try
            {
                hexBinary = SoapHexBinary.Parse(input);
            }
            catch (Exception)
            {
                string msg = string.Format("\"{0}\" - не является шестнадцатеричным кодом", input);
                var myEx = new Exception(msg);
                throw myEx;
            }

            return hexBinary.Value;
        }
        static Byte[] GetBytesFromHexString(string _Input)
        {
            string strInput = _Input.Replace(" ", "");
            Byte[] bytArOutput = new Byte[] { };
            if (!string.IsNullOrEmpty(strInput) && strInput.Length % 2 == 0)
            {
                SoapHexBinary hexBinary = null;
                try
                {
                    hexBinary = SoapHexBinary.Parse(strInput);
                    if (hexBinary != null)
                    {
                        bytArOutput = hexBinary.Value;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            return bytArOutput;
        }
        private static HexString ReadPortIncomes()
        {
            //string bin_answer = "";
            //string txt_answer = "";
            //Thread.Sleep(150);
            int gotbytes = 0;
            try { gotbytes = Code.genr.BytesToRead; }
            catch { };
            var hexstring = new HexString();
            try
            {
                for (int i = 0; i < gotbytes; i++)
                {
                    int bit = Code.genr.ReadByte();
                    hexstring.AddByte(bit);
                    //bin_answer += bit.ToString("X2") + " ";
                    //txt_answer += ( (char)bit).ToString();
                }
            }
            catch (TimeoutException ex)
            {
                Trace.WriteLine(ex.Message);
            }
            //bin_answer = bin_answer.Trim();

            //string returnBin = bin_answer.Replace(" ", "").ToLower();
            //string returnTxt = txt_answer.ToLower();
            return hexstring;
        }
        private static string funcReadBetween(string input, string find1, string find2, bool ReplaceSpaces = false)
        {
            string _return = null;
            input = input.ToLower();
            find1 = find1.ToLower();
            find2 = find2.ToLower();
            if (ReplaceSpaces)
            {
                input = input.Replace(" ", "");
                find1 = find1.Replace(" ", "");
                find2 = find2.Replace(" ", "");
            }
            string[] separators = { find1, find2 };
            string[] matches = input.Split(separators, StringSplitOptions.None);
            if (matches.Length == 3)
            {
                _return = matches[1];
            }
            if (matches.Length > 3)
            {
                _return = matches[1];
            }
            return _return;
        }
        private static string HexStrToStringStr(string hex)
        {
            hex = hex.Replace(" ", "");
            var bytes = new byte[hex.Length / 2];
            for (int i = 0, j = 0; i < hex.Length; i += 2, j++)
                bytes[j] = Convert.ToByte(hex.trySubstring(i, 2), 16);
            return Encoding.Default.GetString(bytes);
        }
        private static byte[] Fold(byte[] a, byte[] b)
        {
            byte[] returnRe = a;
            int SumLengh = a.Length + b.Length;
            Array.Resize(ref returnRe, SumLengh);
            int j = 0;
            for (int i = a.Length; i < SumLengh; i++, j++)
            {
                returnRe[i] = b[j];
            }
            return returnRe;
        }
    }

    public struct MeasureStruct
    {
        public float x;
        public float y;
        public float Lv;
    }
}
