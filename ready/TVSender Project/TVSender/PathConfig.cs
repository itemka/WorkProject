//  Last Check Data: 16.09.14
//  Change Data: 16.09.14

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using TVSender.Properties;

namespace TVSender
{
    public class PathConfig
    {
        const string configFolder = "config";
        string FullPathCurrent;
        string rootPath;
        string DisplayPath;        
        string[] DisplayPathParts;// = new string[0];
        int labelStartLocationX = 10;
        int labelStepLocationX = 20;
        string nameCurrent = "";
        int indexCurrentSize = -1;
        string itemToSelect = null;

        public int LabelStartLocationX { get { return labelStartLocationX; } }
        public int LabelStepLocationX { get { return labelStepLocationX; } }
        public string NameCurrent { get { return nameCurrent; } }
        public int Index { get { return indexCurrentSize; } }
        public string[] displayPathParts { get { return DisplayPathParts; } }
        public string FullPath { get { return FullPathCurrent; } }
        public string ItemToSelect { get { return itemToSelect; } }
        public string RootPath { get { return rootPath; } }

        public PathConfig(string _path = null)
        {
            DisplayPath = "";
            DisplayPathParts = new string[0];
            SetDefaultPath();            

            if (_path != null)
            {
                if (!(Directory.Exists(_path) ))
                {
                    try
                    {
                        Directory.CreateDirectory(_path);
                    }
                    catch (Exception ex)
                    {
                        string msg1 = string.Format("Будет используется стандартный путь:\n\n{0}", rootPath);
                        Ex.Show(string.Format("{0}\n\n{1}", ex.Message, msg1) );
                        return;
                    }
                }
                FullPathCurrent = _path;
                rootPath = _path;
            }            
        }

        private void SetDefaultPath()
        {
            rootPath = Directory.GetCurrentDirectory();
            //rootPath = Path.Combine(rootPath, configFolder);
            rootPath = Path.Combine(rootPath, PublicData.configFolder);
            FullPathCurrent = rootPath;
            if (!(Directory.Exists(rootPath) ))
            {
                Directory.CreateDirectory(rootPath);
            }            
        }
    
        public void EnterNextFolder(string inc)
        {
            nameCurrent = inc;
            FullPathCurrent = Path.Combine(FullPathCurrent, inc);
            DisplayPath = FullPathCurrent.Replace(rootPath, "").Trim('\\');
            DisplayPathParts = DisplayPath.Split('\\');
            indexCurrentSize = DisplayPathParts.Length - 1;
        }

        public void EnterFullPath(string incFullPath)
        {
            if (incFullPath.Contains(rootPath) )
            {
                itemToSelect = null;
                FullPathCurrent = (Directory.Exists(incFullPath) ) ? incFullPath : LastDelete(incFullPath);
                //FullPath = incFullPath;            

                DisplayPath = FullPathCurrent.Replace(rootPath.Trim('\\'), "");
                if (DisplayPath == "") DisplayPathParts = new string[0];
                else
                    DisplayPathParts = DisplayPath.Split(new string[]{@"\"}, StringSplitOptions.RemoveEmptyEntries);
                indexCurrentSize = DisplayPathParts.Length - 1;
                if (indexCurrentSize < 0) nameCurrent = "";
                else
                    nameCurrent = DisplayPathParts[indexCurrentSize];
            }
        }    

        private string LastDelete(string incFullPath)
        {
			int index = incFullPath.LastIndexOf('\\');
			string Return = incFullPath.Remove(index);
			return Return;

			//string fileEnd = Settings.Default.FileEndProg;
			//string[] FullPathParts = incFullPath.TrimEnd('\\').Split('\\');
			//itemToSelect = FullPathParts[FullPathParts.Length - 1];
			//itemToSelect = (itemToSelect.EndsWith(fileEnd) ) ? itemToSelect.Replace(fileEnd, "") : itemToSelect;
			//Array.Resize(ref FullPathParts, FullPathParts.Length - 1);
			//return PathFromPartsToWhole(FullPathParts); 
        }

        public void BackToTarget(int index)
        {
            if (index < 0) return;
            Array.Resize(ref DisplayPathParts, index);
            indexCurrentSize = index - 1;
            if (indexCurrentSize < 0) nameCurrent = "";
            else nameCurrent = DisplayPathParts[indexCurrentSize];

            DisplayPath = PublicData.PathFromPartsToWhole(DisplayPathParts);
            FullPathCurrent = Path.Combine(rootPath, DisplayPath);
        }

        public List<string> GetFolders()
        {
            var arr = new List<string>();
            if (Directory.Exists(FullPathCurrent) )
            {
                DirectoryInfo[] folders = new DirectoryInfo(FullPathCurrent).GetDirectories();
                foreach (var tab in folders)
                {
                    arr.Add(tab.Name);
                }
            }
            return arr;
        }

        public List<string> GetFilesNames()
        {
            string fileEnd = new SavingManager().Key(Setting.FileEndProg).Value;
            var array = new List<string>();
            if (Directory.Exists(FullPathCurrent) )
            {
                FileInfo[] files = new DirectoryInfo(FullPathCurrent).GetFiles("*" + fileEnd);
                foreach (var tab in files)
                {
                    //if (tab.Name.EndsWith(fileEnd) ) 
                    array.Add(tab.Name.Replace(fileEnd, "") );
                }
            }
            return array;
        }

        public string GetFilePath(string incName)
        {
            string fileFullPath = Path.Combine(FullPathCurrent, incName + new SavingManager().Key(Setting.FileEndProg).Value);
            if (!File.Exists(fileFullPath) ) return null;
            return fileFullPath;
        }

		public static string PathFromPartsToWhole(string[] inc)
		  {
			  string result = "";
			  foreach (string tab in inc)
				  result += tab + "\\";

			  result = result.TrimEnd('\\');

			  return result;
		  }
    }
}
