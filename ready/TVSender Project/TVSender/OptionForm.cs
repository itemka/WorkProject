using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Diagnostics;
using System.IO;
using NLog;
//using RJCP.IO.Ports;

namespace TVSender
{
    public partial class OptionsForm1 : Form
    {
        Form1 mainForm;
        string pathAutoFolder;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private readonly string n = Environment.NewLine;

        #region для exe
        public string pathTempFile = Path.GetTempPath();//находим папку темп юзера
        public string nameEXEFile = "COM_Registration_Tool.exe";//название файла
        public String file_exe = Environment.GetFolderPath(Environment.SpecialFolder.InternetCache) + "\\COM_Registration_Tool.exe";
        #endregion

        public OptionsForm1(Form1 f1)
        {
            InitializeComponent();

            mainForm = f1;
        }

        private void OptionForm_Load(object sender, EventArgs e)
        {
            PublicData.isLetStartTest = false;
            Reload();            
        }

        private void Reload()
        {
            #region old
            txtWBLogsPath.Text = PublicData.LogsWBPath;
            txtCheckpassLogsPath.Text = PublicData.LogsCheckPassPath;

            string[] ports = SerialPort.GetPortNames();
            string[] baudrates = { "9600", "57600", "115200" };
            comboBoxPort1.Items.Clear();
            comboBoxPort1.Items.AddRange(ports);
            comboBoxPort2.Items.Clear();
            comboBoxPort2.Items.AddRange(ports);
            comboBoxBaudrate1.Items.Clear();
            comboBoxBaudrate1.Items.AddRange(baudrates);
            comboBoxBaudrate2.Items.Clear();
            comboBoxBaudrate2.Items.AddRange(baudrates);

            comboBoxPort1.Text = PublicData.port1Name;
            comboBoxPort2.Text = PublicData.port2Name;
            comboBoxBaudrate1.Text = PublicData.port1_rate;
            comboBoxBaudrate2.Text = PublicData.port2_rate;

            textBoxFileEnd.Text = new SavingManager().Key(Setting.FileEndOptions).Value;
            labelAutoCustomDescrip.Text = string.Format("по {0} файлу с [auto] секцией", textBoxFileEnd.Text);

            //if (Data.port1_name == null) comboBox1.Text = comboBox1.Items[0].ToString();
            //if (Data.port2_name == null) comboBox2.Text = comboBox2.Items[0].ToString();

            portBox1.Checked = PublicData.isPort1;
            portBox2.Checked = PublicData.isPort2;
            checkBoxFixOpen.Checked = PublicData.FixOpenPort;
            #endregion

            txtRejectPath.Text = FormScaner.GetPathRejectFromFile();
            txtConfigPath.Text = PublicData.ReadFromFileRootPath();

            IniFile comini = new IniFile();
            comini.Load("com_settings.ini");
            try
            {
                IniFile.IniSection portsection = comini.GetSection("ports");
                PublicData.placeNumber = portsection.GetAddKey("PlaceNumber").Value ?? PublicData.placeNumber;
            }
            catch { };
            textBoxPlaceNumber.Text = PublicData.placeNumber;

            SetTooltips();
            VisualRefresh();

            var get = new SavingManager(Where.user, PublicData.UniqOperationPath);
            PublicData.isAutoStart = Setting.isAutoStart.Get(get).ValueBool;
            PublicData.isAutoFirstTest = Setting.isAutoFirstTest.Get(get).ValueBool;
            PublicData.timeAfterAutostart = Setting.timeAfterAutostart.Get(get).ValueInt;

            checkBoxAutoStart.Checked = PublicData.isAutoStart;
            panelAutoFirstTest.Enabled = PublicData.isAutoStart;
            panelAutoCustom.Enabled = PublicData.isAutoStart;
            numericUpDownTimeAfterAutostart.Value = PublicData.timeAfterAutostart;

            radioButtonAutoFirstTest.Checked = PublicData.isAutoFirstTest;
            radioButtonAutoCustom.Checked = !PublicData.isAutoFirstTest;
            checkBoxAutonomMode.Checked = Ex.isAutonomMode;
            checkBoxRootPath.Checked = PublicData.isUseCustomRootPath;
            checkBoxAutoSync.Checked = PublicData.isAutoSyncConfig;
            string tempDate = Setting.dateConfigUpdate.Get(new SavingManager()).Value;
            Ex.Try(() =>
            {                
                labelSyncDate.Text = (tempDate.Length == 8) ? tempDate.Insert(6, ".").Insert(4, ".") : tempDate;
            });
        }

