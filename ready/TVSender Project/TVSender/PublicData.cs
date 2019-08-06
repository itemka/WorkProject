using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using CA200SRVRLib;
using System.Text.RegularExpressions;
using NLog;
using System.Threading.Tasks;

namespace TVSender
{
    public static class Extensions
    {
        private static readonly string n = Environment.NewLine;

        public static void RunParallel(this Task task) {}
        public static string trySubstring(this String inc, int startIndex, int length)
        {
            try
            {
                return inc.Substring( startIndex, length );
            }
            catch
            { return inc; }
        }
        public static string trySubstring(this String inc, int length)
        {
            return trySubstring(inc, 0, length);
        }
        public static string space(this String inc, int length=45)
        {
            //length=45 for textbox
            string result =  inc;
            Regex regex = new Regex($@"[^\s]{{{length},}}" );
            while( regex.IsMatch(result) )
            {
                Match matchReg = regex.Match(result);
                int sizeFound = matchReg.Length;
                var index = matchReg.Index;
                result = result.Insert(index + length - 1, " ");
            }
            return result;
        }
        public static string prefix(this String mainMsg, string prefixMsg, int countSeparators=1)
        {
            string separator = "";
            for (int i = 0; i < countSeparators; i++)
            {
                separator += n;
            }
            if (separator=="")
                separator = " ";
            return (string.IsNullOrEmpty(prefixMsg)) ? mainMsg : prefixMsg + separator + mainMsg;
        }
    }
    public static class PublicData
    {
        #region const
        public const string configFile = "config.ini";
        public const string configSection = "config";
        public const string configFolder = "#config";
        public const string testsFolder = "#tests";
        public const string imageFolder = "#image";
        public const string constAutoName = "Auto";
        public const string exeName = "TVSender";
        #endregion

        #region properties
        public static string CustomRootPath
        {
            get { return customRootPath; }
            set
            {                
                if (!Directory.Exists(value))
                {
                    customRootPath = value;
                    //customRootPath = Environment.CurrentDirectory;
                    string msg1 = $"Не найдена указанная директория:\n{value}\n\nПереключаюсь на локальную директорию:\n{RootPath}";
                    if (!String.IsNullOrEmpty(value))
                    { Ex.Show($"{Check.Message}\n\n{msg1}"); }
                    return;
                }
                else
                {
                    var lastFolder = GetLastFolder(value);
                    customRootPath = (lastFolder == configFolder || lastFolder == testsFolder) ? PathDeacreaseByFolder(value) : value;
                }
            }
        }
        public static string RootPath
        {
            get
            {
                if(isUseCustomRootPath)
                {
                    if (!string.IsNullOrEmpty(CustomRootPath))
                    {
                        if (Directory.Exists(CustomRootPath))
                        {
                            return CustomRootPath;
                        }
                    }
                }
                return Environment.CurrentDirectory;
            }
        }
        public static string FolderConfigPath
        {
            get { return folderPath; }
            set
            {
                if (value != null)
                    folderPath = value;
                if (value == null)
                    folderPath = Check.AddGetDirectory(RootPath, configFolder);
            }
        }
        public static HexString IDDataTV
        {
            get { return idDataTv; }
            set
            {
                idDataTv = (value == null) ? null : value.GetThisWithoutNull();
            }
        }
        public static string UniqOperationPath
        {
            get
            { return Path.Combine(String.Join("\\", DisplayModel), operation); }
        }
        #endregion

        #region Fields
        private static Logger logger = LogManager.GetCurrentClassLogger();
        //public delegate bool Func<bool>();
        public static Dictionary<string, Func<bool>> CreatedTests;
        public static bool isUseCustomRootPath;
        public static bool isAutoSyncConfig = true;
        private static string customRootPath;
        private static string folderPath;
        public static string LogsWBPath;
        public static string LogsCheckPassPath;

        private static HexString idDataTv;
        public static string model;
        public static string fileOperation;
        public static string operation;
        
