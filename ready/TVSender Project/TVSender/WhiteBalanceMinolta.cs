
using CA200SRVRLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using NLog;
using System.IO.Ports;
using System.Threading.Tasks;
//using RJCP.IO.Ports;

namespace TVSender
{
    public partial class WhiteBalanceMinolta : Form
    {
        #region fields/props
        private static Logger logger = LogManager.GetLogger("whiteBalance");
        private static readonly string WBSetSection = "WBsettings";
        //LogUser log;
        private string logMsg = string.Empty;
        bool isLogMeasure = false;
        byte MaxRGBValue = 160;
        const byte MaxRGBStepIncrease = 12;
        public const string RESERVE_TEMP_DATA = "reserve_tempdata.txt";
        //public const string RESERVE_START_DATA = "reserve_startdata.txt";
        public const string LAST_TEMP_DATA = "last_tempdata.txt";
        public const string TEMP_DATA_FILE = "temp_data.txt";
        public const string START_DATA_FILE = "start_data.txt";
        //public const string NEW_TEMP_DATA = "new_tempdata.txt";
        /// <summary>
        /// количество успешно сделанных балансов
        /// </summary>
        public int SaveDataNumber=0;
        /// <summary>
        /// количество резервных (следующих на перезапись основных) сделанных балансов
        /// </summary>
        public int ReservedSaveNumbers=0;
        private int MaxAverageNumber = 5;
        public string GetTempDataFile(string nameDataFile)
        {
            return GetDataFile(nameDataFile, GetTempDataContentDefault());
        }
        public string StartDataFile
        {
            get
            {
                return GetDataFile(START_DATA_FILE, GetStartDataContentDefault());
            }
        }
        //public string ReserveStartFile
        //{
        //    get
        //    {
        //        return GetDataFile(RESERVE_START_DATA, GetStartDataContentDefault());
        //    }
        //}
        private string GetDataFile(string nameDataFile, string content)
        {
            string file = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            file = Path.Combine(file, PublicData.exeName);
            file = Path.Combine(file, PublicData.UniqOperationPath);
            if (!Directory.Exists(file))
            {
                try
                {
                    Directory.CreateDirectory(file);
                } catch (Exception ex)
                {
                    ex.Show($"ОШИБКА создания {file}");
                    return null;
                }
            }
            file = Path.Combine(file, nameDataFile);
            if (!File.Exists(file))
            {
                try
                {
                    File.WriteAllText(file, content);
                } catch (Exception ex)
                {
                    ex.Show($"ОШИБКА создания {nameDataFile} в {file}");
                    return file;
                }
            }
            return file;
        }
        #endregion
        #region params
        [DllImport("dll_test4.dll")]
        private static extern int AdjustRGB_ColorTemp(float ucCompare_Sx, float ucCompare_Sy, float ucCompare_Lv, float uMeasure_Sx, float uMeasure_Sy, float uMeasure_Lv, float ucTemp_Bri, float ucTemp_ok, int ucStep);
        public float target_Sx = 2800f;
        public float target_Sy = 2900f;
        public float target_Lv = 250f;
        public float ucMeasure_Sx = 2800f;
        public float ucMeasure_Sy = 2900f;
        public float ucMeasure_Lv = 250f;
        public float ucPrevMeasure_Sx = 2800f;
        public float ucPrevMeasure_Sy = 2900f;
        public float ucPrevMeasure_Lv = 250f;
        public int CurrentColorTempMode;
        public byte[,] uColorTemp_Table = new byte[3, 3]
        {
          { (byte) 128, (byte) 128, (byte) 128 },
          { (byte) 128, (byte) 128, (byte) 128 },
          { (byte) 128, (byte) 128, (byte) 128 }
        };
        public byte[,] uColorTempAdj_Data = new byte[3, 3]
        {
          { (byte) 128, (byte) 128, (byte) 128 },
          { (byte) 128, (byte) 128, (byte) 128 },
          { (byte) 128, (byte) 128, (byte) 128 }
        };
        public int uColorTempMode;
        #endregion
        #region bullshit params
        public int[,] uColorTempStd = new int[3, 2];
        public float[,] uColorTempCurr = new float[3, 3];
        public int[] PassedColorTempModes = new int[3];
        public string uPort1 = "COM4";
        public int uBaudrate = 115200;
        public string uReceiveStr = "";
        public int TVReturnStatus = 1;
        public bool EnableReturnCommand = true;
        public byte Adjust_StepLength = 8;
        public byte Adjust_MaxValue = 128;
        public byte Adjust_MaxValue_Backup = 128;
        public DateTime startTime = new DateTime();
        public DateTime stopTime = new DateTime();
        public DateTime beginningTime = new DateTime();
        public DateTime endTime = new DateTime();
        public TimeSpan ts_total = new TimeSpan();
        public TimeSpan ts_diff = new TimeSpan();
        public int LvMinus_Count = 5;
        public float Panel_Lv_max = 250f;
        public float Brightness_Tolerance = 10f;
        public float Temp_Tolerance = 50f;
        public float Cool_TargetBrightness = 210f;
        public float Standard_TargetBrightness = 210f;
        public float Warm_TargetBrightness = 210f;
        public bool UseMaxPanelLv75Percent = true;
        public bool StandardGGainGreaterThan128 = true;
        public int[,] uColorTemp_StartStepSum = new int[3, 4];
        public int[] uColorTemp_StartStepIndex = new int[3];
        private System.Windows.Forms.Timer timerClock = new System.Windows.Forms.Timer();

        public bool uUartReceiveFlag;
        public bool TargetType_Monitor;
        public int Adjust_State;
        public int Adjust_Repeat;
        public int Adjust_Result;
        public int Panel_width;
        public int Panel_height;
        public int Panel_x;
        public int Panel_y;
        public int this_width;
        public int this_height;
        public int this_x;
        public int this_y;
        public WhiteBalanceMinolta.InitValue InitValueType;
        public bool SaveDefaultData;
        #endregion