        private void VisualRefresh()
        {
            SetVisualsAutoSections();
        }

        private void CloseAndSave()
        {
            buttonOk.Enabled = false;

            #region old
            //SetPorts(comboBox2.Text, comboBox2.Text, comboBox3.Text, comboBox4.Text);            
            PublicData.isPort1 = portBox1.Checked;
            PublicData.isPort2 = portBox2.Checked;
            PublicData.FixOpenPort = checkBoxFixOpen.Checked;
            PublicData.isAutoStart = checkBoxAutoStart.Checked;
            PublicData.port1_rate = comboBoxBaudrate1.Text;
            PublicData.port2_rate = comboBoxBaudrate2.Text;
            PublicData.port1Name = comboBoxPort1.Text;
            PublicData.port2Name = comboBoxPort2.Text;
            PublicData.placeNumber = textBoxPlaceNumber.Text;

            mainForm.SetPorts(comboBoxPort1.Text, comboBoxPort2.Text,
                comboBoxBaudrate1.Text, comboBoxBaudrate2.Text);

            string on1, on2, fixon, auto, minolta;
            on1 = PublicData.isPort1 ? "1" : "0";
            on2 = PublicData.isPort2 ? "1" : "0";
            fixon = PublicData.FixOpenPort ? "1" : "0";
            auto = PublicData.isAutoStart ? "1" : "0";
            minolta = PublicData.isConnectedMinolta ? "1" : "0";

            IniFile ini = new IniFile("com_settings.ini");
            ini.SetKeyValue("ports", "port1_name", comboBoxPort1.Text);
            ini.SetKeyValue("ports", "port2_name", comboBoxPort2.Text);
            ini.SetKeyValue("ports", "port1_enable", on1);
            ini.SetKeyValue("ports", "port2_enable", on2);
            ini.SetKeyValue("ports", "port1_rate", comboBoxBaudrate1.Text);
            ini.SetKeyValue("ports", "port2_rate", comboBoxBaudrate2.Text);
            ini.SetKeyValue("ports", "FixOpenPort", fixon);
            ini.SetKeyValue("ports", "AutoStart", auto);
            ini.SetKeyValue("ports", "PlaceNumber", textBoxPlaceNumber.Text);
            ini.SaveShowMessage("com_settings.ini");

            ini = new IniFile(PublicData.configFile);
            ini.ForceRenameKey("brak", FormScaner.GetPathRejectFromFile(), txtRejectPath.Text);
            if (txtRejectPath.Text.Length == 0) ini.RemoveSection("brak");
            if (txtConfigPath.Text.Length == 0)
            {
                ini.RemoveSection(PublicData.configSection);
                PublicData.CustomRootPath = null;
            }
            string oldPath = PublicData.ReadFromFileRootPath();
            if (oldPath == null) oldPath = "";
            if (ini.ForceRenameKey(PublicData.configSection, oldPath, txtConfigPath.Text))
            { PublicData.CustomRootPath = txtConfigPath.Text; }
            ini.SaveShowMessage();
            #endregion

            var set = new SavingManager();
            set.Key(Setting.FileEndOptions).Value = "." + textBoxFileEnd.Text.Trim(' ', '.', '"');
            set.Save();

            var local = new SavingManager(Where.local);
            PublicData.LogsCheckPassPath = txtCheckpassLogsPath.Text;
            PublicData.LogsWBPath = txtWBLogsPath.Text;
            local.Key(Setting.LogsCheckPassPath).Value = PublicData.LogsCheckPassPath;
            local.Key(Setting.LogsWBPath).Value = PublicData.LogsWBPath;
            Ex.isAutonomMode = checkBoxAutonomMode.Checked;
            local.Key(Setting.isAutonomMode).ValueBool = Ex.isAutonomMode;
            PublicData.isUseCustomRootPath = checkBoxRootPath.Checked;
            local.Key(Setting.isUseCustomRootPath).ValueBool = PublicData.isUseCustomRootPath;
            PublicData.isAutoSyncConfig = checkBoxAutoSync.Checked;
            local.Key(Setting.isAutoSyncConfig).ValueBool = PublicData.isAutoSyncConfig;
            local.Save();


            var appdata = new SavingManager(Where.user, PublicData.UniqOperationPath);
            appdata.Key(Setting.isAutoStart).ValueBool = checkBoxAutoStart.Checked;
            appdata.Key(Setting.isAutoFirstTest).ValueBool = radioButtonAutoFirstTest.Checked;
            appdata.Key(Setting.timeAfterAutostart).Value = numericUpDownTimeAfterAutostart.Value.ToString();
            appdata.Save();

            Code.InitializePort1();
            Code.InitializePortGenr();

            PublicData.isAutoStart = appdata.Key(Setting.isAutoStart).ValueBool;
            PublicData.isAutoFirstTest = appdata.Key(Setting.isAutoFirstTest).ValueBool;
            PublicData.timeAfterAutostart = appdata.Key(Setting.timeAfterAutostart).ValueInt;


            if (PublicData.FixOpenPort)
            { Code.OpenPorts(); }
            else
            { Code.ClosePorts(); }

            mainForm.stats();
            logger.Debug($"autostart = {PublicData.isAutoStart}");
            if (PublicData.isAutoStart) { mainForm.AutoStartLauncher().RunParallel(); }
            this.Close();
        }

