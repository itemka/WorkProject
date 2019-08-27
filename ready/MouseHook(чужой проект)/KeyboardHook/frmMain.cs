using System;
using System.Windows.Forms;
using Hooks;

namespace KeyboardHook
{
    public partial class frmMain : Form
    {
        bool ctrl = false;


        public frmMain()
        {
            InitializeComponent();
            this.FormClosed += new FormClosedEventHandler(frmMain_FormClosed);
            MouseHook.MouseDown += new MouseEventHandler(MouseHook_MouseDown);
            MouseHook.MouseMove += new MouseEventHandler(MouseHook_MouseMove);
            MouseHook.MouseUp += new MouseEventHandler(MouseHook_MouseUp);
            MouseHook.LocalHook = false;
            listBox1.Items.Clear();
            MouseHook.InstallHook();
            label1.Text = string.Format("Installed:{0}\r\nModule:{1}\r\nLocal{2}",
                MouseHook.IsHookInstalled, MouseHook.ModuleHandle, MouseHook.LocalHook);
        }

        void MouseHook_MouseMove ( object sender, MouseEventArgs e ) {
            listBox1.Items.Add(e.Location);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        void MouseHook_MouseUp ( object sender, MouseEventArgs e ) {
            listBox1.Items.Add(e.Button);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        void MouseHook_MouseDown ( object sender, MouseEventArgs e ) {
            listBox1.Items.Add(e.Button);
            listBox1.SelectedIndex = listBox1.Items.Count - 1;
        }

        private void frmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            MouseHook.UnInstallHook(); // Обязательно !!!
        }
    }
}
