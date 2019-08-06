using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Windows.Forms;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.IO;

namespace TVSender
{
    public class Codes //скелет тестов(класса Tests) - чисто (ну почти) коды команд без ничего лишнего, которые потом используются
        // в классе Tests, НО так было изначально, но щас уже и этот класс немного оброс функциями
    {
        public SerialPort port;
        SerialPort genr;

        public Codes(SerialPort _port, SerialPort _genr)
        {
            port = _port;
            genr = _genr;        
        }

        private void write(byte[] send)
        {
            if (PublicData.port1)
            {
                if (!port.IsOpen) try { port.Open(); }
                    catch (Exception ex) { MessageBox.Show(ex.Message + "\n(открываем закрытый порт)"); }
                try
                {
                    port.Write(send, 0, send.Length);
                    string str = "";
                    for (int i = 0; i < send.Length; i++)
                        str += send[i].ToString("X2") + " ";
                    PublicData.write_pc(str);
                    //File.AppendAllText("log hex.txt", ">>> " + str + Environment.NewLine);                    
                }
                catch (UnauthorizedAccessException) { PublicData.AutoStart = false; }
                catch (InvalidOperationException) { PublicData.AutoStart = false; }
                catch (Exception ex) { throw; }
            }
        }

        private void write_genr(byte[] send)
        {
            if (PublicData.port2)
            {
                try
                {
                    genr.Write(send, 0, send.Length);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "\n>> code.write(genr) <<");
                }
            }
        }

        private byte[] crc16_byte(byte[] input)
        {
            string str = "";
            foreach (var bit in input) str += bit.ToString();
            str += CRC16_str(str);

            return StrToByte(str);
        }

        public string CRC16_str(string strInput)
        {
            ushort crc = 0xFFFF;
            byte[] data = GetBytesFromHexString(strInput);
            for (int i = 0; i < data.Length; i++)
            {
                crc ^= (ushort)(data[i] << 8);
                for (int j = 0; j < 8; j++)
                {
                    if ((crc & 0x8000) > 0)
                        crc = (ushort)((crc << 1) ^ 0x1021);
                    else
                        crc <<= 1;
                }
            }
            return crc.ToString("X4");
        }

        public Byte[] GetBytesFromHexString(string _Input)
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