        private void SetVisualsAutoSections()
        {
            pictureBoxAutoCustomFound.SizeMode = PictureBoxSizeMode.StretchImage;
            var section = PublicData.GetAutostartCustomSection();
            if (section == null)
            {
                SetNOAutoFoundVisual();
            }
            else //section = OK
            {
                SetYesAutoFoundVisual();
                var pathAutoFile = section?.GetParentIniFile().Path;
                pathAutoFolder = PublicData.PathDeacreaseByFolder(pathAutoFile);

                var partedPath = pathAutoFile.Split(new[] { @"\", "/" }, StringSplitOptions.RemoveEmptyEntries);
                buttonOpenAutoFile.Text = partedPath[partedPath.Length - 1];
                buttonOpenFolderAuto.Text = partedPath[partedPath.Length - 2];
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Enter)
            {
                buttonOk.PerformClick();
                return true;
            }

            if (keyData == Keys.Escape)
            {                
                this.Close();
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void SetTooltips()
        {
            ToolTip t = new ToolTip();                                 
            t.SetToolTip(label5, "Off: Порты открываются при каждом запуске теста и закрываются по его завершению (теста)\n"+
"On: Порты постоянно открыты, а закрываются при закрытии программы");
            t.SetToolTip(checkBoxFixOpen, "Off: Порты открываются при каждом запуске теста и закрываются по его завершению (теста)\n" +
"On: Порты постоянно открыты, а закрываются при закрытии программы");
            t.SetToolTip(label7, "Используется в отчетах брака");
            t.SetToolTip(textBoxPlaceNumber, "Используется в отчетах брака");
            t.SetToolTip(lblRejectPath, "Расположение папки, куда сохранять отчеты брака");
            t.SetToolTip(this.txtRejectPath, "Расположение папки, куда сохранять отчеты брака");
            string dir1 = $"\"{PublicData.configFolder}\" - содержащая конфигурации исполнения тестов";
            string dir2 = $"\"{PublicData.testsFolder}\" - содержащая создаваемые тесты";
            string dir3 = $"\"{PublicData.imageFolder}\" - содержащая изображения, использующиеся в создании тестов";
            string confToolTip = $"Расположение сервер-папки, в которой будут содержаться папки:\n{dir1}\n{dir2}\n{dir3}";
            t.SetToolTip(lblConfigPath, confToolTip);
            t.SetToolTip(txtConfigPath, (string.IsNullOrEmpty(txtConfigPath.Text) ? string.Empty : txtConfigPath.Text + n) + confToolTip);
            var autonomeModeTip = "Ошибки не будут прерывать работу программы (полезен для безоператорного места).";
            t.SetToolTip(checkBoxAutonomMode, autonomeModeTip);
            t.SetToolTip(label11, autonomeModeTip);
            t.SetToolTip(buttonSyncConfigNetnLocal, $"Копирует конфиг Сервер -> ПК: с заменой с указанного пути в локальную папку расположения программы.{n}Папка #Tests приводится в полное соответсвие (лишние удаляются).");
            t.SetToolTip(checkBoxAutoSync, $"Автоматически запускает синхронизацию (Сервер -> ПК) после 10-ти успешных тестов, если сервер-папка конфига включена.");
            t.SetToolTip(labelSyncDate, $"Дата последней синхронизации (год месяц число).");
            t.SetToolTip(checkBoxRootPath, $"On: (сервер) используется конфиг по указанному адресу {n}Off: (ПК) игнорирует указанный адрес; используется локальный конфиг (место расположения программы).");
            t.SetToolTip(txtWBLogsPath, txtWBLogsPath.Text);
            t.SetToolTip(txtCheckpassLogsPath, txtCheckpassLogsPath.Text);            
        }

        private void portBox2_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            CloseAndSave();
        }        

        private void autoBox_CheckedChanged(object sender, EventArgs e)
        {
            panelAutoFirstTest.Enabled = checkBoxAutoStart.Checked;
            panelAutoCustom.Enabled = checkBoxAutoStart.Checked;
        }

        private void textBox1_MouseClick(object sender, MouseEventArgs e)
        {            
            textBoxPlaceNumber.SelectAll();
        }

        private void btnConfigPathSelect_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog() )
            {
                dialog.Description = "Выберите директорию";
                dialog.RootFolder = Environment.SpecialFolder.Desktop;
                dialog.SelectedPath = PublicData.RootPath;                

                if (dialog.ShowDialog() == DialogResult.OK)
                    txtConfigPath.Text = dialog.SelectedPath;
                txtConfigPath.SelectionStart = txtConfigPath.Text.Length;
            }
        }

        private void btnRejectPathSelect_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog() )
            {
                dialog.Description = "Выберите директорию";
                dialog.RootFolder = Environment.SpecialFolder.Desktop;
                dialog.SelectedPath = txtRejectPath.Text;                

                if (dialog.ShowDialog() == DialogResult.OK)
                    txtRejectPath.Text = dialog.SelectedPath;
                txtRejectPath.SelectionStart = txtRejectPath.Text.Length;                
            }
        }

