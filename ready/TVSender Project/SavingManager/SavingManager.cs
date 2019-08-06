//  Last Check Data: 2017.12.04
//  Change Data: 2017.12.04

using System;
using System.Collections.Generic;
using System.IO;

public enum Setting
{
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
    Green128Below
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
    private Dictionary<Setting, string> Default;
    private bool isLocal = false;
    #endregion

    #region Constructor
    public SavingManager() //user
    {
        isLocal = false;
        DefaultSettings();
        section = GetSettingsSection();
    }
    public SavingManager(Where type) //local
    {
        isLocal = (type == Where.local) ? true : false;
        DefaultSettings();
        section = GetSettingsSection();
    }
    public SavingManager(string _fileName) //local
    {
        isLocal = true;
        DefaultSettings();
        fileName = _fileName;
        string sectionName = _fileName.Remove(_fileName.LastIndexOf('.'));
        section = GetSettingsSection(sectionName);
    }
    public SavingManager(IniFile.IniSection incSection) : this(Where.local) //local
    {
        if (incSection == null)
        {
            return;
        }
        section = incSection;
    }
    public SavingManager(Where type, string incFullPath) //custom
    {
        if (type != Where.custom)
        {
            throw new ArgumentException("type is not Custom");
        }
        DefaultSettings();
        section = new IniFile(incFullPath).AddSection("WBSettings");
    }
    #endregion

    #region Public Methods
    public void Save()
    {
        var iniFile = section.GetParentIniFile();
        iniFile.Save();
    }
    public sKey Key(Setting inc_key)
    {
        IniFile.IniSection.IniKey key = section.GetKey(inc_key.ToString());
        if (key == null)
        {
            if (Default.ContainsKey(inc_key))
            {
                key = section.AddKey(inc_key.ToString());
                key.Value = Default[inc_key];
            }
        }
        if (key != null)
        {
            if (key.Value == null)
            {
                if (Default.ContainsKey(inc_key))
                {
                    key.Value = Default[inc_key];
                }
            }
        }
        return new sKey(key);
    }
    public class sKey
    {
        IniFile.IniSection.IniKey key;
        bool isNull;
        public string Value
        {
            get { return this.key.Value; }
            set { this.key.Value = value; }
        }
        protected internal sKey(IniFile.IniSection.IniKey inc_key)
        {
            key = inc_key;
        }
        public int ValueInt
        {
            set { key.Value = value.ToString(); }
            get
            {
                int intValue;
                try
                {
                    intValue = Int32.Parse(Value);
                }
                catch { isNull = true; return 0; }
                return intValue;
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
                catch { isNull = true; return 0; }
                return doubleValue;
            }
        }
        public bool ValueBool
        {
            get { return Value == "true"; }
            set { this.key.Value = value ? "true" : "false"; }
        }
        public bool noNull
        {
            get { return (Value != null & !isNull); }
        }
    }
    #endregion

    #region Private Methods
    private void DefaultSettings()
    {
        Default = new Dictionary<Setting, string>();
        Default.Add(Setting.RootPathForReports, "");
        Default.Add(Setting.LogsWBPath, "");
        Default.Add(Setting.LogsCheckPassPath, "");
        Default.Add(Setting.CoolX, "0.270");
        Default.Add(Setting.CoolY, "0.270");
        Default.Add(Setting.WarmX, "0.300");
        Default.Add(Setting.WarmY, "0.305");
        Default.Add(Setting.StandartX, "0.280");
        Default.Add(Setting.StandartY, "0.290");
        Default.Add(Setting.Green128Below, "true");
        Default.Add(Setting.ToleranceXY, "0.01");
        Default.Add(Setting.Average, "false");
    }
    private IniFile.IniSection GetSettingsSection(string nameSection = "settings")
    {
        IniFile ini = new IniFile(GetPath());
        var section = AddGetSection(ini, nameSection);
        return section;
    }
    private string GetPath()
    {
        string path;
        if (!isLocal) //NOT local
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            path = AddGetDirectory(path, folderName);
            path = AddGetFile(path, fileName);
        }
        else //is Local
        {
            path = Environment.CurrentDirectory;
            path = AddGetFile(path, fileName);
        }
        return path;
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
    private string AddGetFile(string path, string name)
    {
        path = Path.Combine(path, name);
        if (!System.IO.File.Exists(path))
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
    private string AddGetDirectory(string path, string name)
    {
        path = Path.Combine(path, name);
        if (!System.IO.File.Exists(path))
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