        public WhiteBalanceMinolta()
        {
            InitializeComponent();
        }
        private bool send_normal_com(byte com, byte value1, byte value2, byte value3)
        {
            if (com == 12)
            { logger.Debug($"set TV RGB: {value1}, {value2}, {value3}"); }
            else if (com == 1)
            {
                string state = (value1 == 0) ? "COOL" : (value1 == 1) ? "NORMAL" : "WARM";
                logger.Debug($"set TV = {state}");
            }
            byte[] buffer = new byte[7]
                {
                0xab, 7, com, value1, value2, value3, 0
                };
            buffer[6] = (byte)
                (
                  byte.MaxValue - (uint)(byte)
                  (
                      byte.MaxValue &
                      buffer[0] + buffer[1] + buffer[2] +
                      buffer[3] + buffer[4] + buffer[5]
                  )
                );
            try
            {
                if (PublicData.isPort1)
                { Code.write(buffer); }
                //Code.port.Write(buffer, 0, (int)buffer[1]);
            }
            catch
            {
                //serialPort1.Close();
                //uRS232Connect = false;
                richTextBox21.ForeColor = Color.Red;
                richTextBox21.Text = "Fail";
                buttonSave.Enabled = true;
                //buttonSave.Text = "Open serial port";
                buttonConnectUSB.Enabled = true;
                //buttonStart.Enabled = false;
                //buttonStart.Text = "Start";
                Adjust_State = 0;
                Ex.Show("Serial port is disconnected, please check USB cable!\n" +
                    "And reopen the WBAA tool.");
                return true;
            }
            return false;
        }
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (TargetType_Monitor)
                Thread.Sleep(100);
            else
                Thread.Sleep(20);
            try
            {
                int bytesToRead = Code.port.BytesToRead;
                byte[] buffer = new byte[bytesToRead];
                int num1 = Code.port.Read(buffer, 0, bytesToRead);
                if (num1 == 12 && (int)buffer[0] == 172 && (int)buffer[1] == 12)
                {
                    if ((int)(byte)((int)byte.MaxValue - ((int)byte.MaxValue & (int)buffer[0] + (int)buffer[1]
                        + (int)buffer[2] + (int)buffer[3] + (int)buffer[4] + (int)buffer[5] + (int)buffer[6]
                        + (int)buffer[7] + (int)buffer[8] + (int)buffer[9] + (int)buffer[10]))
                        == (int)buffer[11]
                        )
                    #region roll
                    {

                        uUartReceiveFlag = true;
                        TVReturnStatus = 0;
                        if (InitValueType != WhiteBalanceMinolta.InitValue.DefaultValue || SaveDefaultData)
                            return;
                        uColorTemp_Table[0, 0] = buffer[2];
                        uColorTemp_Table[0, 1] = buffer[3];
                        uColorTemp_Table[0, 2] = buffer[4];
                        uColorTemp_Table[1, 0] = buffer[5];
                        uColorTemp_Table[1, 1] = buffer[6];
                        uColorTemp_Table[1, 2] = buffer[7];
                        uColorTemp_Table[2, 0] = buffer[8];
                        uColorTemp_Table[2, 1] = buffer[9];
                        uColorTemp_Table[2, 2] = buffer[10];
                        if (!File.Exists(GetTempDataFile(TEMP_DATA_FILE)))
                            return;
                        using (StreamWriter streamWriter = new StreamWriter(GetTempDataFile(TEMP_DATA_FILE), false))
                        {
                            streamWriter.WriteLine(
                                "0," +
                                uColorTemp_Table[0, 0].ToString() + "," +
                                uColorTemp_Table[0, 1].ToString() + "," +
                                uColorTemp_Table[0, 2].ToString()
                                );
                            streamWriter.WriteLine(
                                "1," +
                                uColorTemp_Table[1, 0].ToString() + "," +
                                uColorTemp_Table[1, 1].ToString() + "," +
                                uColorTemp_Table[1, 2].ToString()
                                );
                            streamWriter.WriteLine(
                                "2," +
                                uColorTemp_Table[2, 0].ToString() + "," +
                                uColorTemp_Table[2, 1].ToString() + "," +
                                uColorTemp_Table[2, 2].ToString()
                                );
                            streamWriter.Close();
                        }
                        SaveDefaultData = true;
                        BeginInvoke((MethodInvoker)(() =>
                        {
                            ColortempToDisplay();
                        }));
                    }
                    else
                        BeginInvoke((MethodInvoker)(() =>
                        {
                            //serialPort1.Close();
                            //uRS232Connect = false;
                            richTextBox21.ForeColor = Color.Red;
                            richTextBox21.Text = "Fail";
                            buttonSave.Enabled = true;
                            buttonSave.Text = "Open serial port";
                            buttonConnectUSB.Enabled = true;
                            buttonStart.Enabled = false;
                            buttonStart.Text = "Start";
                            Adjust_State = 0;
                            Ex.Show("Getting TV defalt RGB Gain failed.");
                        }));
                }
                #endregion
                else
                {
                    if (num1 != 4 || (int)buffer[0] != 172 || ((int)buffer[1] != 4 ||
                        (int)(byte)((int)byte.MaxValue - ((int)byte.MaxValue & (int)buffer[0] +
                        (int)buffer[1] + (int)buffer[2])) != (int)buffer[3])
                        )
                        return;
                    uUartReceiveFlag = true;
                    if ((int)buffer[2] == 0)
                        TVReturnStatus = 0;
                    else
                        TVReturnStatus = 1;
                }
            }
            catch
            {
                #region catch
                BeginInvoke((MethodInvoker)(() =>
                {
                    //serialPort1.Close();
                    //uRS232Connect = false;
                    richTextBox21.ForeColor = Color.Red;
                    richTextBox21.Text = "Fail";
                    buttonSave.Enabled = true;
                    buttonSave.Text = "Open serial port";
                    buttonConnectUSB.Enabled = true;
                    buttonStart.Enabled = false;
                    buttonStart.Text = "Start";
                    Adjust_State = 0;
                    Ex.Show("Serial port is disconnected.");
                }));
                #endregion
            }
        }
        public void OnTimerEvent(object myObject, EventArgs myEventArgs)
        {
            switch (Adjust_State)
            {
                #region early cases
                #region roll
                case 0:
                    Adjust_Repeat = 0;

                    Adjust_State = 0;
                    break;
                #endregion
                case 1:
                    startTime = DateTime.Now;
                    #region roll
                    richTextBox3.Text = "Time: ";
                    timerClock.Interval = !EnableReturnCommand ? 160 : 80;
                    timerClock.Start();
                    Adjust_Repeat = 0;
                    if (!send_normal_com((byte)16, (byte)1, (byte)0, (byte)0))
                    {
                        Adjust_State = 2;
                        break;
                    }
                    break;
                #endregion
                case 2:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)11, (byte)0, (byte)0, (byte)0))
                        {
                            Adjust_State = 3;
                            break;
                        }
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)16, (byte)1, (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 3:
                    if (check_cmd_return())
                    #region roll
                    {
                        #region bullshit
                        PassedColorTempModes[0] = 1;
                        PassedColorTempModes[1] = 1;
                        PassedColorTempModes[2] = 1;
                        richTextBoxCoolX.Text = "";
                        richTextBoxMaxLv.Text = "";
                        richTextBoxCoolY.Text = "";
                        richTextBoxCoolLv.Text = "";
                        richTextBoxNormX.Text = "";
                        richTextBoxNormY.Text = "";
                        richTextBoxNormLv.Text = "";
                        richTextBoxWarmX.Text = "";
                        richTextBoxWarmY.Text = "";
                        richTextBoxWarmLv.Text = "";
                        richTextBox21.Text = "";
                        richTextBoxCoolLvPercent.Text = "";
                        richTextBoxNormLvPercent.Text = "";
                        richTextBoxWarmLvPercent.Text = "";
                        richTextBoxG.Text = "";
                        Adjust_Result = 0;
                        Adjust_Repeat = 0;
                        #endregion
                        Adjust_State = 7;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)11, (byte)0, (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 5:
                    if (UseMaxPanelLv75Percent)
                        #region roll
                        LvMinus_Count = InitValueType != WhiteBalanceMinolta.InitValue.DefaultValue ? (uColorTempMode != 15 ? 5 + uColorTemp_StartStepIndex[0] : 7 + uColorTemp_StartStepIndex[0]) : (uColorTempMode != 15 ? 5 : 7);
                    if (!TargetType_Monitor)
                    {
                        if (InitValueType == WhiteBalanceMinolta.InitValue.DefaultValue)
                        {
                            timerClock.Interval = !EnableReturnCommand ? 150 : 65;
                            //Adjust_State = 10;
                            Adjust_State = 11;
                            break;
                        }
                        Adjust_MaxValue = Adjust_MaxValue_Backup;
                        timerClock.Interval = !EnableReturnCommand ? 200 : 75;

                        Adjust_State = 14;
                        //Adjust_State = 7;
                        break;
                    }
                    Adjust_MaxValue = (byte)128;
                    timerClock.Interval = !EnableReturnCommand ? 200 : 75;

                    Adjust_State = 14;
                    //Adjust_State = 7;
                    break;
                #endregion
                case 7:
                    Adjust_Repeat = 0;
                    #region Cool Send ColorTemp 0
                    SetCurrentColorTempMode(0);
                    if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                    {
                        isLogMeasure = true;
                        logMsg = "cool";
                        Adjust_State = 8;
                        break;
                    }
                    break;
                #endregion
                case 8:
                    Adjust_Repeat = 0;
                    #region Norm Send ColorTemp 1
                    Measure_CA();
                    SetCurrentColorTempMode(1);
                    if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                    {
                        isLogMeasure = true;
                        logMsg = "norm";
                        Adjust_State = 9;
                        break;
                    }
                    break;
                #endregion
                case 9:
                    Adjust_Repeat = 0;
                    #region Warm Send ColorTemp 2
                    Measure_CA();
                    SetCurrentColorTempMode(2);
                    if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                    {
                        isLogMeasure = true;
                        logMsg = "warm";
                        Adjust_State = 10;
                        break;
                    }
                    break;
                #endregion
                case 10:
                    Adjust_Repeat = 0;
                    #region Send ColorTemp 1 (normal)
                    Measure_CA();
                    SetCurrentColorTempMode(1);
                    if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                    {
                        Adjust_State = 5;
                        break;
                    }
                    break;
                #endregion
                case 11:
                    if (check_cmd_return())
                    #region Measure Before
                    {
                        Measure_CA();
                        Adjust_Repeat = 0;
                        uColorTempAdj_Data[GetCurrentColorTempMode(), 0] = (byte)128;
                        uColorTempAdj_Data[GetCurrentColorTempMode(), 1] = (byte)128;
                        uColorTempAdj_Data[GetCurrentColorTempMode(), 2] = (byte)128;
                        Thread.Sleep(200);
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                        {
                            Adjust_State = 12;
                            break;
                        }
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 12:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        if ((double)ucMeasure_Lv < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        Adjust_StepLength = (byte)8;
                        uColorTempAdj_Data[GetCurrentColorTempMode(), 0] += Adjust_StepLength;
                        uColorTempAdj_Data[GetCurrentColorTempMode(), 1] += Adjust_StepLength;
                        uColorTempAdj_Data[GetCurrentColorTempMode(), 2] += Adjust_StepLength;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                        {
                            Adjust_State = 13;
                            break;
                        }
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 13:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        if ((double)ucMeasure_Lv < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (Check_RGB_saturation())
                        {
                            uColorTempAdj_Data[GetCurrentColorTempMode(), 0] -= (byte)((uint)Adjust_StepLength * 2U);
                            uColorTempAdj_Data[GetCurrentColorTempMode(), 1] -= (byte)((uint)Adjust_StepLength * 2U);
                            uColorTempAdj_Data[GetCurrentColorTempMode(), 2] -= (byte)((uint)Adjust_StepLength * 2U);
                            Adjust_StepLength = (byte)((uint)Adjust_StepLength / 2U);
                            if ((int)Adjust_StepLength == 1)
                            {
                                Adjust_MaxValue = uColorTempAdj_Data[GetCurrentColorTempMode(), 1];
                                if ((int)Adjust_MaxValue < (int)Adjust_MaxValue_Backup)
                                    Adjust_MaxValue = Adjust_MaxValue_Backup;
                                else if (InitValueType == WhiteBalanceMinolta.InitValue.DefaultValue)
                                {
                                    logger.Debug($"InitValueType==Default: MaxRGB = {Adjust_MaxValue_Backup}->{Adjust_MaxValue}");
                                    Adjust_MaxValue_Backup = Adjust_MaxValue;
                                }
                                timerClock.Interval = !EnableReturnCommand ? 200 : 75;
                                Adjust_State = 14;
                                break;
                            }
                        }
                        else
                        {
                            uColorTempAdj_Data[GetCurrentColorTempMode(), 0] += Adjust_StepLength;
                            uColorTempAdj_Data[GetCurrentColorTempMode(), 1] += Adjust_StepLength;
                            uColorTempAdj_Data[GetCurrentColorTempMode(), 2] += Adjust_StepLength;
                        }
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                        {
                            Adjust_State = 13;
                            break;
                        }
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 14:
                    Adjust_Repeat = 0;
                    #region Enable White
                    if (!send_normal_com((byte)10, (byte)1, (byte)0, (byte)0))
                    {
                        Adjust_State = 15;
                        break;
                    }
                    break;
                #endregion
                case 15:
                    if (check_cmd_return())
                    #region MaxLv
                    {
                        Adjust_Repeat = 0;
                        Thread.Sleep(800);
                        Measure_CA();
                        Panel_Lv_max = ucMeasure_Lv;
                        richTextBoxMaxLv.Text = Panel_Lv_max.ToString().Substring(0, 5);
                        CheckMinusMaxLvMeasure();
                        logger.Debug($"Max Lv={richTextBoxMaxLv.Text}");
                        if ((double)Panel_Lv_max < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (!send_normal_com((byte)10, (byte)0, (byte)0, (byte)0))
                        {
                            Adjust_State = 25;
                            break;
                        }
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)10, (byte)1, (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 25:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        SetCurrentColorTempMode(0);
                        timerClock.Interval = !EnableReturnCommand ? 160 : 65;
                        Adjust_State = 26;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)10, (byte)0, (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 26:
                    Adjust_Repeat = 0;
                    #region roll
                    beginningTime = DateTime.Now;
                    ColorTempAdjStepUpdate(GetCurrentColorTempMode());
                    uColorTempAdj_Data[GetCurrentColorTempMode(), 0] = (int)uColorTemp_Table[GetCurrentColorTempMode(), 0] <= (int)Adjust_MaxValue ? uColorTemp_Table[GetCurrentColorTempMode(), 0] : Adjust_MaxValue;
                    uColorTempAdj_Data[GetCurrentColorTempMode(), 1] = (int)uColorTemp_Table[GetCurrentColorTempMode(), 1] <= (int)Adjust_MaxValue ? uColorTemp_Table[GetCurrentColorTempMode(), 1] : Adjust_MaxValue;
                    uColorTempAdj_Data[GetCurrentColorTempMode(), 2] = (int)uColorTemp_Table[GetCurrentColorTempMode(), 2] <= (int)Adjust_MaxValue ? uColorTemp_Table[GetCurrentColorTempMode(), 2] : Adjust_MaxValue;
                    if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                    {
                        Adjust_State = 27;
                        break;
                    }
                    break;
                #endregion
                case 27:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                        {
                            if (UseMaxPanelLv75Percent)
                            {
                                if (uColorTempMode == 15)
                                {
                                    if (GetCurrentColorTempMode() == 0)
                                        target_Lv = (float)((double)float.Parse(comboBox4.Text) * (double)Panel_Lv_max / 100.0);
                                    else if (GetCurrentColorTempMode() == 1)
                                        target_Lv = (float)((double)float.Parse(comboBox6.Text) * (double)Panel_Lv_max / 100.0);
                                    else if (GetCurrentColorTempMode() == 2)
                                        target_Lv = (float)((double)float.Parse(comboBox7.Text) * (double)Panel_Lv_max / 100.0);
                                }
                                else
                                    target_Lv = Panel_Lv_max * (float)(1.0 - 0.05 * (double)LvMinus_Count);
                            }
                            else if (GetCurrentColorTempMode() == 0)
                                target_Lv = Cool_TargetBrightness;
                            else if (GetCurrentColorTempMode() == 1)
                                target_Lv = Standard_TargetBrightness;
                            else if (GetCurrentColorTempMode() == 2)
                                target_Lv = Warm_TargetBrightness;
                            Brightness_Tolerance = (float)((double)float.Parse(richTextBoxToleranceLv.Text) * (double)Panel_Lv_max / 100.0);
                            Adjust_StepLength = (byte)2;
                            Adjust_State = 30;
                            break;
                        }
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                #endregion
                #region lategame cases
                case 30:
                    if (check_cmd_return())
                    #region Check temp
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        if ((double)ucMeasure_Lv < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (Check_lv_temp_ok(target_Sx, target_Sy, Temp_Tolerance, target_Lv, Brightness_Tolerance))
                        {
                            if ((double)ucMeasure_Sx < (double)ucMeasure_Sy)
                            {
                                PassedColorTempModes[GetCurrentColorTempMode()] = 0;
                                Adjust_State = 98;
                                break;
                            }
                            uColorTempAdj_Data[GetCurrentColorTempMode(), 0] -= (byte)6;
                            ++uColorTempAdj_Data[GetCurrentColorTempMode(), 1];
                            uColorTempAdj_Data[GetCurrentColorTempMode(), 2] -= (byte)2;
                            if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            {
                                Adjust_State = 30;
                                break;
                            }
                            break;
                        }
                        Adjust_State = 31;
                        endTime = DateTime.Now;
                        ts_diff = endTime - beginningTime;
                        //удалить строку ниже
                        //ts_diff = TimeSpan.FromSeconds(5);
                        if (ts_diff.TotalSeconds > 15.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 31:
                    //алгоримт Lv
                    if (check_cmd_return())
                    #region алгоритм Lv
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        if ((double)ucMeasure_Lv < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (!Check_lv_ok(target_Lv, Brightness_Tolerance))
                        {   //заменить ABS(ucMeasure_Lv, uCompare_Lv) переменной
                            Adjust_StepLength = (double)ABS(ucMeasure_Lv, target_Lv) <= 10.0 ? (byte)2 : (byte)((double)ABS(ucMeasure_Lv, target_Lv) / 10.0 * 3.0);
                            if ((double)ucMeasure_Lv > (double)target_Lv)
                                uColorTempAdj_Data[GetCurrentColorTempMode(), 1] -= Adjust_StepLength;
                            else if ((int)uColorTempAdj_Data[GetCurrentColorTempMode(), 1] < (int)Adjust_MaxValue - (int)Adjust_StepLength)
                            {
                                uColorTempAdj_Data[GetCurrentColorTempMode(), 1] += Adjust_StepLength;
                            }
                            else
                            {
                                if ((int)uColorTempAdj_Data[GetCurrentColorTempMode(), 1] >= (int)Adjust_MaxValue)
                                {
                                    Adjust_State = 34;
                                    break;
                                }
                                uColorTempAdj_Data[GetCurrentColorTempMode(), 1] = Adjust_MaxValue;
                            }
                            Adjust_State = 31;
                        }
                        else
                            Adjust_State = 32;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 32:
                    //алгоритм Y
                    if (check_cmd_return())
                    #region алгоритм Y
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        if ((double)ucMeasure_Lv < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (!Check_y_temp_ok(target_Sy, Temp_Tolerance))
                        {
                            Adjust_StepLength = (double)ABS(ucMeasure_Sy, target_Sy) <= 100.0 ? (byte)2 : (byte)((double)ABS(ucMeasure_Sy, target_Sy) / 100.0 * 3.0);
                            if ((double)ucMeasure_Sy > (double)target_Sy)
                            {
                                if ((int)uColorTempAdj_Data[GetCurrentColorTempMode(), 2] <= (int)Adjust_MaxValue - (int)Adjust_StepLength)
                                {
                                    uColorTempAdj_Data[GetCurrentColorTempMode(), 2] += Adjust_StepLength;
                                }
                                else
                                {
                                    if ((int)uColorTempAdj_Data[GetCurrentColorTempMode(), 2] >= (int)Adjust_MaxValue)
                                    {
                                        Adjust_State = 34;
                                        break;
                                    }
                                    uColorTempAdj_Data[GetCurrentColorTempMode(), 2] = Adjust_MaxValue;
                                }
                            }
                            else
                                uColorTempAdj_Data[GetCurrentColorTempMode(), 2] -= Adjust_StepLength;
                            Adjust_State = 32;
                        }
                        else
                            Adjust_State = 33;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 33:
                    //алгоритм X
                    if (check_cmd_return())
                    #region алгоритм X
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        if ((double)ucMeasure_Lv < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (!Check_x_temp_ok(target_Sx, Temp_Tolerance))
                        {
                            Adjust_StepLength = (double)ABS(ucMeasure_Sx, target_Sx) <= 35.0 ? (byte)2 : (byte)((double)ABS(ucMeasure_Sx, target_Sx) / 35.0 * 3.0);
                            if ((double)ucMeasure_Sx > (double)target_Sx)
                                uColorTempAdj_Data[GetCurrentColorTempMode(), 0] -= Adjust_StepLength;
                            else if ((int)uColorTempAdj_Data[GetCurrentColorTempMode(), 0] <= (int)Adjust_MaxValue - (int)Adjust_StepLength)
                            {
                                uColorTempAdj_Data[GetCurrentColorTempMode(), 0] += Adjust_StepLength;
                            }
                            else
                            {
                                if ((int)uColorTempAdj_Data[GetCurrentColorTempMode(), 0] >= (int)Adjust_MaxValue)
                                {
                                    Adjust_State = 34;
                                    break;
                                }
                                uColorTempAdj_Data[GetCurrentColorTempMode(), 0] = Adjust_MaxValue;
                            }
                            Adjust_State = 33;
                        }
                        else
                            Adjust_State = 30;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)12, uColorTempAdj_Data[GetCurrentColorTempMode(), 0], uColorTempAdj_Data[GetCurrentColorTempMode(), 1], uColorTempAdj_Data[GetCurrentColorTempMode(), 2]))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 34:
                    if (UseMaxPanelLv75Percent)
                    #region ErrorsLV
                    {
                        Adjust_Repeat = 0;
                        ++LvMinus_Count;
                        Adjust_State = 26;
                        if (uColorTempMode == 15)
                        {
                            LvMinus_Count = 7;
                            PassedColorTempModes[GetCurrentColorTempMode()] = 1;
                            Adjust_State = 99;
                            break;
                        }
                        if (GetCurrentColorTempMode() == 0)
                        {
                            if (LvMinus_Count > 7)
                            {
                                LvMinus_Count = 5;
                                PassedColorTempModes[GetCurrentColorTempMode()] = 1;
                                Adjust_State = 99;
                                break;
                            }
                            break;
                        }
                        if (GetCurrentColorTempMode() == 1)
                        {
                            if (LvMinus_Count > 6)
                            {
                                LvMinus_Count = 5;
                                PassedColorTempModes[GetCurrentColorTempMode()] = 1;
                                Adjust_State = 99;
                                break;
                            }
                            break;
                        }
                        //if (GetCurrentColorTempMode() == 2 && LvMinus_Count > 8)
                        if (GetCurrentColorTempMode() == 2)
                        {
                            if (LvMinus_Count > 8)
                            {
                                LvMinus_Count = 5;
                                PassedColorTempModes[GetCurrentColorTempMode()] = 1;
                                Adjust_State = 99;
                                break;
                            }
                            break;
                        }
                        break;
                    }
                    PassedColorTempModes[GetCurrentColorTempMode()] = 1;
                    Adjust_State = 99;
                    break;
                #endregion
                case 98:
                    //занесение результата
                    Adjust_Repeat = 0;
                    #region cool
                    if (GetCurrentColorTempMode() == 0)
                    {
                        uColorTempCurr[0, 0] = ucMeasure_Sx / 10000f;
                        richTextBoxCoolX.Text = uColorTempCurr[0, 0].ToString().Substring(0, 5);
                        uColorTempCurr[0, 1] = ucMeasure_Sy / 10000f;
                        richTextBoxCoolY.Text = uColorTempCurr[0, 1].ToString().Substring(0, 5);
                        uColorTempCurr[0, 2] = ucMeasure_Lv;
                        richTextBoxCoolLv.Text = uColorTempCurr[0, 2].ToString().Substring(0, 5);
                        ToLog("after cool", new float[] { ucMeasure_Sx, ucMeasure_Sy, ucMeasure_Lv });
                        logger.Debug($"final cool={ucMeasure_Sx},{ucMeasure_Sy},{ucMeasure_Lv}");
                        richTextBoxCoolLvPercent.ReadOnly = true;
                        richTextBoxCoolLvPercent.BackColor = SystemColors.Control;
                        if (UseMaxPanelLv75Percent)
                        {
                            if (uColorTempMode == 15)
                            {
                                richTextBoxCoolLvPercent.Text = ((int)((double)ucMeasure_Lv / (double)Panel_Lv_max * 100.0)).ToString() + "%";
                                richTextBoxCoolLvPercent.ForeColor = Color.Green;
                            }
                            else
                            {
                                richTextBoxCoolLvPercent.Text = (100 - LvMinus_Count * 5).ToString() + "%";
                                if (LvMinus_Count > 5)
                                    richTextBoxCoolLvPercent.ForeColor = Color.Red;
                                else
                                    richTextBoxCoolLvPercent.ForeColor = Color.Green;
                                if (InitValueType == WhiteBalanceMinolta.InitValue.DefaultValue)
                                {
                                    if (uColorTempMode == 15)
                                        ++uColorTemp_StartStepSum[0, LvMinus_Count - 7];
                                    else
                                        ++uColorTemp_StartStepSum[0, LvMinus_Count - 5];
                                }
                            }
                        }
                        else
                        {
                            richTextBoxCoolLvPercent.Text = ((int)((double)ucMeasure_Lv / (double)Panel_Lv_max * 100.0)).ToString() + "%";
                            if ((int)((double)ucMeasure_Lv / (double)Panel_Lv_max * 100.0) < 75)
                                richTextBoxCoolLvPercent.ForeColor = Color.Red;
                            else
                                richTextBoxCoolLvPercent.ForeColor = Color.Green;
                        }
                    }
                    #endregion
                    #region norm
                    else if (GetCurrentColorTempMode() == 1)
                    {
                        uColorTempCurr[1, 0] = ucMeasure_Sx / 10000f;
                        richTextBoxNormX.Text = uColorTempCurr[1, 0].ToString().Substring(0, 5);
                        uColorTempCurr[1, 1] = ucMeasure_Sy / 10000f;
                        richTextBoxNormY.Text = uColorTempCurr[1, 1].ToString().Substring(0, 5);
                        uColorTempCurr[1, 2] = ucMeasure_Lv;
                        richTextBoxNormLv.Text = uColorTempCurr[1, 2].ToString().Substring(0, 5);
                        ToLog("after norm", new float[] { ucMeasure_Sx, ucMeasure_Sy, ucMeasure_Lv });
                        logger.Debug($"final normal={ucMeasure_Sx},{ucMeasure_Sy},{ucMeasure_Lv}");
                        richTextBoxG.ReadOnly = true;
                        richTextBoxG.Text = uColorTempAdj_Data[1, 1].ToString();
                        richTextBoxG.BackColor = SystemColors.Control;
                        if ((int)uColorTempAdj_Data[1, 1] < 128)
                            richTextBoxG.ForeColor = Color.Red;
                        else
                            richTextBoxG.ForeColor = Color.Green;
                        richTextBoxNormLvPercent.ReadOnly = true;
                        richTextBoxNormLvPercent.BackColor = SystemColors.Control;
                        if (UseMaxPanelLv75Percent)
                        {
                            if (uColorTempMode == 15)
                            {
                                richTextBoxNormLvPercent.Text = ((int)((double)ucMeasure_Lv / (double)Panel_Lv_max * 100.0)).ToString() + "%";
                                richTextBoxNormLvPercent.ForeColor = Color.Green;
                            }
                            else
                            {
                                richTextBoxNormLvPercent.Text = (100 - LvMinus_Count * 5).ToString() + "%";
                                if (LvMinus_Count > 5)
                                    richTextBoxNormLvPercent.ForeColor = Color.Red;
                                else
                                    richTextBoxNormLvPercent.ForeColor = Color.Green;
                                if (InitValueType == WhiteBalanceMinolta.InitValue.DefaultValue)
                                {
                                    if (uColorTempMode == 15)
                                        ++uColorTemp_StartStepSum[1, LvMinus_Count - 7];
                                    else
                                        ++uColorTemp_StartStepSum[1, LvMinus_Count - 3];
                                }
                            }
                        }
                        else
                        {
                            richTextBoxNormLvPercent.Text = ((int)((double)ucMeasure_Lv / (double)Panel_Lv_max * 100.0)).ToString() + "%";
                            if ((int)((double)ucMeasure_Lv / (double)Panel_Lv_max * 100.0) < 75)
                                richTextBoxNormLvPercent.ForeColor = Color.Red;
                            else
                                richTextBoxNormLvPercent.ForeColor = Color.Green;
                        }
                    }
                    #endregion
                    #region warm
                    else if (GetCurrentColorTempMode() == 2)
                    {
                        uColorTempCurr[2, 0] = ucMeasure_Sx / 10000f;
                        richTextBoxWarmX.Text = uColorTempCurr[2, 0].ToString().Substring(0, 5);
                        uColorTempCurr[2, 1] = ucMeasure_Sy / 10000f;
                        richTextBoxWarmY.Text = uColorTempCurr[2, 1].ToString().Substring(0, 5);
                        uColorTempCurr[2, 2] = ucMeasure_Lv;
                        richTextBoxWarmLv.Text = uColorTempCurr[2, 2].ToString().Substring(0, 5);
                        logger.Debug($"final warm={ucMeasure_Sx},{ucMeasure_Sy},{ucMeasure_Lv}");
                        richTextBoxWarmLvPercent.ReadOnly = true;
                        richTextBoxWarmLvPercent.BackColor = SystemColors.Control;
                        if (UseMaxPanelLv75Percent)
                        {
                            if (uColorTempMode == 15)
                            {
                                richTextBoxWarmLvPercent.Text = ((int)((double)ucMeasure_Lv / (double)Panel_Lv_max * 100.0)).ToString() + "%";
                                richTextBoxWarmLvPercent.ForeColor = Color.Green;
                            }
                            else
                            {
                                richTextBoxWarmLvPercent.Text = (100 - LvMinus_Count * 5).ToString() + "%";
                                if (LvMinus_Count > 5)
                                    richTextBoxWarmLvPercent.ForeColor = Color.Red;
                                else
                                    richTextBoxWarmLvPercent.ForeColor = Color.Green;
                                if (InitValueType == WhiteBalanceMinolta.InitValue.DefaultValue)
                                {
                                    if (uColorTempMode == 15)
                                        ++uColorTemp_StartStepSum[2, LvMinus_Count - 7];
                                    else
                                        ++uColorTemp_StartStepSum[2, LvMinus_Count - 5];
                                }
                            }
                        }
                        else
                        {
                            richTextBoxWarmLvPercent.Text = ((int)((double)ucMeasure_Lv / (double)Panel_Lv_max * 100.0)).ToString() + "%";
                            if ((int)((double)ucMeasure_Lv / (double)Panel_Lv_max * 100.0) < 75)
                                richTextBoxWarmLvPercent.ForeColor = Color.Red;
                            else
                                richTextBoxWarmLvPercent.ForeColor = Color.Green;
                        }
                    }
                    #endregion
                    #region hz
                    while (PassedColorTempModes[GetCurrentColorTempMode()] == 0)
                    {
                        ++CurrentColorTempMode;
                        if (GetCurrentColorTempMode() > 2)
                        {
                            Adjust_State = !EnableReturnCommand ? 101 : 99;
                            break;
                        }
                    }
                    if (GetCurrentColorTempMode() <= 2)
                    {
                        if (UseMaxPanelLv75Percent)
                        {
                            if (InitValueType == WhiteBalanceMinolta.InitValue.DefaultValue)
                            {
                                if (uColorTempMode == 15)
                                    LvMinus_Count = 7;
                                else if (GetCurrentColorTempMode() == 1)
                                    LvMinus_Count = 3;
                                else if (GetCurrentColorTempMode() == 2)
                                    LvMinus_Count = 5;
                            }
                            else if (uColorTempMode == 15)
                            {
                                if (GetCurrentColorTempMode() == 1)
                                    LvMinus_Count = 7 + uColorTemp_StartStepIndex[1];
                                else if (GetCurrentColorTempMode() == 2)
                                    LvMinus_Count = 7 + uColorTemp_StartStepIndex[2];
                            }
                            else if (GetCurrentColorTempMode() == 1)
                                LvMinus_Count = 3 + uColorTemp_StartStepIndex[1];
                            else if (GetCurrentColorTempMode() == 2)
                                LvMinus_Count = 5 + uColorTemp_StartStepIndex[2];
                        }
                        Adjust_State = 26;
                        break;
                    }
                    break;
                #endregion
                #region useless cases
                case 99:
                    Adjust_Repeat = 0;
                    #region roll
                    if (PassedColorTempModes[0] == 0 && PassedColorTempModes[1] == 0 && PassedColorTempModes[2] == 0)
                    {
                        if (StandardGGainGreaterThan128 && (int)uColorTempAdj_Data[1, 1] < 128)
                        {
                            Adjust_Result = 2;
                            Adjust_State = 120;
                            break;
                        }
                        if (!send_normal_com((byte)8, (byte)0, (byte)0, (byte)0))
                        {
                            timerClock.Interval = !EnableReturnCommand ? 200 : 100;
                            Adjust_State = 100;
                            break;
                        }
                        break;
                    }
                    Adjust_Result = 1;
                    Adjust_State = 120;
                    break;
                #endregion
                case 100:
                    Adjust_Repeat = 0;
                    #region roll
                    Adjust_State = 110;
                    break;
                #endregion
                case 101:
                    Adjust_Repeat = 0;
                    #region roll
                    if (PassedColorTempModes[0] == 0)
                    {
                        SetCurrentColorTempMode(0);
                        ColorTempAdjStepUpdate(GetCurrentColorTempMode());
                        if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                        {
                            Adjust_State = 102;
                            break;
                        }
                        break;
                    }
                    Adjust_State = 99;
                    break;
                #endregion
                case 102:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        if ((double)ucMeasure_Lv < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (Check_temp_ok(target_Sx, target_Sy, Temp_Tolerance + 50f))
                        {
                            Adjust_State = 103;
                            break;
                        }
                        PassedColorTempModes[0] = 1;
                        richTextBoxCoolLvPercent.Text = "";
                        Adjust_State = 26;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 103:
                    Adjust_Repeat = 0;
                    #region roll
                    if (PassedColorTempModes[2] == 0)
                    {
                        SetCurrentColorTempMode(2);
                        ColorTempAdjStepUpdate(GetCurrentColorTempMode());
                        if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                        {
                            Adjust_State = 104;
                            break;
                        }
                        break;
                    }
                    Adjust_State = 99;
                    break;
                #endregion
                case 104:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        if ((double)ucMeasure_Lv < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (Check_temp_ok(target_Sx, target_Sy, Temp_Tolerance + 50f))
                        {
                            Adjust_State = 105;
                            break;
                        }
                        PassedColorTempModes[2] = 1;
                        richTextBoxWarmLvPercent.Text = "";
                        Adjust_State = 26;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 105:
                    Adjust_Repeat = 0;
                    #region roll
                    if (PassedColorTempModes[1] == 0)
                    {
                        SetCurrentColorTempMode(1);
                        ColorTempAdjStepUpdate(GetCurrentColorTempMode());
                        if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                        {
                            Adjust_State = 106;
                            break;
                        }
                        break;
                    }
                    Adjust_State = 99;
                    break;
                #endregion
                case 106:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        if ((double)ucMeasure_Lv < 100.0)
                        {
                            Adjust_State = 99;
                            break;
                        }
                        if (Check_temp_ok(target_Sx, target_Sy, Temp_Tolerance + 50f))
                        {
                            Adjust_State = 99;
                            break;
                        }
                        PassedColorTempModes[1] = 1;
                        richTextBoxNormLvPercent.Text = "";
                        richTextBoxG.Text = "";
                        Adjust_State = 26;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 110:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        Adjust_State = 111;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)8, (byte)0, (byte)0, (byte)0))
                        {
                            Adjust_State = 100;
                            break;
                        }
                        break;
                    }
                    break;
                #endregion
                case 111:
                    Adjust_Repeat = 0;
                    #region roll
                    if (SaveDataNumber < MaxAverageNumber)
                    {
                        buttonDefault.Enabled = false;
                        buttonDefault.BackColor = Color.Green;
                        buttonAverage.Enabled = false;
                        buttonAverage.BackColor = SystemColors.Control;
                        InitValueType = WhiteBalanceMinolta.InitValue.DefaultValue;
                        ++SaveDataNumber;
                        richTextBox20.Text = SaveDataNumber.ToString();
                        if (!File.Exists(GetTempDataFile(TEMP_DATA_FILE)))
                            return;
                        WriteColorTempAdj(GetTempDataFile(TEMP_DATA_FILE));
                        if (SaveDataNumber >= MaxAverageNumber)
                        {
                            buttonDefault.Enabled = true;
                            buttonDefault.BackColor = SystemColors.Control;
                            buttonAverage.Enabled = false;
                            buttonAverage.BackColor = Color.Green;
                            InitValueType = WhiteBalanceMinolta.InitValue.AverageVaule;
                            if (!File.Exists(GetTempDataFile(TEMP_DATA_FILE)))
                                return;
                            TempDataReadMax();
                            ColortempToDisplay();
                        }
                        if (UseMaxPanelLv75Percent)
                        {
                            if (!File.Exists(StartDataFile))
                                return;
                            WriteStartData(StartDataFile);
                            for (int index1 = 0; index1 < 3; ++index1)
                            {
                                uColorTemp_StartStepIndex[index1] = 0;
                                for (int index2 = 1; index2 < 4; ++index2)
                                {
                                    if (
                                        uColorTemp_StartStepSum[index1, index2]
                                        >
                                        uColorTemp_StartStepSum[index1, uColorTemp_StartStepIndex[index1]])
                                    {
                                        uColorTemp_StartStepIndex[index1] = index2;
                                    }
                                }
                            }
                        }
                    }
                    timerClock.Interval = !EnableReturnCommand ? 160 : 65;
                    Adjust_Result = 3;
                    Adjust_State = 118;
                    break;
                #endregion
                case 118:
                    Adjust_Repeat = 0;
                    #region roll
                    SetCurrentColorTempMode(1);
                    if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                    {
                        Adjust_State = 119;
                        break;
                    }
                    break;
                #endregion
                case 119:
                    if (check_cmd_return())
                    #region roll
                    {
                        Adjust_Repeat = 0;
                        Measure_CA();
                        Adjust_State = 120;
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        if (!send_normal_com((byte)1, (byte)GetCurrentColorTempMode(), (byte)0, (byte)0))
                            break;
                        break;
                    }
                    break;
                #endregion
                case 120:
                    Adjust_Repeat = 0;
                    #region roll
                    if (!send_normal_com((byte)16, (byte)0, (byte)0, (byte)0))
                    {
                        Adjust_State = 121;
                        break;
                    }
                    break;
                #endregion
                #endregion
                #endregion
                case 121:
                    if (check_cmd_return())
                    #region DialogResult
                    {
                        if (Adjust_Result == 3)
                        {
                            WriteColorTempAdj(GetTempDataFile(LAST_TEMP_DATA), string.Empty, false);
                            if (SaveDataNumber >= MaxAverageNumber)
                            {
                                ReservedSaveNumbers++;
                                WriteColorTempAdj(GetTempDataFile(RESERVE_TEMP_DATA), ReservedSaveNumbers.ToString());
                                //WriteStartData(ReserveStartFile);
                            }
                            richTextBox21.ForeColor = Color.Green;
                            richTextBox21.Text = "Pass";
                            DialogResult = DialogResult.OK;
                        }
                        else if (Adjust_Result == 2)
                        {
                            richTextBox21.ForeColor = Color.Yellow;
                            richTextBox21.Text = "Fail";
                            labelError.Visible = true;
                            //DialogResult = DialogResult.Cancel;
                        }
                        else
                        {
                            richTextBox21.ForeColor = Color.Red;
                            richTextBox21.Text = "Fail";
                            labelError.Visible = true;
                            //DialogResult = DialogResult.Cancel;
                        }
                        Adjust_State = 0;
                        Adjust_Repeat = 0;
                        Adjust_Result = 0;
                        stopTime = DateTime.Now;
                        ts_total = stopTime - startTime;
                        buttonStart.Text = "Start";
                        buttonStart.Enabled = true;
                        buttonConnectUSB.Enabled = true;
                        //buttonConnectUSB.Text = "Disconnect CA210";
                        richTextBox3.Text = "Time: " + ((int)ts_total.TotalSeconds).ToString() + "s";
                        break;
                    }
                    ++Adjust_Repeat;
                    if (Adjust_Repeat > 15)
                    {
                        Adjust_Repeat = 0;
                        send_normal_com((byte)16, (byte)0, (byte)0, (byte)0);
                        break;
                    }
                    break;
                    #endregion                    
            }
            RecheckLayout();
        }
        private void Measure_CA()
        {
            Thread.Sleep(200);
            checkMeasure();
            Thread.Sleep(200);
            ucPrevMeasure_Sx = ucMeasure_Sx;
            ucPrevMeasure_Sy = ucMeasure_Sy;
            ucPrevMeasure_Lv = ucMeasure_Lv;
            ucMeasure_Sx = PublicData.m_IProbe.sx * 10000f;
            ucMeasure_Sy = PublicData.m_IProbe.sy * 10000f;
            ucMeasure_Lv = PublicData.m_IProbe.Lv;
            if (isLogMeasure)
            {
                ToLog("before", new float[] { ucMeasure_Sx, ucMeasure_Sy, ucMeasure_Lv });
                logger.Debug($"before={ucMeasure_Sx},{ucMeasure_Sy},{ucMeasure_Lv}");
            }
            isLogMeasure = false;
        }

        public void Init_Timer()
        {
            timerClock.Tick += new EventHandler(OnTimerEvent);
            timerClock.Interval = 75;
            timerClock.Start();
        }
        private void Init_mouse()
        {
        }
        private void Init_key()
        {
        }
        private void Init_Var()
        {
            richTextBox32.Text = Cool_TargetBrightness.ToString();
            richTextBox37.Text = Standard_TargetBrightness.ToString();
            richTextBox38.Text = Warm_TargetBrightness.ToString();
            richTextBoxToleranceLv.Text = "3";
            comboBox4.Text = "60";
            comboBox6.Text = "65";
            comboBox7.Text = "60";
            radioButton1.Checked = true;
            radioButton2.Checked = false;

            radioButton5.Checked = true;
            radioButton6.Checked = false;
            radioButton7.Checked = true;
            radioButton8.Checked = false;
            uColorTempMode = 0;
            comboBoxToleranceXY.Text = "0.005";
            Temp_Tolerance = 50f;
            PassedColorTempModes[0] = 1;
            PassedColorTempModes[1] = 1;
            PassedColorTempModes[2] = 1;
            LvMinus_Count = 5;
            UseMaxPanelLv75Percent = true;
            StandardGGainGreaterThan128 = false;
            EnableReturnCommand = true;
            TargetType_Monitor = false;
            Top = 0;
            Left = 0;
            radioButtonGreen128No.Checked = false;
            radioButtonGreen128Yes.Checked = true;
            comboBoxXYPreset.Text = "User_defined_color_temp";
            LoadPresets();
        }

        private string GetTempDataContentDefault()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine("0,128,128,128");
            str.AppendLine("1,128,128,128");
            str.AppendLine("2,128,128,128");
            return str.ToString();
        }
        private string GetStartDataContentDefault()
        {
            StringBuilder str = new StringBuilder();
            str.AppendLine(" 0, 0, 0, 0, 0");
            str.AppendLine(" 1, 0, 0, 0, 0");
            str.AppendLine(" 2, 0, 0, 0, 0");
            str.AppendLine(" 3, 128");
            return str.ToString();
        }
        private void Init_LoadFiles()
        {
            try
            {
                try { ReadSaveDataNumber(); }
                catch
                {
                    Ex.Try(() => File.Delete(GetTempDataFile(TEMP_DATA_FILE)));                    
                    ReadSaveDataNumber();
                }
                try { ReadReservedDataNumber(); }
                catch
                {
                    Ex.Try(() => File.Delete(GetTempDataFile(RESERVE_TEMP_DATA)));
                    ReadReservedDataNumber();
                }
            }
            catch
            {
                ClearData();
                return;
            }

            if (SaveDataNumber < MaxAverageNumber)
            {
                buttonDefault.Enabled = false;
                buttonDefault.BackColor = Color.Green;
                buttonAverage.Enabled = false;
                buttonAverage.BackColor = SystemColors.Control;
                InitValueType = WhiteBalanceMinolta.InitValue.DefaultValue;
                if (SaveDefaultData)
                {
                    if (!File.Exists(GetTempDataFile(TEMP_DATA_FILE)))
                        return;
                    TempDataReadMinimal();
                }
                else
                {
                    ColorTempToDefault();
                }
            }
            else //SaveDataNumber > Max
            {
                if (ReservedSaveNumbers >= SaveDataNumber)
                {
                    SwapReserveTempStartDataFiles();                    
                }
                buttonDefault.Enabled = true;
                buttonDefault.BackColor = SystemColors.Control;
                buttonAverage.Enabled = false;
                buttonAverage.BackColor = Color.Green;
                InitValueType = WhiteBalanceMinolta.InitValue.AverageVaule;
                if (!File.Exists(GetTempDataFile(TEMP_DATA_FILE)))
                    return;
                TempDataReadMax();
            }
            ColortempToDisplay();
            if (!File.Exists(StartDataFile))
                return;
            ReadStartData();
            if (SaveDataNumber >= MaxAverageNumber)
            {
                for (int index1 = 0; index1 < 3; ++index1)
                {
                    uColorTemp_StartStepIndex[index1] = 0;
                    for (int index2 = 1; index2 < 4; ++index2)
                    {
                        if (uColorTemp_StartStepSum[index1, index2] > uColorTemp_StartStepSum[index1, uColorTemp_StartStepIndex[index1]])
                            uColorTemp_StartStepIndex[index1] = index2;
                    }
                }
                richTextBoxCoolLvPercent.Text = (100 - 5 * (5 + uColorTemp_StartStepIndex[0])).ToString() + "%";
                richTextBoxNormLvPercent.Text = (100 - 5 * (3 + uColorTemp_StartStepIndex[1])).ToString() + "%";
                richTextBoxWarmLvPercent.Text = (100 - 5 * (5 + uColorTemp_StartStepIndex[2])).ToString() + "%";
            }
            else
            {
                richTextBoxCoolLvPercent.Text = "75%";
                richTextBoxNormLvPercent.Text = "85%";
                richTextBoxWarmLvPercent.Text = "75%";
            }
        }

        private void ReadReservedDataNumber()
        {
            if (!File.Exists(GetTempDataFile(RESERVE_TEMP_DATA)))
            { throw new FileNotFoundException(); }
            using (StreamReader streamReader = File.OpenText(GetTempDataFile(RESERVE_TEMP_DATA)))
            {
                string str;
                while ((str = streamReader.ReadLine()) != null)
                {
                    string[] strArray = str.Split(',');
                    ReservedSaveNumbers = int.Parse(strArray[0]) / 10;
                    logger.Debug($"ReserveTempData: |{str}| ReservedSaves={ReservedSaveNumbers}|");
                }                
                streamReader.Close();
            }
        }
        private void ReadSaveDataNumber()
        {
            if (!File.Exists(GetTempDataFile(TEMP_DATA_FILE)))
            { throw new FileNotFoundException(); }
            using (StreamReader streamReader = File.OpenText(GetTempDataFile(TEMP_DATA_FILE)))
            {
                string str;
                while ((str = streamReader.ReadLine()) != null)
                {
                    string[] strArray = str.Split(',');
                    SaveDefaultData = int.Parse(strArray[0]) > 2;
                    SaveDataNumber = int.Parse(strArray[0]) / 10;
                    logger.Debug($"TempData: |{str}| SaveDataNumber={SaveDataNumber}|");
                }
                richTextBox20.Text = SaveDataNumber.ToString();
                streamReader.Close();
            }            
        }
        private void TempDataReadMinimal()
        {
            try
            {
                using (StreamReader streamReader = File.OpenText(GetTempDataFile(LAST_TEMP_DATA)))
                {
                    string str;
                    while ((str = streamReader.ReadLine()) != null)
                    {
                        string[] strArray = str.Split(',');
                        if (int.Parse(strArray[0]) == 0)
                        {
                            uColorTemp_Table[0, 0] = byte.Parse(strArray[1]);
                            uColorTemp_Table[0, 1] = byte.Parse(strArray[2]);
                            uColorTemp_Table[0, 2] = byte.Parse(strArray[3]);
                        }
                        else if (int.Parse(strArray[0]) == 1)
                        {
                            uColorTemp_Table[1, 0] = byte.Parse(strArray[1]);
                            uColorTemp_Table[1, 1] = byte.Parse(strArray[2]);
                            uColorTemp_Table[1, 2] = byte.Parse(strArray[3]);
                        }
                        else if (int.Parse(strArray[0]) == 2)
                        {
                            uColorTemp_Table[2, 0] = byte.Parse(strArray[1]);
                            uColorTemp_Table[2, 1] = byte.Parse(strArray[2]);
                            uColorTemp_Table[2, 2] = byte.Parse(strArray[3]);
                            break;
                        }
                    }
                    streamReader.Close();
                }
            }catch
            { ClearData(); }
        }
        private void ColortempToDisplay()
        {
            richTextBox30.Text = uColorTemp_Table[0, 0].ToString();
            richTextBox27.Text = uColorTemp_Table[0, 1].ToString();
            richTextBox24.Text = uColorTemp_Table[0, 2].ToString();
            richTextBox29.Text = uColorTemp_Table[1, 0].ToString();
            richTextBox26.Text = uColorTemp_Table[1, 1].ToString();
            richTextBox23.Text = uColorTemp_Table[1, 2].ToString();
            richTextBox28.Text = uColorTemp_Table[2, 0].ToString();
            richTextBox25.Text = uColorTemp_Table[2, 1].ToString();
            richTextBox22.Text = uColorTemp_Table[2, 2].ToString();
            richTextBoxG.Text = uColorTemp_Table[1, 1].ToString();
        }
        private void TempDataReadMax()
        {
            try
            {
                using (StreamReader streamReader = File.OpenText(GetTempDataFile(TEMP_DATA_FILE)))
                {
                    int[] numArray1 = new int[3];
                    int[] numArray2 = new int[3];
                    int[] numArray3 = new int[3];
                    string str;
                    while ((str = streamReader.ReadLine()) != null)
                    {
                        string[] strArray = str.Split(',');
                        if (int.Parse(strArray[0]) >= 10)
                        {
                            numArray1[int.Parse(strArray[0]) % 10] += int.Parse(strArray[1]);
                            numArray2[int.Parse(strArray[0]) % 10] += int.Parse(strArray[2]);
                            numArray3[int.Parse(strArray[0]) % 10] += int.Parse(strArray[3]);
                            SaveDataNumber = int.Parse(strArray[0]) / 10;
                        }
                    }
                    try
                    {
                        uColorTemp_Table[0, 0] = (byte)(numArray1[0] / SaveDataNumber);
                        uColorTemp_Table[0, 1] = (byte)(numArray2[0] / SaveDataNumber);
                        uColorTemp_Table[0, 2] = (byte)(numArray3[0] / SaveDataNumber);
                        uColorTemp_Table[1, 0] = (byte)(numArray1[1] / SaveDataNumber);
                        uColorTemp_Table[1, 1] = (byte)(numArray2[1] / SaveDataNumber);
                        uColorTemp_Table[1, 2] = (byte)(numArray3[1] / SaveDataNumber);
                        uColorTemp_Table[2, 0] = (byte)(numArray1[2] / SaveDataNumber);
                        uColorTemp_Table[2, 1] = (byte)(numArray2[2] / SaveDataNumber);
                        uColorTemp_Table[2, 2] = (byte)(numArray3[2] / SaveDataNumber);
                    }
                    catch
                    {
                        Ex.Show(" No saved data. ");
                    }
                    streamReader.Close();
                }
            }catch
            {
                ClearData();
            }
        }

        private void ReadStartData()
        {
            try
            {
                using (StreamReader streamReader = File.OpenText(StartDataFile))
                {
                    string str;
                    while ((str = streamReader.ReadLine()) != null)
                    {
                        string[] strArray = str.Split(',');
                        if (int.Parse(strArray[0]) < 3)
                        {
                            uColorTemp_StartStepSum[int.Parse(strArray[0]), 0] = int.Parse(strArray[1]);
                            uColorTemp_StartStepSum[int.Parse(strArray[0]), 1] = int.Parse(strArray[2]);
                            uColorTemp_StartStepSum[int.Parse(strArray[0]), 2] = int.Parse(strArray[3]);
                            uColorTemp_StartStepSum[int.Parse(strArray[0]), 3] = int.Parse(strArray[4]);
                        }
                        else if (int.Parse(strArray[0]) == 3)
                        { Adjust_MaxValue_Backup = byte.Parse(strArray[1]); }
                    }
                    streamReader.Close();
                }
                //Adjust_MaxValue_Backup = (checkBoxLimitRGB.Checked) ? Adjust_MaxValue_Backup : (byte)255;
                Adjust_MaxValue_Backup = MaxRGBValue;
            }
            catch { ClearData(); }
        }

        private void Init_SerialPort()
        {
            Code.port.DataReceived += serialPort1_DataReceived;
            Update();
        }

        private void Init_CA210()
        {
            if (PublicData.isConnectedMinolta)
            {
                PublicData.m_IMemory.ChannelNO = 90;
                DelayXms(50);
                PublicData.m_ICa.SetAnalogRange(2.5f, 2.5f);
                if (PublicData.m_ICa.DisplayMode != 0)
                    PublicData.m_ICa.DisplayMode = 0;
                PublicData.m_IMemory.SetChannelID(" ");
                if (PublicData.m_IMemory.ChannelID != "WB AutoAdj")
                    PublicData.m_IMemory.SetChannelID("WB AutoAdj");
                Measure_CA();
            }
            else
            {
                Ex.Show("CA210 is not connected.");
            }
        }
        private void DisConnect_CA210()
        {
            try
            {
                if (!PublicData.isConnectedMinolta)
                    return;
                PublicData.m_ICa.RemoteMode = 0;
                PublicData.isConnectedMinolta = false;
                PublicData.uPowerStatus = true;
            }
            catch
            {
                Ex.Show("CA210 is not connected.");
            }
        }
        private void Connect_CA210()
        {
            buttonConnectUSB.Enabled = false;
            if (!PublicData.uPowerStatus)
            {
                try
                {
                    PublicData.m_ICa200.AutoConnect();
                }
                catch
                {
                    Ex.Show("Error! Check USB connection please.");
                    buttonConnectUSB.Enabled = true;
                    return;
                }
                DelayXms(50);
                PublicData.uPowerStatus = true;
                PublicData.isConnectedMinolta = true;
                PublicData.m_ICas = (ICas)PublicData.m_ICa200.Cas;
                PublicData.m_ICa = (ICa)PublicData.m_ICas.get_ItemOfNumber(1);
                PublicData.m_IProbe = (IProbe)PublicData.m_ICa.SingleProbe;
                PublicData.m_IMemory = (IMemory)PublicData.m_ICa.Memory;
            }
            else if (!PublicData.isConnectedMinolta)
            {
                try
                {
                    PublicData.m_ICa.RemoteMode = 1;
                    PublicData.uPowerStatus = true;
                    PublicData.isConnectedMinolta = true;
                }
                catch
                {
                    Ex.Show("Error! Check USB connection please.");
                    return;
                }
            }
            DelayXms(50);
            try
            {
                Init_CA210();
            }
            catch
            {
                PublicData.m_ICa.RemoteMode = 0;
                PublicData.isConnectedMinolta = false;
                Ex.Show("Error! Try again.");
            }
        }        
        private void checkMeasure()
        {
            try
            {
                PublicData.m_ICa.Measure(1);
            }
            catch (Exception ex)
            {
                ex.Show();
                if (ex.Message == "Communication Port Error\n--fail USB sending\n--check hard")
                {
                }
            }
        }
        private void ToLog(string msg, float[] income)
        {
            if (string.IsNullOrEmpty(PublicData.LogsWBPath)) return;
            if (!Directory.Exists(PublicData.LogsWBPath))
            {
                try
                {
                    Directory.CreateDirectory(PublicData.LogsWBPath);
                }
                catch (Exception ex)
                {
                    PublicData.write_info($"Лог баланса не сохранен. Ошибка = {ex.Message}: {PublicData.LogsWBPath}");
                    return;
                }
            }
            //string path = @"\\Fserver-366\SoftWare\Журнал записи прошивок на USB";
            //string date = DateTime.Now.ToString("dd.MM.yyyy");
            if (PublicData.IDDataTV == null)
                return;
            //PublicData.TVDataID = "WB log";
            string path = "wb" + PublicData.IDDataTV + ".txt"; //"log.txt";
            path = Path.Combine(PublicData.LogsWBPath, path);
            string time = DateTime.Now.ToString("HH:mm:ss");
            string who = string.Format("{0}({1})", Environment.MachineName, Environment.UserName);
            string header = string.Format("[{0}] ", PublicData.IDDataTV);

            string message = string.Format
                (
                "{3} {5} : x={0}, y={1}, Lv={2}{4}",
                (income[0] / 10000f).ToString().trySubstring(0, 5),
                (income[1] / 10000f).ToString().trySubstring(0, 5),
                income[2].ToString().trySubstring(0, 5),
                msg,
                Environment.NewLine,
                logMsg
                );
            string add = (msg.Contains("after") && msg.Contains("warm")) ? Environment.NewLine : "";
            logMsg = string.Empty;
            TaskEx.Run(() =>
            {
                try
                {
                    File.AppendAllText(path, header + message + add);
                }
                catch (Exception ex)
                {
                    PublicData.write_info($"Не сохранился лог баланса - {ex.Message}: {path}");
                }
            });
        }

        private void DelayXms(int x)
        {
            logger.Trace("TEST start");
            int num = 0;
            do
            {
                for (int index = 0; index < (int)ushort.MaxValue; ++index)
                    ++num;
                --x;
            }
            while (x > 0);
            logger.Trace("TEST end");
        }
        private float PERCENT(float x, float ucCompare_Lv)
        {
            return (float)( (double)ABS(x, ucCompare_Lv) / (double)ucCompare_Lv * 100.0);
        }
        private float ABS(float x, float y)
        {
            if ( (double)x >= (double)y)
                return x - y;
            return y - x;
        }
        private float MAX(float x, float y)
        {
            if ( (double)x > (double)y)
                return x;
            return y;
        }
        private float MIN(float x, float y)
        {
            if ( (double)x < (double)y)
                return x;
            return y;
        }
        private void ColorTempAdjStepUpdate(int uCompare_Step)
        {
            if (uCompare_Step == 0)
            {
                target_Sx = (float)uColorTempStd[0, 0];
                target_Sy = (float)uColorTempStd[0, 1];
            }
            else if (uCompare_Step == 1)
            {
                target_Sx = (float)uColorTempStd[1, 0];
                target_Sy = (float)uColorTempStd[1, 1];
            }
            else if (uCompare_Step == 2)
            {
                target_Sx = (float)uColorTempStd[2, 0];
                target_Sy = (float)uColorTempStd[2, 1];
            }
            else
            {
                target_Sx = (float)uColorTempStd[0, 0];
                target_Sy = (float)uColorTempStd[0, 1];
            }
        }
        private void SetCurrentColorTempMode(int uMode)
        {
            CurrentColorTempMode = uMode;
        }
        private int GetCurrentColorTempMode()
        {
            return CurrentColorTempMode;
        }
        private bool check(float stand_low_value, float stand_high_value, float currrent_value)
        {
            return (double)stand_high_value >= (double)currrent_value && (double)stand_low_value <= (double)currrent_value;
        }
        private bool Check_lv_ok(float lv_compare, float lv_distance)
        {
            return check(lv_compare - lv_distance, lv_compare + lv_distance, ucMeasure_Lv);
        }
        private bool Check_y_temp_ok(float fy_type, float fxy_distance)
        {
            return check(fy_type - fxy_distance, fy_type + fxy_distance, ucMeasure_Sy);
        }
        private bool Check_x_temp_ok(float fx_type, float fxy_distance)
        {
            return check(fx_type - fxy_distance, fx_type + fxy_distance, ucMeasure_Sx);
        }
        private bool Check_temp_ok(float fx_type, float fy_type, float fxy_distance)
        {
            return check(fy_type - fxy_distance, fy_type + fxy_distance, ucMeasure_Sy) && check(fx_type - fxy_distance, fx_type + fxy_distance, ucMeasure_Sx);
        }
        private bool Check_lv_temp_ok(float fx_type, float fy_type, float fxy_distance, float lv_compare, float lv_distance)
        {
            return check(fy_type - fxy_distance, fy_type + fxy_distance, ucMeasure_Sy) && check(fx_type - fxy_distance, fx_type + fxy_distance, ucMeasure_Sx) && check(lv_compare - lv_distance, lv_compare + lv_distance, ucMeasure_Lv);
        }
        private bool Check_RGB_saturation()
        {
            return (double)ABS(ucPrevMeasure_Lv, ucMeasure_Lv) < 2.0;
        }
        private bool Check_saturation()
        {
            return (double)ABS(ucPrevMeasure_Lv, ucMeasure_Lv) < 0.5 && (double)ABS(ucPrevMeasure_Sy, ucMeasure_Sy) < 5.0 && (double)ABS(ucPrevMeasure_Sx, ucMeasure_Sx) < 5.0;
        }        

        private void CheckMinusMaxLvMeasure()
        {
            int minusLv = 0;
            int.TryParse(numericUpDownMinusLv.Value.ToString(), out minusLv);
            Panel_Lv_max = Panel_Lv_max - minusLv;
            labelMinusLv.Text = (minusLv == 0) ? string.Empty : $"(-{minusLv})";
        }

        private void WriteStartData(string startDataFile)
        {
            using (StreamWriter streamWriter = new StreamWriter(startDataFile, false))
            {
                streamWriter.WriteLine(
                    "0" + "," +
                    uColorTemp_StartStepSum[0, 0].ToString() + "," +
                    uColorTemp_StartStepSum[0, 1].ToString() + "," +
                    uColorTemp_StartStepSum[0, 2].ToString() + "," +
                    uColorTemp_StartStepSum[0, 3].ToString()
                    );
                streamWriter.WriteLine(
                    "1" + "," +
                    uColorTemp_StartStepSum[1, 0].ToString() + "," +
                    uColorTemp_StartStepSum[1, 1].ToString() + "," +
                    uColorTemp_StartStepSum[1, 2].ToString() + "," +
                    uColorTemp_StartStepSum[1, 3].ToString()
                    );
                streamWriter.WriteLine(
                    "2" + "," +
                    uColorTemp_StartStepSum[2, 0].ToString() + "," +
                    uColorTemp_StartStepSum[2, 1].ToString() + "," +
                    uColorTemp_StartStepSum[2, 2].ToString() + "," +
                    uColorTemp_StartStepSum[2, 3].ToString()
                    );
                streamWriter.WriteLine(
                    "3" + "," + Adjust_MaxValue_Backup.ToString()
                    );
                streamWriter.Close();
            }
        }
        private void RecheckLayout()
        {
            if (PublicData.isPort1)
                richTextBox1.Text = " Serial port: " + Code.port.PortName.ToString() + " opened";
            else
                richTextBox1.Text = " Serial port: " + Code.port.PortName.ToString() + " closed";
            if (PublicData.isConnectedMinolta)
                richTextBox1.Text = richTextBox1.Text + "           CA210 is connected";
            else
                richTextBox1.Text = richTextBox1.Text + "           CA210 is disconnected";
            if (PassedColorTempModes[0] == 1)
                richTextBox2.Text = "  Cool NG     ";
            else
                richTextBox2.Text = "  Cool OK     ";
            if (PassedColorTempModes[1] == 1)
                richTextBox2.Text = richTextBox2.Text + " Standard NG     ";
            else
                richTextBox2.Text = richTextBox2.Text + " Standard OK     ";
            if (PassedColorTempModes[2] == 1)
                richTextBox2.Text = richTextBox2.Text + " Warm NG ";
            else
                richTextBox2.Text = richTextBox2.Text + " Warm OK ";
        }
        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            //uPort1 = comboBox1.Text;
            //serialPort1.PortName = uPort1;
        }
        private void comboBox3_TextChanged(object sender, EventArgs e)
        {
            //uBaudrate = Convert.ToInt32(comboBox3.Text);
            //serialPort1.BaudRate = uBaudrate;
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        private void Save()
        {
            try
            {
                var set = new SavingManager(PublicData.FindSectionInTreeBySection(WBSetSection));
                set.Key(Setting.CoolX).Value = richTextBox4.Text;
                set.Key(Setting.CoolY).Value = richTextBox5.Text;
                set.Key(Setting.WarmX).Value = richTextBox6.Text;
                set.Key(Setting.WarmY).Value = richTextBox7.Text;
                set.Key(Setting.StandartX).Value = richTextBox8.Text;
                set.Key(Setting.StandartY).Value = richTextBox9.Text;
                set.Key(Setting.ToleranceXY).Value = comboBoxToleranceXY.Text;
                set.Key(Setting.Green128Below).ValueBool = radioButtonGreen128Yes.Checked;
                set.Key(Setting.MinimumLvCool).Value = comboBox4.Text;
                set.Key(Setting.MinimumLvStandart).Value = comboBox6.Text;
                set.Key(Setting.MinimumLvWarm).Value = comboBox7.Text;
                //set.Key(Setting.LimitRGB).ValueBool = checkBoxLimitRGB.Checked;
                set.Key(Setting.AverageNumber).Value = numericUpDownAverageMax.Value.ToString();
                set.Key(Setting.MinusMaxLv).Value = numericUpDownMinusLv.Value.ToString();
                set.Key(Setting.MaxRGBValue).Value = numericUpDownMaxRGB.Value.ToString();
                set.Save();
            }
            catch (Exception ex)
            {
                ex.Show(WBSetSection + "Error.\n" + ex.Message);
            }
        }

        private void OpenPort()
        {
            //try
            //{
            //    if (uRS232Connect)
            //    {
            //        Code.port.Close();
            //        //uRS232Connect = false;
            //        buttonOpenPort.Text = "Open serial port";
            //        buttonStart.Enabled = false;
            //        comboBox1.Enabled = true;
            //        comboBox3.Enabled = true;
            //    }
            //    else
            //    {
            //        Code.port.Open();
            //        //uRS232Connect = true;
            //        buttonOpenPort.Text = "Close serial port";
            //        if (uRS232Connect && PublicData.isConnectedCA210)
            //        {
            //            buttonStart.Enabled = true;
            //            buttonStart.Focus();
            //        }
            //        comboBox1.Enabled = false;
            //        comboBox3.Enabled = false;
            //    }
            //}
            //catch
            //{
            //    //Code.port.Close();
            //    uRS232Connect = false;
            //    Ex.Show("Error, check serial port connection please!");
            //}
        }
        private void buttonConnectUSB_Click(object sender, EventArgs e)
        {
            buttonStart.Enabled = false;
            buttonConnectUSB.Enabled = false;
            try
            {
                PublicData.m_ICa.Measure(1);
            }
            catch (Exception)
            {
                try
                {
                    PublicData.m_ICa.RemoteMode = 1;
                }
                catch (Exception)
                {
                    PublicData.Connect_Minolta();
                }
            }
            //buttonConnectUSB.Text = "Disconnect CA210";
            buttonStart.Enabled = true;
            buttonConnectUSB.Enabled = true;
        }
        private void buttonStart_Click(object sender, EventArgs e)
        {
            Start();
        }

        private void Start()
        {
            logger.Debug($"START {string.Join(" ", PublicData.DisplayModel)}; sn={PublicData.IDDataTV};");
            var foundOptionsSection = PublicData.FindSectionInTreeBySection(WBSetSection);
            if (foundOptionsSection == null)
            {
                Ex.Show("Не найден файл настроек баланса белого"
                    + " c секцией [" + WBSetSection + "].");
                DialogResult = DialogResult.Abort;
                Close();
                return;
            }
            var get = new SavingManager(foundOptionsSection);
            bool isAverage = get.Key(Setting.Average).ValueBool;
            if (!isAverage)
            {
                buttonDefault.PerformClick();
            }

            labelError.Text = string.Empty;
            labelError.Visible = false;
            if (PublicData.isConnectedMinolta)
            {
                if (Adjust_State == 0)
                {
                    Adjust_State = 1;
                    buttonStart.Text = "Stop";
                    richTextBox21.Text = "";
                    buttonSave.Enabled = false;
                    buttonConnectUSB.Enabled = false;
                }
                else
                {
                    Adjust_Result = 1;
                    Adjust_State = 120;
                }
            }
            else
            {
                Ex.Show("Connect CA210 please.");
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {

            if (keyData == Keys.Enter)
            {
                buttonStart.PerformClick();
                return true;
            }
            if (keyData == Keys.Escape)
            {
                Close();
                return true;
            }
            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void ColorSystemTools_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 13 && e.KeyChar != 32)
                return;
            if (PublicData.isPort1 && PublicData.isConnectedMinolta)
            {
                if (Adjust_State == 0)
                {
                    Adjust_State = 1;
                    buttonStart.Text = "Stop";
                    richTextBox21.Text = "";
                    buttonSave.Enabled = false;
                    buttonConnectUSB.Enabled = false;
                }
                else
                {
                    Adjust_Result = 1;
                    Adjust_State = 120;
                }
            }
            else
            {
                Ex.Show("Open serial port and connect CA210 please.");
            }
        }
        private void comboBox5_TextChanged(object sender, EventArgs e)
        {
            LoadPresets();
        }

        private void LoadPresets()
        {
            uColorTempMode = 16;
            uColorTempReadOnly(false);

            comboBox4.Enabled = uColorTempMode == 15;
            comboBox6.Enabled = uColorTempMode == 15;
            comboBox7.Enabled = uColorTempMode == 15;
            richTextBoxCoolLvPercent.Text = "75%";
            richTextBoxNormLvPercent.Text = "85%";
            richTextBoxWarmLvPercent.Text = "75%";
        }

        private void LoadOptions()
        {
            var get = new SavingManager(PublicData.FindSectionInTreeBySection(WBSetSection));
            richTextBox4.Text = get.Key(Setting.CoolX).Value;
            richTextBox5.Text = get.Key(Setting.CoolY).Value;
            richTextBox8.Text = get.Key(Setting.StandartX).Value;
            richTextBox9.Text = get.Key(Setting.StandartY).Value;
            richTextBox6.Text = get.Key(Setting.WarmX).Value;
            richTextBox7.Text = get.Key(Setting.WarmY).Value;
            comboBoxToleranceXY.Text = get.Key(Setting.ToleranceXY).Value;
            comboBox4.Text = get.Key(Setting.MinimumLvCool).Value;
            comboBox6.Text = get.Key(Setting.MinimumLvStandart).Value;
            comboBox7.Text = get.Key(Setting.MinimumLvWarm).Value;
            //checkBoxLimitRGB.Checked = get.Key(Setting.LimitRGB).ValueBool;            
            numericUpDownMinusLv.Value = get.Key(Setting.MinusMaxLv).ValueInt;
            MaxAverageNumber = get.Key(Setting.AverageNumber).ValueInt;
            numericUpDownAverageMax.Value = MaxAverageNumber;            
            MaxRGBValue = get.Key(Setting.MaxRGBValue).ValueByte;
            numericUpDownMaxRGB.Value = MaxRGBValue;
        }

        private void uColorTempReadOnly(bool ucOption)
        {
            if (ucOption)
            {
                richTextBox4.ReadOnly = true;
                richTextBox5.ReadOnly = true;
                richTextBox6.ReadOnly = true;
                richTextBox7.ReadOnly = true;
                richTextBox8.ReadOnly = true;
                richTextBox9.ReadOnly = true;
                richTextBox4.BackColor = SystemColors.Control;
                richTextBox5.BackColor = SystemColors.Control;
                richTextBox6.BackColor = SystemColors.Control;
                richTextBox7.BackColor = SystemColors.Control;
                richTextBox8.BackColor = SystemColors.Control;
                richTextBox9.BackColor = SystemColors.Control;
            }
            else
            {
                richTextBox4.ReadOnly = false;
                richTextBox5.ReadOnly = false;
                richTextBox6.ReadOnly = false;
                richTextBox7.ReadOnly = false;
                richTextBox8.ReadOnly = false;
                richTextBox9.ReadOnly = false;
                richTextBox4.BackColor = SystemColors.ControlLightLight;
                richTextBox5.BackColor = SystemColors.ControlLightLight;
                richTextBox6.BackColor = SystemColors.ControlLightLight;
                richTextBox7.BackColor = SystemColors.ControlLightLight;
                richTextBox8.BackColor = SystemColors.ControlLightLight;
                richTextBox9.BackColor = SystemColors.ControlLightLight;
            }
        }
        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBoxToleranceXY.Text == "0.005")
                Temp_Tolerance = 50f;
            else if (comboBoxToleranceXY.Text == "0.010")
                Temp_Tolerance = 100f;
            else
                Temp_Tolerance = 50f;
        }
        private void richTextBox4_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox4.Text.Length < 7)
                {
                    uColorTempStd[0, 0] = int.Parse(richTextBox4.Text.Substring(2, richTextBox4.Text.Length - 2) ) * 10;
                }
                else
                {
                    Ex.Show("Input invalid!");
                }
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private void richTextBox5_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox5.Text.Length < 7)
                {
                    uColorTempStd[0, 1] = int.Parse(richTextBox5.Text.Substring(2, richTextBox5.Text.Length - 2) ) * 10;
                }
                else
                {
                    Ex.Show("Input invalid!");
                }
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private void richTextBox6_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox6.Text.Length < 7)
                {
                    uColorTempStd[2, 0] = int.Parse(richTextBox6.Text.Substring(2, richTextBox6.Text.Length - 2) ) * 10;
                }
                else
                {
                    Ex.Show("Input invalid!");
                }
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private void richTextBox7_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox7.Text.Length < 7)
                {
                    uColorTempStd[2, 1] = int.Parse(richTextBox7.Text.Substring(2, richTextBox7.Text.Length - 2) ) * 10;
                }
                else
                {
                    Ex.Show("Input invalid!");
                }
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private void richTextBox8_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox8.Text.Length < 7)
                {
                    uColorTempStd[1, 0] = int.Parse(richTextBox8.Text.Substring(2, richTextBox8.Text.Length - 2) ) * 10;
                }
                else
                {
                    Ex.Show("Input invalid!");
                }
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private void richTextBox9_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (richTextBox9.Text.Length < 7)
                {
                    uColorTempStd[1, 1] = int.Parse(richTextBox9.Text.Substring(2, richTextBox9.Text.Length - 2) ) * 10;
                }
                else
                {
                    Ex.Show("Input invalid!");
                }
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private bool check_cmd_return()
        {
            if (!EnableReturnCommand)
                return true;
            if (!uUartReceiveFlag)
                return false;
            uUartReceiveFlag = false;
            return TVReturnStatus == 0;
        }
        private void richTextBox32_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Cool_TargetBrightness = float.Parse(richTextBox32.Text);
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private void richTextBox37_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Standard_TargetBrightness = float.Parse(richTextBox37.Text);
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private void richTextBox38_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Warm_TargetBrightness = float.Parse(richTextBox38.Text);
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private void richTextBox33_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Brightness_Tolerance = (float)( (double)float.Parse(richTextBoxToleranceLv.Text) *
                    (double)Panel_Lv_max / 100.0);
            }
            catch
            {
                Ex.Show("Input invalid!");
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void ClearData()
        {
            SaveDefaultData = false;
            SaveDataNumber = 0;
            richTextBox20.Text = SaveDataNumber.ToString();
            buttonDefault.Enabled = false;
            buttonDefault.BackColor = Color.Green;
            buttonAverage.Enabled = false;
            buttonAverage.BackColor = SystemColors.Control;
            InitValueType = WhiteBalanceMinolta.InitValue.DefaultValue;
            using (StreamWriter streamWriter = new StreamWriter(GetTempDataFile(TEMP_DATA_FILE), false))
            {
                streamWriter.WriteLine("0," + "128" + "," + "128" + "," + "128");
                streamWriter.WriteLine("1," + "128" + "," + "128" + "," + "128");
                streamWriter.WriteLine("2," + "128" + "," + "128" + "," + "128");
                streamWriter.Close();
            }
            ColorTempToDefault();
            ColortempToDisplay();
            CleanStartDataDefault();
            Ex.Try(() => File.Delete(GetTempDataFile(TEMP_DATA_FILE)));
            Ex.Try(() => File.Delete(GetTempDataFile(RESERVE_TEMP_DATA)));
            Ex.Try(() => File.Delete(GetTempDataFile(LAST_TEMP_DATA)));
            Ex.Try(() => File.Delete(StartDataFile));
            for (int index1 = 0; index1 < 3; ++index1)
            {
                uColorTemp_StartStepIndex[index1] = 0;
                for (int index2 = 0; index2 < 4; ++index2)
                    uColorTemp_StartStepSum[index1, index2] = 0;
            }
            richTextBoxCoolLvPercent.Text = "75" + "%";
            richTextBoxNormLvPercent.Text = "85" + "%";
            richTextBoxWarmLvPercent.Text = "75" + "%";
            richTextBoxCoolLvPercent.ForeColor = SystemColors.WindowText;
            richTextBoxNormLvPercent.ForeColor = SystemColors.WindowText;
            richTextBoxWarmLvPercent.ForeColor = SystemColors.WindowText;
            richTextBoxG.ForeColor = SystemColors.WindowText;
        }

        private void ColorTempToDefault()
        {
            uColorTemp_Table[0, 0] = (byte)128;
            uColorTemp_Table[0, 1] = (byte)128;
            uColorTemp_Table[0, 2] = (byte)128;
            uColorTemp_Table[1, 0] = (byte)128;
            uColorTemp_Table[1, 1] = (byte)128;
            uColorTemp_Table[1, 2] = (byte)128;
            uColorTemp_Table[2, 0] = (byte)128;
            uColorTemp_Table[2, 1] = (byte)128;
            uColorTemp_Table[2, 2] = (byte)128;
        }
        private void CleanStartDataDefault()
        {
            using (StreamWriter streamWriter = new StreamWriter(StartDataFile, false) )
            {
                streamWriter.WriteLine(" 0" + "," + " 0" + "," + " 0" + "," + " 0" + "," + " 0");
                streamWriter.WriteLine(" 1" + "," + " 0" + "," + " 0" + "," + " 0" + "," + " 0");
                streamWriter.WriteLine(" 2" + "," + " 0" + "," + " 0" + "," + " 0" + "," + " 0");
                streamWriter.WriteLine(" 3" + "," + " 128");
                streamWriter.Close();
            }
        }
        private void buttonDefault_Click(object sender, EventArgs e)
        {
            if (!File.Exists(GetTempDataFile(TEMP_DATA_FILE) ) )
                return;
            TempDataReadMinimal();
            ColortempToDisplay();
            buttonDefault.Enabled = false;
            buttonDefault.BackColor = Color.Green;
            buttonAverage.Enabled = true;
            buttonAverage.BackColor = SystemColors.Control;
            InitValueType = WhiteBalanceMinolta.InitValue.DefaultValue;
        }
        private void buttonAverage_Click(object sender, EventArgs e)
        {
            if (!File.Exists(GetTempDataFile(TEMP_DATA_FILE) ) )
                return;
            TempDataReadMax();
            richTextBox20.Text = SaveDataNumber.ToString();
            ColortempToDisplay();
            buttonDefault.Enabled = true;
            buttonDefault.BackColor = SystemColors.Control;
            buttonAverage.Enabled = false;
            buttonAverage.BackColor = Color.Green;
            InitValueType = WhiteBalanceMinolta.InitValue.AverageVaule;
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton1.Checked)
                return;
            radioButton2.Checked = false;
            UseMaxPanelLv75Percent = true;
            richTextBox32.ReadOnly = true;
            richTextBox32.BackColor = SystemColors.Control;
            richTextBox32.Text = "210";
            richTextBox37.ReadOnly = true;
            richTextBox37.BackColor = SystemColors.Control;
            richTextBox37.Text = "210";
            richTextBox38.ReadOnly = true;
            richTextBox38.BackColor = SystemColors.Control;
            richTextBox38.Text = "210";
        }
        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton2.Checked)
                return;
            radioButton1.Checked = false;
            UseMaxPanelLv75Percent = false;
            richTextBox32.ReadOnly = false;
            richTextBox32.BackColor = SystemColors.ControlLightLight;
            richTextBox32.Text = Cool_TargetBrightness.ToString();
            richTextBox37.ReadOnly = false;
            richTextBox37.BackColor = SystemColors.ControlLightLight;
            richTextBox37.Text = Standard_TargetBrightness.ToString();
            richTextBox38.ReadOnly = false;
            richTextBox38.BackColor = SystemColors.ControlLightLight;
            richTextBox38.Text = Warm_TargetBrightness.ToString();
        }
        private void radioButtonGreen128Yes_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonGreen128Yes.Checked)
            {
                radioButtonGreen128No.Checked = true;
                return;
            }
            radioButtonGreen128No.Checked = false;
            StandardGGainGreaterThan128 = false;
        }
        private void radioButtonGreen128No_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButtonGreen128No.Checked)
                return;
            radioButtonGreen128Yes.Checked = false;
            StandardGGainGreaterThan128 = true;
        }
        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton5.Checked)
                return;
            radioButton6.Checked = false;
            EnableReturnCommand = true;
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton6.Checked)
                return;
            radioButton5.Checked = false;
            EnableReturnCommand = false;
        }
        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton7.Checked)
                return;
            radioButton8.Checked = false;
            TargetType_Monitor = false;
            comboBox3.Text = "115200";
            uBaudrate = Convert.ToInt32(comboBox3.Text);
            //serialPort1.BaudRate = uBaudrate;
            radioButtonGreen128No.Checked = true;
            radioButtonGreen128Yes.Checked = false;
            StandardGGainGreaterThan128 = true;
            radioButtonGreen128Yes.Enabled = true;
            radioButtonGreen128No.Enabled = true;
            radioButton5.Enabled = true;
            radioButton6.Enabled = true;
        }
        private void radioButton8_CheckedChanged(object sender, EventArgs e)
        {
            if (!radioButton8.Checked)
                return;
            radioButton7.Checked = false;
            TargetType_Monitor = true;
            comboBox3.Text = "128000";
            uBaudrate = Convert.ToInt32(comboBox3.Text);
            //serialPort1.BaudRate = uBaudrate;
            radioButtonGreen128Yes.Checked = true;
            radioButtonGreen128No.Checked = false;
            StandardGGainGreaterThan128 = false;
            radioButtonGreen128Yes.Enabled = false;
            radioButtonGreen128No.Enabled = false;
            radioButton5.Enabled = false;
            radioButton6.Enabled = false;
        }
        #region bullshit
        public struct GAMMA_POINT_ARRAY
        {
            public double x;
            public double y;
        }
        public enum RS232_CMD
        {
            INPUT_SOURCE_com,
            TEMPERATURE_com,
            R_GAIN_com,
            G_GAIN_com,
            B_GAIN_com,
            R_OFFSET_com,
            G_OFFSET_com,
            B_OFFSET_com,
            COPY_TO_All_SOURCE_com,
            ADC_CALIBRATION_com,
            WHITE_PATTERN_com,
            GET_RGB_GAIN_DATA_com,
            RGB_GAIN_VALUE_com,
            R_GAIN_VALUE_com,
            G_GAIN_VALUE_com,
            B_GAIN_VALUE_com,
            AUTO_ADJUST_MODE_com,
        }
        public enum ColorTempNCW
        {
            Cool,
            Normal,
            Warm,
        }
        public enum ColorTempGain
        {
            Red,
            Green,
            Blue,
        }
        public enum InitValue
        {
            DefaultValue,
            AverageVaule,
        }
