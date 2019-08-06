using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using NLog;
using RedRat.IR;

namespace TVSender
{
    public static class CodeTransform
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
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

        public static ModulatedSignal SignalStringToModSig(string Data_RR)
        {
            logger.Trace("start");
            var sigData = new byte[Data_RR.Length];
            for (int i = 0; i < Data_RR.Length; i++)
            {
                //sigData[i] = (byte)Char.GetNumericValue(Data_RR_char[i]);
                sigData[i] = (byte)Char.GetNumericValue(Data_RR[i]);
            }

            var sig1 = RedratHelper.CreateSignal(sigData);
            string test = string.Join("", sigData);
            logger.Trace("end");
            return sig1;
        }

        //_____Д__40bf2fd0____К____40bf2f_____
        public static string NecToSignal(string NEC)
        {

            string Data_REDRAT = NEC;

            try
            {

                if (Data_REDRAT.Length == 6)
                {
                    Data_REDRAT = NECShort(Data_REDRAT);
                }
                if (Data_REDRAT.Length == 8)
                {
                    Data_REDRAT = NECLong(Data_REDRAT);
                }
                else
                {
                    new ArgumentException("Длина не соответствует NEC-коду.").Throw();
                }

            } catch (Exception ex)
            {
                ex.Show("Введите правильно NEC код.");
            }
            return Data_REDRAT;
        }

        private static string NECShort(string NEC)
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
            //MessageBox.Show(Convert.ToString( el_5 + " " + el_6) );

            //Преобразует символьный элемент в десятичное число
            byte el_5_byte = Convert.ToByte(Convert.ToString(el_5), 16);
            byte el_6_byte = Convert.ToByte(Convert.ToString(el_6), 16);
            //MessageBox.Show("el_5_byte - " + Convert.ToString(el_5_byte) + "_____el_6_byte - " + Convert.ToString(el_6_byte) );

            //Преобразует байтовый элемент десятичный в текстовый двоичный
            string el_5_str = ToBin(Convert.ToInt32(el_5_byte));
            string el_6_str = ToBin(Convert.ToInt32(el_6_byte));

            if (el_5_str.Length == 0)
            { el_5_str = "0000"; }
            if (el_5_str.Length == 1)
            { el_5_str = "000" + el_5_str; }
            if (el_5_str.Length == 2)
            { el_5_str = "00" + el_5_str; }
            if (el_5_str.Length == 3)
            { el_5_str = "0" + el_5_str; }

