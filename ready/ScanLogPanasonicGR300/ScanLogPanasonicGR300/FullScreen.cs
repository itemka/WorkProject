using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ScanLogPanasonicGR300
{
    public partial class FullScreen : Form
    {
        public FullScreen(string text, Color color)
        {
            InitializeComponent();
            label1.Text = text;
            BackColor = color;
        }
    }
}