#endregion
        private void ColorSystemTools_Shown(object sender, EventArgs e)
        {
            Start();
        }

        private void ColorSystemTools_Load(object sender, EventArgs e)
        {
            Init_SerialPort();
            Init_Timer();
            Init_Var();
            LoadOptions();
            Init_LoadFiles();            
            try
            {
                var get = new SavingManager(PublicData.FindSectionInTreeBySection(WBSetSection) );
                radioButtonGreen128Yes.Checked = get.Key(Setting.Green128Below).ValueBool;
            }
            catch (Exception ex)
            {
                ex.Show(WBSetSection + "Error.\n" + ex.Message);
            }
            if (!PublicData.isConnectedMinolta)
            {
                PublicData.Connect_Minolta();
            }
            if (!PublicData.isConnectedMinolta)
            {
                DialogResult = DialogResult.Abort;
            }
        }

        private void WriteColorTempAdj(string tempDataFile)
        {
            WriteColorTempAdj(tempDataFile, SaveDataNumber.ToString(), true);
        }
        private void WriteColorTempAdj(string tempDataFile, string saveNumber, bool isNotOverwrite = true)
        {
            using (StreamWriter streamWriter = new StreamWriter(tempDataFile, isNotOverwrite))
            {
                streamWriter.WriteLine(
                    saveNumber + "0," +
                    uColorTempAdj_Data[0, 0].ToString() + "," +
                    uColorTempAdj_Data[0, 1].ToString() + "," +
                    uColorTempAdj_Data[0, 2].ToString()
                    );
                streamWriter.WriteLine(
                    saveNumber + "1," +
                    uColorTempAdj_Data[1, 0].ToString() + "," +
                    uColorTempAdj_Data[1, 1].ToString() + "," +
                    uColorTempAdj_Data[1, 2].ToString()
                    );
                streamWriter.WriteLine(
                    saveNumber + "2," +
                    uColorTempAdj_Data[2, 0].ToString() + "," +
                    uColorTempAdj_Data[2, 1].ToString() + "," +
                    uColorTempAdj_Data[2, 2].ToString()
                    );
                streamWriter.Close();
            }
        }
        private void SwapReserveTempStartDataFiles()
        {
            //Ex.Try(() => File.Delete(StartDataFile));
            CleanStartDataDefault();
            var curTemp = GetTempDataFile(TEMP_DATA_FILE);
            var reservTemp = GetTempDataFile(RESERVE_TEMP_DATA);
            ///var curStart = StartDataFile;
            //var reservStart = ReserveStartFile;
            SwapFiles(curTemp, reservTemp);
            //SwapFiles(curStart, reservStart);
            ReservedSaveNumbers = 0;
        }
        private void SwapFiles(string deleteFile, string newFile)
        {
            try
            {
                File.Delete(deleteFile);
                File.Copy(newFile, deleteFile, true);
                File.Delete(newFile);
            } catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                ex.Show();
            }
        }
        private void ColorSystemTools_FormClosing(object sender, FormClosingEventArgs e)
        {
            timerClock.Stop();
            Code.port.DataReceived -= serialPort1_DataReceived;            
            Save();
            //log.Out();
        }

        private void numericUpDownMaxRGB_Click(object sender, EventArgs e)
        {
            
        }
    }
}

