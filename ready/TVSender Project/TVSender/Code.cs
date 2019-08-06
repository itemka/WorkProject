using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.IO;
using System.Threading;
using NLog;
using System.Threading.Tasks;
using DisableDevice;
//using RJCP.IO.Ports;

namespace TVSender
{
    public static class Code // работа с com-портом: компановка кодов (данных), отправка данных по com-портам.
    {
        public static SerialPort port;
        public static SerialPort genr;        
     
        public enum restartState
        {
            firstOnce,
            repeat,
            secondTry,
            error,
            reOpenSerial,
            exit
        }
        //public static void WriteRestart(this SerialPort serial, byte[] send)
        //{
        //    RestartFunc( ()=>serial.Write(send, 0, send.Length) );
        //}
        //public static void OpenRestart(this SerialPort serial)
        //{
        //    RestartFunc( ()=>serial.Open() );
        //}
        public static bool OpenPorts()
        {
            if (PublicData.isPort1)
            {
                if (!port.IsOpen)
                {
                    try
                    {
                        port = port.OpenRestart(InitializePort1);
                    }
                    catch (Exception ex)
                    {
                        ex.Log();
                        PublicData.write_info(ex.Message);
                        return false;
                    }
                }
            }
            OpenPort2();
            return true;
        }

        public static bool OpenPort2()
        {
            if (PublicData.isPort2)
            {
                if (!genr.IsOpen)
                {
                    try
                    {
                        genr = genr.OpenRestart(InitializePortGenr);
                    }
                    catch (Exception ex)
                    {
                        ex.Log();
                        PublicData.write_info(ex.Message);
                        return false;
                    }
                }
            }
            return true;
        }

        public static void ClosePorts(bool isNoClose)
        {
            if (!isNoClose)
            {
                ClosePorts();
            }
        }
        public static void ClosePorts()
        {
            try
            {
                logger.Trace("ClosePorts()");
                //logger.Trace($"CanTimeout={Code.port.CanTimeout};");
                //Code.port.DiscardInBuffer();
                //Code.port.DiscardOutBuffer();
                Ex.CatchAsync(port.Close).RunParallel();
                Ex.CatchAsync(genr.Close).RunParallel();
                //stats();
            }
            catch (UnauthorizedAccessException ex)
            {
                ex.Log("\n(Порт " + port.PortName +
                    " не доступен.)\nUnauthorizedAccessException\nClosePort()");
            }
            catch (Exception ex)
            {
                ex.Log("\n(Порт " + port.PortName + " не доступен.)\nClosePort()");
            }
        }
        public static SerialPort WriteRestart(this SerialPort serial, byte[] send, Func<SerialPort> funcInitializer)
        {
            try
            {
                serial.Write(send, 0, send.Length);              
            }
            catch (Exception ex)
            {
                logger.Info($"Ошибка={ex.Message}");
                PublicData.write_info($"Завис порт: {ex.Message}");
                bool isRestart = false;
                try
                {
                    isRestart = ComRestart();
                } catch (Exception ex2)
                {
                    logger.Error(ex2, $"throw {ex2.Message}");
                    throw;
                }
                serial = funcInitializer();
                Thread.Sleep(100);
                serial = serial.OpenRestart(funcInitializer);
                serial.Write(send, 0, send.Length);
            }
            return serial;
        }
        private static bool ComRestart(SerialPort serial)
        {
            try
            {
                serial.Close();
                DeviceManagerApi.ToogleComPortDevice(serial, false);
                Thread.Sleep(100);
                DeviceManagerApi.ToogleComPortDevice(serial, true);
                Thread.Sleep(100);
                return true;
            } catch (Exception ex)
            {
                logger.Error(ex, $"{serial.PortName}: {ex.Message}");
                PublicData.write_info($"Не удалось перезапустить порт {serial.PortName} ({ex.Message}) - придется сделать это вручную.");
                return false;
            }
        }
        private static bool ComRestart()
        {
            return ComRestart(port);
        }
        public static SerialPort OpenRestart(this SerialPort serial, Func<SerialPort> funcInitializer)
        {
            try
            {
                if (!serial.IsOpen)
                {
                    serial.Open();
                    logger.Trace($"Успешно открыт {serial.PortName}");
                }                
            }
            catch (Exception ex)
            {
                logger.Info($"Ошибка={ex.Message}");
                PublicData.write_info($"{serial.PortName} Error: {ex.Message}");
                bool isRestart = false;
                try
                {
                    isRestart = ComRestart();
                } catch (Exception ex2)
                {
                    logger.Error(ex2, $"{ex2.Message}");
                    throw;
                }
                if (isRestart)
                {
                    serial = funcInitializer();
                    Thread.Sleep(100);
                    serial.Open();
                }
            }
            return serial;
        }
        private static void RestartFunc(Action func)
        {
            restartState state = restartState.firstOnce;
            while (state != restartState.exit)
            {
                try
                {
                    func();
                    //Ex.timeout( ()=> port.Write(send, 0, send.Length) );                    
                    state = restartState.exit;
                }
                catch (Exception ex)
                {
                    logger.Error(ex);
                    state = restartState.repeat;
                    try
                    {
                        InitializePort1();
                    }
                    catch (Exception ex2)
                    {
                        logger.Error(ex2);
                        if (state == restartState.secondTry)
                        { state = restartState.error; }
                        else
                        { state = restartState.secondTry; }
                    }
                    if (state == restartState.secondTry)
                    { state = restartState.error; }
                    else
                    { state = restartState.secondTry; }
                    if (state == restartState.error)
                    { throw; }
                }
                logger.Debug($"while end state={state}");
            }
        }

