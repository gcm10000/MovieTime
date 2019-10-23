using System;
using System.IO;
using System.Windows.Forms;

namespace MovieTimeWindowsForms
{
    static class Program
    {
        /// <summary>
        /// Ponto de entrada principal para o aplicativo.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if (!File.Exists(CheckUpdate.file))
            {
                Application.Run(new CheckUpdate());
            }
            else
            {
                Application.Run(new Default());
            }
        }
    }
}
