using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindWakerHD_Rewrite
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
           
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            if(System.Diagnostics.Debugger.IsAttached)
            {
                Application.Run(new Main());
            } else
            {
                Application.Run(new Splash());
            }
        }
    }
}
