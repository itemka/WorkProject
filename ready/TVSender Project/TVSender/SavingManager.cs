//  Last Check Data: 2017.12.04
//  Change Data: 2017.12.04

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

public enum Setting
{
    isAutoSyncConfig,
    dateConfigUpdate,
    isUseCustomRootPath,
    isAutonomMode,
    MaxRGBValue,
    LimitRGB,
    AverageNumber,
    MinusMaxLv,
    MinimumLvCool,
    MinimumLvStandart,
    MinimumLvWarm,
    timeAfterAutostart,
    isAutoStart,
    isAutoFirstTest,
    LogsWBPath,
    LogsCheckPassPath,
    RootPathForReports,
    CoolX,
    CoolY,
    WarmX,
    WarmY,
    StandartX,
    StandartY,
    ToleranceXY,
    Average,
    Green128Below,
    FileEndOptions,
    LastTVSelection,
    FileEndProg,
    FontListView,
    Split4,
    Split3,
    Split1,
    FormLocationX,
    FormLocationY,
    FormSizeX,
    FormSizeY
}

public enum Where
{
    user,
    local,
    custom
}

public class SavingManager
{
    #region Fields
    private const string folderName = "TVSender";
    private const string defaultfileName = "settings.ini";
    private string fileName = defaultfileName;

    private IniFile.IniSection section;    
    private bool isLocal = false;
    #endregion
    #region default settings
    private static Dictionary<Setting, string> defaultSets = new Dictionary<Setting, string>
        {
        [Setting.isAutoSyncConfig] = "true",
        [Setting.dateConfigUpdate] = "0",
        [Setting.isUseCustomRootPath] = "true",
        [Setting.isAutonomMode] = "false",
        [Setting.MaxRGBValue] = "160",
        [Setting.LimitRGB] = "false",
        [Setting.AverageNumber] = "5",
        [Setting.MinimumLvCool] = "65",
        [Setting.MinimumLvStandart] = "65",
        [Setting.MinimumLvWarm] = "60",
        [Setting.MinusMaxLv] = "0",
        [Setting.timeAfterAutostart] = "10",
        [Setting.isAutoFirstTest] = "false",
        [Setting.isAutoStart] = "false",
        [Setting.RootPathForReports] = "",
        [Setting.LogsWBPath] = "",
        [Setting.LogsCheckPassPath] = "",
        [Setting.CoolX] = "0.270",
        [Setting.CoolY] = "0.270",
        [Setting.WarmX] = "0.300",
        [Setting.WarmY] = "0.305",
        [Setting.StandartX] = "0.280",
        [Setting.StandartY] = "0.290",
        [Setting.Green128Below] = "true",
        [Setting.ToleranceXY] = "0.01",
        [Setting.Average] = "false",
        [Setting.FileEndOptions] = ".opt",
        [Setting.LastTVSelection] = "",
        [Setting.FileEndProg] = ".hz",
        [Setting.FontListView] = "12",
        [Setting.Split4] = "65",
        [Setting.Split3] = "25",
        [Setting.Split1] = "125",
        [Setting.FormLocationX] = "10",
        [Setting.FormLocationY] = "100",
        [Setting.FormSizeX] = "265",
        [Setting.FormSizeY] = "383"
        };
    #endregion
    #region Constructor
    public SavingManager() //user
    {
        isLocal = false;
        section = GetSettingsSection();
    }
    public SavingManager(Where type) //local
    {
        isLocal = (type == Where.local) ? true : false;
        section = GetSettingsSection();
    }
    public SavingManager(string _fileName) //local
    {
        isLocal = true;
        fileName = _fileName;
        string sectionName = _fileName.Remove(_fileName.LastIndexOf('.') );
        section = GetSettingsSection(sectionName);
    }
    public SavingManager(IniFile.IniSection incSection) //custom
    {
        if (incSection == null)
        {
            return;
        }		  
        section = incSection;
    }
    public SavingManager(Where type, string incFullPath, string NameSection="settings") //user+path //custom
    {
        if (type == Where.local)
        {
            throw new ArgumentException("Type cant be Local (with custom path)");
        }
        isLocal = false;
        if (type == Where.custom)
        {            
            section = new IniFile(incFullPath).AddSection(NameSection);
        }
        if (type == Where.user)
        {
            string locPath = Path.Combine(GetDefaultPathDirectory(), incFullPath);
            AddGetDirectory(locPath, null);
            locPath = AddGetFile(locPath);
            section = new IniFile(locPath).AddSection(NameSection);
        }
    }
    #endregion

