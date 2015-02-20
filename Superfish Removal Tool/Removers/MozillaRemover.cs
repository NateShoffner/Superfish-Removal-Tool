#region

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

#endregion

namespace Superfish_Removal_Tool.Removers
{
    internal class MozillaRemover : ISuperfishRemover
    {
        private const string CERT_ISSUER = "Superfish, Inc.";

        #region Implementation of ISuperfishRemover

        public string Name
        {
            get { return "Mozilla Root Certificate"; }
        }

        public bool NeedsRemoved()
        {
            var profiles = GetMozillaProfiles();

            foreach (var profile in profiles)
            {
                if (ProfileHasCert(profile))
                    return true;
            }

            return false;
        }

        public SuperfishRemovalResult Remove()
        {
            var profiles = GetMozillaProfiles();

            foreach (var profile in profiles)
            {
                if (ProfileHasCert(profile))
                {
                    var result = ExecuteCertUtil(profile, CertUtilCommand.Delete);

                    if (!result)
                    {
                        //todo better error handling for execution
                        return new SuperfishRemovalResult(false);
                    }
                }
            }

            return new SuperfishRemovalResult(true);
        }

        #endregion

        private static bool ProfileHasCert(string profile)
        {
            return ExecuteCertUtil(profile, CertUtilCommand.List);
        }

        private static bool ExecuteCertUtil(string profile, CertUtilCommand command)
        {
            string commandArg = null;

            switch (command)
            {
                case CertUtilCommand.Delete:
                    commandArg = "-D";
                    break;
                case CertUtilCommand.List:
                    commandArg = "-L";
                    break;
            }

            var p = new Process
            {
                StartInfo =
                {
                    FileName = "nss\\certutil.exe",
                    Arguments = string.Format("{0} -n \"{1}\" -d \"{2}\"", commandArg, CERT_ISSUER, profile),
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

                return p.ExitCode == 0;
            }

            catch
            {
                return false;
            }
        }

        private static IEnumerable<string> GetMozillaProfiles()
        {
            var profiles = new List<string>();

            var firefoxProfiles = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Mozilla\\Firefox\\Profiles");
            var thunderBirdAppData = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Thunderbird\\Profiles");

            if (Directory.Exists(firefoxProfiles))
                profiles.AddRange(Directory.GetDirectories(firefoxProfiles, "*.*", SearchOption.TopDirectoryOnly));

            if (Directory.Exists(thunderBirdAppData))
                profiles.AddRange(Directory.GetDirectories(thunderBirdAppData, "*.*", SearchOption.TopDirectoryOnly));


            return profiles;
        }

        internal enum CertUtilCommand
        {
            Delete,
            List
        }
    }
}