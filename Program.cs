using ExpertEvaluator.Forms;
using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ExpertEvaluator {

    public static class Program {

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main() {
            using (Mutex mutex = new Mutex(false, "Global\\" + Assembly
                .GetExecutingAssembly().GetCustomAttribute<GuidAttribute>().Value)) {
                if (!mutex.WaitOne(0, false)) {
                    return;
                }
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
        }
    }
}