    #region Public Methods
    public void Save()
    {
        var iniFile = section.GetParentIniFile();
        iniFile.SaveShowMessage();
    }
    public sKey Key(Setting incKey)
    {
        IniFile.IniSection.IniKey key = section.GetKey(incKey.ToString() );
        if (key == null)
        {
            if (defaultSets.ContainsKey(incKey) )
            {
                key = section.AddKey(incKey.ToString() );
                key.Value = defaultSets[incKey];
            }
        }
        if (key != null)
        {
            if (key.Value == null)
            {
                if (defaultSets.ContainsKey(incKey) )
                {
                    key.Value = defaultSets[incKey];
                }
            }
        }
        return new sKey(key, defaultSets[incKey]);
    }
    public class sKey
    {
        IniFile.IniSection.IniKey key;
        bool isNull;
        string defaultValue;
        public string Value
        {
            get { return this.key.Value; }
            set { this.key.Value = value; }
        }
        //protected internal sKey(IniFile.IniSection.IniKey inc_key)
        //{
        //    key = inc_key;
        //}
        protected internal sKey(IniFile.IniSection.IniKey inc_key, string _defaultValue)
        {
            key = inc_key;
            defaultValue = _defaultValue;
        }
        public int ValueInt
        {
            set { key.Value = value.ToString(); }
            get
            {
                int parsValue;
                try
                {
                    parsValue = Int32.Parse(Value);
                }
                catch { isNull = true; return Int32.Parse(defaultValue); }
                return parsValue;
            }
        }
        public byte ValueByte
        {
            set { key.Value = value.ToString(); }
            get
            {
                Byte parsValue;
                try
                {
                    parsValue = Byte.Parse(Value);
                } catch { isNull = true; return Byte.Parse(defaultValue); }
                return parsValue;
            }
        }
        public double ValueDouble
        {
            set { key.Value = value.ToString(); }
            get
            {
                double doubleValue;
                try
                {
                    doubleValue = Double.Parse(Value);
                }
                catch { isNull = true; return Double.Parse(defaultValue); }
                return doubleValue;
            }
        }
        public float ValueFloat
        {
            set { key.Value = value.ToString(); }
            get
            {
                float floatValue;
                try
                {
                    floatValue = float.Parse(Value);
                } catch { isNull = true; return float.Parse(defaultValue); }
                return floatValue;
            }
        }
        public bool ValueBool
        {
            set { this.key.Value = value ? "true" : "false"; }
            get
            {
                //если указанно не осмысленное значение, тогда вернуть дефолт
                if (Value != "true" && Value != "false" && Value != "1" && Value != "0")
                { return defaultValue == "true" || defaultValue == "1"; }
                return Value == "true" || Value == "1";
            }
        }
        public bool notNull
        {
            get { return (Value != null & !isNull); }
        }
    }
    #endregion

    #region Private Methods
    private IniFile.IniSection GetSettingsSection(string nameSection = "settings")
    {
        IniFile ini = new IniFile(GetDefaultPathFile() );
        var section = AddGetSection(ini, nameSection);
        return section;
    }
    private string GetDefaultPathDirectory()
    {
        string path;
        if (!isLocal) //NOT local = user
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            path = AddGetDirectory(path, folderName);
        }
        else //is Local
        {
            path = Environment.CurrentDirectory;
        }
        return path;
    }
    private string GetDefaultPathFile()
    {
        string directory = GetDefaultPathDirectory();
        return AddGetFile(directory, fileName);
    }
    private IniFile.IniSection AddGetSection(IniFile ini, string name)
    {
        IniFile.IniSection section = ini.GetSection(name);
        if (section == null)
        {
            section = ini.AddSection(name);
            //ini.Save(path);
        }
        return section;
    }
    private string AddGetFile(string path, string name = defaultfileName)
    {
        path = Path.Combine(path, name);
        if (!System.IO.File.Exists(path) )
        {
            try
            {
                System.IO.File.CreateText(path).Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return path;
    }
    private string AddGetDirectory(string path, string name=null)
    {
        if (name != null) path = Path.Combine(path, name);
        if (!File.Exists(path) )
        {
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        return path;
    }
    #endregion
}

public static class ExtensionSetting
{
    public static SavingManager.sKey Get(this Setting set, SavingManager obj)
    {
        return obj.Key(set);
    }
}

