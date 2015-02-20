#region

using System;
using System.Diagnostics;
using System.IO;

#endregion

namespace Superfish_Removal_Tool.Removers
{
    internal class ServiceRemover : ISuperfishRemover
    {
        #region Implementation of ISuperfishRemover

        public string Name
        {
            get { return "Service Application"; }
        }

        public bool NeedsRemoved()
        {
            return File.Exists(GetUninstallerPath());
        }

        public SuperfishRemovalResult Remove()
        {
            var p = new Process
            {
                StartInfo =
                {
                    FileName = GetUninstallerPath(),
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    UseShellExecute = false
                }
            };

            try
            {
                if (p.Start())
                {
                    p.WaitForExit(20000);
                }

                return new SuperfishRemovalResult(p.ExitCode == 0);
            }

            catch (Exception ex)
            {
                return new SuperfishRemovalResult(false, ex);
            }
        }

        #endregion

        private static string GetUninstallerPath()
        {
            return Path.Combine(GetInstallationDirectory(), "uninstall.exe");
        }

        private static string GetInstallationDirectory()
        {
            return Path.Combine(GetProgramFilesx86(), "Lenovo\\VisualDiscovery");
        }

        private static string GetProgramFilesx86()
        {
            return Environment.GetEnvironmentVariable("ProgramFiles(x86)") ?? Environment.GetEnvironmentVariable("ProgramFiles");
        }
    }
}