﻿using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace KartoffelReminder
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GUI2());
        }
    }
}
