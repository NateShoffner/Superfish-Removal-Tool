#region

using System;
using System.IO;
using System.Windows.Forms;
using Superfish_Removal_Tool.Forms;

#endregion

namespace Superfish_Removal_Tool
{
    internal static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if (IsRunningWin8())
            {
                if (!Directory.Exists("nss") || !File.Exists("nss/certutil.exe"))
                    MessageBox.Show("CertUtil.exe is required to remove certificates from Mozilla products..", "Missing CertUtil.exe", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                Application.Run(new MainForm());
            }

            else
            {
                MessageBox.Show("This tool is for Windows 8 and newer.", "Invalid Windows Version", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool IsRunningWin8()
        {
            return Environment.OSVersion.Version >= new Version(6, 2, 9200, 0);
        }
    }
}