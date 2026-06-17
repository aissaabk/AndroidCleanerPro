using AndroidCleanerPro.Models;
using AndroidCleanerPro.Services;
using System.Collections.ObjectModel;

namespace AndroidCleanerPro.ViewModels
{
    public class MainViewModel
    {
        // ✅ ADD THIS
        public ObservableCollection<AndroidDevice> Devices { get; set; }
            = new();

        public ObservableCollection<AppInfo> Apps { get; set; }
            = new();

        public DashboardStats Stats { get; set; }
            = new();

        public string CurrentApp { get; set; } = "";

        public void LoadApps(AdbService adb, string serial)
        {
            Apps.Clear();

            var scanner = new AppScannerService();

            var result = scanner.GetInstalledApps(adb, serial);

            foreach (var app in result)
                Apps.Add(app);

            Stats.Calculate(Apps);
        }
    }
}