        private void txtConfigPath_Click(object sender, EventArgs e)
        {
            txtConfigPath.SelectAll();
        }

        private void txtRejectPath_Click(object sender, EventArgs e)
        {
            txtRejectPath.SelectAll();            
        }

        private void btnWBlogs_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog() )
            {
                dialog.Description = "Выберите директорию";
                dialog.RootFolder = Environment.SpecialFolder.Desktop;
                dialog.SelectedPath = txtWBLogsPath.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                    txtWBLogsPath.Text = dialog.SelectedPath;
                txtWBLogsPath.SelectionStart = txtWBLogsPath.Text.Length;
            }
        }

        private void btnCheckpassLogs_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog() )
            {
                dialog.Description = "Выберите директорию";
                dialog.RootFolder = Environment.SpecialFolder.Desktop;
                dialog.SelectedPath = txtCheckpassLogsPath.Text;

                if (dialog.ShowDialog() == DialogResult.OK)
                    txtCheckpassLogsPath.Text = dialog.SelectedPath;
                txtCheckpassLogsPath.SelectionStart = txtCheckpassLogsPath.Text.Length;
            }
        }

        private void btnOpenConfigPath_Click(object sender, EventArgs e)
        {
            string pathToOpen = string.IsNullOrEmpty(txtConfigPath.Text) ? 
                PublicData.RootPath : txtConfigPath.Text;
            try
            {
                Process.Start(Path.Combine(pathToOpen,PublicData.configFolder) );
            }
            catch (Exception ex)
            {
                ex.Show("Ошибка. Не удалось открыть папку " + pathToOpen);
            }
            
        }

        private void btnOpenRejectPath_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(txtRejectPath.Text);
            }
            catch (Exception ex)
            {
                ex.Show("Ошибка. Не удалось открыть папку.");
            }
        }

        private void btnOpenWBReports_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(txtWBLogsPath.Text);
            }
            catch (Exception ex)
            {
                ex.Show("Ошибка. Не удалось открыть папку.");
            }
        }

        private void btnOpenTestLogs_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(txtCheckpassLogsPath.Text);
            }
            catch (Exception ex)
            {
                ex.Show("Ошибка. Не удалось открыть папку.");
            }
        }

        private void btnOpenWBFile_Click(object sender, EventArgs e)
        {
            var section = PublicData.FindSectionInTreeBySection("WBSettings");
            var file = (section == null) ? null : section.IniFile.Path;
            if (file == null)
            {
                Ex.Show("Файл настроек Баланса Белого не найден.\r\n"
                    + "Ищется: файл с секцией [WBSettings].");
                return;
            }
            try
            {
                Process.Start("notepad.exe", file);
            }
            catch (Exception ex)
            {
                ex.Show("Ошибка. Не удалось открывать файл.");
            }
        }

        private void btnCreateWBFile_Click(object sender, EventArgs e)
        {
            string fileEnd = new SavingManager().Key(Setting.FileEndOptions).Value;
            string fileName = "WBSettings" + fileEnd;
            string fullPath = Path.Combine(PublicData.FolderConfigPath, fileName);
            try
            {                
                File.CreateText(fullPath).Close();
            }
            catch (Exception ex)
            {
                PublicData.ShowMessage(ex, "Ошибка. Не удалось создать файл.");
                return;
            }
            try
            {
                var set = new SavingManager(Where.custom, fullPath, "WBSettings");
                set.Key(Setting.CoolX);
                set.Key(Setting.CoolY);
                set.Key(Setting.StandartX);
                set.Key(Setting.StandartY);
                set.Key(Setting.WarmX);
                set.Key(Setting.WarmY);
                set.Key(Setting.ToleranceXY);
                set.Key(Setting.Average);
                set.Key(Setting.Green128Below);
                set.Save();
                Process.Start(fullPath);
            }
            catch (Exception ex)
            {
                PublicData.ShowMessage(ex, "Ошибка. Не удалось открыть файл.");
            }           
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            Reload();
        }

        private void buttonOpenAutoFile_Click(object sender, EventArgs e)
        {
            var section = PublicData.FindSectionInTreeBySection(PublicData.constAutoName);
            string file = (section == null) ? null : section.IniFile.Path;
            if (file == null)
            {
                Ex.Show("Файл настройки автостарта не найден.\r\n"
                    + "Ищется: файл с секцией [Auto].");
                return;
            }
            try
            {
                Process.Start("notepad.exe", file);
            }
            catch (Exception ex)
            {
                PublicData.ShowMessage(ex, "Ошибка. Не удалось открывать файл.");
            }
        }

        private void SetFolderVisibleVisual(bool set)
        {
            labelFolder.Visible = set;
            buttonOpenFolderAuto.Visible = set;
        }

        private void SetNOAutoFoundVisual()
        {
            buttonOpenAutoFile.Visible = false;
            SetFolderVisibleVisual(false);
            labelAutoFound.Text = "файл НЕ найден.";
            buttonCreateAutoCustomFile.Visible = true;
            buttonOpenAutoFile.Text = "";
            buttonOpenFolderAuto.Text = "";
            pictureBoxAutoCustomFound.Image = Properties.Resources.noCross;            
        }

        private void SetYesAutoFoundVisual()
        {
            buttonOpenAutoFile.Visible = true;
            SetFolderVisibleVisual(true);
            labelAutoFound.Text = "найден:";
            pictureBoxAutoCustomFound.Image = Properties.Resources.yesCheck;
            buttonCreateAutoCustomFile.Visible = false;
        }

        private void radioButtonAutoFirstTest_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAutoFirstTest.Checked) radioButtonAutoCustom.Checked = false;
            VisualSetEnabledFromRadioBtnChecked();
        }

        private void radioButtonAutoCustom_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonAutoCustom.Checked) radioButtonAutoFirstTest.Checked = false;
            VisualSetEnabledFromRadioBtnChecked();
        }

        private void VisualSetEnabledFromRadioBtnChecked()
        {
            panelAutoCustomInside.Enabled = !radioButtonAutoFirstTest.Checked;
            pictureBoxAutoCustomFound.Enabled = !radioButtonAutoFirstTest.Checked;
            labelAutoFirst.Enabled = !radioButtonAutoCustom.Checked;
            buttonOpenFirstTest.Enabled = !radioButtonAutoCustom.Checked;            
        }

        private void YesBoxAuto_Click(object sender, EventArgs e)
        {

        }

        private void buttonOpenFirstTest_Click(object sender, EventArgs e)
        {
            var test = PublicData.GetTest(0);
            if (test != null)
            {
                Process.Start( (test).FilePath );
            }
            else
            { Ex.Show("Тест не является внешним.\n(или просто не найден)"); }
        }

        private void buttonOpenFolderAuto_Click(object sender, EventArgs e)
        {
            Ex.Try( ()=>Process.Start(pathAutoFolder), ex=>logger.Error(ex, ex.Message) );
        }

        private void labelAutoCustomDescrip_Click(object sender, EventArgs e)
        {
            radioButtonAutoCustom.Checked = true;
        }
        
        private void labelAutoFirst_Click(object sender, EventArgs e)
        {
            radioButtonAutoFirstTest.Checked = true;
        }

        private void buttonCreateAutoCustomFile_Click(object sender, EventArgs e)
        {
            Ex.Catch( ()=>
            {
                var fileEnd = Setting.FileEndOptions.Get(new SavingManager()).Value;
                var filePath = Path.Combine(PublicData.FolderConfigPath, $"autostart.{fileEnd}");
                //if ( File.Exists(filePath) ) throw new IOException($"Файл уже существует.\n{filePath}");
                var section = new IniFile(filePath, true).AddSection(PublicData.constAutoName);
                section.AddKey("code").SetValue(" ");
                section.AddKey("regex").SetValue(".+");
                section.IniFile.SaveShowMessage();
            });
            VisualRefresh();
        }

        private void OptionsForm1_FormClosing(object sender, FormClosingEventArgs e)
        {
            PublicData.isLetStartTest = true;
        }

        private void buttonOpenWBForm_Click(object sender, EventArgs e)
        {
            new WhiteBalanceChroma(false).Show();
        }

        private void buttonSyncConfigNetnLocal_Click(object sender, EventArgs e)
        {
            PublicData.UpdateFromNetToLocalConfig().RunParallel();
        }



        //Создание байтового массива из exe файла в ресурсах и создание из этого массива exe файл на ПК
        // cyberforum.ru/csharp-beginners/thread1869406.html
        // cyberforum.ru/csharp-beginners/thread339688.html
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {                
                byte[] b = Properties.Resources.COM_Registration_Tool;

                FileStream fs = new FileStream(file_exe, FileMode.Create);
                fs.Write(b, 0, b.Length);
                fs.Close();

                Process.Start(file_exe);

            }
            catch (Exception ex)
            {
                MessageBox.Show("EXE-файл 'COM_Registrations_Tool.exe' не сохранен \n" + ex, "WARNING!", MessageBoxButtons.OK);
            }
        }
 
    }
}
