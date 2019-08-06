using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace TVSender
{
    public partial class SelectForm : Form //эта форма уже не используется. заменена на FormNewChoice
    {
        public SelectForm()
        {
            InitializeComponent();
            string filename = Application.ProductVersion.ToString();
            this.Text += "   v." + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString() + " (" + filename.Substring(filename.Length - 5)+ ")";
            //System.Reflection.Assembly.GetExecutingAssembly().GetName().
        }        

        private void SelectForm_Load(object sender, EventArgs e)
        {            
            IniFile ini = new IniFile();
            if (!System.IO.File.Exists("config.ini"))
            {
                MessageBox.Show("Не найден файл config.ini.\nСоздан новый.");
                System.IO.File.CreateText("config.ini");                
            }
            ini.Load("config.ini");
            if (ini.GetSection("models") == null)
            {
                label3.Text = "Не найден раздел [models] в файле config.ini";
                ini.AddSection("models");                
            }
            var models = ini.GetSection("models");
            listBox1.Items.Clear();
            
            foreach (IniFile.IniSection.IniKey k in models.Keys)
            {
                listBox1.Items.Add((string.Format(" {0}", k.Name)));                
            }
            if (ini.GetSection("operations") == null)
            {
                label3.Text = "Не найден раздел [operations] в файле config.ini"; 
                ini.AddSection("operations");                
            }
            var operations = ini.GetSection("operations");
            listBox2.Items.Clear();
            foreach (IniFile.IniSection.IniKey k in operations.Keys)
            {
                listBox2.Items.Add((string.Format(" {0}", k.Name)));
            }
            //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "ProjectName");
            if (models.Keys.Count > 0) listBox1.SetSelected(0, true);
            if (operations.Keys.Count > 0) listBox2.SetSelected(0, true);

            var last = new IniFile();
            if (!System.IO.File.Exists("last.ini"))
            {
                System.IO.File.CreateText("last.ini");                
            }
            try { last.Load("last.ini"); }
            catch {}

            if (last.GetSection("main") == null) last.AddSection("main");
            
            var model = last.GetSection("main").GetKey("model");
            if (model == null)
            {
                last.GetSection("main").AddKey("model");
                model = last.GetSection("main").GetKey("model");
            }

            var operation = last.GetSection("main").GetKey("operation");
            if (operation == null)
            {
                last.GetSection("main").AddKey("operation");
                operation = last.GetSection("main").GetKey("operation");
            }
            if (model.Value != null)
            {
                int index = listBox1.FindStringExact(" " + model.Value);                
                if (index != ListBox.NoMatches) listBox1.SetSelected(index, true);
            }
            if (operation.Value != null)
            {
                int index = listBox2.FindStringExact(" " + operation.Value);
                if (index != ListBox.NoMatches) listBox2.SetSelected(index, true);
            }
            //listBox1.SelectedItem = 3;
            this.KeyPreview = true;
            //KeyDown += new KeyEventHandler(SelectForm_KeyDown);
        }

        private void save()
        {
            try
            {
                PublicData.model = listBox1.SelectedItem.ToString().Trim();                     
            }
            catch
            {
                label3.Text = "Не выбрана модель!";
                //MessageBox.Show("Не выбрана модель!");
                return;
            }

            try
            {                
                PublicData.operation = listBox2.SelectedItem.ToString().Trim();
            }
            catch
            {
                label3.Text = "Не выбрана операция";
                //MessageBox.Show("Не выбрана модель!");
                return;
            }

            PublicData.firstLaunch = false;

            var last = new IniFile();
            last.Load("last.ini");
            last.SetKeyValue("main", "model", PublicData.model);
            last.SetKeyValue("main", "operation", PublicData.operation);
            last.Save("last.ini");

            Close();
        }
        /*
        protected override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: listBox1.Select(); break;
                case Keys.Right: listBox2.Select(); break;                
            }
        }
        */
        private void button1_Click(object sender, EventArgs e)
        {
            save();                     
        }
        
        private void SelectForm_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left: 
                    listBox1.Select();
                    e.Handled = true;
                    break;
                case Keys.Right: 
                    listBox2.Select();
                    e.Handled = true;
                    break;
                case Keys.Enter: save(); break;
                case Keys.Escape: Close(); break;
            }
        }
        
        private void SelectForm_Shown(object sender, EventArgs e)
        {
            SendKeys.SendWait("{TAB}");
        }
        
        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Up)
                if (listBox1.SelectedIndex == 0)
                {
                    listBox1.SetSelected(listBox1.Items.Count-1, true);                    
                    e.Handled = true;
                }
            
            if (e.KeyCode == Keys.Down)
                if (listBox1.SelectedIndex == listBox1.Items.Count-1)
                {
                    listBox1.SetSelected(0, true);
                    e.Handled = true;
                }   
        }

        private void listBox2_KeyDown(object sender, KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (e.KeyCode == Keys.Up)
                if (listBox2.SelectedIndex == 0)
                {
                    listBox2.SetSelected(listBox2.Items.Count - 1, true);
                    e.Handled = true;
                }

            if (e.KeyCode == Keys.Down)
                if (listBox2.SelectedIndex == listBox2.Items.Count - 1)
                {
                    listBox2.SetSelected(0, true);
                    e.Handled = true;
                }   
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }
    }
}