        private byte[] StrToByte(string input)
        {
            SoapHexBinary hexBinary = null;
            try
            {
                hexBinary = SoapHexBinary.Parse(input);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

            return hexBinary.Value;
        }

        public string GetStr(string inc, string defaultTim, string defaultPat)
        {
            var fileOptions = PublicData.FindFileThroughTreeBySection("Generator");
            IniFile ini = new IniFile();
            ini.Load(fileOptions);
            string tim = "";
            string pat = "";
            try
            {
                tim = ini.GetSection("Generator").GetKey(inc + ".tim").Value;
                pat = ini.GetSection("Generator").GetKey(inc + ".pat").Value;
            }
            catch
            {
                var add = ini.AddSection("Generator");
                add.AddKey(inc + ".tim");
                add.AddKey(inc + ".pat");
                ini.Save(fileOptions);
            }

            if (tim == null || tim == "")
            {
                tim = defaultTim;//81
                ini.SetKeyValue("Generator", inc + ".tim", defaultTim);
                ini.Save(fileOptions);
            }
            if (pat == null || pat == "")
            {
                pat = defaultPat;//3501            
                ini.SetKeyValue("Generator", inc + ".pat", defaultPat);
                ini.Save(fileOptions);
            }            

            string str = "load tim " + tim + ";load pat " + pat + ";run;";
            return str;
        }

        public void genr_hdmi()
        {
            string str = GetStr("HDMI", "71", "3505");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }

        public void genr_pc()
        {
            string str = GetStr("VGA", "229", "3502");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }

        public void genr_cmp()
        {

            string str = GetStr("cmp", "81", "3501");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }

        public void genr_scart()
        {
            string str = GetStr("SCART", "3603", "3502");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }

        public void genr_scart_rgb()
        {
            string str = GetStr("SCART.RGB", "3601", "3505");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }

        public void genr_3d()
        {
            string str = GetStr("3D", "74", "853");
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }

        public void power_on_55()
        {
            byte[] send = new byte[] { 0x70, 0x20, 0x01, 0x20, 0x01, 0x0d };
            write(send);
        }

        public void power_off_55()
        {
            byte[] send = new byte[] { 0x70, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void dtv_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void atv_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x01, 0x0d };
            write(send);
        }

        public void av_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x02, 0x0d };
            write(send);
        }

        public void scart_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x03, 0x0d };
            write(send);
        }

        public void ypbpr_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x04, 0x0d };
            write(send);
        }

        public void pc_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x05, 0x0d };
            write(send);
        }

        public void hdmi1_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x06, 0x0d };
            write(send);
        }

        public void hdmi2_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x07, 0x0d };
            write(send);
        }

        public void hdmi3_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x08, 0x0d };
            write(send);
        }

        public void usb_55()
        {
            byte[] send = new byte[] { 0x69, 0x20, 0x01, 0x20, 0x09, 0x0d };
            write(send);
        }

        public void atv1_55()
        {
            byte[] send = new byte[] { 0x5b, 0x20, 0x01, 0x20, 0x01, 0x0d };
            write(send);
        }

        public void atv5_55()
        {
            byte[] send = new byte[] { 0x5b, 0x20, 0x01, 0x20, 0x05, 0x0d };
            write(send);
        }

        public void atv6_55()
        {
            byte[] send = new byte[] { 0x5b, 0x20, 0x01, 0x20, 0x06, 0x0d };
            write(send);
        }

        public void dtv1_55()
        {
            byte[] send = new byte[] { 0x5d, 0x20, 0x01, 0x20, 0x01, 0x0d };
            write(send);
        }

        public void zoom_55()
        {
            byte[] send = new byte[] { 0x72, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void pp_55()
        {
            byte[] send = new byte[] { 0x79, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void auto_pc_55()
        {
            byte[] send = new byte[] { 0x65, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void mute_55()
        {
            byte[] send = new byte[] { 0x77, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void balance_55()
        {
            byte[] send = new byte[] { 0x78, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void reset_55()
        {
            byte[] send = new byte[] { 0x4e, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void factory_55()
        {
            byte[] send = new byte[] { 0x4d, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void V_plus_55()
        {
            byte[] send = new byte[] { 0x76, 0x20, 0x01, 0x20, 0x01, 0x0d };
            write(send);
        }

        public void v_minus_55()
        {
            byte[] send = new byte[] { 0x76, 0x20, 0x01, 0x20, 0x00, 0x0d };
            write(send);
        }

        public void download_55()
        {
            byte[] send = new byte[] { 0x44, 0x4f, 0x57, 0x4e, 0x4c, 0x4f, 0x41, 0x44, 0x0d };
            write(send);
        }

        public void red()
        {
            byte[] send = new byte[] { 0xee, 0x05, 0x26, 0x03, 0x00, 0xd2 };
            if(PublicData.port1) port.Write(send, 0, send.Length);
        }

        public  void green()
        {
            byte[] send = new byte[] { 0xee, 0x05, 0x26, 0x04, 0x00, 0xd1 };
            write(send);
        }

        public  void blue( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0x26, 0x05, 0x00, 0xd0 };
            write(send);
        }

        public  void black( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0x26, 0x02, 0x00, 0xd3 };
            write(send);
        }

        public  void white( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0x26, 0x01, 0x00, 0xd4 };
            write(send);
        }

        public  void HDMI1(  )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xf0, 0x60, 0x00, 0xab };
            write(send);
            string str = "load tim 74;load pat 3502;run;";
            byte[] msg = Encoding.Default.GetBytes(str);
            write_genr(send);            
        }

        public  void HDMI2(  )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xf0, 0x61, 0x00, 0xaa };
            write(send);
            string str = "load tim 74;load pat 3502;run;";
            send = Encoding.Default.GetBytes(str);
            write_genr(send);            
        }

        public  void LedOn( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xfd, 0x01, 0x00, 0xfd };
            write(send);
        }

        public  void LedOff( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xfd, 0x02, 0x00, 0xfc };
            write(send);
        }

        public  void EXT1(  )
        {
            string str = "load tim 3601;load pat 3502;run;";
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
            send = new byte[] { 0xee, 0x05, 0xf0, 0x40, 0x00, 0xcb };
            write(send);
        }

        public  void EXT11( )
        {
            string str = "load tim 3606;load pat 3504;run;";
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
        }

        public  void PC( )
        {
            string str = "load tim 221;load pat 3502;run;";
            byte[] send = Encoding.Default.GetBytes(str);
            write_genr(send);
            send = new byte[] { 0xee, 0x05, 0xf0, 0x70, 0x00, 0x9b };
            write(send);
        }

        public  void ATV65( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xf0, 0x11, 0x41, 0xb9 };
            write(send);
        }

        public  void ATV61( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xf0, 0x11, 0x3d, 0xbd };
            write(send);
        }

        public  void TVText( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0x21, 0x01, 0x00, 0xd9 };
            write(send);
        }

        public  void TVTextOff( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0x21, 0x02, 0x00, 0xd8 };
            write(send);
        }

        public  void DBVC( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xf0, 0x10, 0x00, 0xfb };
            write(send);
        }

        public  void EUR1( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xfa, 0x00, 0x04, 0xfd };
            write(send);
        }

        public  void EUR2( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xfa, 0x01, 0x04, 0xfc };
            write(send);
        }

        public  void Vol30( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0x22, 0x00, 0x1e, 0xbb };
            write(send);
        }

        public  void SetVol( byte vol)
        {
            byte[] send = new byte[6];
            send[0] = 238;
            send[1] = 5;
            send[2] = 34;
            send[3] = 0;
            send[4] = vol;
            send[5] = vol;
            write(send);
        }

        public  void Vol70( )
        {
            byte[] send = new byte[] { 0xee, 0x05, 0x22, 0x00, 0x46, 0x93 };
            write(send);
        }

        public void db_genst_mod()
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xe2, 0x03, 0x00, 0x16 };
            write(send);
        }

        public void msg_enable()
        {
            byte[] send = new byte[] { 0xee, 0x05, 0xe2, 0x01, 0x00, 0x18 };
            write(send);
        }

        public void tcl_cmd_on()
        {
            string str = "AA 05 e2";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_ps_on ()
        {
            string str = "AA 06 10 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_usbtotv()
        {
            string str = "AA 06 C1 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_ps_off ()
        {
            string str = "AA 06 10 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_Scart ()
        {
            string str = "AA 06 22 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_pc ()
        {
            string str = "AA 06 24 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_tv ()
        {
            string str = "AA 06 22 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_HDMI1()
        {
            string str = "AA 06 25 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_HDMI2()
        {
            string str = "AA 06 25 02";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_HDMI3()
        {
            string str = "AA 06 25 03";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_BalanceSound ()
        {
            string str = "AA 06 61 32";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_ResetAll ()
        {
            string str = "AA 06 19 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_KeyTest_off ()
        {
            string str = "AA 06 14 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_KeyTest_on()
        {
            string str = "AA 06 14 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_volume (string vol)
        {
            string str = "AA 06 60 " + vol ;
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_PowerMode_last()
        {
            string str = "AA 06 38 02";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_PowerMode_standby()
        {
            string str = "AA 06 38 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_PowerMode_alwaysOn()
        {
            string str = "AA 06 38 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_svideo()
        {
            string str = "AA 06 22 03";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_usb()
        {
            string str = "AA 06 26 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_OSD_on()
        {
            string str = "AA 06 11 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_OSD_off()
        {
            string str = "AA 06 11 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_mute_off()
        {
            string str = "AA 06 67 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_mute_on()
        {
            string str = "AA 06 67 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_av2()
        {
            string str = "AA 06 22 02";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_shop_init()
        {
            string str = "AA 06 19 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_CSMdisplay_off()
        {
            string str = "AA 06 11 03";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_CSMdisplay_on()
        {
            string str = "AA 06 11 02";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_bright100()
        {
            string str = "AA 06 36 64";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_DTV1()
        {
            string str = "AA 08 21 00 00 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_DTV2()
        {
            string str = "AA 08 21 00 00 02";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_DTV3()
        {
            string str = "AA 08 21 00 00 03";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_DTV4()
        {
            string str = "AA 08 21 00 00 04";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_DTV804()
        {
            string str = "AA 08 21 00 03 24";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_DTV803()
        {
            string str = "AA 08 21 00 03 23";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_DTV802()
        {
            string str = "AA 08 21 00 03 22";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_DVBC()
        {
            string str = "AA 08 21 01 00 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_ATV5()
        {
            string str = "AA 07 20 00 05";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_ATV6()
        {
            string str = "AA 07 20 00 06";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_ATV9()
        {
            string str = "AA 07 20 00 09";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_ATV10()
        {
            string str = "AA 07 20 00 0A";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_components()
        {
            string str = "AA 06 23 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_components2()
        {
            string str = "AA 06 23 02";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_read_keyTest()
        {
            string str = "AA 06 76 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_lanTest()
        {
            string str = "AA 06 B0 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_usbtest()
        {
            string str = "AA 06 17 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_3doff()
        {
            string str = "AA 06 D0 00";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void tcl_3don()
        {
            string str = "AA 06 D0 01";
            str += CRC16_str(str);
            byte[] send = StrToByte(str);
            write(send);
        }

        public void otrum_ypbpr()
        {
            string str = "05 02 04 03";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void otrum_av()
        {
            string str = "05 02 08 01";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_sw()
        {
            string str = "ee05f070009b";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_chtable()
        {
            string str = "ee05e2010018";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_reset()
        {
            string str = "ee051f0000dc";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_atv()
        {
            string str = "ee05f06000a1";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_dtv()
        {
            string str = "ee05f06000a2";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_dvbc()
        {
            string str = "ee05f06000a3";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_hdmi1()
        {
            string str = "ee05f06000a4";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_hdmi2()
        {
            string str = "ee05f06100a5";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_hdmi3()
        {
            string str = "ee05f06100a6";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_vga()
        {
            string str = "ee05f06100a7";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_scart()
        {
            string str = "ee05f06100aa";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_av()
        {
            string str = "ee05f06100ab";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_cmp()
        {
            string str = "ee05f06100ac";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_adc()
        {
            string str = "ee05f055009b";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_ci()
        {
            string str = "ee05f055009a";
            byte[] send = StrToByte(str);
            write(send);
        }

        public void bp_keytest()
        {
            string str = "ee05fd0200fc";
            byte[] send = StrToByte(str);
            write(send);
        }
    }    
}
