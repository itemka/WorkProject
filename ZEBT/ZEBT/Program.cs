using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ZEBT
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>

        // Словарь для пользователей
        public static Dictionary<string, string> UpdataUsers = new Dictionary<string, string>();

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
