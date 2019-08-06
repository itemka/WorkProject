using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using TVSender.Properties;
using System.IO;

namespace TVSender
{
    public partial class OptionsSelectForm : Form
    {
        FormNewChoice mainForm;

        public OptionsSelectForm(FormNewChoice _mainForm)
        {
            mainForm = _mainForm;
            InitializeComponent();
        }

        private void SelectOptionForm_Load(object sender, EventArgs e)
        {
            SetTooltips();
            LoadSettings();
        }
        private void SetTooltips()
        {
            ToolTip t = new ToolTip();
            string dir1 = string.Format("\"{0}\" - содержащая конфигурации исполнения тестов", PublicData.configFolder);
            string dir2 = string.Format("\"{0}\" - содержащая создаваемые тесты", PublicData.testsFolder);
            string dir3 = string.Format("\"{0}\" - содержащая изображения, использующиеся в создании тестов", PublicData.imageFolder);
            string confToolTip = string.Format("Расположение сервер-папки, в которой будут содержаться папки:\n{0}\n{1}\n{2}", dir1, dir2, dir3);
            t.SetToolTip(lblConfigPath, confToolTip);
            t.SetToolTip(this.txtConfigPath, confToolTip);
        }

        private void Save()
        {
            if (numericUpDown1.Text != "")
            {
                int size = Convert.ToInt32(numericUpDown1.Text);
                mainForm.changeFontListView(size);

                var set = new SavingManager();
                set.Key(Setting.FontListView).ValueInt = size;
                set.Key(Setting.FileEndProg).Value = "." + textBoxFileEnd.Text.Trim(' ','.','"');
                set.Save();
            }

            IniFile ini = new IniFile(PublicData.configFile);
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
        }

        private void LoadSettings()
        {
            numericUpDown1.Text = new SavingManager().Key(Setting.FontListView).Value;
            textBoxFileEnd.Text = new SavingManager().Key(Setting.FileEndProg).Value;
            txtConfigPath.Text = PublicData.ReadFromFileRootPath();
        }        

        private void buttonAccept_Click(object sender, EventArgs e)
        {
            AcceptChanges();
        }

        private void buttonOKClose_Click(object sender, EventArgs e)
        {
            AcceptChanges();
            this.Close();
        }

        private void AcceptChanges()
        {
            Save();
            UpdateForms();
        }

        private void UpdateForms()
        {

            string configPath = PublicData.Check.AddGetDirectory(PublicData.RootPath, PublicData.configFolder);
            
            mainForm.UpdateFromPath(configPath);
            mainForm.LabelDeleteSince();
            mainForm.GetItemsListView();            
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
    }
}