        private static Logger logger = LogManager.GetCurrentClassLogger();
        //private static Logger logger = LogManager.GetLogger("port");
        public static SerialPort InitializePort1()
        {
            logger.Info("Port1Initialize()");
            port?.Close();
            port?.Dispose();

            try
            {                
                port = new SerialPort(PublicData.port1Name);
                port.WriteTimeout = 1500;
                port.ReadTimeout = 1500;
                logger.Debug($"new SerialPort() {port.PortName}");
            }
            catch(TimeoutException ex)
            {
                logger.Error(ex, ex.Message);
            }
            catch (Exception ex)
            {
                ex.Show($"port1: COM-порт \"{PublicData.port1Name}\" не корректен.");              
                port = new SerialPort();
            }
            try
            {
                port.BaudRate = int.Parse(PublicData.port1_rate);
            }
            catch
            {
                port.BaudRate = 9600;
            }
            PublicData.port1_rate = port.BaudRate.ToString();
            return port;
        }
        public static SerialPort InitializePortGenr()
        {
            genr?.Close();
            genr?.Dispose();
            try
            {                
                genr = new SerialPort(PublicData.port2Name);
                genr.WriteTimeout = 1000;
            }
            catch (Exception z)
            {
                z.Show(PublicData.port2Name);
                genr = new SerialPort();
            }
            try
            {
                genr.BaudRate = int.Parse(PublicData.port2_rate);
            }
            catch
            {
                genr.BaudRate = 9600;
            }

            PublicData.port2_rate = genr.BaudRate.ToString();
            return genr;
        }
        public static void write(byte[] send)
        {
            if (PublicData.isPort1)
            {
                port = port.OpenRestart(InitializePort1);                
                port = port.WriteRestart(send, InitializePort1);
                logger.Trace($"Успешно {port.PortName} port.WriteRestart()");
                PublicData.write_pc(HexByteToStrHex(send) + Environment.NewLine);              
            }
        }        
        public static void write_genr(byte[] send)
        {
            if (PublicData.isPort2)
            {
                genr.Write(send, 0, send.Length);
                PublicData.write_genr(HexStrToStrTxt(HexByteToStrHex(send)));
            }
        }

