using System.Collections.ObjectModel;
using AndroidCleanerPro.Models;

namespace AndroidCleanerPro.Services
{
    public class ScannerService
    {
        private readonly AdbService _adb = new();

        public ObservableCollection<AndroidApp> ScanApps()
        {
            var list = new ObservableCollection<AndroidApp>();

            string output = _adb.Run("shell pm list packages -3");

            foreach (var line in output.Split('\n'))
            {
                if (!line.Contains("package:")) continue;

                string pkg = line.Replace("package:", "").Trim();

                int risk = AnalyzeRisk(pkg);

                list.Add(new AndroidApp
                {
                    PackageName = pkg,
                    Risk = risk
                });
            }

            return list;
        }

        private int AnalyzeRisk(string package)
        {
            int score = 0;

            foreach (var sdk in AdDatabase.AdSdkSignatures)
            {
                if (package.Contains(sdk))
                    score += 40;
            }

            // زيادة خطر إذا يحتوي على كلمات مشبوهة
            if (package.Contains("ads")) score += 20;
            if (package.Contains("video")) score += 10;
            if (package.Contains("crash")) score += 10;

            return score;
        }
    }
}