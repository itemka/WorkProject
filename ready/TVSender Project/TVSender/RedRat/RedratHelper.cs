using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using NLog;
using RedRat.IR;
using TVSender.FormsSmall;

namespace TVSender
{
    public static class RedratHelper
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public static ModulatedSignal CreateSignal(byte[] mainSignal, string name = "New Signal")
        {
            if (mainSignal == null)
            {
                throw new Exception("Основные сигналы не могут быть нулевыми");
            }
            ModulatedSignal modSig = GetSignalDefault();
            modSig.Name = name;
            var sigData = new byte[mainSignal.Length + 2];
            var sigDataCount = 0;
            for (var i = 0; i < mainSignal.Length; i++, sigDataCount++)
            {
                sigData[sigDataCount] = mainSignal[i];
            }
            sigData[sigDataCount++] = ModulatedSignal.EOS_MARKER;
            modSig.SigData = sigData;
            return modSig;
        }
        private static ModulatedSignal GetSignalDefault()
        {
            var modSig = new ModulatedSignal { ModulationFreq = 38000 }; //38000
            var lengths = new double[]
            {
                //3.343, 1.675, 0.414, 1.265
                //8.908, 4.506, 0.558, 1.692, 2.269 //Hartens
                8.9, 4.52, 0.55, 1.6, 7.92 //default
            };
            modSig.Lengths = lengths;
            modSig.IntraSigPause = 40; // 39.62
            modSig.NoRepeats = 1;
            return modSig;
        }
        public static ModulatedSignal GetSignalFromSigData(byte[] sigData)
        {            
            if (sigData == null)
            {
                throw new Exception("Основные сигналы не могут быть нулевыми");
            }
            ModulatedSignal modSig = GetSignalDefault();
            modSig.SigData = sigData;
            return modSig;
        }
        public static ModulatedSignal GetSignalFromMain(byte[] mainSignal)
        {
            if (mainSignal == null)
            {
                throw new Exception("Основные сигналы не могут быть нулевыми");
            }
            ModulatedSignal modSig = GetSignalDefault();
            modSig.MainSignal = mainSignal;
            return modSig;
        }
        public static ModulatedSignal GetSignalFromLengthStr(string sigStr)
        {
            if (sigStr == null)
            {
                throw new Exception("Основные сигналы не могут быть нулевыми");
            }
            ModulatedSignal modSig = GetSignalDefault();
            var regexLength = new Regex(@"<[^a-z]+>");
            try
            {
                string lengthSig = regexLength.IsMatch(sigStr) ? regexLength.Match(sigStr).Value : null;
                var lengthStr = lengthSig.Trim(' ', '<', '>').Split(' ', '-');
                double[] lengthDouble = Array.ConvertAll(lengthStr, s => double.Parse(s));
                modSig.Lengths = lengthDouble;

                var sigDataStr = sigStr.Replace(lengthSig, string.Empty).Trim();
                var sigDataArr = new List<byte>(52);
                foreach (var tab in sigDataStr)
                {
                    byte number;
                    if (Byte.TryParse(tab.ToString(), out number))
                    { sigDataArr.Add(number); }
                    else
                    {
                        sigDataArr.Add((byte)tab);
                    }
                }
                modSig.SigData = sigDataArr.ToArray();
            }
            catch (Exception ex)
            {
                new Exception("Неверная строка сигнала", ex).Throw();
            }
            return modSig;
        }
        public static string SignalToString(ModulatedSignal signal)
        {
            if (signal == null)
            {
                new ArgumentNullException("Сигнал пуст").Throw();
            }
            string _return = "<";
            _return += string.Join(" ", signal.Lengths);
            _return += "> ";
            string sigData = string.Empty;
            foreach (byte tab in signal.SigData)
            {
                if (tab >= 0 && tab <= 9)
                { sigData += tab; }
                else
                { sigData += Convert.ToChar(tab); }
            }
            return _return + sigData;
        }
        public static async Task CapturedSignal()
        {
            SignalOutput SSO = new SignalOutput();
            if (SSO.FindRedRat3() != null)
            {
                PublicData.write_info("Начат прием сигнала: ожидание 10с.");
                IRsignalTrainingMode IRSTM = new IRsignalTrainingMode();
                    var waitsignal = Ex.LongRun(() => IRSTM.CaptureSignal());
                    var form = new FormTimer("", "white", 10, "Подайте сигнал с пульта");
                    form.Show();
                    await waitsignal;
                    Ex.Try(() => form.Close());
                var signal = IRSTM.GetSignal();
                if (signal == null)
                {
                    PublicData.write_info("Сигнал не был получен. Прием окончен.");
                    return;
                }

                var ir = SignalToString(signal);
                string nec = null;
                Ex.Try(
                    () => nec = CodeTransform.SignalToNEC(signal)
                    //, ex => PublicData.write_info(ex.Message)
                    );                
                PublicData.write_info("Сигнал принят.");
                nec = (nec?.Length == 6 || nec?.Length == 8) ? nec : null;
                Ex.Try(() => Clipboard.SetText(nec ?? ir));
                new FormTextLine(nec, ir).Show();
            }
            else
            {
                MessageBox.Show("Нет подключенных устройств RedRat3. Подключите RedRat3 и попробуйте снова.", "Проверка подключения", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
    }
}
