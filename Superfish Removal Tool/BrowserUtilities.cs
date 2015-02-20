#region

using System.Collections.Generic;
using System.Diagnostics;

#endregion

namespace Superfish_Removal_Tool
{
    internal class BrowserProcess
    {
        public BrowserProcess(string name, string processName)
        {
            Name = name;
            ProcessName = processName;
        }

        public string Name { get; private set; }
        public string ProcessName { get; private set; }
    }

    internal static class BrowserUtilities
    {
        private static readonly List<BrowserProcess> _browserProcesses = new List<BrowserProcess>()
        {
            new BrowserProcess("Mozilla Firefox", "firefox"),
            new BrowserProcess("Google Chrome", "chrome"),
            new BrowserProcess("Internet Explorer", "iexplore"),
            new BrowserProcess("Opera", "opera"),
            new BrowserProcess("Safari", "safari")
        };

        public static List<BrowserProcess> GetRunningBrowsers()
        {
            var browsers = new List<BrowserProcess>();

            foreach (var browser in _browserProcesses)
            {
                if (Process.GetProcessesByName(browser.ProcessName).Length > 0)
                    browsers.Add(browser);
            }

            return browsers;
        }
    }
}