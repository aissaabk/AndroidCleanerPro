using System.Collections.Generic;
using System.Linq;
using AndroidCleanerPro.Models;

namespace AndroidCleanerPro.Services
{
    public class AdKillerService
    {
        private readonly ScannerService _scanner = new();
        private readonly AdbService _adb = new();

        public List<AndroidApp> GetAdApps()
        {
            var apps = _scanner.ScanApps();

            return apps
                .Where(a => a.Risk >= 50)
                .OrderByDescending(a => a.Risk)
                .ToList();
        }

        public void RemoveAdApps(List<AndroidApp> apps)
        {
            foreach (var app in apps)
            {
                // safe uninstall (user level)
                _adb.Run($"uninstall {app.PackageName}");
            }
        }

        public void DisableAdApps(List<AndroidApp> apps)
        {
            foreach (var app in apps)
            {
                _adb.Run($"shell pm disable-user --user 0 {app.PackageName}");
            }
        }
    }
}