        //public static Task BackgroundPortChecker = new Task(async ()=>
        //{
        //    logger.Info("start");
        //    while (true)
        //    {
        //        await TaskEx.Delay(2000);
        //        try
        //        {
        //            if (!PublicData.isStopReader && PortReaderThread.IsCompleted)
        //            {
        //                if (port.IsOpen)
        //                {
        //                    PortReaderThread = TaskEx.Run(PortReaderMethod);
        //                }
        //            }
        //        }
        //        catch (Exception ex) { ex.Show(); break; }
        //    }
        //});
        public static async Task StopPortReader()
        {
            PublicData.isStopReader = true;
            try
            { await PortReaderOld(); }
            catch { }
        }
        //private static Task PortReaderThread = Ex.TaskEmpty;
        public static async Task PortReaderOld()
        {
            while (!PublicData.isStopReader)
            {
                if (port.IsOpen)
                {
                    int gotbytes = 0;
                    Ex.Try(() =>
                    { gotbytes = Code.port.BytesToRead; });
                    try
                    {
                        if (gotbytes > 0)
                        {
                            await TaskEx.Run(() =>
                            {
                                logger.Trace($"gotbytes (to read) = {gotbytes}");
                                string hex = "";
                                string txt = "";
                                for (int i = 0; i < gotbytes; i++)
                                {
                                    int bit = Code.port.ReadByte();
                                    txt += ((char)bit).ToString();
                                    hex += bit.ToString("X2") + " ";
                                }
                                string hexanswer = hex;
                                string answer = PublicData.pred_answer + hex;
                                logger.Trace($"got answer(txt)={txt}");
                                logger.Trace($"got answer(hex)={hex.trySubstring(30)}");
                            #region TCL                            
                            if (answer.Length >= 15)
                                {
                                    if (find(answer, "AB 05 0A DF 4E", hex, out hex))
                                        PublicData.write_tv("PASS comand" + Environment.NewLine + Environment.NewLine);
                                    if (find(answer, "AB 05 0E 9F CA", hex, out hex))
                                        PublicData.write_tv("NOT Correct comand" + Environment.NewLine + Environment.NewLine);
                                    if (find(answer, "AB 05 0F 8F EB", hex, out hex))
                                        PublicData.write_tv("CRC16 FAILED" + Environment.NewLine + Environment.NewLine);
                                }

                                if (answer.Length >= 18)
                                {
                                    if (find(answer, "AB 06 77 00 53 C7", hex, out hex))
                                        PublicData.set_key("Power"); //Power
                                if (find(answer, "AB 06 77 01 43 E6", hex, out hex))
                                        PublicData.set_key("Menu"); //Menu
                                if (find(answer, "AB 06 77 02 73 85", hex, out hex))
                                        PublicData.set_key("Vol+"); //Vol+
                                if (find(answer, "AB 06 77 03 63 A4", hex, out hex))
                                        PublicData.set_key("Vol-"); //Vol-
                                if (find(answer, "AB 06 77 04 13 43", hex, out hex))
                                        PublicData.set_key("P+"); //P+
                                if (find(answer, "AB 06 77 05 03 62", hex, out hex))
                                        PublicData.set_key("P-"); //P-
                                if (find(answer, "AB 06 77 06 33 01", hex, out hex))
                                        PublicData.set_key("OK"); //OK
                                if (find(answer, "AB 07 C3 01 01 DA", hex, out hex))
                                        PublicData.write_tv("CLONE SUCCESS");
                                    if (find(answer, "AB 07 C3 01 00 CA", hex, out hex))
                                        PublicData.write_tv("CLONE FAIL");
                                }

                                if (answer.Length >= 27)
                                {
                                    if (find(answer, "4E 6F 20 75 73 62 20 64 69", hex, out hex))
                                        PublicData.cancel_keyform();
                                    if (find(answer, "63 75 73 74 6f 6d 20 36 30", hex, out hex))
                                        Code.tcl_tv();
                                }
                            #endregion
                            #region Blaupunkt

                            if (answer.Length >= 18)
                                {
                                    if (find(answer, "EE 05 FD 04 00 FA", hex, out hex))
                                        PublicData.set_key("vol-"); //Vol-
                                if (find(answer, "EE 05 26 01 00 D4", hex, out hex))
                                        PublicData.set_key("vol+"); //Vol+
                                if (find(answer, "EE 05 26 02 00 D3", hex, out hex))
                                        PublicData.set_key("ch-"); //Ch-
                                if (find(answer, "EE 05 26 03 00 D2", hex, out hex))
                                        PublicData.set_key("ch+"); //Ch+
                                if (find(answer, "EE 05 FD 01 00 FD", hex, out hex))
                                        PublicData.set_key("menu"); //Menu
                                if (find(answer, "EE 05 26 04 00 D1", hex, out hex))
                                        PublicData.set_key("source"); //Source
                                if (find(answer, "EE 05 FD 03 00 FB", hex, out hex))
                                        PublicData.set_key("power"); //Power
                            }
                            #endregion
                            #region ktc Buttons
                            if (find(answer, "65 65 30 35 32 36 30 34 30 30 64 31 0D 0A", hex, out hex))
                                    PublicData.set_key("source"); //source
                            if (find(answer, "65 65 30 35 32 36 30 34 30 30 64 32 0D 0A", hex, out hex))
                                    PublicData.set_key("ch+"); //ch+
                            if (find(answer, "65 65 30 35 32 36 30 34 30 30 64 33 0D 0A", hex, out hex))
                                    PublicData.set_key("ch-"); //ch-
                            if (find(answer, "65 65 30 35 66 64 30 31 30 30 66 34 0D 0A", hex, out hex))
                                    PublicData.set_key("vol+"); //vol+
                            if (find(answer, "65 65 30 35 66 64 30 31 30 30 66 61 0D 0A", hex, out hex))
                                    PublicData.set_key("vol-"); //vol-
                            if (find(answer, "65 65 30 35 66 64 30 31 30 30 66 64 0D 0A", hex, out hex))
                                    PublicData.set_key("menu"); //menu
                            if (find(answer, "65 65 30 35 66 64 30 33 30 30 66 62 0D 0A", hex, out hex))
                                    PublicData.set_key("power"); //power                     
                            #endregion
                            #region KTC Remote
                            if (find(answer, "0d 0a 30 78 42 28", hex, out hex))
                                    PublicData.set_key("вверх"); //up
                            if (find(answer, "0d 0a 30 78 44 28", hex, out hex))
                                    PublicData.set_key("OK"); //ok
                            if (find(answer, "0d 0a 30 78 45 28", hex, out hex))
                                    PublicData.set_key("Вниз"); //down

                            if (find(answer, "75 38 4f 70 4e 75 6d 28", hex, out hex))
                                    PublicData.set_key("вверх"); //up
                            #endregion
                            PublicData.pred_answer = hex;

                            //if (hexanswer.Length > 105)
                            //    hexanswer = hexanswer.Substring(0, 105);
                            hexanswer.trySubstring(0, 105);
                                PublicData.write_tv("text = " + txt + Environment.NewLine);
                                PublicData.write_tv("code = " + hexanswer + Environment.NewLine);
                            });
                        }
                    }
                    catch { }
                }
                await TaskEx.Delay(500);
            }
        }
        private static bool find(string str, string kusok, string hex_in, out string hex_out)
        {
            if (str.ToLower().Contains(kusok.ToLower()))
            {
                hex_out = hex_in.Replace(kusok, "");
                return true;
            }
            hex_out = hex_in;
            return false;
        }
        public static string HexByteToStrHex(byte[] send)
        {
            string str = "";
            for (int i = 0; i < send.Length; i++)
                str += send[i].ToString("X2") + " ";
            return str;
        }
        public static string HexStrToStrTxt(string hex)
        {
            hex = hex.Replace(" ", "");
            var bytes = new byte[hex.Length / 2];
            for (int i = 0, j = 0; i < hex.Length; i += 2, j++)
                bytes[j] = Convert.ToByte(hex.Substring(i, 2), 16);
            return Encoding.Default.GetString(bytes);
        }
        public static byte[] HexStrToByteHex(string input)
        {
            SoapHexBinary hexBinary = null;
            try
            {
                hexBinary = SoapHexBinary.Parse(input);
            }
            catch
            {
                string msg = string.Format("\"{0}\" - не является шестнадцатеричным кодом", input);
                var myEx = new Exception(msg);
                throw myEx;
            }

            return hexBinary.Value;
        }
        public static byte[] StrTxtToByteHex(string str)
        {
            return Encoding.Default.GetBytes(str);
        }
        public static string StrTxtToStrHex(string str)
        {
            byte[] bytes = StrTxtToByteHex(str);
            return HexByteToStrHex(bytes);
        }
        static byte[] crc16_byte(byte[] input)
        {
            string str = "";
            foreach (var bit in input) str += bit.ToString();
            str += CRC16_str(str);

            return HexStrToByteHex(str);
        }
        static string CRC16_str(string strInput)
        {
            ushort crc = 0xFFFF;
            byte[] data = GetBytesFromHexString(strInput);
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ( (crc & 0x8000) > 0)
                        crc = (ushort)( (crc << 1) ^ 0x1021);
                    else
                        crc <<= 1;
                }
            }
            return crc.ToString("X4");
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
                    ex.Show( );
                }
            }
            return bytArOutput;
        }
        static string GetStrForGenerator(string inc, string defaultTim, string defaultPat)
        {
            string section = "Generator";
            string filend = new SavingManager().Key(Setting.FileEndOptions).Value;
            var fileOptions = PublicData.FindFileInTreeBySection(section);

            if (fileOptions == null)
            {
                fileOptions = Path.Combine(PublicData.RootPath, "options" + filend);
					 try
					 {
						 File.CreateText(fileOptions).Close();
					 }
					 catch (Exception ex) { ex.Show( ); }
            }

            IniFile ini = new IniFile();
            ini.Load(fileOptions);
            string tim = "";
            string pat = "";
            try
            {
                tim = ini.GetSection(section).GetAddKey(inc + ".tim").Value;
                pat = ini.GetSection(section).GetAddKey(inc + ".pat").Value;
            }
            catch
            {
                var add = ini.AddSection(section);
                add.AddKey(inc + ".tim");
                add.AddKey(inc + ".pat");
                ini.SaveShowMessage(fileOptions);
            }

            if (tim == null || tim == "")
            {
                tim = defaultTim;//81
                ini.SetKeyValue(section, inc + ".tim", defaultTim);
                ini.SaveShowMessage(fileOptions);
            }
            if (pat == null || pat == "")
            {
                pat = defaultPat;//3501            
                ini.SetKeyValue(section, inc + ".pat", defaultPat);
                ini.SaveShowMessage(fileOptions);
            }            

            string str = "load tim " + tim + ";load pat " + pat + ";run;";
            return str;
        }

        #region constant tests
        public static void genr_hdmi()
        {
            string str = GetStrForGenerator("HDMI", "71", "3505");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }
        public static void genr_pc()
        {
            string str = GetStrForGenerator("VGA", "229", "3502");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }
        public static void genr_cmp()
        {

            string str = GetStrForGenerator("cmp", "81", "3501");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }
        public static void genr_scart()
        {
            string str = GetStrForGenerator("SCART", "3603", "3502");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }
        public static void genr_scart_rgb()
        {
            string str = GetStrForGenerator("SCART.RGB", "3601", "3505");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }
        public static void genr_3d()
        {
            string str = GetStrForGenerator("3D", "74", "853");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }       

        public static void db_genst_mod()
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xe2, 0x03, 0x00, 0x16 };
            write(send);
        }

        public static void msg_enable()
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xe2, 0x01, 0x00, 0x18 };
            write(send);
        }

        public static void tcl_cmd_on()
        {
            string str = "AA 05 e2";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_ps_on ()
        {
            string str = "AA 06 10 01";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_usbtotv()
        {
            string str = "AA 06 C1 00";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_ps_off ()
        {
            string str = "AA 06 10 00";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_tv ()
        {
            string str = "AA 06 22 00";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_HDMI1()
        {
            string str = "AA 06 25 01";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }        

        public static void tcl_KeyTest_off ()
        {
            string str = "AA 06 14 00";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_KeyTest_on()
        {
            string str = "AA 06 14 01";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_volume (string vol)
        {
            string str = "AA 06 60 " + vol ;
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_read_keyTest()
        {
            string str = "AA 06 76 00";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_lanTest()
        {
            string str = "AA 06 B0 00";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void tcl_usbtest()
        {
            string str = "AA 06 17 00";
            str += CRC16_str(str);
            byte[] send = HexStrToByteHex(str);
            write(send);
        }


        public static void otrum_ypbpr()
        {
            string str = "05 02 04 03";
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void otrum_av()
        {
            string str = "05 02 08 01";
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void bp_sw()
        {
            string str = "ee05f070009b";                         
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void ktc_sw()
        {
            string str = "ee05f070009b";
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void bp_ci()
        {
            string str = "ee05f055009a";
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void bp_keytest()
        {
            string str = "ee05fd0200fc";            
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void ktc_keysOn()
        {
            string str = "ee05fd0200fa";
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void ktc_keysOff()
        {
            string str = "ee05fd0200fb";
            byte[] send = HexStrToByteHex(str);
            write(send);
        }

        public static void ktc_ci()
        {
            string str = "ee05f04200c1";
            byte[] send = HexStrToByteHex(str);
            write(send);
        }
#endregion
    }
}