            if (el_6_str.Length == 0)
            { el_6_str = "0000"; }
            if (el_6_str.Length == 1)
            { el_6_str = "000" + el_6_str; }
            if (el_6_str.Length == 2)
            { el_6_str = "00" + el_6_str; }
            if (el_6_str.Length == 3)
            { el_6_str = "0" + el_6_str; }

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
            return NEC;
        }

        private static string NECLong(string NEC)
        {
            //007f5ea1
            string Data_REDRAT;
            //________________________________________Длинное____________________________________________

            StringBuilder str1 = new StringBuilder();
            for (int i = 0; i < NEC.Length; i += 2)
            {
                str1.Append(NEC[i + 1]);
                str1.Append(NEC[i]);
            }

            //string RevNEC1 = string.Join(null, NEC1);
            string RevNEC1 = str1.ToString();
            //00f7e51a

            //Разбиваю стоку(код) на символы массива b
            char[] b = new char[RevNEC1.Length];
            for (int i = 0; i < RevNEC1.Length; i++)
            {
                b[i] = Convert.ToChar(RevNEC1[i]);
            }
            //48 48 102 55 101 53 49 97

            //Преобразует символьный элемент b в десятичныне числа
            byte[] DEC = new byte[b.Length];
            for (int i = 0; i < b.Length; i++)
            {
                DEC[i] = Convert.ToByte(Convert.ToString(b[i]), 16);
                //MessageBox.Show("DEC - " + Convert.ToString(DEC[i]) );
            }
            //0  0   15  7  14  5  1 19

            //Преобразует байтовый элемент DEC в текстовый BIN
            string[] BIN = new string[DEC.Length];
            for (int i = 0; i < DEC.Length; i++)
            {
                BIN[i] = ToBin(Convert.ToInt32(DEC[i]));

                if (BIN[i].Length == 0)
                { BIN[i] = "0000"; }
                if (BIN[i].Length == 1)
                { BIN[i] = "000" + BIN[i]; }
                if (BIN[i].Length == 2)
                { BIN[i] = "00" + BIN[i]; }
                if (BIN[i].Length == 3)
                { BIN[i] = "0" + BIN[i]; }

                //MessageBox.Show("BIN - " + BIN[i]);
            }
            //0000 0000 1111 0111 1110 0101 0001 1010

            ////Переворачивает строку задом на перед
            string[] REVER = new string[BIN.Length];
            for (int i = 0; i < BIN.Length; i++)
            {
                REVER[i] = ReverseString(BIN[i]);
                //MessageBox.Show("REVER - " + REVER[i]);
            }
            //0000 0000 1111 1110 0111 1010 1000 0101

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

                    if (two_three[j] == "0")
                    { two_three[j] = "22"; }
                    if (two_three[j] == "1")
                    { two_three[j] = "23"; }

                    //MessageBox.Show("two_three " + j+1 + " - " + two_three[j]);

                }

                string Long = string.Join(null, two_three);
                //MessageBox.Show("Long " + i + " - " + Long);

                TwoLong[i] = Long;
            }
            //22222222 22222222 23232323 23232322 22232323 23222322 23222222 22232223
            string InTwoLong = string.Join(null, TwoLong);
            //2222222222222222232323232323232222232323232223222322222222232223

            string DataREDRAT = "01" + InTwoLong + "2";
            //0122222222222222222323232323232322222323232322232223222222222322232

            Data_REDRAT = DataREDRAT;
            return Data_REDRAT;
        }

        public static string SignalToNEC(ModulatedSignal incIRPacket)
        {
            if (incIRPacket == null)
            {
                new ArgumentNullException("Сигнал пуст").Throw();
            }
            return SignalToNEC(string.Join("", incIRPacket.MainSignal));
        }

        public static string SignalToHex(byte[] byteData)
        {
            if (byteData == null)
            {
                new ArgumentNullException("Сигнал пуст").Throw();
            }
            return Code.HexByteToStrHex(byteData).Replace(" ", "");
            //return string.Join("", incIRPacket.MainSignal);
        }

        public static string SignalToNEC(string incSigData)
        {
            //0122222222222222222323232323232322222323232322232223222222222322232
            string result = incSigData;
            try
            {
                result = new Regex("2$").Replace(result, "");
                result = new Regex("^01").Replace(result, "");
                //2222222222222222232323232323232222232323232223222322222222232223
                result = new Regex("22").Replace(result, "0");
                result = new Regex("23").Replace(result, "1");
                result = new Regex("24").Replace(result, "1");
                //00000000111111100111101010000101
                var parts = (from Match m in Regex.Matches(result, @".{4}") select m.Value).ToArray();
                //0000 0000 1111 1110 0111 1010 1000 0101
                string[] reversedParts = new string[parts.Length];
                for (int i = 0; i < parts.Length; i++)
                {
                    reversedParts[i] = ReverseString(parts[i]);
                }
                //0000 0000 1111 0111 1110 0101 0001 1010
                for (int i = 0; i < reversedParts.Length; i++)
                {
                    parts[i] = Convert.ToInt32(reversedParts[i], 2).ToString("x");
                }
                //???
                result = string.Join("", parts);

                StringBuilder str1 = new StringBuilder();
                for (int i = 0; i < result.Length; i += 2)
                {
                    str1.Append(result[i + 1]);
                    str1.Append(result[i]);
                }
                result = str1.ToString();
            } catch (Exception ex)
            {
                new ArgumentException($"Не соответствует NEC-коду.", ex).Throw();
            }
            if (result.Length != 6 && result.Length != 8)
            {
                new ArgumentException($"Длина сигнала '{result}' не соответствует NEC-коду.").Throw();
            }
            return result;
        }
    }
}
