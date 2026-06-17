using AndroidCleanerPro.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AndroidCleanerPro.Services
{
    public class AppScannerService
    {
        public List<AppInfo> GetInstalledApps(
            AdbService adb,
            string serial)
        {
            var output = adb.RunOnDevice(
                serial,
                "shell pm list packages -3"
            );

            var apps = new List<AppInfo>();

            foreach (var line in output.Split('\n'))
            {
                if (!line.StartsWith("package:"))
                    continue;

                string packageName = line
                    .Replace("package:", "")
                    .Trim();

                DateTime installDate = GetInstallDate(
                    adb,
                    serial,
                    packageName
                );

                apps.Add(new AppInfo
                {
                    PackageName = packageName,
                    InstallDate = installDate
                });
            }

            // 🔥 مهم: ترتيب من الأحدث للأقدم
            return apps
                .OrderByDescending(x => x.InstallDate)
                .ToList();
        }

        private DateTime GetInstallDate(
            AdbService adb,
            string serial,
            string packageName)
        {
            try
            {
                var info = adb.RunOnDevice(
                    serial,
                    $"shell dumpsys package {packageName}"
                );

                foreach (var line in info.Split('\n'))
                {
                    if (!line.Contains("firstInstallTime"))
                        continue;

                    var parts = line.Split('=');

                    if (parts.Length < 2)
                        continue;

                    var value = parts[1].Trim();

                    // 🔥 محاولة parsing قوية
                    if (DateTime.TryParseExact(
                        value,
                        "yyyy-MM-dd HH:mm:ss",
                        CultureInfo.InvariantCulture,
                        DateTimeStyles.None,
                        out DateTime date))
                    {
                        return date;
                    }

                    // fallback
                    if (DateTime.TryParse(value, out date))
                        return date;
                }
            }
            catch
            {
                // ignore errors (ADB sometimes fails per app)
            }

            return DateTime.MinValue;
        }
    }
}