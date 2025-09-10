using System;
using System.Windows.Forms;

namespace PROG7312.POE
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();                 // .NET Framework style
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());                  // Start the menu
        }
    }
}