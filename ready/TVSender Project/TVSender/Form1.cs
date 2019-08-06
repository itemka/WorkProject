using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Globalization;
using System.Diagnostics;
using System.Reflection;
using CA200SRVRLib;
using System.Text.RegularExpressions;
using NLog;
using DisableDevice;
using System.Linq;
using System.Threading.Tasks;

//TestFactory.cs - актуальное ядро программы

namespace TVSender
{    
    public partial class Form1 : Form 
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static object locker = new object();
        #region переменные
        System.Windows.Forms.Timer reportTimer = new System.Windows.Forms.Timer();
        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume);
        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume);
        int count;
        int CountBlink = 0;
        int CountBrak;
        public static int CountSuccess;        

        public static readonly Dictionary<string, FuncDisplay> InBuiltTests = new Dictionary<string, FuncDisplay>(StringComparer.InvariantCultureIgnoreCase)
        {
            #region inBuildTests
            ["pns.keys"] = new FuncDisplay(Tests.pns_keys, "Кнопки МУ PNS"),
            ["testdatatv"] = new FuncDisplay(Tests.TestRandomDataTV, "Test Random DataTV"),
            ["clean"] = new FuncDisplay(Tests.CleanCheckpassLogs, "Clean Temp Files"),
            ["ktc.IR"] = new FuncDisplay(Tests.ktc_remote, "Пульт"),
            ["Horizont.IR"] = new FuncDisplay(Tests.horizont_remote, "Пульт"),
            ["ktc.wb"] = new FuncDisplay(Tests.KtcWhiteBalanceMinolta, "Какой баланс??? (F6)"),
            ["ktc.wb.Minolta"] = new FuncDisplay(Tests.KtcWhiteBalanceMinolta, "White Balance Minolta"),
            ["ktc.wb.Chroma"] = new FuncDisplay(Tests.KtcWhiteBalanceChroma, "White Balance Chroma"),
            ["ktc.ci"] = new FuncDisplay(Tests.ktc_ci, "CI ключ"),
            ["ktc.keys"] = new FuncDisplay(Tests.ktc_keys, "Кнопки МУ KTC"),
            ["осциллограф"] = new FuncDisplay(Tests.Soundgraph, "Наушники"),
            ["брак"] = new FuncDisplay(Tests.brak, "Забраковать"),
            ["забраковать"] = new FuncDisplay(Tests.brak, "Забраковать"),
            ["genr.hdmi"] = new FuncDisplay(Tests.genr_hdmi, "# HDMI (Chroma)"),
            ["genr.3d"] = new FuncDisplay(Tests.genr_3d, "# 3D (Chroma)"),
            ["genr.cmp"] = new FuncDisplay(Tests.genr_cmp, "# YPbPr (Chroma)"),
            ["genr.ypbpr"] = new FuncDisplay(Tests.genr_cmp, "# YPbPr (Chroma)"),
            ["genr.pc"] = new FuncDisplay(Tests.genr_pc, "# PC (Chroma)"),
            ["genr.vga"] = new FuncDisplay(Tests.genr_pc, "# PC (Chroma)"),
            ["genr.scart"] = new FuncDisplay(Tests.genr_scart, "# SCART (Chroma)"),
            ["genr.scart.rgb"] = new FuncDisplay(Tests.genr_scart_rgb, "SCART RGB"),
            ["genr.scart2"] = new FuncDisplay(Tests.genr_scart_rgb, "SCART RGB"),
            ["ir.3d"] = new FuncDisplay(Tests.ir_3d, "3D"),
            ["ir.3d2"] = new FuncDisplay(Tests.ir_3d2, "3D"),
            ["ir.3d3"] = new FuncDisplay(Tests.ir_3d4, "3D"),
            ["ir.3d4"] = new FuncDisplay(Tests.ir_3d4, "3D"),
            ["ir.led"] = new FuncDisplay(Tests.ir_led, "led"),
            ["ir.ledn"] = new FuncDisplay(Tests.ir_ledn, "led"),
            ["ir.hdmi1"] = new FuncDisplay(Tests.ir_hdmi1, "HDMI1"),
            ["ir.hdmi2"] = new FuncDisplay(Tests.ir_hdmi2, "HDMI2"),
            ["ir.hdmi3"] = new FuncDisplay(Tests.ir_hdmi3, "HDMI3"),
            ["ir.hdmi4"] = new FuncDisplay(Tests.ir_hdmi4, "HDMI4"),
            ["ir.hdmi1b"] = new FuncDisplay(Tests.ir_hdmi1b, "HDMI1"),
            ["ir.hdmi2b"] = new FuncDisplay(Tests.ir_hdmi2b, "HDMI2"),
            ["ir.hdmi3b"] = new FuncDisplay(Tests.ir_hdmi3b, "HDMI3"),
            ["ir.hdmi4b"] = new FuncDisplay(Tests.ir_hdmi4b, "HDMI4"),
            ["ir.scart"] = new FuncDisplay(Tests.ir_scart, "SCART"),
            ["ir.vga"] = new FuncDisplay(Tests.ir_vga, "VGA"),
            ["ir.pc"] = new FuncDisplay(Tests.ir_vga, "VGA"),
            ["ir.6"] = new FuncDisplay(Tests.ir_6, "6"),
            ["ir.atv"] = new FuncDisplay(Tests.ir_atv, "ATV"),
            ["ir.dtv"] = new FuncDisplay(Tests.ir_dtv, "DTV"),
            ["ir.green"] = new FuncDisplay(Tests.ir_green, "green"),
            ["ir.blue"] = new FuncDisplay(Tests.ir_blue, "blue"),
            ["ir.red"] = new FuncDisplay(Tests.ir_red, "red"),
            ["ir.black"] = new FuncDisplay(Tests.ir_black, "black"),
            ["ir.white"] = new FuncDisplay(Tests.ir_white, "white"),
            ["ir.reset"] = new FuncDisplay(Tests.ir_reset, "Сброс"),
            ["ir.chtable"] = new FuncDisplay(Tests.ir_chtable, "Таблица каналов"),
            ["ir.version"] = new FuncDisplay(Tests.ir_version, "Version"),
            ["ir.usb"] = new FuncDisplay(Tests.ir_usb, "USB"),
            ["ir.dvbc"] = new FuncDisplay(Tests.ir_dvbc, "DVBC"),
            ["ir.cmp"] = new FuncDisplay(Tests.ir_cmp, "Компоненты"),
            ["ir.cmpn"] = new FuncDisplay(Tests.ir_cmpn, "вкл. компоненты"),
            ["ir.scarts"] = new FuncDisplay(Tests.ir_scart_sound, "SCART"),
            ["ir.scart.sound"] = new FuncDisplay(Tests.ir_scart_sound, "SCART"),
            ["ir.av"] = new FuncDisplay(Tests.ir_av, "AV"),
            ["tcl.cmdserial_off"] = new FuncDisplay(Tests.tcl_cmdserial_off, "tcl_cmdserial_off"),
            ["tcl.starton"] = new FuncDisplay(Tests.tcl_starton, "StartON"),
            ["tcl.keytest"] = new FuncDisplay(Tests.tcl_KeyTest, "Кнопки МУ и панель"),
            ["tcl.cmdon"] = new FuncDisplay(Tests.tcl_cmd_on, "tcl_cmd_on"),
            ["tcl.lan"] = new FuncDisplay(Tests.tcl_lantest, "LAN тест"),
            ["tcl.usbtest"] = new FuncDisplay(Tests.tcl_usbtest, "USB test"),
            ["otrum.ypbpr"] = new FuncDisplay(Tests.otrum_ypbpr, "Компоненты"),
            ["otrum.av"] = new FuncDisplay(Tests.otrum_av, "AV"),
            ["bp.sw"] = new FuncDisplay(Tests.bp_sw, "SW версия"),
            ["bp.keytest"] = new FuncDisplay(Tests.bp_KeyTest, "Кнопки МУ")
            #endregion
        };
        public static Dictionary<string, string> CreatedDisplays;
        public static Dictionary<string, Func<bool>> CreatedTests;        
        public static ListofPairs<string, string> DublicatedTests = new ListofPairs<string, string>();
        #endregion
        #region Tasks
        Task AutoStartTask = Ex.TaskEmpty;
        readonly TaskScheduler context;
        TaskCompletionSource<bool> tcsAfterStartDelay;
        TaskCompletionSource<bool> tcsStartComplete;

        #endregion

        public Form1()
        {
            InitializeComponent();
            context = TaskScheduler.FromCurrentSynchronizationContext();
            NLogInitialize();
            TaskScheduler.UnobservedTaskException += (o, e) => logger.Fatal(e.Exception, "TaskScheduler.UnobservedTaskException");
            AppDomain.CurrentDomain.UnhandledException += (o, e) => logger.Fatal(e.ExceptionObject as Exception, "CurrentDomain.UnhandledException");
            Application.ThreadException += (o, e) => logger.Fatal(e.Exception, "Application.ThreadException");         
            //ChromaCOM.Cal0();
            this.MaximizeBox = false;
            this.Text += PublicData.GetVersionProject();
            this.Cursor = Cursors.WaitCursor;

            
            reportTimer.Interval = 1000;
            reportTimer.Tick += new EventHandler(reportTimer_Tick);
            CreatedTests = new Dictionary<string, Func<bool>>(StringComparer.InvariantCultureIgnoreCase);
            CreatedDisplays = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
            PublicData.write_genr = txtbox_write_genr;
            PublicData.write_pc = txtbox_write_pc;
            PublicData.write_tv = txtbox_write_tv;
            PublicData.write_info = txtbox_write_info;
            PublicData.errormsg = errormsg;
            PublicData.brak_blink = brak_blink;
            PublicData.SetFokusListView = SetFocusListView;
            try
            {
                PublicData.m_ICa200 = (ICa200)new Ca200Class();
            }
            catch (Exception)
            {
                PublicData.isConnectedMinolta = false;
            }

            CountSuccess = 0;
            CountBrak = 0;      

            SetupSystemSoundBar();
            SetupListView();
            LoadOptionsFromFiles();            

            Tests.PlayerSound = new System.Media.SoundPlayer();
        }

        private void LoadOptionsFromFiles()
        {
            #region FileCheck
            if (!File.Exists(PublicData.configFile))
            {
                Ex.Show("Не найден файл config.ini.\nСоздастся новый.");
                try
                {
                    File.CreateText(PublicData.configFile).Close();
                } catch (Exception ex)
                { ex.Show(); }
                //Environment.Exit(0);
                //return;
            }

            if (!File.Exists("com_settings.ini"))
            {
                try
                {
                    File.CreateText("com_settings.ini").Close();
                } catch (Exception ex) { ex.Show(); }
            }
            #endregion

            IniFile comini = new IniFile();
            comini.Load("com_settings.ini");
            if (comini.GetSection("ports") == null)
            {
                comini.AddSection("ports");
            }
            IniFile.IniSection portsection = comini.GetSection("ports");
            PublicData.port1Name = portsection.GetAddKey("port1_name").Value;
            PublicData.port2Name = portsection.GetAddKey("port2_name").Value;
            PublicData.placeNumber = portsection.GetAddKey("PlaceNumber").Value ?? PublicData.placeNumber;
            PublicData.port1_rate = portsection.GetAddKey("port1_rate").Value;
            PublicData.port2_rate = portsection.GetAddKey("port2_rate").Value;

            PublicData.isPort1 = (portsection.GetAddKey("port1_enable").Value == "1") ? true : false;
            PublicData.isPort2 = (portsection.GetAddKey("port2_enable").Value == "1") ? true : false;
            PublicData.FixOpenPort = (portsection.GetAddKey("FixOpenPort").Value == "1") ? true : false;
            //PublicData.isAutoStart = (portsection.GetAddKey("AutoStart").Value == "1") ? true : false;

            var get = new SavingManager(Where.local);
            PublicData.LogsCheckPassPath = get.Key(Setting.LogsCheckPassPath).Value;
            PublicData.LogsWBPath = get.Key(Setting.LogsWBPath).Value;
            Ex.isAutonomMode = get.Key(Setting.isAutonomMode).ValueBool;
            PublicData.isUseCustomRootPath = get.Key(Setting.isUseCustomRootPath).ValueBool;
            PublicData.isAutoSyncConfig = get.Key(Setting.isAutoSyncConfig).ValueBool;

            NewPortsInitialize();
        }
        public bool LoadModel()
        {
            try
            {
                PublicData.isLetStartTest = false;
                PublicData.CustomRootPath = PublicData.ReadFromFileRootPath();
                ResetBeforeLoadModelForm1();
                string configPath = PublicData.Check.AddGetDirectory(PublicData.RootPath, PublicData.configFolder);
                new FormNewChoice(configPath, PublicData.fileOperation).ShowDialog();


                if (PublicData.firstLaunch & PublicData.cancelLaunch)
                    return false;
                if (PublicData.fileOperation == null)
                    return false;


                foreach (string tab in PublicData.DisplayModel)
                    label3.Text += tab + " ";
                ToolTip t = new ToolTip();
                t.SetToolTip(label3, label3.Text);
                MenuItemOperation.Text = PublicData.operation + new SavingManager().Key(Setting.FileEndProg).Value + " (F6)";
                lblOperationName.Text = PublicData.operation;
                IniFile ini = new IniFile();
                if (!System.IO.File.Exists(PublicData.fileOperation))
                {
                    Ex.Show("не найден файл модели \"" + PublicData.fileOperation);
                    return false;
                }
                ini.Load(PublicData.fileOperation);
                var sections = ini.Sections;
                if (sections.Count == 0)
                { return true; }

                foreach (IniFile.IniSection section in sections)
                {
                    foreach (IniFile.IniSection.IniKey key in section.Keys)
                    {
                        PublicData.Functions.Add(GetFuncByName(key.Name));
                    }
                }
                //listView1.Columns[0].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                listView1.Columns[0].Width = listView1.ClientSize.Width;
                PublicData.CreatedTests = CreatedTests;
                return true;
            }
            finally
            {
                PublicData.isLetStartTest = true;
            }

        }
        //private void PreStart()
        //{            
        //    if (PublicData.isLetStartTest)
        //    {
        //        PublicData.isLetStartTest = false;
        //        this.Height = 427;
        //        //Data.stoptest = false;
        //        PublicData.isStopReader = false;
        //        logger.Trace($"Reader Started! (isStopReader=false)");
        //        txtbox_clear();
        //        if (PublicData.Functions == null)
        //        { return;}

        //        label2.Text = "0";
        //        count = 0;
        //        PublicData.brak = false;

        //        pass_lbl.BackColor = Color.White;
        //        pass_lbl.ForeColor = Color.Black;
        //        pass_lbl.Text = "";

        //        if (PublicData.isPort2)
        //            if (!Code.genr.IsOpen)
        //            {
        //                Ex.Show(Code.genr.PortName + " закрыт");
        //                PublicData.isLetStartTest = true;
        //                return;
        //            }
        //        OpenPorts();
        //        startThread = new Thread(Start);
        //        startThread.Start();
        //    }
        //}
        private async Task Start()
        {
            if (PublicData.isLetStartTest)
            {
                logger.Trace("begin Start()");
                PublicData.isLetStartTest = false;
                if (PublicData.Functions == null)
                { return; }
                tcsStartComplete = new TaskCompletionSource<bool>();
                try
                {
                    OnBeforeStartLogic();
                    OnBeforeStartVisual();
                    //Code.OpenPorts();
                    Code.OpenPort2();
                    int i = 0;
                    foreach (Func<bool> function in PublicData.Functions)
                    {
                        if (function != null)
                        {
                            OnBeforeSubtestVisual(i);
                            bool isSkipFirstTest = (i == 0) && PublicData.isSkipFirstTestAuto;
                            bool isPassed = isSkipFirstTest;
                            if (!isSkipFirstTest)
                            {
                                logger.Trace("Попытка функ");
                                isPassed = await TaskEx.Run(()=>Ex.Catch(Ex.LongRun(() => GetResult(function) ) ) );
                                logger.Trace("Успешно функ");
                            }
                            i++;
                            txtbox_write_pc("");
                            WritePassLog(function, isPassed);
                            if (!isPassed)
                            {
                                onErrorAfterStartLogic();
                                break;
                            }
                        }
                        else
                        {
                            onErrorAfterStartLogic();
                            break;
                        }
                    }
                    OnAfterStartVisual(i);
                    Ex.Try(PublicData.AutoUpdateFromNetToLocalConfig()).RunParallel();
                }
                catch(Exception ex)
                { ex.Show(); }
                finally
                {
                    OnEndStartLogic();
                    logger.Trace("end Start()");
                }                
            }
        }
        public async Task AutoStartLauncher()
        {
            #region Before Autostart
            if (PublicData.isPressedAutoStart)
            { return; }
            PublicData.isPressedAutoStart = true;
            logger.Debug("Enter Autostart()");
            if (AutoStartTask != null)
            {
                logger.Debug($"set isAutoStart = {PublicData.isAutoStart} -> false;");
                PublicData.isAutoStart = false;
                await Ex.Catch(AutoStartTask);
                logger.Debug($"set isAutoStart = {PublicData.isAutoStart} -> true;");
                PublicData.isAutoStart = true;
            }
            autostartToolStripMenuItem.ForeColor = PublicData.isAutoStart ? Color.Green : Color.DarkGray;
            var timerAuto = new System.Timers.Timer(60000);
            timerAuto.Elapsed += (sender, e) => txtbox_clear();
            timerAuto.Start();
            logger.Info("Autostart START RUNNING");
            #endregion
            #region While
            while (PublicData.isAutoStart)
            {
                try
                {
                    await (tcsAfterStartDelay?.Task ?? TaskEx.FromResult(true));
                    if (PublicData.isLetStartTest)
                    {
                        #region filechecks
                        TestFactory testCustom;
                        if (PublicData.isAutoFirstTest)
                        {
                            testCustom = PublicData.GetTest(0);
                            if (testCustom == null)
                            {
                                PublicData.write_info("Ошибка. Первый тест не является внешним. (=внутренний)");
                                continue;
                            }                                                      
                        }
                        else
                        {
                            var section = await TaskEx.Run(()=>PublicData.GetAutostartCustomSection());
                            if (section == null)
                            {
                                PublicData.write_info($"Ошибка. Файл с секцией автостарта [{PublicData.constAutoName}] не найден.");
                                continue;
                            }
                            testCustom = new TestFactory(section);
                        }
                        testCustom.isTestAutoStart = true; //!!временно закоментированно                        
                        #endregion
                        logger.Trace($"автостарт: запуск теста={testCustom.Id}");
                        if (await Ex.LongRun(()=>testCustom.Run()))
                        {
                            timerAuto.Stop();
                            PublicData.isSkipFirstTestAuto = PublicData.isAutoFirstTest;
                            await Start();
                            timerAuto.Start();
                            //logger.Trace($"before if(run=true)={tcsAfterStartDelay?.Task};");
                            await (tcsAfterStartDelay?.Task ?? TaskEx.FromResult(true));
                            //logger.Trace($"after if(run=true)={tcsAfterStartDelay?.Task};");
                        }
                        else
                        {
                            Code.ClosePorts(PublicData.FixOpenPort);
                        }
                    }
                    await (tcsStartComplete?.Task ?? TaskEx.FromResult(true));
                    //logger.Trace($"before if(run=false)={tcsAfterStartDelay?.Task};");
                    await (tcsAfterStartDelay?.Task ?? TaskEx.FromResult(true));
                    //logger.Trace($"after if(run=false)={tcsAfterStartDelay?.Task};");
                }
                catch (Exception ex) { logger.Trace("autostart: END While ERROR:"); ex.Log(); }
                await TaskEx.Delay(PublicData.intervalAutostartCheck);
            }
            #endregion
            #region After Autostart
            await Ex.Catch(AutoStartTask);
            logger.Info("Autostart ENDED! Finished!");
            timerAuto.Stop();
            timerAuto.Dispose();
            autostartToolStripMenuItem.ForeColor = Color.DarkGray;
            logger.Trace($"Reader Started! (isStopReader=false)");
            PublicData.isPressedAutoStart = false;            
            #endregion
        }
        private void onErrorAfterStartLogic()
        {
            PublicData.brak = true;            
            //OnAfterStartLogic();
        }
        private void OnEndStartLogic()
        {
            PublicData.IDDataTV = null;            
            timerStart.Stop();
            Code.ClosePorts(PublicData.FixOpenPort);
            PublicData.isSkipFirstTestAuto = false;
            SetDelayAfterStartForAutostart().RunParallel();            
            PublicData.isLetStartTest = true;            
        }
        private async Task SetDelayAfterStartForAutostart()
        {
            tcsAfterStartDelay?.TrySetResult(true);
            logger.Trace($"before SetDelay={tcsAfterStartDelay?.Task};");
            tcsAfterStartDelay = new TaskCompletionSource<bool>();
            tcsStartComplete.TrySetResult(true);
            int differenceDelay = PublicData.timeAfterAutostart * 1000 - PublicData.intervalAutostartCheck;
            int delayAfter = (differenceDelay > 0) ? differenceDelay : 0;
            logger.Trace($"after new SetDelay={tcsAfterStartDelay?.Task};");
            await TaskEx.Delay(delayAfter);
            tcsAfterStartDelay.TrySetResult(true);
            logger.Trace($"after await SetDelay={tcsAfterStartDelay?.Task};");
        }
        private void OnBeforeStartLogic()
        {
            logger.Trace($"Reader Started! (isStopReader=false)");
            count = 0;
            PublicData.brak = false;
            PublicData.IDDataTV = null;
            var test = PublicData.GetTest(0);
            if (test != null)
            { test.isTestAutoStart = false; }
        }
        async Task SoloTest()
        {
            try
            {
                PublicData.isLetStartTest = false;
                logger.Trace($"Reader Started! (isStopReader=false)");
                this.Height = 427;
                if (PublicData.Functions == null)
                    return;
                if (listView1.SelectedItems.Count == 0)
                    return;
                int index = listView1.SelectedItems[0].Index;
                pass_lbl.ForeColor = Color.Black;
                pass_lbl.BackColor = Color.Aqua;
                listView1.Items[index].BackColor = Color.Aqua;
                pass_lbl.Text = listView1.Items[index].Text;

                if (await Ex.LongRun(()=>GetResult(PublicData.Functions[index])))
                    labelPASS();
                else
                    labelFAIL();
                listView1.Items[index].BackColor = Color.White;
                Code.ClosePorts(PublicData.FixOpenPort);
            } catch { }
            finally
            {
                PublicData.isLetStartTest = true;
            }
        }
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                buttonStart.PerformClick();
                return true;
            }

            if (keyData == Keys.F8)
            {
                redratЗахватСигналаToolStripMenuItem.PerformClick();
                return true;
            }

            if (keyData == Keys.F1 || keyData == (Keys.Back | Keys.Control) )
            {
                toolStripMenuItem1.PerformClick();
                return true;
            }

            if (keyData == (Keys.Enter | Keys.Control) )
            {
                SoloTest().RunParallel();
                return true;
            }

            if (keyData == Keys.Delete || keyData == Keys.F12)
            {
                scaner();
                return true;
            }

            //keyData = Keys.Back | Keys.Control;

            if (keyData == Keys.F2)
            {
                toolStripMenuItem2.PerformClick();
                return true;
            }

            if (keyData == Keys.F3)
            {
                toolStripMenuItem3.ShowDropDown();
                return true;
            }

            if (keyData == Keys.F4)
            {
                toolSMItemEditTestF4.PerformClick();
                return true;
            }

            if (keyData == (Keys.F4 | Keys.Shift) )
            {
                toolSMItemПапкаВнешнихТестовMain.PerformClick();
                return true;
            }

            if (keyData == Keys.F5)
            {
                AutostartButton();
                return true;
            }

            if (keyData == Keys.F6)
            {
                MenuItemOperation.PerformClick();
                return true;
            }

            if (keyData == Keys.F7)
            {
                папкаF5ToolStripMenuItem.PerformClick();
                return true;
            }

            if (keyData == Keys.F10)
            {
                toolSMItemВнешниеТесты.PerformClick();
                return true;
            }

            if (keyData == Keys.F11)
            {
                toolSMItemКомандыВнешнихТестов.PerformClick();
                return true;
            }

            if (Soundbar.Visible)
                if (keyData == Keys.Left || keyData == Keys.Right)
                {
                    Soundbar.Select();
                    return base.ProcessCmdKey(ref msg, keyData);
                }

            if (keyData == Keys.Up || keyData == Keys.Down)
            {
                listView1.Select();
                return base.ProcessCmdKey(ref msg, keyData);
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }
        private void AutostartButton()
        {
            PublicData.isAutoStart = !PublicData.isAutoStart;
            autostartToolStripMenuItem.ForeColor = PublicData.isAutoStart ? Color.Green : Color.DarkGray;
            if (PublicData.isAutoStart)
            { AutoStartLauncher().RunParallel(); }
        }
        private void SetTooltips()
        {
            //ToolTip t = new ToolTip();
            //t.SetToolTip(Button, "Откроет файл текущей операции");
            toolSMItemEditTestF4.ToolTipText = "Открывает файл созданого внешнего теста (если не встроенный)\n(Необходимо выделить тест в списке тестов)";
            toolSMItemКомандыВнешнихТестов.ToolTipText = "Открывает список команд для создания внешних тестов";
            toolSMItemВнешниеТесты.ToolTipText = "Открывает список созданных внешних тестов";
            toolSMItemВстроенныеТестыСправка.ToolTipText = "Открывает список встроенных тестов";            
            toolSMItemВстроенныеТестыMain.ToolTipText = "Открывает список встроенных тестов";
            toolSMItemПапкаВнешнихТестовMain.ToolTipText = "(Shift + F4)\nОткрывает папку внешних тестов (созданных)";
            toolSMItemПапкаВнешнихТестовСправка.ToolTipText = "(Shift + F4)\nОткрывает папку внешних тестов (созданных)";
            toolSMItemChangelog.ToolTipText = "Список изменений версий программы";
            toolSMItemФайлыНастроек.ToolTipText = "Справочная информация";
            toolSMItemУправление.ToolTipText = "Справочная информация";            
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            int left = this.Left - 600;
            if (left < 0) this.Left = 0;
            else this.Left = left;
            TurnOnText();
            if (!LoadModel() )
                this.Close();
            stats();
            SetTooltips();
        }
        private async void redratЗахватСигналаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await Ex.Catch(RedratHelper.CapturedSignal());
        }
        #region bullshit
        private void NLogInitialize()
        {
            Ex.Catch(() =>
            {
                var exe = Assembly.GetExecutingAssembly();
                string resourceName = exe.GetManifestResourceNames()
                    .FirstOrDefault(s => s.IndexOf("NLog.config", StringComparison.OrdinalIgnoreCase) > -1);
                if (!string.IsNullOrEmpty(resourceName))
                {
                    using (var xml = new StreamReader(exe.GetManifestResourceStream(resourceName)))
                    {
                        string xmlConfig = xml.ReadToEnd();
                        //if (!File.Exists("NLog.config"))
                        //{ }
                        File.WriteAllText("NLog.config", xmlConfig);
                    }
                }
            });
        }
        private void OnAfterStartVisual(int i)
        {
            labelPASS();
            CountSuccess++;
            if (PublicData.brak)
            {
                labelFAIL();
                CountSuccess--;
                CountBrak++;
            }
            timerStart.Stop();
            label7.Text = CountSuccess.ToString();
            label8.Text = CountBrak.ToString();
            if (i > 0)
            { listView1.Items[i - 1].BackColor = Color.White; }
        }
        private void OnBeforeSubtestVisual(int i)
        {
            pass_lbl.BackColor = Color.Aqua;
            listView1.Items[i].BackColor = Color.Aqua;
            pass_lbl.Text = listView1.Items[i].Text;
            if (i > 0)
            { listView1.Items[i - 1].BackColor = Color.White; }
            Ex.Try(false, () =>
            {
                listView1.EnsureVisible(i);
                listView1.EnsureVisible(i + 2);
            });
        }
        private void OnBeforeStartVisual()
        {
            this.Height = 427;
            if (!Ex.isAutonomMode)
            { txtbox_clear(); }
            label2.Text = "0";
            pass_lbl.BackColor = Color.White;
            pass_lbl.ForeColor = Color.Black;
            pass_lbl.Text = "";
            timerStart.Start();
        }
        private Func<bool> GetFuncByName(string name)
        {
            if (CreatedDisplays.ContainsKey(name) )
            {
                string item = CreatedDisplays[name];
                listView1.Items.Add(item);
                return CreatedTests[name];
            }
            else
            {
                if (InBuiltTests.ContainsKey(name) )
                {
                    string item = InBuiltTests[name].DisplayName;
                    listView1.Items.Add(item);
                    return InBuiltTests[name].Func;
                }
                else
                {
                    listView1.Items.Add(name, 2);
                    return null;
                }
            }
        }
        private void ResetBeforeLoadModelForm1()
        {
            FillUserOperations();
            PublicData.Functions = new List<Func<bool>>();
            listView1.Items.Clear();            
            label3.Text = "";
            MenuItemOperation.Text = "";
            lblOperationName.Text = "";
        }
        public void ReCreateUserTests()
        {
            FillUserOperations();
            PublicData.Functions = new List<Func<bool>>();
        }

        private int countDuplicates = 0;
        private void AddCreatedOperation(string id, Func<bool> func, string display)
        {
            try
            {
                CreatedDisplays.Add(id, display);
                CreatedTests.Add(id, func);
            }
            catch (Exception ex)
            {                
                logger.Info($"Дубликат! {id}={ex.Message}");
                countDuplicates++;                
                var pathDupl = ((TestFactory)func.Target).FilePath;
                var pathMain = ((TestFactory)CreatedTests[id].Target).FilePath;
                DublicatedTests.Add(id, pathMain);
                DublicatedTests.Add(id, pathDupl);
                дубликатыToolStripMenuItem.Visible = true;
                дубликатыToolStripMenuItem.Text = $"! Обнаружены дубликаты тестов = {countDuplicates}";
                PublicData.write_info($"Обнаружен дубликат теста (F3). Не загружен ({countDuplicates}) - {id}");
            }
        }        

        private void FillUserOperations()
        {
            дубликатыToolStripMenuItem.Visible = false;
            countDuplicates = 0;
            CreatedDisplays.Clear();
            CreatedTests.Clear();
            DublicatedTests.Clear();
            Ex.Catch( () =>
            {
                string testsPath = PublicData.Check.AddGetDirectory(PublicData.RootPath, PublicData.testsFolder);
                FilesForUserCreation(testsPath);
            });
        }

        private void FilesForUserCreation(string startPath)
        {
            var files = Directory.GetFiles(startPath);
            foreach (var file in files)
            {
                var ini = new IniFileList(file);
                foreach (IniFileList.IniSectionList section in ini.Sections)
                {
                    var NewTest = new TestFactory(section);                    
                    AddCreatedOperation(section.Name, NewTest.Run, NewTest.Display);
                }
            }
            string[] folders = Directory.GetDirectories(startPath);
            foreach (string folderPath in folders)
            {
                FilesForUserCreation(folderPath);
            }
        }        

        private static void NewPortsInitialize()
        {
            Code.InitializePort1();
            Code.InitializePortGenr();
        }        

        private void SetupListView()
        {         
            listView1.View = View.Details;
            listView1.HeaderStyle = ColumnHeaderStyle.None;
            ColumnHeader h = new ColumnHeader();
            h.TextAlign = HorizontalAlignment.Right;
            //h.Text = "test";            
            h.Width = listView1.ClientSize.Width;
            //SystemInformation.VerticalScrollBarWidth; 
            listView1.Columns.Add(h);
        }

        private void SetupSystemSoundBar()
        {
            Soundbar.Minimum = 0;
            Soundbar.Maximum = 100;

            //int NewVolume = ( (ushort.MaxValue / 100) * 8);
            // Set the same volume for both the left and the right channels
            //uint NewVolumeAllChannels = ( ((uint)NewVolume & 0x0000ffff) | ( (uint)NewVolume << 16) );
            // Set the volume
            //waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);      

            uint CurrVol = 0;
            waveOutGetVolume(IntPtr.Zero, out CurrVol);
            ushort CalcVol = (ushort)(CurrVol & 0x0000ffff);
            Soundbar.Value = CalcVol / (ushort.MaxValue / 100);
            SoundLbl.Text = Soundbar.Value.ToString() + "%";
        }

        private string getvalue(IniFile.IniSection section, string key, string value)
        {
            if (section.GetAddKey(key).Value == null)
            {
                section.GetAddKey(key).Value = value;
                //section.GetKey(key).SetValue(value);
            }

            return section.GetAddKey(key).Value;
        }
        public void stats()
        {
            Action func = () =>
            {
                string mes;
                mes = Code.port.IsOpen ? "ON" : "off";
                labelPort1.Text = Code.port.PortName + ": " + mes;

                labelPort2.Visible = PublicData.isPort2 ? true : false;
                mes = Code.genr.IsOpen ? "ON" : "off";
                labelPort2.Text = Code.genr.PortName + ": " + mes;

                mes = PublicData.isConnectedMinolta ? "ON" : "off";
                labelUSB.Text = "USB " + mes;
            };

            if (InvokeRequired)
            { Invoke(new MethodInvoker(func)); }
            else
            { func(); }
        }

        public void SetPorts(string portname1, string portname2, string portspeed1, string portspeed2)
        {
            try
            {
                Code.port.PortName = portname1;
                Code.genr.PortName = portname2;
            }
            catch { };
            
            int rate1 = 9600;
            int rate2 = 9600;
            try
            {
                rate1 = int.Parse(portspeed1);
            }
            catch
            {
                Ex.Show("Не корректное значение \"Baudrate 1\"\nЗначение осталось прежним: ");
            }
            try
            {
                rate2 = int.Parse(portspeed2);
            }
            catch
            {
                Ex.Show("Не корректное значение \"Baudrate 1\"\nЗначение осталось прежним: ");
            }
            Code.port.BaudRate = rate1;
            Code.genr.BaudRate = rate2;
        }

        public void brak_blink()
        {
            CountBlink = 0;
            button4.ForeColor = Color.White;
            button4.Text = "Успешно";
            button4.BackColor = Color.Green;
            timerBlink.Interval = 1000;
            timerBlink.Enabled = true;
            timerBlink.Start();
        }

        public void brak_blink_thread()
        {
            Stopwatch timer = Stopwatch.StartNew();
            timer.Start();            
                        
            while (timer.ElapsedMilliseconds < 7000)
            {
                Invoke( (MethodInvoker)( () =>
                {
                    pass_lbl.Text = "Успешно Забракован";
                    pass_lbl.BackColor = Color.Red;                    
                    PublicData.pause(700);
                    pass_lbl.BackColor = Color.DarkRed;
                    PublicData.pause(700);
                    pass_lbl.BackColor = Color.White;
                    pass_lbl.Text = "";
                }) );
            }
            timer.Reset();           
        }

        private void timerBlink_Tick(object sender, EventArgs e)
        {
            CountBlink++;
            if (button4.Text == "Забракован") button4.Text = "Успешно";
            else button4.Text = "Забракован";
            if (button4.BackColor == Color.FromArgb(6,32,2) )
            {
                button4.BackColor = Color.FromArgb(3,154,15);
                button4.ForeColor = Color.White;
            }
            else
            {
                button4.BackColor = Color.FromArgb(6,32,2);
                button4.ForeColor = Color.Lime;
            }
            if (CountBlink > 16)
            {
                button4.BackColor = Color.Red;
                button4.ForeColor = Color.White;
                button4.Text = "Забраковать";                
                timerBlink.Stop();
                timerBlink.Enabled = false;
            }
        }

        public void txtbox_write_pc(string txt)
        {
            string msg = txt.Replace("\0", ""); //.Replace("\r", "").Replace("\n", "")
            msg = "===> " + msg + Environment.NewLine;
            msg = (txt == "") ? Environment.NewLine : msg;
            WriterTxtBoxes(msg);
        }

        private void WriterTxtBoxes(string msg)
        {
            if(InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    textBoxCode.AppendText(msg);
                    textBox1.AppendText(msg);
                    textBoxText.AppendText(msg);
                }));
            }
            else
            {
                textBoxCode.AppendText(msg);
                textBox1.AppendText(msg);
                textBoxText.AppendText(msg);
            }
        }

        public void txtbox_write_info(string txt)
        {
            string msg = txt.Replace("\0", ""); //.Replace("\r", "").Replace("\n", "")
            msg = msg + Environment.NewLine;
            msg = (txt == "") ? Environment.NewLine : msg;
            WriterTxtBoxes(msg);
        }

        public void txtbox_write_genr(string txt)
        {
            string msg = txt.Replace("\0", ""); //.Replace("\r", "").Replace("\n", "")
            msg = "#: " + msg + Environment.NewLine;
            WriterTxtBoxes(msg);
        }

        public void txtbox_write_tv(string txt)
        {
            int length = 115;
            bool isCode = txt.StartsWith("code");
            length = isCode ? length : length/4*3;
            string msg = txt.Replace("\0", "");//.Replace("\n", "\n\t"); //.Replace("\r", "").Replace("\n", "")
            msg = "<--- " + msg;
            msg = msg.trySubstring(0, length).space();
            msg += Environment.NewLine;
            msg = (txt == "") ? Environment.NewLine : msg;
            WriterTxtBoxes(msg);
        }

        public void errormsg(string msg1, string msg2)
        {
            Invoke( (MethodInvoker)( () =>
            {
                label5.Text = msg1;
                label6.Text = msg2;                 

                this.Height = 478;
            }) );
        }

        public void txtbox_clear()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(() =>
                {
                    textBox1.Clear();
                    textBoxText.Clear();
                    textBoxCode.Clear();
                }));
            }
            else
            {
                textBox1.Clear();
                textBoxText.Clear();
                textBoxCode.Clear();
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            //PublicData.Disconnect_CA210();
            FormScaner.MoveFiles();
            try
            {
                //CloseThreads();
                Code.ClosePorts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Закрытие: " + ex.Info());
            }
            try
            {
                Environment.Exit(0);
            }
            catch { };
        }

        int timeReport = 0;
        public void reportTimer_Tick(object sender, EventArgs e)
        {
            timeReport++;
        }
        
        public void timer(object sender, EventArgs e)
        {            
            count++;
            if (count > 60)
            {
                int sec = count % 60;
                if (sec < 10) label2.Text = (count / 60).ToString() + " : 0" + sec.ToString();
                else label2.Text = (count / 60).ToString() + " : " + sec.ToString();                
            }
            else label2.Text = count.ToString(); 
        }        

        private void WriteLog(string idTest, bool passed)
        {
            if (string.IsNullOrEmpty(PublicData.LogsCheckPassPath)) return;
            if (!Directory.Exists(PublicData.LogsCheckPassPath))
            {
                try
                {
                    Directory.CreateDirectory(PublicData.LogsCheckPassPath);
                }
                catch (Exception ex)
                {
                    PublicData.write_info($"Лог теста не сохранен. Ошибка = {ex.Message}: {PublicData.LogsWBPath}");
                    return;
                }
            }
            string file = "ps"+PublicData.IDDataTV + ".txt";           
            var log = new IniFile();
			string path = Path.Combine(PublicData.LogsCheckPassPath, file);
			log.CreateLoad(path);
            string value = passed ? "OK" : "Fail";            
            log.SetKeyValue(PublicData.IDDataTV.Txt, idTest, value);
            TaskEx.Run(async () =>
            {
                try
                {
                    log.Save(path);
                }
                catch (IOException ex)
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (Ex.Try(() => log.Save(path)))
                        { break; }
                        if(i==4)
                        { PublicData.write_info($"Лог теста не сохранен. Ошибка = {ex.Message}: {path}"); }
                        await TaskEx.Delay(500);
                    }
                }
                catch (Exception ex)
                { PublicData.write_info($"Лог теста не сохранен. Ошибка = {ex.Message}: {path}"); }
            });              
        }
        private void WritePassLog(Func<bool> function, bool passed)
        {
            if (PublicData.IDDataTV != null)
            {
                string key = isTestFactory(function);
                if (key != null)
                {
                    WriteLog(key, passed);
                }
                if (key == null)
                {
                    key = isInBuiltTests(function);
                    if (key != null)
                    {
                        WriteLog(key, passed);
                    }
                    else
                    {
                        WriteLog("?undefined?", passed);
                    }
                }
            }           
        }
        private string isTestFactory(Func<bool> function)
        {
            if (function.Target != null)
            {
                bool isType = function.Target.GetType() == typeof(TestFactory);
                if (isType)
                {
                    return ( (TestFactory)function.Target).Id;
                }
            }
            return null;
        }
        private string isInBuiltTests(Func<bool> function)
        {            
            foreach (var tab in InBuiltTests)
            {
                if (tab.Value.Func == function)
                {
                    return tab.Key;
                }
            }
            return null;
        }

        private async void buttonStart_Click(object sender, EventArgs e)
        {
            await Start();
        }
        private bool GetResult(Func<bool> function)
        {
            bool _return = false;
            try
            {                
                _return = function();
            }
            catch (Exception ex)
            {
                LogicSetsOnError(ex);
                return false;
            }
            return _return;
        }

        void Report()
        {
            try
            {
                reportTimer.Stop();
            }
            catch { }
            string hz = timeReport.ToString();
            timeReport = 0;

            reportTimer.Start();
        }        

        private void LogicSetsOnError(Exception ex)
        {
            PublicData.brak = true;
            timerStart.Stop();
            Ex.Show(ex);            
            PublicData.isLetStartTest = true;
        }

        private void toolStripMenuItemSelectModel_Click(object sender, EventArgs e)
        {
            LoadModel();
        }    

        private void press(Button button) 
        {
            button.FlatStyle = FlatStyle.Flat;        
        }

        private void button8_Click(object sender, EventArgs e)
        {
        }

        private void Soundbar_Scroll(object sender, EventArgs e)
        {
            SoundLbl.Text = Soundbar.Value.ToString() + "%";

            // Calculate the volume that's being set
            int NewVolume = ( (ushort.MaxValue / 100) * Soundbar.Value);
            // Set the same volume for both the left and the right channels
            uint NewVolumeAllChannels = ( ((uint)NewVolume & 0x0000ffff) | ( (uint)NewVolume
            << 16) );
            // Set the volume
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);      
        }

        private void toolStripMenuItemOptions_Click(object sender, EventArgs e)
        {
            string oldRoot = PublicData.RootPath;

            var option_form = new OptionsForm1(this);
            option_form.ShowDialog();

            if (oldRoot != PublicData.RootPath)
            {
                PublicData.fileOperation = null;
                PublicData.FolderConfigPath = null;
                LoadModel();
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SoloTest().RunParallel();
        }        

        private void labelPASS()
        {
            pass_lbl.BackColor = Color.Green;
            pass_lbl.ForeColor = Color.Lime;
            pass_lbl.Text = "PASS";
        }

        private void labelFAIL()
        {
            pass_lbl.BackColor = Color.FromArgb(255, 13, 13);
            pass_lbl.ForeColor = Color.White;
            pass_lbl.Text = "Брак";
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;

            var get = new SavingManager(Where.user, PublicData.UniqOperationPath);
            PublicData.isAutoStart = Setting.isAutoStart.Get(get).ValueBool;
            PublicData.isAutoFirstTest = Setting.isAutoFirstTest.Get(get).ValueBool;
            PublicData.timeAfterAutostart = Setting.timeAfterAutostart.Get(get).ValueInt;
            
            if (PublicData.FixOpenPort)
            { Code.OpenPorts(); }
            //Code.BackgroundPortChecker.Start();
            logger.Debug($"{nameof(PublicData.isAutoStart)} = {PublicData.isAutoStart}");
            if (PublicData.isAutoStart) { AutoStartLauncher().RunParallel(); }            
        }

        private void buttonCode_Click(object sender, EventArgs e)
        {
            textBox1.Visible = false;
            textBoxText.Visible = false;
            textBoxCode.Visible = true;
            buttonCode.Enabled = false;
            buttonText.Enabled = true;
        }
        private void buttonText_Click(object sender, EventArgs e)
        {
            TurnOnText();
        }

        private void TurnOnText()
        {
            textBox1.Visible = false;
            textBoxText.Visible = true;
            textBoxCode.Visible = false;
            buttonText.Enabled = false;
            buttonCode.Enabled = true;
        }

        private void pass_lbl_Click(object sender, EventArgs e)
        {
            scaner();
            //Data.brak_blink();
        }

        public void scaner()
        {
            var form = new FormScaner();
            var result = form.ShowDialog();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            scaner();
        }

        private void folderF7ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void MenuItemSound_Click(object sender, EventArgs e)
        {
            if (Soundbar.Visible) Soundbar.Visible = false;
            else Soundbar.Visible = true;
        }

        private void MenuItemConfig_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start("notepad.exe", PublicData.configFile);
            }
            catch
            {
                PublicData.write_info("Файл "+PublicData.configFile+" не найден");
            };
        }

        private void MenuItemOperation_Click(object sender, EventArgs e)
        {
            OpenOperationFile();
        }

        private static void OpenOperationFile()
        {
            try
            {
                Process.Start("notepad.exe", PublicData.fileOperation);                
            }
            catch
            {
                PublicData.write_info("Файл " + PublicData.fileOperation + " не найден");
            };
        }

        private void toolStripMenuItem3_DropDownOpened(object sender, EventArgs e)
        {
            toolStripMenuItem3.Text = "Скрыть (Esc)";
        }

        private void toolStripMenuItem3_DropDownClosed(object sender, EventArgs e)
        {
            toolStripMenuItem3.Text = "Открыть.. (F3)";
        }

        private void папкаF5ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start(PublicData.FolderConfigPath);
        }

        private void командыПрограммированияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = "команды программирования.txt";				
			var writeFile = File.CreateText(filePath);				
            string previous = "";
            writeFile.WriteLine(string.Format("{0, -15} {1}", "Команды:", "Их значение:") );
            foreach (KeyValuePair<string, string> tab in TestFactory.KeysToTests)
            {
                if (tab.Value != previous) writeFile.WriteLine();
                writeFile.WriteLine(string.Format("{0, -15} {1}", tab.Key, tab.Value) );
                previous = tab.Value;
            }
            writeFile.Close();
            Process.Start(filePath);
        }

        private void editTestF4ToolStripMenuItem_Click(object sender, EventArgs e)
        {            
            if (listView1.SelectedItems.Count == 0) return;
            int index = listView1.SelectedItems[0].Index;

            var test = PublicData.GetTest(index);
            if (test != null)
            {
                Process.Start( (test).FilePath);
            }
            else
            { Ex.Show("Это встроенный тест. (не внешний)"); }           
        }

        private void SetFocusListView(TestFactory findClass)
        {            
            Task.Factory.StartNew(() =>
            {                
                lock(locker)
                {
                    int index = PublicData.Functions.LastIndexOf(findClass.Run);
                    if (index < 0)
                    { return; }
                    Invoke((MethodInvoker)(() =>
                    {
                        var item = listView1.Items[index];
                        item.Focused = true;
                        item.Selected = true;
                    }));
                }
            });
        }

        private void созданныеFToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), PublicData.exeName);
            filePath = Path.Combine(filePath, "Созданные тесты.txt");
            try
            {
                using (var writeFile = File.CreateText(filePath))
                {
                    string header = "";
                    foreach (var tab in CreatedTests)
                    {
                        header = ((TestFactory)(tab.Value.Target)).FilePath;
                        break;
                    }
                    header = new Regex(@"[^/\\]+$").Replace(header, "");
                    writeFile.WriteLine(header);
                    writeFile.WriteLine();
                    header = $"{"ID тестов:",-25}  {"Внешние названия:",-22}  {"Файл:"}";
                    writeFile.WriteLine(header);
                    writeFile.WriteLine();
                    foreach (var tab in CreatedDisplays)
                    {
                        string keyFormat = $"[{tab.Key}]";
                        string testsPath = PublicData.Check.AddGetDirectory(PublicData.RootPath, PublicData.testsFolder);
                        string file = ((TestFactory)CreatedTests[tab.Key].Target).FilePath.Replace(testsPath, "").TrimStart('\\');
                        writeFile.WriteLine($"{keyFormat,-25}  {tab.Value,-22}  {file}");
                    }
                    writeFile.Close();
                }
                Process.Start(filePath);
            } catch (Exception ex)
            {
                logger.Error(ex , ex.Message);
                ex.Show();
            }
        }

        private void файлыНастроекToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string opt = new SavingManager().Key(Setting.FileEndOptions).Value; ;
            string hz = new SavingManager().Key(Setting.FileEndProg).Value; ;
            string msg = "";
            msg += $"Поиск файлов ИК сигнала .wav и файлов настроек {opt} начинается c папки, где находится файл набора тестов {hz}," +
            $" и продолжается по папкам выше, пока не найдет .wav файл либо искомый раздел (секцию) в {opt} файлах\n\n";
			msg += $"Разделы, поиск которых осуществляется в {opt} файлах:\n";
			msg += $"{" • [WBSettings]",-40} {""}\n\n";
            msg += " • [Generator]\n";
            msg += "    VGA.tim=229\n";
            msg += "    VGA.pat=3502\n";
            msg += "    HDMI.tim=71\n";
            msg += "    HDMI.pat=3505\n";
            msg += "    SCART.tim=3603\n";
            msg += "    SCART.pat=3502\n";
            msg += "    SCART.RGB.tim=3601\n";
            msg += "    SCART.RGB.pat=3505\n";
            msg += "    3D.tim = 74\n";
            msg += "    3D.pat = 803\n";
            msg += "    cmp.tim = 105\n";
            msg += "    cmp.pat = 601\n";            
            MessageBox.Show(msg);            
        }

        private void встроенныеТестыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), PublicData.exeName);
            filePath = Path.Combine(filePath, "Встроенные тесты.txt");
            try
            {
                var writeFile = File.CreateText(filePath);
                string header = $"{"ID тестов:", -28} {"Отображаемое имя:",-19} {"Их значения:"}";
                writeFile.WriteLine(header);
                writeFile.WriteLine();
                foreach (var tab in InBuiltTests)
                {
                    string idNameTest = $"[{tab.Key}]";
                    string methodName = InBuiltTests[tab.Key].Func.Method.Name;
                    writeFile.WriteLine($"{idNameTest,-28} {tab.Value,-19} {methodName}");
                }
                writeFile.Close();
                System.Diagnostics.Process.Start(filePath);
            }
            catch (Exception ex) { ex.Show(); }
        }

        private void папкаТестовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count == 0)
            { return; }
            int index = listView1.SelectedItems[0].Index;
            var test = PublicData.GetTest(index);
            if (test == null)
            {
                Ex.Show("Тест не является внешним. (или не найден).");
                return;
            }
            var dir = new FileInfo(test.FilePath).DirectoryName;
            Ex.Catch(()=>Process.Start(dir));
        }

        private void changelogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var msg = new StringBuilder();
            msg.AppendLine("1.12.00:");
            msg.AppendLine("• Добавлена автоматическая синхронизация конфига Сервер -> ПК.");
            msg.AppendLine("• Исправленно на более точное определение необходимости в калибровке через Chroma в Балансе Белого.");
            msg.AppendLine("1.11.00:");
            msg.AppendLine("• Добавлены/отредактированы всплывающие описания/подсказки в меню настроек в соответсвии с новым функционалом;");
            msg.AppendLine("1.10.05:");
            msg.AppendLine("• Добавлена кнопка (F8) вызова захвата RedRat сигнала;");
            msg.AppendLine("• Заработала галочка включения/выключения использования сетевого пути");
            msg.AppendLine("• Пофиксены ошибки с пустыми/недоступными указанными адресами для логов баланса белого и логов тестов;");
            msg.AppendLine("• Добавлена кнопка 'Синхронизовать', которая копирует с заменой конфиг с сервера (с указанного адреса) в папку программы;");
            msg.AppendLine("1.9.20: исправлены некоторые ошибки Баланса Белого;");
            msg.AppendLine("1.9.15: added RedRat universal reading and sending any signal;");
            MessageBox.Show(msg.ToString());
        }

        private void управлениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string file = "управление.txt";
            try
            {
                Process.Start(file);
            }
            catch
            {
                PublicData.write_info("Файл "+file+" не найден");
            };
        }

        private void lblOperationName_Click(object sender, EventArgs e)
        {
            LoadModel();            
        }
        private void отключитьПортToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    DeviceManagerApi.ToogleComPortDevice(Code.port, false);
            //    Thread.Sleep(100);
            //    DeviceManagerApi.ToogleComPortDevice(Code.port, true);
            //    Thread.Sleep(100);
            //} catch (Exception ex)
            //{
            //    logger.Error(ex, $"{Code.port.PortName}: {ex.Message}");
            //    Ex.Show($"Не удалось перезапустить порт {Code.port.PortName} ({ex.Message}) - придется сделать это вручную.");
            //}
            try
            {
                DeviceManagerApi.ToogleRedRat(false);
                Thread.Sleep(100);
                DeviceManagerApi.ToogleRedRat(true);
                Thread.Sleep(100);
            } catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                PublicData.write_info($"Не удалось перезапустить Redrat - придется сделать это вручную.\n{ex.Message}");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtbox_clear();
        }

        private void дубликатыToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), PublicData.exeName);
            filePath = Path.Combine(filePath, "Дубликаты тестов.txt");
            try
            {
                using (var writeFile = File.CreateText(filePath))
                {
                    writeFile.WriteLine("Первая строка - тест, загруженный в программу;");
                    writeFile.WriteLine("Вторая строка - дубликат, НЕ загруженный в программу;");
                    writeFile.WriteLine("Если несколько дубликатов одного и того же теста, то первая строка дублируется для каждой пары");
                    writeFile.WriteLine();
                    writeFile.WriteLine($"{"ID теста:",-11}  {"Файл:"}");
                    writeFile.WriteLine();
                    bool firstLine = true;
                    foreach (var tab in DublicatedTests)
                    {
                        writeFile.WriteLine($"{tab.Key,-11}  {tab.Value}");
                        if (!firstLine) { writeFile.WriteLine(); }
                        firstLine = !firstLine;
                    }
                    writeFile.Close();
                }
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                ex.Show();
            }
        }

        private void lblOperationName_DoubleClick(object sender, EventArgs e)
        {
            OpenOperationFile();
        }

        private void autostartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutostartButton();
        }

        private void exeLauncherToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string msg = "В файле должна быть секция [exeLauncher]\n";
            msg += "а под ней имя файла .exe (или полный путь к файлу)\n";
            msg += "например:\n";
            msg += "[exe]\n";
            msg += "USBWriter.exe\n";
            Ex.Show(msg);
        }
        #endregion
    }
}
