using AndroidCleanerPro.Services;
using System.Collections.ObjectModel;

namespace AndroidCleanerPro.ViewModels
{
    public class AdScannerViewModel
    {
        public ObservableCollection<string> Apps { get; set; } = new();

        public void LoadAppss(AdbService adb, string serial)
        {
            Apps.Clear();

            var list = adb.RunOnDevice(serial, "shell pm list packages -3");

            foreach (var line in list.Split('\n'))
            {
                if (line.Contains("package:"))
                    Apps.Add(line.Replace("package:", "").Trim());
            }
        }
    }
}