        public static string[] DisplayModel = new string[0];                
        public static string pred_answer;        
        public static string port1_rate;
        public static string port2_rate;
        public static bool firstLaunch = true;
        public static bool cancelLaunch = true;
        public static bool isPort1;
        public static bool isPort2;
        public static string port1Name;
        public static string port2Name;
        public static string placeNumber = "999";
        public static bool FixOpenPort;
        public static bool isAutoStart { get; set; }
        public static bool isPressedAutoStart=false;
        public static bool isAutoFirstTest;
        public static bool brak;        
        public static bool isStopReader { get; set; } = true;
        public static bool isLetStartTest = true;
        public static bool isSkipFirstTestAuto = false;
        public static bool isConnectedMinolta = false;
        public static bool isConnectedChroma = false;
        public static ICas m_ICas;
        public static ICa m_ICa;
        public static IProbe m_IProbe;
        public static IMemory m_IMemory;
        public static bool uPowerStatus = false;
        public static ICa200 m_ICa200; //= (ICa200)new Ca200Class();
        public static List<Func<bool>> Functions;
        public static int timeAfterAutostart;
        public static int intervalAutostartCheck = 1000;
        #endregion

        #region Delegates
        public static Action<string, string> errormsg;
        public static Action<string> write_pc;
        public static Action<string> write_tv;
        public static Action<string> write_genr;
        public static Action<string> write_info;
        public static Action<TestFactory> SetFokusListView;
        public static Action<string> set_key;
        public static Action cancel_keyform;
        public static Action brak_blink;
        #endregion

        #region Methods
        public static async Task AutoUpdateFromNetToLocalConfig()
        {
            if (!PublicData.isAutoSyncConfig) return;
                var today = DateTime.Today;
                string month = today.Month < 10 ? $"0{today.Month}" : $"{today.Month}";
                string day = today.Day < 10 ? $"0{today.Day}" : $"{today.Day}";
                string datestring = $"{today.Year}{month}{day}";
                int todayDateValue = 0;
                try
                {
                    todayDateValue = int.Parse(datestring);
                }
                catch { return; }
                var sets = new SavingManager();
                int dateSaved = Setting.dateConfigUpdate.Get(sets).ValueInt;

                if (isUseCustomRootPath)
                {
                    if (todayDateValue > dateSaved)
                    {
                        if (Form1.CountSuccess > 9)
                        {
                            await UpdateFromNetToLocalConfig();
                            sets.Key(Setting.dateConfigUpdate).ValueInt = todayDateValue;
                            sets.Save();
                        }
                    }
                }
        }
        public static async Task UpdateFromNetToLocalConfig()
        {
            if (string.IsNullOrEmpty(PublicData.CustomRootPath)) return;
            if (!Directory.Exists(PublicData.CustomRootPath))
            {
                var msg = $"Указанный путь не существует: {PublicData.CustomRootPath}";
                Ex.Show(msg);
                PublicData.write_info(msg);
                return;
            }
            PublicData.write_info($"Началась синхронизация конфигов...");
            //copy from network to local
            string localRoot = Environment.CurrentDirectory;
            var fromCopy = Directory.EnumerateFiles(PublicData.CustomRootPath, "*", SearchOption.AllDirectories);
            await TaskEx.Run( () => Parallel.ForEach(fromCopy, tab =>
            {
                var where = tab.Replace(PublicData.CustomRootPath, localRoot);
                var dir = new FileInfo(where).DirectoryName;
                Ex.Try(() =>
                {
                    if (!Directory.Exists(dir))
                    { Directory.CreateDirectory(dir); }
                    File.Copy(tab, where, true);
                });
            }) );
            //delete local tests
            var pathLocalTests = Path.Combine(localRoot, testsFolder);
            var pathNetTests = Path.Combine(PublicData.CustomRootPath, testsFolder);
            var filesLocalTests = Directory.EnumerateFiles(pathLocalTests, "*", SearchOption.AllDirectories);
            var filesNetTests = Directory.EnumerateFiles(pathNetTests, "*", SearchOption.AllDirectories);
            var whereToFind = new List<string>(filesNetTests);
            await TaskEx.Run(() => Parallel.ForEach(filesLocalTests, localFile =>
            {
                var fileToFind = localFile.Replace(localRoot, PublicData.CustomRootPath);
                if (!whereToFind.Contains(fileToFind))
                {
                    Ex.Try(() => File.Delete(localFile));
                }
            }) );
            PublicData.write_info($"Синхронизация конфигов завершена.");
        }
        public static class Check
        {
            private static string msg = string.Empty;
            public static string Message { get { return msg; } }

