using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Globalization;
using System.IO;
using NLog;
using System.Threading.Tasks;

namespace TVSender
{
    public static class Tests //работа с тестами: составление, изменение, компановка для юзер-интерфейса.
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private static Logger logger = LogManager.GetLogger("port");
        enum WBType
        {
            Chroma,
            Minolta
        }
        //public delegate bool dlg_bool();
        //private delegate void dlg_code();
        //private delegate void dlg_sound(string way);
        public static System.Media.SoundPlayer PlayerSound;

        static bool loadKeyForm(Action[] func, string mes, string[] incArray, 
                    string name = "", bool color = false)
        {
            logger.Trace($"Reader Started! (isStopReader=false)");
            PublicData.isStopReader = false;
            bool retry = false;
            bool final = false;
            var reader = Code.PortReaderOld();
            do
            {
                PublicData.pred_answer = "";
                if (func != null)
                    foreach (var f in func)
                    {
                        if (f != null)
                        {
                            f();
                            Thread.Sleep(400);
                        }
                    }                
                var form = new KeyForm(mes, incArray, name, color);
                var result = form.ShowDialog();
                switch (result)
                {
                    case DialogResult.Retry: retry = true; break;
                    case DialogResult.OK: return true;
                    default: return false;
                }
            }
            while (retry);
            Code.StopPortReader().Wait();
            Ex.Catch(() => reader.Wait());
            return final;
        }
        public static bool loadmenu(Func<bool> func, string mes, string name, string image)
        {
            bool retry = false;
            bool final = false;
            var form = new AskForm(mes, name, image);
            var result = form.ShowDialog();
            switch (result)
            {
                case DialogResult.Retry: PlayerSound.Stop(); retry = true; break;
                case DialogResult.OK: return true;
                default: PlayerSound.Stop(); return false;
            }
            if (retry) return func();
            return final;
        }
        private static bool FormCallWhiteBalance(Func<bool> func, WBType type)
        {
            bool retry = false;
            bool final = false;
            DialogResult result;
            switch (type)
            {
                case WBType.Chroma: 
                    result = new WhiteBalanceChroma().ShowDialog();  
                    break;
                case WBType.Minolta: 
                    result = new WhiteBalanceMinolta().ShowDialog(); 
                    break;
                default:
                    return false;
            }

            switch (result)
            {
                case DialogResult.Retry: PlayerSound.Stop(); retry = true; break;
                case DialogResult.OK: return true;
                default: PlayerSound.Stop(); return false;
            }
            if (retry) return func();
            return final;
        }
        public static bool loadEntryMenu(Func<bool> func, string mes, string name, string image, out string reply)
        {
            reply = null;
            var form = new EnterForm(mes, name, image);
            var result = form.ShowDialog();
            switch (result)
            {
                case DialogResult.OK: reply = form.Result; return true;
                default: PlayerSound.Stop(); return false;
            }
        }
        public static bool Oscillograph(Func<bool> func, string mes, string name)
        {
            bool retry = false;
            bool final = false;

            //if (func != null) func(way);
            //Thread.Sleep(700);
            var form = new AskSoundForm(mes, name);
            var result = form.ShowDialog();
            switch (result)
            {
                case DialogResult.Retry: PlayerSound.Stop(); retry = true; break;
                case DialogResult.OK: return true;
                default: PlayerSound.Stop(); return false;
            }
            if (retry) return func();
            return final;
        }
        public static bool sound(string fileName)
        {
            PlayerSound.Stop();
            string file = PublicData.FindFileInTreeByName(fileName);
            try
            {
                PlayerSound = new System.Media.SoundPlayer(file);
                PlayerSound.Play();
                return true;
            }
            catch
            {
                Ex.Show("Не найден файл " + fileName);               
                return false;
            }
        }
        static bool find(string str, string kusok, string hex_in, out string hex_out)
        {
            if (str.Contains(kusok) )
            {
                hex_out = hex_in.Replace(kusok, "");
                return true;
            }
            hex_out = hex_in;
            return false;
        }
        public static bool Soundgraph() //осциллограф
        {
            string msg = "Наушники: проверьте наличие двух осциллограмм";
            string name = "Наушники";

            return Oscillograph(Soundgraph, msg, name);
        }
        public static bool pns_keys()
        {
            Thread.Sleep(700);
            Action[] func = new Action[2];
            func[0] = Code.ktc_keysOn;
            func[1] = null;
            string msg = "Проверьте кнопки управления телевизора";
            string name = "KeyTest PNS";
            string[] keys = new string[] { "Ch +", "Ch -", "Vol +", "Vol -", "Menu" };
            var result = loadKeyForm(func, msg, keys, name);
            Code.ktc_keysOff();
            Thread.Sleep(500); //1000
            return result;
        }
        public static bool CleanCheckpassLogs()
        {
            string file = "ps" + PublicData.IDDataTV + ".txt";
            string path = Path.Combine(PublicData.LogsCheckPassPath, file);
            PublicData.IDDataTV = null;
            TaskEx.Run(() => Ex.Try( () =>File.Delete(path) ) );
            return true;
        }
        public static bool TestRandomDataTV()
        {
            Random rand = new Random();
            PublicData.IDDataTV = new HexString().FromLower
                (
                rand.Next(10000, 99999).ToString(), 
                "010101"
                );
            return true;
        }
        public static bool ktc_remote()
		{
            Action[] func = null;
			string msg = "Проверьте кнопки пульта";
			string name = "KeyTest KTC Remote";
			string[] keys = new string[] {"OK"};
			var result = loadKeyForm(func, msg, keys, name);
			Thread.Sleep(200);
			return result;
		}
        public static bool horizont_remote()
        {
            Action[] func = null;
            string msg = "Проверьте кнопки пульта";
            string name = "KeyTest";
            string[] keys = new string[] { "Вверх", "OK", "Вниз" };
            var result = loadKeyForm(func, msg, keys, name);
            Thread.Sleep(200);
            return result;
        }
        public static bool KtcWhiteBalanceMinolta()
        {                  
            return FormCallWhiteBalance(KtcWhiteBalanceMinolta, WBType.Minolta);
        }
        public static bool KtcWhiteBalanceChroma()
        {                      
            return FormCallWhiteBalance(KtcWhiteBalanceChroma, WBType.Chroma);
        }
        public static bool brak()
        {
            var form = new FormScaner();
            var result = form.ShowDialog();
            return true;
        }
        #region IR
        public static bool ir_3d()
        {
            
            string file = "3d";
            file +=  ".wav";
            string image = "white";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "3D: Проверить 3D изображение";
            string name = "3D";
            return loadmenu(ir_3d, msg, name, image);
        }
        public static bool ir_3d2()
        {
            
            string file = "3d";
            file +=  ".wav";
            string image = "white";
            Thread.Sleep(2000);
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "3D: Проверить 3D изображение";
            string name = "3D";
            return loadmenu(ir_3d2, msg, name, image);
        }
        public static bool ir_3d3()
        {
            
            string file = "3d";
            file +=  ".wav";
            string image = "white";
            Thread.Sleep(3000);
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "3D: Проверить 3D изображение";
            string name = "3D";
            return loadmenu(ir_3d3, msg, name, image);
        }
        public static bool ir_3d4()
        {
            
            string file = "3d";
            file +=  ".wav";
            string image = "white";
            Thread.Sleep(4000);
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "3D: Проверить 3D изображение";
            string name = "3D";
            return loadmenu(ir_3d4, msg, name, image);
        }
        public static bool ir_scart()
        {
            
            string file = "scart";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "SCART: Наличие изображения и звука";
            string name = "scart";
            return loadmenu(ir_scart, msg, name, image);
        }
        public static bool ir_vga()
        {
            
            string file = "vga";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "VGA: Наличие изображения";
            string name = "vga";
            return loadmenu(ir_vga, msg, name, image);
        }
        public static bool ir_hdmi1b()
        {
            
            string file = "hdmi1";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "HDMI 1: Наличие изображения и звука";
            string name = "hdmi1";
            return loadmenu(ir_hdmi1b, msg, name, image);
        }
        public static bool ir_hdmi2b()
        {
            
            string file = "hdmi2";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "HDMI 2: Наличие изображения и звука";
            string name = "hdmi2";
            return loadmenu(ir_hdmi2b, msg, name, image);
        }
        public static bool ir_hdmi3b()
        {
            
            string file = "hdmi3";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "HDMI 3: Наличие изображения и звука";
            string name = "hdmi3";
            return loadmenu(ir_hdmi3b, msg, name, image);
        }
        public static bool ir_hdmi4b()
        {
            
            string file = "hdmi4";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "HDMI 4: Наличие изображения и звука";
            string name = "hdmi4";
            return loadmenu(ir_hdmi4b, msg, name, image);
        }
        public static bool ir_hdmi1()
        {
            
            string file = "hdmi1";
            file +=  ".wav";            
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "HDMI 1: Наличие изображения и звука";
            string name = "hdmi1";
            return Oscillograph(ir_hdmi1, msg, name);
        }
        public static bool ir_hdmi2()
        {            
            string file = "hdmi2";
            file +=  ".wav";            
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "HDMI 2: Наличие изображения и звука";
            string name = "hdmi2";
            return Oscillograph(ir_hdmi2, msg, name);
        }
        public static bool ir_hdmi3()
        {
            
            string file = "hdmi3";
            file +=  ".wav";            
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "HDMI 3: Наличие изображения и звука";
            string name = "hdmi3";
            return Oscillograph(ir_hdmi3, msg, name);
        }
        public static bool ir_hdmi4()
        {            
            string file = "hdmi4";
            file +=  ".wav";            
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "HDMI 4: Наличие изображения и звука";
            string name = "hdmi4";
            return Oscillograph(ir_hdmi4, msg, name);
        }
        public static bool ir_cmp()
        {
            
            string file = "cmp";
            file +=  ".wav";
            string image = "swip";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "Компоненты: Наличие изображения";
            string name = "cmp";
            return loadmenu(ir_cmp, msg, name, image);
        }
        public static bool ir_cmpn()
        {
            
            string file = "cmp";
            file +=  ".wav";
            if (!sound(file) ) return false;
            Thread.Sleep(500);
            return true;
        }
        public static bool ir_led()
        {
            
            string file = "led";
            file +=  ".wav";
            string image = "white";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "LED: проверьте подсветку";
            string name = "led";
            return loadmenu(ir_cmp, msg, name, image);
        }
        public static bool ir_ledn()
        {
            
            string file = "led";
            file +=  ".wav";
            if (!sound(file) ) return false;
            Thread.Sleep(500);
            return true;
        }
        public static bool ir_scart_sound()
        {
            
            string file = "scart";
            file +=  ".wav";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "SCART: Наличие изображения и звука";
            string name = "scart + Наушники";
            return Oscillograph(ir_scart_sound, msg, name);
        }
        public static bool ir_av()
        {
            
            string file = "av";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "AV: Наличие изображения и звука";
            string name = "AV";
            return loadmenu(ir_av, msg, name, image);
        }
        public static bool ir_dtv()
        {
            
            string file = "dtv";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "DTV: Наличие изображения и звука";
            string name = "dtv";
            return loadmenu(ir_dtv, msg, name, image);
        }
        public static bool ir_atv()
        {
            
            string file = "atv";
            file +=  ".wav";
            string image = "secam";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "ATV: Наличие изображения и звука";
            string name = "atv";
            return loadmenu(ir_atv, msg, name, image);
        }
        public static bool ir_dvbc()
        {
            
            string file = "dvbc";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "DVBC: Наличие изображения и звука";
            string name = "dvbc";
            return loadmenu(ir_dvbc, msg, name, image);
        }
        public static bool ir_6()
        {
            
            string file = "6";
            file +=  ".wav";
            string image = "pal";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "ATV: Наличие изображения и звука";
            string name = "atv";
            return loadmenu(ir_6, msg, name, image);
        }
        public static bool ir_reset()
        {
            
            string file = "reset";
            file +=  ".wav";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            return true;
        }
        public static bool ir_exit()
        {
            
            string file = "exit";
            file +=  ".wav";
            if (!sound(file) ) return false;
            Thread.Sleep(500);
            return true;
        }
        public static bool ir_chtable()
        {
            
            string file = "chtable";
            file +=  ".wav";
            if (!sound(file) ) return false;
            Thread.Sleep(500);
            return true;
        }
        public static bool ir_version()
        {            
            string file = "version";
            file +=  ".wav";
            string image = "white";
            if (!sound(file) ) return false;
            Thread.Sleep(500);
            string msg = "Проверьте версию";
            string name = "version";
            return loadmenu(ir_version, msg, name, image);
        }
        public static bool ir_usb()
        {
            
            string file = "usb";
            file +=  ".wav";
            string image = "hdmi";
            if (!sound(file) ) return false;
            Thread.Sleep(300);
            string msg = "USB: Наличие изображения и звука";
            string name = "usb";
            return loadmenu(ir_usb, msg, name, image);
        }
        public static bool ir_blue()
        {
            
            string file = "blue";
            file +=  ".wav";
            string image = "horizont";
            if (!sound(file) ) return false;
            Thread.Sleep(700);
            string msg = "Blue: качество панели и изображения";
            string name = "blue";
            return loadmenu(ir_blue, msg, name, image);
        }
        public static bool ir_green()
        {
            
            string file = "green";
            file +=  ".wav";
            string image = "horizont";
            if (!sound(file) ) return false;
            Thread.Sleep(700);
            string msg = "Green: качество панели и изображения";
            string name = "green";
            return loadmenu(ir_green, msg, name, image);
        }
        public static bool ir_red()
        {
            
            string file = "red";
            file +=  ".wav";
            string image = "horizont";
            if (!sound(file) ) return false;
            Thread.Sleep(700);
            string msg = "Red: качество панели и изображения";
            string name = "red";
            return loadmenu(ir_red, msg, name, image);
        }
        public static bool ir_black()
        {
            
            string file = "black";
            file +=  ".wav";
            string image = "horizont";
            if (!sound(file) ) return false;
            Thread.Sleep(700);
            string msg = "Black: качество панели и изображения";
            string name = "black";
            return loadmenu(ir_black, msg, name, image);
        }
        public static bool ir_white()
        {
            
            string file = "white";
            file +=  ".wav";
            string image = "horizont";
            if (!sound(file) ) return false;
            Thread.Sleep(700);
            string msg = "White: качество панели и изображения";
            string name = "white";
            return loadmenu(ir_white, msg, name, image);
        }
        #endregion        
        //private static bool RunStream = false;
        public static bool ktc_ci()
        {
            Code.port.DiscardInBuffer();
            Code.port.ReadTimeout = 1000;

            string SuccessTxt = "aaaa73756363657373aa";
            byte[] bytesSuccess = Encoding.Default.GetBytes(SuccessTxt);
            var hex = BitConverter.ToString(bytesSuccess);
            string SuccessHex = hex.Replace("-", " ");

            string bin_answer = "";
            string txt_answer = "";
            Code.ktc_ci();

            Thread.Sleep(300);

            int gotbytes = 0;
            try { gotbytes = Code.port.BytesToRead; }
            catch { };
            if (gotbytes >= bytesSuccess.Length)
            {
                try
                {
                    for (int i = 0; i < gotbytes; i++)
                    {
                        int bit = Code.port.ReadByte();
                        bin_answer += bit.ToString("X2") + " ";
                        txt_answer += ( (char)bit).ToString();
                    }
                }
                catch (TimeoutException)
                {
                    PublicData.write_tv(" НЕТ ОТВЕТА от ТВ");
                    //PublicData.errormsg("Перезагрузите телевизор",
                    //    "Проблема в Телевизоре / LAN-кабеле");
                    return false;
                }
            }
            bin_answer = bin_answer.Trim();
            PublicData.write_tv("answer TV =  " + txt_answer);

            if (txt_answer.Contains(SuccessTxt) || bin_answer.Contains(SuccessTxt) || bin_answer.Contains(SuccessHex) )
            {
                PublicData.write_tv("CI ok");
                Thread.Sleep(300);
                return true;
            }

            string mes = "CI fail";
            PublicData.write_tv(mes);
            string ask = "CI FAIL.\nПовторить тест?";
            string hz = "Предупреждение";
            var otvet = MessageBox.Show(ask, hz, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
            if (otvet == DialogResult.Retry)
            {
                return ktc_ci();
            }
            else
            {              
                return false;
            }
        }
        public static bool ktc_keys()
        {
            //PublicData.stopread = true;
            //Code.port.DiscardInBuffer();
            //RunStream = true;

            Thread.Sleep(700);
            Action[] func = new Action[2];
            func[0] = Code.ktc_keysOn;
            func[1] = null;
            string msg = "Проверьте кнопки управления телевизора";
            string name = "KeyTest KTC";
            string[] keys = new string[] { "Source", "Ch +", "Ch -", "Vol +", "Vol -", "Menu", "Power" };
            var result = loadKeyForm(func, msg, keys, name);
            Code.ktc_keysOff();
            //RunStream = false;
            //PublicData.stopread = false;
            Thread.Sleep(500); //1000
            return result;
        }
        #region blaupunkt
        public static bool bp_KeyTest()
        {
            //dlg_code func = Codes.bp_keytest;
            Thread.Sleep(700);
            Action[] func = new Action[2];            
            func[0] = Code.bp_keytest;
            func[1] = null;
            string msg = "Проверьте кнопки управления телевизора";
            string name = "KeyTest";
            string[] keys = new string[] { "Power", "Source", "Menu", "Ch +", "Ch -", "Vol +", "Vol -" };
            var result = loadKeyForm(func, msg, keys, name, false);
            Code.bp_keytest();
            Thread.Sleep(500); //1000
            return result;
        }        
        public static bool bp_sw()
        {
            Code.port.DiscardInBuffer();
            Code.port.ReadTimeout = 1000;
            IniFile ini = new IniFile();
            var findFile = PublicData.FindFileInTreeBySection("SW");
            if (findFile != null) ini.Load(findFile);
            else
            {
                string fileEnd = new SavingManager().Key(Setting.FileEndOptions).Value;
                PublicData.write_tv(" НЕ НАЙДЕН ФАЙЛ НАСТРОЙКИ SW");
                PublicData.errormsg("Не найден раздел [SW] либо файл " + fileEnd,
                    "с разделом [SW]");
                return false;
            }
            string SWstring = "";
            var file = ini.GetSection("SW").Keys;
            foreach (IniFile.IniSection.IniKey key in file)
            { SWstring = key.Name; break; }
            string SWhex = "";
            //tempsw = Data.ByteString(tempsw);
            //byte[] temp = Data.GetBytesFromHexString(tempsw);
            byte[] ba = Encoding.Default.GetBytes(SWstring);
            var hex = BitConverter.ToString(ba);
            SWhex = hex.Replace("-", " ");



            //byte[] answer = new byte[16];
            string bin_answer = "";
            string txt_answer = "";
            Code.bp_sw();

            Thread.Sleep(300);

            int gotbytes = 0;
            try { gotbytes = Code.port.BytesToRead; }
            catch { };
            if (gotbytes >= ba.Length) //(gotbytes > 15)
            {
                try
                {
                    for (int i = 0; i < gotbytes; i++)
                    {
                        int bit = Code.port.ReadByte();
                        bin_answer += bit.ToString("X2") + " ";
                        //int num = int.Parse(bit.ToString("X2"), NumberStyles.AllowHexSpecifier);
                        txt_answer += ( (char)bit).ToString();
                        //string s = Encoding.ASCII.str;
                    }
                }
                catch (TimeoutException)
                {
                    PublicData.write_tv(" НЕТ ОТВЕТА SW");
                    PublicData.errormsg("Перезагрузите телевизор (вставьте LAN-кабель)",
                        "Проблема в Телевизоре / LAN-кабеле");
                    return false;
                }
            }


            bin_answer = bin_answer.Trim();
            //if (txt_answer.Length > 2) txt_answer = txt_answer.Substring(2, 8);
            PublicData.write_tv("SW TV =  " + txt_answer);

            if (bin_answer.Contains(SWstring) || txt_answer.Contains(SWstring) || bin_answer.Contains(SWhex) )
            {
                PublicData.write_tv("SW TV OK");
                Thread.Sleep(300);
                return true;
            }

            string mes = "SW undefined";            
            PublicData.write_tv(mes);
            string ask = "SW VERSION FAIL.\nПовторить тест?";
            string hz = "Предупреждение";
            var otvet = MessageBox.Show(ask, hz, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
            if (otvet == DialogResult.Retry)
            {
                return bp_sw();
            }
            else
            {                
                logger.Trace($"Reader Started! (isStopReader=false)");
                return false;
            }           
        }
        public static bool COPY_bp_sw()
        {
            Code.port.DiscardInBuffer();
            Code.port.ReadTimeout = 1000;
            IniFile ini = new IniFile();
            var findFile = PublicData.FindFileInTreeBySection("SW");
            if (findFile != null) ini.Load(findFile);
            else
            {
                string fileEnd = new SavingManager().Key(Setting.FileEndOptions).Value;
                PublicData.write_tv(" НЕ НАЙДЕН ФАЙЛ НАСТРОЙКИ SW");
                PublicData.errormsg("Не найден раздел [SW] либо файл " + fileEnd,
                    "с разделом [SW]");
                return false;
            }
            string SWString = "";
            var file = ini.GetSection("SW").Keys;
            foreach (IniFile.IniSection.IniKey key in file)
            { SWString = key.Name; break; }
            string SWhex = "";
            //tempsw = Data.ByteString(tempsw);
            //byte[] temp = Data.GetBytesFromHexString(tempsw);
            byte[] ba = Encoding.Default.GetBytes(SWString);
            var hex = BitConverter.ToString(ba);
            SWhex = hex.Replace("-", " ");



            //byte[] answer = new byte[16];
            string bin_answer = "";
            string txt_answer = "";
            Code.bp_sw();

            Thread.Sleep(300);

            int gotbytes = 0;
            try { gotbytes = Code.port.BytesToRead; }
            catch { };
            if (gotbytes >= ba.Length) //(gotbytes > 15)
            {
                try
                {
                    for (int i = 0; i < gotbytes; i++)
                    {
                        int bit = Code.port.ReadByte();
                        bin_answer += bit.ToString("X2") + " ";
                        //int num = int.Parse(bit.ToString("X2"), NumberStyles.AllowHexSpecifier);
                        txt_answer += ( (char)bit).ToString();
                        //string s = Encoding.ASCII.str;
                    }
                }
                catch (TimeoutException)
                {
                    PublicData.write_tv(" НЕТ ОТВЕТА SW");
                    PublicData.errormsg("Перезагрузите телевизор (вставьте LAN-кабель)",
                        "Проблема в Телевизоре / LAN-кабеле");
                    return false;
                }
            }


            bin_answer = bin_answer.Trim();
            //if (txt_answer.Length > 2) txt_answer = txt_answer.Substring(2, 8);
            PublicData.write_tv("SW TV =  " + txt_answer);

            if (bin_answer.Contains(SWhex) || txt_answer.Contains(SWhex) )
            {
                PublicData.write_tv("SW TV OK");
                Thread.Sleep(300);
                return true;
            }

            string mes = "SW undefined";
            PublicData.write_tv(mes);
            string ask = "SW VERSION FAIL.\nПовторить тест?";
            string hz = "Предупреждение";
            var otvet = MessageBox.Show(ask, hz, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
            if (otvet == DialogResult.Retry)
            {
                return bp_sw();
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region TCL
        public static bool tcl_cmd_on()
        {
            Code.tcl_cmd_on();
            string image = "horizont";
            string msg = "tcl_cmd_on";
            string name = "tcl_cmd_on";
            return loadmenu(tcl_cmd_on, msg, name, image);
        }
        public static bool tcl_cmdserial_off()
        {
            Code.tcl_ps_off();
            string image = "horizont";
            string msg = "tcl_cmdserial_off   ";
            string name = "tcl_cmdserial_off   ";
            return loadmenu(tcl_cmdserial_off, msg, name, image);
        }
        public static bool tcl_KeyTest()
        {
            Action[] func = new Action[2];
            func[0] = Code.tcl_KeyTest_on;
            func[1] = Code.tcl_read_keyTest;
            string msg = "Проверьте кнопки управления телевизора\nи качество панели";
            string name = "KeyTest";
            string[] keys = new string[] { "P+", "P-", "Vol+", "Vol-", "Menu", "OK", "Power" };
            var result = loadKeyForm(func, msg, keys, name, true);
            Code.tcl_KeyTest_off();
            Thread.Sleep(300);
            return result;
        }
        public static bool tcl_starton()
        {         
            Code.port.ReadTimeout = 2000;
            Code.port.DiscardInBuffer();
            string fail1 = "49 6E 70 75 74";
            string fail2 = "AA 5E 46 5E 50";
            string success = "AB 05 0A DF 4E";
            byte[] answer = new byte[5];
            Code.tcl_ps_on();
            Thread.Sleep(200);            
            string str_answer = "";
            try
            {
                Code.port.Read(answer, 0, 5);
            }
            catch (TimeoutException)
            {
                PublicData.write_tv(" НЕТ ОТВЕТА StartON. ");
                PublicData.write_tv(" Перезагрузите ТВ");
                PublicData.errormsg("Перезагрузите телевизор  (замените VGA-кабель)",
                    "Проблема в Телевизоре / VGA-кабеле / программаторе");
                return false;
            }

            foreach (var bit in answer)
            {
                str_answer += bit.ToString("X2") + " ";
            }

            str_answer = str_answer.Trim();
            PublicData.write_tv(str_answer);            

            if (str_answer == fail1 || str_answer == fail2)
            {                
                Code.tcl_cmd_on();
                Thread.Sleep(200);                
                return tcl_starton();
            }
            else if (str_answer != success)
            {
                PublicData.write_tv("StartON FAIL");
                return false;
            }            
            Code.tcl_volume(SoundVolumeAdjust() );
            Thread.Sleep(300);
            return true;
        }
        static string SoundVolumeAdjust()
        {
            string sectionName = "Sound";
            bool NeedToSave = false;
            var fileOptions = PublicData.FindFileInTreeBySection(sectionName);
            IniFile ini = new IniFile();
            ini.Load(fileOptions);
            IniFile.IniSection sectionVolume = ini.GetSection(sectionName);
            var vol_key = sectionVolume.GetAddKey("volume");
                if (vol_key == null)
                { vol_key = sectionVolume.AddKey("volume"); NeedToSave = true; }
            int dec_volume = 20;
                try
                {            
            dec_volume = int.Parse(vol_key.Value);
                }
                catch (Exception)
                {
                    dec_volume = 20;
                    NeedToSave = true;
                }
                if (dec_volume > 100) { dec_volume = 20; NeedToSave = true; }
            vol_key.SetValue(dec_volume.ToString() );
                if (NeedToSave) ini.SaveShowMessage(fileOptions);
            return dec_volume.ToString("X2");
        }
        public static bool tcl_lantest()
        {
            Code.port.DiscardInBuffer();            
            Code.port.ReadTimeout = 1000;
            string fail1 = "AB 05 0A DF 4E AB 06 B1 00 EF 35";
            string success = "AB 05 0A DF 4E AB 06 B1 01 FF 14";
            byte[] answer = new byte[11];
            string str_answer = "";
            Code.tcl_lanTest();

            Thread.Sleep(300);            

            try
            {            
                for (int i = 0; i < 11; i++)
                {
                    int bit = Code.port.ReadByte();
                    str_answer += bit.ToString("X2") + " ";
                }
            }
            catch (TimeoutException)
            {
                PublicData.write_tv(" НЕТ ОТВЕТА LAN");
                PublicData.write_tv(" Перезагрузите ТВ");
                PublicData.errormsg("Перезагрузите телевизор (вставьте LAN-кабель)",
                    "Проблема в Телевизоре / LAN-кабеле");
                return false;
            }

            str_answer = str_answer.Trim();
            PublicData.write_tv(str_answer);

            if (str_answer == success)
            {
                PublicData.write_tv("LAN OK");
                Thread.Sleep(300);
                return true;
            }

            string mes = "LAN undefined answer";
            if (str_answer == fail1) mes = "LAN НЕ ОБНАРУЖЕН";
            PublicData.write_tv(mes);            
            string ask = "LAN TEST FAIL.\nПовторить тест?";
            string hz = "Предупреждение";
            var otvet = MessageBox.Show(ask, hz, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
            if (otvet == DialogResult.Retry)
            {
                return tcl_lantest();
            }
            else
            {
                PublicData.write_tv(mes);
                return false;            
            }
        }
        public static bool tcl_usbtest()
        {
            Code.port.DiscardInBuffer();
            Code.port.ReadTimeout = 1500;
            string fail1 = "AB 05 0A DF 4E AB 06 18 01 58 F2";
            string success = "AB 05 0A DF 4E AB 06 18 00 48 D3";
            byte[] answer = new byte[11];
            string str_answer = "";
            Code.tcl_usbtest();

            Thread.Sleep(300);

            try
            {
                for (int i = 0; i < 11; i++)
                {
                    int bit = Code.port.ReadByte();
                    str_answer += bit.ToString("X2") + " ";
                }
            }
            catch (TimeoutException)
            {
                PublicData.write_tv(" НЕТ ОТВЕТА USB");
                PublicData.write_tv(" Перезагрузите ТВ");
                return false;
            }

            str_answer = str_answer.Trim();
            PublicData.write_tv(str_answer);

            if (str_answer == success)
            {
                PublicData.write_tv("USB OK");
                Thread.Sleep(300);
                return true;
            }

            string mes = "USB undefined answer";
            if (str_answer == fail1) mes = "USB НЕ ОБНАРУЖЕН";

            string ask = "USB TEST FAIL.\nПовторить тест?";
            string hz = "Предупреждение";
            var otvet = MessageBox.Show(ask, hz, MessageBoxButtons.RetryCancel, MessageBoxIcon.Stop);
            if (otvet == DialogResult.Retry)
            {
                return tcl_usbtest();
            }
            else
            {
                PublicData.write_tv(mes);
                return false;
            }
        }
        #endregion
        public static bool otrum_ypbpr()
        {
            Code.otrum_ypbpr();
            string image = "swip";
            string msg = "Компоненты: Проверьте наличие изображения и звука";
            string name = "otrum_components";
            return loadmenu(otrum_ypbpr, msg, name, image);
        }
        public static bool otrum_av()
        {
            Code.otrum_av();
            string image = "swip";
            string msg = "AV: Проверьте наличие изображения и звука";
            string name = "otrum_components";
            return loadmenu(otrum_av, msg, name, image);
        }
        #region generator
        public static bool genr_pc()
        {
            Code.genr_pc();
            return true;
        }
        public static bool genr_hdmi()
        {
            Code.genr_hdmi();
            return true;
        }
        public static bool genr_scart_rgb()
        {
            Code.genr_scart_rgb();
            string image = "rgb";
            string msg = "SCART RGB: Наличие изображение и звука";
            string name = "SCAR RGB";
            return loadmenu(genr_scart_rgb, msg, name, image);
        }
        public static bool genr_scart()
        {
            Code.genr_scart();
            return true;
        }
        public static bool genr_cmp()
        {
            Code.genr_cmp();
            return true;
        }
        public static bool genr_3d()
        {
            Code.genr_3d();
            return true;
        }
        #endregion
    }
}


