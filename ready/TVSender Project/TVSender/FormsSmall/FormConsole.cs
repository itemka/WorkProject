using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace SoundCatcher //используется для осциллографа (а может и не используется. ни для чего вообще тогда)
{
    public partial class FormConsole : Form
    {
        string text;
        public FormConsole(string _text)
        {
            InitializeComponent();
            text = _text;
        }

        private void FormConsole_Load(object sender, EventArgs e)
        {
            textBoxConsole.AppendText(text);
        }
    }
}