            public static string Path(string path)
            {
                if (!Directory.Exists(path) )
                {
                    try { Directory.CreateDirectory(path); }
                    catch (Exception ex)
                    {
                        msg = ex.Info();
                        return null;
                    }
                }
                return path;
            }
            public static string AddGetDirectory(string path, string folder)
            {
                if (path == null) { msg = "path = null"; return null; }

                string pathReturn = path;
                bool isNeedToAdd = folder != GetLastFolder(path);
                if (isNeedToAdd)
                {
                    pathReturn = System.IO.Path.Combine(path, folder);
                }
                return Check.Path(pathReturn);
            }
        }
        public static string GetVersionProject()
        {
            var dateVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string version = Application.ProductVersion.ToString();
            DateTime buildDate = new DateTime(2000, 1, 1).AddDays(dateVersion.Build).AddSeconds(dateVersion.Revision * 2);
            string data = $"{buildDate:[dd/MM/yy]}";            
            string result = $" [{version}]  {data}";
            LogManager.Configuration.Variables["vers"] = $"{version} {data}";
            return result;
            //return string.Format("  v. {0}  {1}", buildDate.ToShortDateString(), buildDate.ToShortTimeString() );
        }
        public static Byte[] GetBytesFromHexString(string _Input)
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
                    Ex.Show(ex.Info());
                }
            }
            return bytArOutput;
        }
        public static string ByteString(string input)
        {
            string result = "";
            for (int i = 0; i < input.Length; i++)
                result += "0" + input[i];
            return result;
        }
        public static void pause(int n)
        {
            Thread.Sleep(n);
        }
        public static string PathFromPartsToWhole(string[] inc)
        {
            string result = "";
            foreach (string tab in inc)
                result += tab + "\\";

            result = result.TrimEnd('\\');

            return result;
        }
        public static string PathDeacreaseByFolder(string PathIncome)
        {
            if (string.IsNullOrEmpty(PathIncome) ) return PathIncome;
			int index = PathIncome.LastIndexOf('\\');
            if (index < 0) return PathIncome;

            string Return = PathIncome.Remove(index);
			return Return;
			//string[] PathParts = PathIncome.Split('\\');
			//    if (PathParts.Length > 0)
			//Array.Resize(ref PathParts, PathParts.Length - 1);
			//    else return PathIncome;
			//return PathFromPartsToWhole(PathParts);
        }
        public static IniFileList.IniSectionList GetAutostartCustomSection()
        {
            return FindSectionInTreeBySection(constAutoName, true);
        }
        public static IniFile.IniSection FindSectionInTreeBySection(string section)
		{
			var path = PublicData.FolderConfigPath;
			while (path.Contains(PublicData.RootPath) )
			{
				IniFile.IniSection found = FindSectionInFolderBySection(section, path);
				if (found != null) return found;
				path = PublicData.PathDeacreaseByFolder(path);
			}
			return null;
		}
        public static IniFileList.IniSectionList FindSectionInTreeBySection
            (string section, bool duplicate = false )
        {
            var path = PublicData.FolderConfigPath;
            while (path.Contains(PublicData.RootPath) )
            {
                IniFileList.IniSectionList found = FindSectionInFolderBySection(section, path, true);
                if (found != null) return found;
                path = PublicData.PathDeacreaseByFolder(path);
            }
            return null;
        }
		public static string FindFileInTreeBySection(string section)
		{
			var path = PublicData.FolderConfigPath;
			while (path.Contains(PublicData.RootPath) )
			{
				string found = FindFileInFolderBySection(section, path);
				if (found != null) return found;
				path = PublicData.PathDeacreaseByFolder(path);
			}
			return null;
		}
		public static string FindFileInTreeByName(string fileName)
		{
			var path = PublicData.FolderConfigPath;
			while (path.Contains(PublicData.RootPath) )
			{
				var found = FindFileInFolderByName(fileName, path);
				if (found != null) return found;
				path = PublicData.PathDeacreaseByFolder(path);
			}
			return null;
		}
		private static IniFile.IniSection FindSectionInFolderBySection(string section, string path = null)
		{
			string fileEnd = new SavingManager().Key(Setting.FileEndOptions).Value;
			if (path == null) path = PublicData.FolderConfigPath;

			FileInfo[] files = new DirectoryInfo(path).GetFiles("*" + fileEnd);
			foreach (var tabFile in files)
			{
				string filePath = Path.Combine(tabFile.DirectoryName, tabFile.Name);
				IniFile ini = new IniFile();
                Ex.Catch( () => ini.Load(filePath));
				var findSection = ini.GetSection(section);
				if (findSection != null)
					return findSection;
			}
			return null;
		}
        private static IniFileList.IniSectionList FindSectionInFolderBySection
            (string section, string path = null, bool duplicate = false)
        {
            string fileEnd = new SavingManager().Key(Setting.FileEndOptions).Value;
            if (path == null) path = PublicData.FolderConfigPath;

            FileInfo[] files = new DirectoryInfo(path).GetFiles("*" + fileEnd);
            foreach (var tabFile in files)
            {
                string filePath = Path.Combine(tabFile.DirectoryName, tabFile.Name);
                IniFileList ini = new IniFileList();
                ini.Load(filePath);
                var findSection = ini.GetSection(section);
                if (findSection != null)
                    return findSection;
            }
            return null;
        }
		private static string FindFileInFolderBySection(string section, string path = null)
        {
            string fileEnd = new SavingManager().Key(Setting.FileEndOptions).Value;
            if (path == null) path = PublicData.FolderConfigPath;

            FileInfo[] files = new DirectoryInfo(path).GetFiles("*" + fileEnd);
            foreach (var tabFile in files)
            {
                string filePath = Path.Combine(tabFile.DirectoryName, tabFile.Name);
                IniFile ini = new IniFile();
                ini.Load(filePath);
                var findSection = ini.GetSection(section);
                if (findSection != null)
                    return filePath;                
            }
            return null;
        }		        
		private static string FindFileInFolderByName(string fileName, string path = null)
        {
            string fileEnd = new SavingManager().Key(Setting.FileEndOptions).Value;
            if (path == null) path = PublicData.FolderConfigPath;

            FileInfo[] files = new DirectoryInfo(path).GetFiles(fileName);
            foreach (var tabFile in files)
            {
                return Path.Combine(tabFile.DirectoryName, tabFile.Name);                
            }
            return null;
        }
        public static string GetCheckFirstKeyFromSection(string filePath, string _section, string saveDefaultKey = null)
        {
            string keyReturn = null;

            IniFile ini = new IniFile(filePath);
            var sect = ini.GetSection(_section);
            LogUser Log = new LogUser();
            if (sect == null)
            {
                sect = ini.AddSection(_section);
                if (saveDefaultKey != null) sect.AddKey(saveDefaultKey);
                ini.SaveShowMessage();
            }
            if (sect.Keys.Count == 0 & saveDefaultKey != null)
            {
                    sect.AddKey(saveDefaultKey);
                    ini.SaveShowMessage();
            }
            Log.Out();
            SetFirstKeyFromSection(sect, ref keyReturn);
            return keyReturn;
        }
        public static void SetFirstKeyFromSection(IniFile.IniSection section, ref string keyReturn)
        {
            foreach (IniFile.IniSection.IniKey key in section.Keys)
            {
                keyReturn = key.Name; 
                break; 
            }
        }
        public static string GetDefaultConfigPath()
        {
            string configPath = Directory.GetCurrentDirectory();
            configPath = Path.Combine(configPath, configFolder);
            if (!(Directory.Exists(configPath) ))
            {
                Directory.CreateDirectory(configPath);
            }
            return configPath;
        }
        public static string ReadFromFileRootPath()
        {
            return PublicData.GetCheckFirstKeyFromSection(configFile, configSection);
        }
        public static string GetLastFolder(string PathIncome)
        {
            if (PathIncome != null)
            {
                string[] PathParts = PathIncome.Split('\\');
                if (PathParts.Length > 0)
                    return PathParts[PathParts.Length - 1].Trim('\\', '/');
                else return PathIncome;
            }
            else { return null; }
        }
        public static TestFactory GetTest(int index)
        {
            #region checks
            if (Functions == null)
            { throw new NullReferenceException("Functions == null"); }
            if(index >= Functions.Count)
            {
                var ex = new IndexOutOfRangeException
                    ($"Запрашиваемый индекс(={index}) превышает количество={Functions.Count} Functions.");
                ex.Data.Add("indexToGet", index);
                ex.Data.Add("totalCount", Functions.Count);
                throw ex;
            }
            #endregion
            var objectClass = Functions[index]?.Target;
            if (objectClass != null)
            {
                if (objectClass.GetType() == typeof(TestFactory) )
                {
                    return (TestFactory)objectClass;
                }
                logger.Trace($"тип теста = {objectClass.GetType()}");
            }
            return null;
        }        
        public static void ShowMessage(Exception ex, string msg = null, bool logTo=true)
        {
            if (logTo)
            {
                LogUser login = new LogUser();
                LogException(msg + ex.ToString() );
                login.Out();
            }
            ex.Show(msg);
        }
        private static void LogException(string incMsg, bool local=false)
        {            
            string fileName = DateTime.Now.ToString("yy.MM.dd   HH-mm-ss #FFF") + ".txt";
            var folderPath = local ? 
                Environment.CurrentDirectory 
                : PathDeacreaseByFolder(LogsCheckPassPath);
            folderPath = Path.Combine(folderPath, "ExceptionLogs");
            if(!Directory.Exists(folderPath) )
            {
                try { Directory.CreateDirectory(folderPath); }
                catch
                {
                    if (!local) LogException(incMsg, true);
                    return;
                }
            }
            string fullPath = Path.Combine(folderPath, fileName);
            try
            { File.WriteAllText(fullPath, incMsg); }
            catch
            { if (!local) LogException(incMsg, true); }
        }
        private static void LogException(Exception ex)
        {
            LogException(ex.ToString() );
        }
        #endregion

        #region White Balance
        public static void Disconnect_CA210()
        {
            try
            {
                PublicData.m_ICa.RemoteMode = 0;
            }
            catch { }
        }
        public static void Connect_Minolta()
        {
            try
            {
                m_ICa.Measure(1);
            }
            catch (NullReferenceException)
            {
                Connect_Minolta_0();
                return;
            }
            catch (Exception)
            {
                try
                {
                    m_ICa.RemoteMode = 1;
                }
                catch (Exception)
                {
                    Connect_Minolta_0();
                }
            }
            isConnectedMinolta = true;
        }
        private static void Connect_Minolta_0()
        {
            if (PublicData.m_ICa200 == null)
            {
                MessageBox.Show("Не установлен драйвер для Minolta CA-210.\nБаланс белого не будет доступен.",
                    "Driver Minolta CA210 not found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isConnectedMinolta = false;
                return;
            }
            PublicData.write_info("Подключение к Minolta. Ожидайте...");
            try
            {
                m_ICa200.AutoConnect();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error! Check USB connection please.\n" + ex.Info(),
                    "Can't connect USB CA210", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //this.buttonConnectUSB.Enabled = true;
                isConnectedMinolta = false;
                return;
            }
            Thread.Sleep(50);
            uPowerStatus = true;
            isConnectedMinolta = true;
            m_ICas = (ICas)m_ICa200.Cas;
            m_ICa = (ICa)m_ICas.get_ItemOfNumber(1);
            m_IProbe = (IProbe)m_ICa.SingleProbe;
            m_IMemory = (IMemory)m_ICa.Memory;
            Thread.Sleep(50);
            loopInit_CA210();
            PublicData.write_info("Подключение Minolta завершенно.");
        }
        private static void Init_CA210()
        {
            m_IMemory.ChannelNO = 90;
            Thread.Sleep(50);
            m_ICa.SetAnalogRange(2.5f, 2.5f);
            if (m_ICa.DisplayMode != 0)
                m_ICa.DisplayMode = 0;
            m_IMemory.SetChannelID(" ");
            if (m_IMemory.ChannelID != "WB AutoAdj")
                m_IMemory.SetChannelID("WB AutoAdj");
            m_ICa.Measure(1);
        }
        private static void loopInit_CA210()
        {
            try
            {
                Init_CA210();
            }
            catch (Exception ex)
            {
                string error1 = "SDK Command Error\n--measurement fail\n--check probe/display_setting";
                if (ex.Message.Contains(error1))
                {
                    CalibrateZero();
                    Thread.Sleep(50);
                    loopInit_CA210();
                    return;
                }
                m_ICa.RemoteMode = 0;
                isConnectedMinolta = false;
                MessageBox.Show("Error! Try again.", "Can't connect USB CA210",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private static void CalibrateZero(string mes = "")
        {
            MessageBox.Show("Переключите пробник в режим \"0-CAL\"\n\n" + mes,
                (mes == "") ? "Нужно провести калибровку" : mes,
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            try
            {
                m_ICa.CalZero();
                MessageBox.Show("Переключите пробник в режим \"MEAS\"", "Успешно",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception exCal)
            {
                string error2 = "CA Command Error\n--too bright\n--block light";
                if (exCal.Message == error2)
                {
                    CalibrateZero("Слишком ярко!");
                }
            }
        }
        #endregion
    }

}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class
         | AttributeTargets.Method)]
    public sealed class ExtensionAttribute : Attribute { }
}

