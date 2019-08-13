using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace FS
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Form2());
            Start();
        }
        public static void Start()
        {
            Form2 FS = new Form2();
            FS.Show();
            FS.Hide();
            while (true) {
                FS.Show();
                Thread.Sleep(2000);
                FS.Hide();
                Thread.Sleep(2000);
            }
        }

    }
}
